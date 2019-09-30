using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil.Models;
using System.Collections.Generic;
using System.Linq;
using static Log2Net.LogInfo.LogCom;

namespace Log2Net.Appender
{
    //Appender工厂
    internal class AppenderFac
    {
        static readonly object locker = new object();
        static BaseAppender appender = null;

        //1、写到文件；2、直接写到数据库；3、通过队列写到数据库；4、消息队列写到数据库；
        public static BaseAppender AppenderFactory()
        {
            if (appender != null)
            {
                return appender;
            }
            lock (locker)
            {
                if (appender == null)
                {
                    string typeStr = "文件";
                    LogAppendType logAppendType = AppConfig.GetFinalConfig("appenderType", LogAppendType.File, LogApi.GetLogAppendType());
                    if (logAppendType == LogAppendType.DB)
                    {
                        appender = new DirectDBAppender();
                        typeStr = "直连数据库";
                    }
                    else if (logAppendType == LogAppendType.Queue2DB)
                    {
                        appender = new Queue2DBAppender();
                        typeStr = "队列数据库";
                    }
                    else if (logAppendType == LogAppendType.MQ2DB)
                    {
                        appender = new MQ2DBAppender();
                        typeStr = "消息队列数据库";
                    }
                    else
                    {
                        appender = new FileAppender();
                        typeStr = "文件";
                    }
                    string msg = "日志记录方式为【" + typeStr + "】";
                    WriteModelToFileForDebug(new { 内容 = msg });
                }
            }
            return appender;
        }

    }

    //公共日志追加器
    internal abstract class BaseAppender
    {
        //写Log_OperateTraceR日志
        protected abstract ExeResEdm WriteLog(Log_OperateTrace model);

        //写Log_SystemMonitorR日志
        protected abstract ExeResEdm WriteLog(Log_SystemMonitorMQ model);


        //写日志，失败时写到文件中
        internal bool WriteLogAndHandFail(object model)
        {
            //return true;
            bool bList = model.GetType().IsGenericType || model.GetType().IsArray;
            IEnumerable<Log_OperateTrace> obj = bList ? model as IEnumerable<Log_OperateTrace> : new List<Log_OperateTrace>() { model as Log_OperateTrace };
            if (obj != null && obj.Count() > 0)//是操作轨迹类数据
            {
                foreach (var item in obj)
                {
                    var res = WriteLog(item);
                    if (res.ErrCode != 0)
                    {
                        WriteExceptToFile(res.ExBody, (res.ErrMsg + "," + res.Module).Trim(','));
                        //  System.Threading.Thread.Sleep(10);
                        WriteBackupLogToFile(item, Models.MQType.TraceLog);
                    }
                    else
                    {
                        bool bok = new InfluxDBHelper("LogTraceW").WriteData(item);
                    }
                }
            }
            else
            {
                IEnumerable<Log_SystemMonitorMQ> obj2 = bList ? model as IEnumerable<Log_SystemMonitorMQ> : new List<Log_SystemMonitorMQ>() { model as Log_SystemMonitorMQ };
                if (obj2 != null && obj2.Count() > 0)//是监控类数据
                {
                    foreach (var item in obj2)
                    {
                        if (item.OnlineCnt <= 0 || item.AllVisitors <= 0)
                        {
                            continue;
                        }
                        var res = WriteLog(item);
                        if (res.ErrCode != 0)
                        {
                            WriteExceptToFile(res.ExBody, (res.ErrMsg + "," + res.Module).Trim(','));
                            WriteBackupLogToFile(item, Models.MQType.MonitorLog);
                        }
                        else
                        {
                            bool bok = new InfluxDBHelper("LogMonitorW").WriteData(item);
                        }
                    }
                }
            }
            return true;
        }

        //写日志，失败时不写到文件中，参数为单数
        internal bool WriteLogAgain(object oneLog)
        {
            var model1 = oneLog as Log_OperateTrace;
            if (model1 != null)
            {
                var bOK = WriteLog(model1);

                if (bOK.ErrCode == 0)
                {
                    bool bok = new InfluxDBHelper("LogTraceW").WriteData(model1);
                }
                return bOK.ErrCode == 0;
            }
            else
            {
                var model2 = oneLog as Log_SystemMonitorMQ;
                if (model2 != null)
                {
                    var bOK = WriteLog(model2);
                    if (bOK.ErrCode == 0)
                    {
                        bool bok = new InfluxDBHelper("LogMonitorW").WriteData(model2);
                    }
                    return bOK.ErrCode == 0;
                }
            }
            return false;
        }


    }

}
