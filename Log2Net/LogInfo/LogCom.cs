using Log2Net.Appender;
using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Log2Net.LogInfo
{
    internal class LogCom
    {
        static int logBackupWarnNum = 100;   //日志备份数量上限，到达则提醒
        static int logReadThreadSleepNum = 10;// 10秒
        static ICache dataCache = CacheFac.CacheFactory();
        static BaseAppender appender = AppenderFac.AppenderFactory();

        const string dateTimeFieldFormat = "yyyy-MM-dd HH:mm:ss.fff";  //数据库中时间字段的格式
        const string dateTimeFileFormat = "yyyyMMdd_HHmmsssfff_";  //文件名中时间字段的格式
        const string dateFileFormat = "yyyyMMdd_";  //文件名中日期字段的格式

        //在进行日志系统注册之后，可使用该方法获系统编号
        public static SysCategory GetSystemID()
        {
            try
            {
                SysCategory sysID = (SysCategory)dataCache.GetCache(AppConfig.GetCacheKey(CacheConst.systemName));
                return sysID;
            }
            catch
            {
                return SysCategory.NoDefined;
            }


        }


        //获取服务器的运行时间
        public static TimeSpan GetServerRunningTime()
        {
            var timeSpanOri = TimeSpan.FromMilliseconds((Environment.TickCount));
            //Environment.TickCount为int 32 ，TickCount 将在约 24.9天内从零递增至 Int32.MaxValue，然后跳至 Int32.MinValue（这是一个负数），再在接下来的 24.9 天内递增至零。
            try
            {
                var hex = Environment.TickCount.ToString("x");
                var dec = Convert.ToUInt64(hex, 16);
                var timeSpan = TimeSpan.FromMilliseconds(dec);   //服务器上次启动到现在已运行时间
                return timeSpan;
            }
            catch (Exception ex)
            {
                LogApi.WriteInfoToFile(new { 内容 = "GetServerRunningTime 出错：" + ex.Message });
                return TimeSpan.FromMilliseconds(0);
            }


        }

        //将日志备份数据以json格式写入到文件中，以便以后读取到appender中
        public static void WriteBackupLogToFile<T>(T model, MQType mqType)
        {
            WriteModelToFile(model, mqType.ToString(), FileType.Backup);
        }

        //将日志保存到文件中
        internal static ExeResEdm WriteLogToFile<T>(T model, MQType mqType)
        {
            return WriteModelToFile(model, mqType.ToString(), FileType.File);
        }

        //将数据记录到文件中，
        //若是本地调试文件和日志，则文件名为日期，每行数据加时间；
        //若是备份日志，每条记录一个文件，每行数据为Json;
        internal static ExeResEdm WriteModelToFile<T>(T model, string typeName, FileType fileType)
        {
            ExeResEdm exeResEdm = new ExeResEdm() { };
            string msg = SerializerHelper.SerializeToString(model);
            try
            {
                string path = GetWholeFolderPath(fileType);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string txtFile = DateTime.Now.ToString(dateTimeFileFormat) +/* Guid.NewGuid().ToString("N") + "_" */ typeName + ".log";

                if (txtFile.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                {
                    typeName = "typeNameValid";
                    txtFile = DateTime.Now.ToString(dateFileFormat) +/* Guid.NewGuid().ToString("N") + "_" */   "typeNameValid.log";
                    msg = msg + "---" + typeName;
                }
                if (fileType != FileType.Backup /* fileType == FileType.Debug || fileType == FileType.File || fileType == FileType.Info*/)
                {
                    txtFile = DateTime.Now.ToString(dateFileFormat) + typeName + ".log";
                    msg = DateTime.Now.ToString("HH:mm:ss.fff：") + msg;
                }
                txtFile = Path.Combine(path, txtFile);
                if (fileType == FileType.Backup)
                {
                    if (File.Exists(txtFile))
                    {
                        txtFile = DateTime.Now.ToString(dateTimeFileFormat) + DateTime.Now.Ticks +/* Guid.NewGuid().ToString("N") + "_" */ typeName + ".log";
                        txtFile = Path.Combine(path, txtFile);
                    }
                }
                System.IO.File.AppendAllLines(txtFile, new string[] { msg });
                return exeResEdm;
            }
            catch (Exception ex)
            {
                exeResEdm.ErrCode = 1;
                exeResEdm.Module = "WriteModelToFile方法";
                exeResEdm.ExBody = ex;
                return exeResEdm;

            }
        }

        //将异常信息写入到文件中
        public static void WriteExceptToFile(Exception ex, string module = "系统")
        {
            if (ex == null && string.IsNullOrEmpty(module))
            {
                return;
            }
            try
            {
                var errMsg = ex == null ? module : ex.Message;
                if (ex != null)
                {
                    try { var innerMsg = ex.InnerException.InnerException.InnerException.Message; errMsg = !string.IsNullOrEmpty(innerMsg) ? innerMsg : errMsg; } catch { }
                }
                else
                {
                    module = "系统";
                }
                Log_OperateTraceR exItem = new Log_OperateTraceR() { Detail = errMsg, LogType = LogType.异常, TabOrModu = module };
                WriteBackupLogToFile(exItem, MQType.TraceLogEx);
            }
            catch
            {

            }

        }

        //开启线程，将备份日志写到Appender中
        public static void StartThreadToWriteFileToAppender()
        {
            Thread thread = new Thread(() =>
            //     thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(1000 * 60);
                while (true)
                {
                    try
                    {
                        ReadBackupLogInFileToAppender();
                    }
                    catch //(Exception ex)
                    {
                        //throw ex;//不能让日志系统的异常导致整个系统的崩溃
                    }
                    finally
                    {
                        Thread.Sleep(logReadThreadSleepNum * 1000); //10秒
                    }

                }
            });
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
            Log_OperateTraceBllEdm logModel = new Log_OperateTraceBllEdm() { Detail = "读备份日志服务启动，服务执行周期为" + logReadThreadSleepNum + "秒" };
            LogApi.WriteLog(LogLevel.Info, logModel);
        }

        //将文件中的日志备份数据写入到Appender中
        static void ReadBackupLogInFileToAppender()
        {
            string path = GetWholeFolderPath(FileType.Backup);
            if (!Directory.Exists(path))
            {
                return;
            }
            string[] allFiles = Directory.GetFiles(path, "*.log");
            List<int> errIndex = new List<int>();
            for (int i = 0; i < allFiles.Length; i++)
            {
                if (errIndex.Count >= 2 && errIndex[0] == 0 && errIndex[1] == 1)//连续两个失败，则退出
                {
                    logReadThreadSleepNum *= 2;//循环时间延长
                    if (allFiles.Length > logBackupWarnNum)
                    {
                        logReadThreadSleepNum *= 10;//循环时间延长         
                        throw new Exception("系统中备份的日志数据已达到【" + allFiles.Length + "】条，请确认日志收集系统的队列服务工作正常。");
                    }
                    break;
                }
                var item = allFiles[i];
                var curText = System.IO.File.ReadAllText(item);
                object model = null;
                var bOK = false;

                if (item.Contains("_" + MQType.MonitorLog.ToString() + ".log"))
                {
                    model = SerializerHelper.DeserializeToObject<Log_SystemMonitorMQ>(curText);   //不能是Log_SystemMonitorR
                    bOK = appender.WriteLogAgain(model);
                }
                else if (item.Contains("_" + MQType.TraceLog.ToString() + ".log") || item.Contains("_" + MQType.TraceLogEx.ToString() + ".log"))
                {
                    model = SerializerHelper.DeserializeToObject<Log_OperateTrace>(curText);  //不能是Log_OperateTraceR
                    bOK = appender.WriteLogAgain(model);
                }

                if (bOK)
                {
                    System.IO.File.Delete(item);
                    Thread.Sleep(500);
                }
                else
                {
                    errIndex.Add(i);
                    Thread.Sleep(5000);
                }

            }
        }


        /// <summary>
        /// 操作轨迹实体，写到队列时使用
        /// </summary>
        [Serializable]
        public class Log_OperateTraceR
        {
            public long Id { get; set; }
            public string LogTime { get { return _Time; } }
            public string UserID { get; set; }//用户工号
            public string UserName { get; set; }//用户姓名
            public LogType LogType { get; set; } //日志类型
            public SysCategory SystemID { get { return _SystemID; } }//业务系统编号
            public string ServerHost { get { return _server.Host; } }//服务器名称
            public string ServerIP { get { return _server.IP; } }//服务器IP地址
            public string ClientHost { get { return _client.Host; } }//客户端名称
            public string ClientIP { get { return _client.IP; } }//客户端IP地址
            public string TabOrModu { get; set; }//表名或模块名称
            public string Detail { get; set; } //日志内容
            public string Remark { get; set; } //其他信息
            public Log_OperateTraceR()
            {
                _server = ClientServerInfo.ClientInfo.GetServerIPHost();
                _client = ClientServerInfo.ClientInfo.GetClientInfo();
                _Time = DateTime.Now.ToString(dateTimeFieldFormat);
                _SystemID = GetSystemID();
                if (_SystemID == SysCategory.NoDefined)
                {
                    throw new Exception("请确认您已经在启动事件中注册了 Log2net 日志组件");
                }
            }

            ClientServerInfo.ClientInfo.IPHost _server;
            ClientServerInfo.ClientInfo.IPHost _client;
            string _Time;
            SysCategory _SystemID;
        }


        /// <summary>
        /// 系统监控信息实体,写到队列中用
        /// </summary>
        [Serializable]
        public class Log_SystemMonitorR
        {
            public long Id { get; set; }
            public string LogTime { get { return _Time; } }
            public SysCategory SystemID { get { return _SystemID; } }//业务系统编号
            public string ServerHost { get { return _server.Host; } }//服务器名称
            public string ServerIP { get { return _server.IP; } }//服务器IP地址
            //public int OnlineCnt { get { return _onLineNum; } }    //在线人数
            //public int AllVisitors { get { return _allVisitNum; } }//历史访客
            public int OnlineCnt { get { return _onLineNum; } set { _onLineNum = value; } }    //在线人数
            public int AllVisitors { get { return _allVisitNum; } set { _allVisitNum = value; } }//历史访客
            public double RunHours { get { return _runHours; } }//运行时长
            public double CpuUsage { get { return _cpuUse; } }//cpu使用率
            public double MemoryUsage { get { return _memUse; } }//内存使用率
            public int ProcessNum { get { return _processNum; } }//进程总数
            public int ThreadNum { get { return _threadNum; } }//进程总数 -----------
            public int CurProcThreadNum { get { return _curProcThreadNum; } }//当前进程的线程数 -----------
            public double CurProcMem { get { return _curProcMem; } }//当前进程内存（单位M） -----------
            public double CurProcMemUse { get { return _CurProcMemUse; } }//当前进程内存使用率 -----------
            public double CurProcCpuUse { get { return _CurProcCpuUse; } }//当前进程cpu使用率 -----------
            public double CurSubProcMem { get { return _curSubProcMem; } }//当前程序内存（单位M） -----------
            public string Remark { get; set; } //其他信息
            public SqlXml PageViewNumR { get; set; }//重要页面访问量    
            public SqlXml DiskSpaceR { get { return _DiskSpace; } }//磁盘使用情况 
            public Log_SystemMonitorR()
            {
                _SystemID = GetSystemID();
                if (_SystemID == SysCategory.NoDefined)
                {
                    throw new Exception("请确认您已经在启动事件中注册了 Log2net 日志组件");
                }
                _Time = DateTime.Now.ToString(dateTimeFieldFormat);
                _server = ClientServerInfo.ClientInfo.GetServerIPHost();
                try
                {
                    int[] temp = GetVisitNumFun();
                    _onLineNum = temp[0];
                    _allVisitNum = temp[1];
                }
                catch
                {

                }

                _runHours = GetServerRunningTime().TotalHours;
                _memUse = ClientServerInfo.ServerInfo.CpuMemInfo.GetMemoryUsing();//内存使用率
                _cpuUse = ClientServerInfo.ServerInfo.CpuMemInfo.GetTotalCpuusing2ByVMI();//CPU使用率

                var process = Process.GetProcesses();
                _processNum = process.Count();   //进程总数
                _threadNum = process.Select(a => a.Threads.Count).Sum();//线程总数

                var curThread = Process.GetCurrentProcess();
                _curProcThreadNum = curThread.Threads.Count;
                _curProcMem = (curThread.WorkingSet64 / 1048576.0);
                _curSubProcMem = ((Double)GC.GetTotalMemory(false) / 1048576.0);    //当前程序占用内存

                List<DiskSpaceEdm> DiskSpace = ClientServerInfo.ServerInfo.GetHardDiskSpace();
                _DiskSpace = XmlSerializeHelper.ToSqlXml(DiskSpace);
            }

            SysCategory _SystemID;
            ClientServerInfo.ClientInfo.IPHost _server;
            string _Time;
            int _onLineNum = 0;
            int _allVisitNum = 0;
            double _cpuUse = 0;
            double _memUse = 0;
            int _processNum;
            int _threadNum;
            int _curProcThreadNum;
            double _curProcMem;
            double _curSubProcMem = 0;
            double _CurProcMemUse = 0;
            double _CurProcCpuUse = 0;
            double _runHours = 0;
            SqlXml _DiskSpace;

            int[] GetVisitNumFun()
            {
                VisitOnline.IVisitCount IVisitCount = VisitOnline.VisitCountFactory.GetInstance();
                int OnlineCnt = IVisitCount.GetOnlineNum(); //System.Web.HttpContext.Current.Session.Contents.Count;       //当前Session数量
                int AllVisitors = IVisitCount.GetCurVisitorNum();
                return new int[] { OnlineCnt, AllVisitors };
            }

        }


        //根据文件类型得到文件夹名称，只有文件类型为 File 时采用配置文件，其他情况下程序内部设定
        static string GetWholeFolderPath(FileType fileType)
        {
            string resValue = "App_Data/Log_" + fileType.ToString();
            if (fileType == FileType.File)
            {                
                string configValue1 = Log2NetConfig.GetConfigVal("logToFilePath");
                resValue = string.IsNullOrEmpty(configValue1) ? resValue : configValue1;
            }
            if (string.IsNullOrEmpty(Path.GetPathRoot(resValue)))
            {
                resValue = AppDomain.CurrentDomain.BaseDirectory + "/" + resValue;
            }
            return resValue + "/";
        }


    }
}
