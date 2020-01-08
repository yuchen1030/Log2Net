using Log2Net.Appender;
using Log2Net.Config;
using Log2Net.LogInfo;
using static Log2Net.LogInfo.LogCom;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil;
using Log2Net.Util.DBUtil.EF2DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;


#if NET
using System.Diagnostics;
using System.Reflection;
using System.Web;
using static Log2Net.LogInfo.VisitOnline;
#else
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Log2Net.DNCMiddleware;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
#endif

namespace Log2Net
{

    #region 日志组件使用说明
    /******************日志系统使用方法（所有被调用方法均位于 Log2Net.LogApi中）： /******************/
    //1、在应用程序开始时(Application Started)中注册日志系统（必须）：LogApi.RegisterLogInitMsg()方法;
    //2、在应用程序结束处(Application Stopped)，写网站停止日志（非必须）：LogApi.WriteServerStopLog方法；
    //3、在数据库操作、业务操作等必要的地方，记录日志（必须）：LogApi.WriteLog()方法；
    //NET平台中（4，5，6，7）////////////////////////
    //4、在程序错误事件中，记录错误日志（必须）：LogApi.HandAndWriteException方法；
    //5、在Session_Start事件中，设置在线人数和访客人数加1（非必须）：LogApi.IncreaseOnlineVisitNum方法；
    //6、在Session_End事件中，在线人数减1（非必须）：ReduceOnlineNum方法；
    //7、在Application_BeginRequest事件中，写初次访问日志（非必须）：LogApi.WriteFirstVisitLog方法；
    //NETCore平台中（8，9）////////////////////////
    //8、在服务配置(ConfigureServices)中添加服务：LogApi.AddLog2netService方法；
    //9、在Configure配置(Configure)中添加配置：LogApi.AddLog2netConfigure方法；


    //日志组件使用的配置项有24个，根据实际应用情况进行配置：前20个在Log2NetCfg文件中配置，
    //接下来的两个在configuration/connectionStrings中配置，最后的两个在configuration/appSettings中配置。

    /****日志组件基础配置******/
    //<!--日志级别：1、Off；2、Error；3、Warn； 4、Business ；5、DBRec； 6、Info；7、Debug （默认为7）-->
    //<add key = "log2NetLevel" value="7" />

    //<!--日志记录方式：1、写到文件；2、直接写到数据库；3、通过队列写到数据库；4、消息队列写到数据库；默认为1-->
    //<add key = "appenderType" value="1"/>

    //<!--监控日志每隔多少分钟记录一次，默认为10分钟,若小于0则不监控-->
    //<add key = "logMonitorIntervalMins" value="10"/>

    //<!--是否将info信息（仅供调试，不记录到日志的信息）记录到本地Debug文件：0不记录，1记录（默认为0）-->
    //<add key = "bWriteInfoToDebugFile" value="0"/>

    //<!--netCore中session过期时间配置，单位为分钟，默认值为20-->
    //<add key = "dncSessionTimeoutMins" value="20"/>
    /****日志组件基础配置******/

    /****写日志到文件的配置******/
    //<!--写文件的路径（仅在日志记录方式为1时有效）-->
    //<add key = "logToFilePath" value="App_Data/Log_Files"/>
    /****写日志到文件的配置******/

    /****数据库方式配置******/
    //<!-- 访问数据库的方式：ADONET = 1,  EF = 2,  NH = 3 。默认为1-->
    //<add key = "DBAccessTypeKey" value="2" />

    //<!--trace数据库的类型：SqlServer = 1,  Oracle = 2, MySql = 3,  Access = 4,   PostgreSQL = 5,  SQLite = 6。默认为1-->
    //<add key = "UserCfg_TraceDBTypeKey" value="1" />
    //<!--monitor数据库的类型：SqlServer = 1,  Oracle = 2, MySql = 3,  Access = 4,   PostgreSQL = 5,  SQLite = 6。默认为1-->
    //<add key = "UserCfg_MonitorDBTypeKey" value="1" />

    //<!--trace数据库的数据库连接字符串name值。默认为logTraceSqlStr-->
    //<add key = "UserCfg_TraceDBConKey" value="logTraceSqlStr" />
    //<!--monitor数据库的数据库连接字符串name值。默认为logMonitorSqlStr-->
    //<add key = "UserCfg_MonitorDBConKey" value="logMonitorSqlStr" />

    //<!--是否使用代码中的数据库连接字符串-->
    //<add key = "ConnectStrIsInCode" value="0" />

    /****数据库方式配置******/

    /****oracle配置******/
    // <!--是否使用EF初始化数据库Trace 表：0不使用，1使用，默认为0-->
    //<add key = "initTraceDBWhenOracle" value="0"/>

    //<!--是否使用EF初始化数据库 monitor 表：0不使用，1使用，默认为0-->
    //<add key = "initMonitorDBWhenOracle" value="0"/>

    //<!--Oracle数据库驱动方式：0 oracle驱动， 1 微软驱动，默认为0-->
    //<add key = "OracleDriverType" value="0"/>
    /****oracle配置******/

    /****消息队列配置******/
    //<!--消息队列服务器(地址、用户名、密码)-->
    //<add key = "RabbitMQServer_Log" value="localhost:5672;oawxAdmin1;admin123.123"/>
    /****消息队列配置******/

    /****InfluxDB配置******/
    //<!--是否需要写到InfluxDB数据库：0不写入，1写入（默认为0）-->
    //<add key = "bWriteToInfluxDB" value="0"/>

    //<!--InfluxDB服务器(地址、用户名、密码)-->
    //<add key = "InfluxDBServer_Log" value="http://127.0.0.1:8086/;userName;userPwd"/>
    /****InfluxDB配置******/

    /****缓存配置******/
    //<!--缓存策略：0、NET缓存；1、CacheManager中的NET系统缓存；2、Memcached缓存；3、Redis缓存；默认为0-->
    //<add key = "CacheStrategy" value="1"/>

    //<!--Memcache缓存服务器-->
    //<add key = "MemCacheServer" value="127.0.0.1:11211;127.0.0.2:11211"/>

    //<!--Redis缓存服务器-->
    //<add key = "RedisCacheServer" value="127.0.0.1:6379;127.0.0.2:6379"/>
    /****缓存配置******/

    /****数据库配置******/
    //<!--操作轨迹日志的数据库-->
    //<add name = "logTraceSqlStr" connectionString="Data Source =127.0.0.1;Initial Catalog = LogTraceW;uid=sa;pwd=123456"/>

    //<!--系统监控日志的数据库-->
    //<add name = "logMonitorSqlStr" connectionString="Data Source =127.0.0.1;Initial Catalog = LogMonitorW;uid=sa;pwd=123456"/>
    /****数据库配置******/


    /****WebConfig总体配置******/
    //<!--Log2Net模块的section名称-->
    //<add key = "log2netCfgSectionName" value="log2netCfg" />

    //<!--使用哪里的配置：File = 1,使用文件中的配置；Code = 2,使用代码中的配置；MixF = 3,混合使用，冲突时使用文件中的配置；MixC = 4,混合使用，冲突时使用代码中的配置；默认值为3(MixF)-->
    //<add key = "ConfigInWhere" value="1" />
    /****WebConfig总体配置******/

    /********************************************************使用方法介绍结束***************************************************/
    #endregion 日志组件使用说明          


    /// <summary>
    /// 对外使用的日志组件接口类：写用户业务日志、监控日志、特殊日志等
    /// 写用户业务日志和监控日志时，不需要包含服务器信息，不需要包含用户信息等，会自动采集；
    /// 特殊日志有以下几类:
    /// 服务器启动时，获取操作系统，.NET CLR版本；
    /// 网站被初次访问，记录记录IIS版本；
    /// 服务器停止时，获取已运行时间；
    /// 系统异常时，记录异常日志
    /// </summary>  
    public static class LogApi
    {
        #region 变量定义
        static int logReadThreadSleepNum = 10;// 10秒
        static int logBackupWarnNum = 100;   //日志备份数量上限，到达则提醒     
        static LogLevel logLevelCfg = LogLevel.Debug;
        static ICache dataCache = null;// CacheFac.CacheFactory();
        static BaseAppender appender = null;//AppenderFac.AppenderFactory();
        static UserCfg userCfg = null;// GetUserConfigItem();
        static Dictionary<SysCategory, string> webAppNames = null;
#if NET
        static object lockObj = new object();
        static bool bRegister = false;
#endif
        #endregion 变量定义


        #region 对外公开的方法

        /// <summary>
        /// 注册日志组件到本系统，为日志组件准备基础信息：服务器IP、服务器主机名，系统名称等
        /// </summary>
        /// <param name="sys">业务系统类型</param>
        /// <param name="application">应用程序的Application对象</param>
        /// <param name="bWriteStartLog">是否是启动日志</param>
        /// <param name="bLogMonitor">是否写定时监控日志</param>
        public static void RegisterLogInitMsg(SysCategory sys, object applicationObj = null, UserCfg userConfig = null, Dictionary<SysCategory, string> webAppName = null, bool bWriteStartLog = true, bool bLogMonitor = true)
        {
            webAppNames = webAppName ?? new Dictionary<SysCategory, string>();
            new HttpCacheHelper().SetCache(AppConfig.GetCacheKey(CacheConst.userCfgInCode), userConfig, expireType: Expire.Month);
            userCfg = GetUserConfigItem();
            userCfg = userCfg ?? new UserCfg();
            dataCache = CacheFac.CacheFactory();
            appender = AppenderFac.AppenderFactory();

            logLevelCfg = AppConfig.GetFinalConfig("log2NetLevel", LogLevel.Debug, GetLog2NetLevel());
            if (logLevelCfg == LogLevel.Off)
            {
                return;
            }

            // AutoMapperConfig.Configure();//注册AutoMapper
#if NET
            bRegister = true;
            ApplicationVisitCount.ApplicationObj = applicationObj;
#endif

            //var machineName = System.Web.HttpContext.Current.Server.MachineName;//服务器计算机名
            var machineName = Environment.MachineName;//服务器计算机名
            var server = ClientServerInfo.ClientInfo.GetIPAccordingHost(machineName);
            string serverIP = server.IP;
            dataCache.SetCache(AppConfig.GetCacheKey(CacheConst.serverIP), serverIP, expireType: Expire.Month);
            dataCache.SetCache(AppConfig.GetCacheKey(CacheConst.serverHost), machineName, expireType: Expire.Month);
            dataCache.SetCache(AppConfig.GetCacheKey(CacheConst.systemName), sys, expireType: Expire.Month);

            #region 使用EF自动创建数据库
            try
            {
                var initTraceDBWhenOracle = AppConfig.GetFinalConfig("initTraceDBWhenOracle", false, LogApi.IsInitTraceDBWhenOracle());
                //     initTraceDBWhenOracle = "1";
                var traceDBType = ComDBFun.GetDBGeneralInfo(DBType.LogTrace).DataBaseType;
                if (traceDBType != DataBaseType.Oracle || (traceDBType == DataBaseType.Oracle && initTraceDBWhenOracle))
                {
                    using (var context = new Log_OperateTraceContext())  //oracle 不建议使用EF，会导致字段名和数据库名必须加引号
                    {
#if NET
                        context.Database.Initialize(true);  //EF6
#else
                        context.Database.EnsureCreated();  //EFCore
#endif
                    }
                }

                var initMonitorDBWhenOracle = AppConfig.GetFinalConfig("initMonitorDBWhenOracle", false, LogApi.IsInitMonitorDBWhenOracle());
                //    initMonitorDBWhenOracle = "1";
                var monitorDBType = ComDBFun.GetDBGeneralInfo(DBType.LogMonitor).DataBaseType;
                if (monitorDBType != DataBaseType.Oracle || (monitorDBType == DataBaseType.Oracle && initMonitorDBWhenOracle))
                {
                    using (var context = new Log_SystemMonitorContext())
                    {
#if NET
                        context.Database.Initialize(true);  //EF6
#else
                        context.Database.EnsureCreated();  //EFCore
#endif
                    }
                }

            }
            catch (Exception ex)
            {
                WriteExceptToFile(ex);
            }
            #endregion 使用EF自动创建数据库

            if (bWriteStartLog)
            {
                WriteServerStartupLog();//系统启动的日志
            }

            StartThreadToWriteFileToAppender();  //开启线程，将备份日志写到Appender中

            //在线人数和访客人数的初始化
            VisitOnline.VisitCountFactory.GetInstance().SetVisitNumWhenInit();
            if (bLogMonitor)
            {
                WriteMonitorLogThread();
            }
        }

        /// <summary>
        /// 获取用户自定义的系统名称
        /// </summary>
        public static Dictionary<SysCategory, string> GetLogWebApplicationsName()
        {
            var cfgWhere = AppConfig.GetConfigMode();
            if (cfgWhere == CfgMode.File)
            {
                var cfgInFile = GetLogWebApplicationsNameFromFile();
                return cfgInFile;
            }
            else if (cfgWhere == CfgMode.Code)
            {
                return webAppNames;
            }
            else
            {
                var cfgInFile = GetLogWebApplicationsNameFromFile();
                var kvInCode = webAppNames;
                if (cfgWhere == CfgMode.MixC)//有相同值，使用Code中的
                {
                    foreach (var item in cfgInFile)
                    {
                        try { kvInCode.Add(item.Key, item.Value); } catch { }
                    }
                    return kvInCode;

                }
                else //   if (cfgWhere == CfgMode.MixF)//有相同值，使用File中的
                {
                    foreach (var item in kvInCode)
                    {
                        try { cfgInFile.Add(item.Key, item.Value); } catch { }
                    }
                    return cfgInFile;
                }
            }

            // return cfgInFile;

        }

        /// <summary>
        /// 获取当前在线人数和历史访客人数，第一个元素为在线人数，第二个元素为历史访客
        /// </summary>
        public static List<int> GetNumOfOnLineAllVisit()
        {
            int onLineUser = VisitOnline.VisitCountFactory.GetInstance().GetOnlineNum();
            int allVisitUser = VisitOnline.VisitCountFactory.GetInstance().GetCurVisitorNum();
            return new List<int>() { onLineUser, allVisitUser };
        }

        /// <summary>
        /// 写操作轨迹类日志数据
        /// </summary>
        /// <param name="model">操作轨迹类数据</param>
        /// <returns>错误信息；若无错误，则返回空</returns>
        public static string WriteLog(LogLevel logLevel, params LogTraceEdm[] model)
        {
            if (logLevel > logLevelCfg)
            {
                return "";
            }
            return WriteLog(model);

        }

        /// <summary>
        /// 写监控类数据
        /// </summary>
        /// <param name="model">监控类数据</param>
        /// <returns></returns>
        public static string WriteLog(LogLevel logLevel, params LogMonitorEdm[] model)
        {
            if (logLevel > logLevelCfg)
            {
                return "";
            }
            return WriteLog(model);
        }

        /// <summary>
        /// 将指定信息记录到文件中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        public static void WriteMsgToInfoFile<T>(T model)
        {
            WriteInfoToFile(model);
        }

        /// <summary>
        /// 将信息写入到文件中，主要记录调试信息但又不想记录到日志中的信息（可通过WebConfig的bWriteInfoToDebugFile 配置项开启关闭之）
        /// </summary>
        /// <typeparam name="T">泛型类，可使用匿名类型</typeparam>
        /// <param name="model"></param>
        public static void WriteMsgToDebugFile<T>(T model)
        {
            WriteModelToFileForDebug(model);
        }

        /// <summary>
        /// 写用户登录日志
        /// </summary>
        public static string WriteLoginLog(string userId, string userName = "")
        {
            var client = ClientServerInfo.ClientInfo.GetClientInfo();
            string userIP = client.IP + "[" + client.Host + "]";
            string loginInfo = "用IP为：" + userIP + ",物理地址为：" + ClientServerInfo.ClientInfo.GetCustomerMac(userIP) + "," + ClientServerInfo.ClientInfo.GetUserAgentInfo() + "的计算机，于" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "登录了平台";
            LogTraceEdm model = new LogTraceEdm()
            {
                UserId = userId,
                UserName = userName,
                LogType = LogType.登录,
                TabOrModu = "登录模块",
                Detail = loginInfo,
            };
            return WriteLog(model);
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string WriteExceptLog(Exception ex, string module = "异常模块", string remark = "")
        {
            try
            {
                var errMsg = ex.Message;
                try { var innerMsg = ex.InnerException.InnerException.InnerException.Message; errMsg = !string.IsNullOrEmpty(innerMsg) ? innerMsg : errMsg; } catch { }
                LogTraceEdm model = new LogTraceEdm() { LogType = LogType.异常, Detail = errMsg, Remark = remark, TabOrModu = module, };
                return WriteLog(model);
            }
            catch (Exception ex1)
            {
                return ex1.Message;
            }
        }

        /// <summary>
        /// 记录系统停止日志
        /// </summary>
        public static void WriteServerStopLog()
        {
            string shutDownMessageShort = "不明原因";
            try
            {
#if NET
                HttpRuntime runtime = (HttpRuntime)typeof(System.Web.HttpRuntime).InvokeMember("_theRuntime", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField, null, null, null);
                if (runtime == null) { return; }
                string shutDownMessage = (string)runtime.GetType().InvokeMember("_shutDownMessage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                string shutDownStack = (string)runtime.GetType().InvokeMember("_shutDownStack", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField, null, runtime, null);
                var shutDownMessageArr = shutDownMessage.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Take(3).ToArray().Select(a => a.Trim('。').Trim('.').Trim(',').Trim(':'));
                shutDownMessageShort = string.Join(",", shutDownMessageArr);
                //写到事件查看器
                new EventLog().WriteEntry(String.Format("\r\n\r\n_shutDownMessage={0}\r\n\r\n_shutDownStack={1}", shutDownMessage, shutDownStack), EventLogEntryType.Information);
#endif
            }
            catch
            {

            }
            //写到自定义日志中
            string detail = string.Format("服务器停止，停止原因为[{0}]，运行时长{1}", shutDownMessageShort, ClientServerInfo.ServerInfo.GetRunningTime());
            LogTraceEdm stop = new LogTraceEdm()
            {
                Detail = detail,
                LogType = LogType.停止,
                Remark = "停止时间" + DateTime.Now,
            };
            WriteLog(stop);
        }

#if NET

        /// <summary>
        /// 在线人数和访客人数加1
        /// </summary>
        public static void IncreaseOnlineVisitNum()
        {
            VisitOnline.VisitCountFactory.GetInstance().GetSetApplicationValue(AppKey.OnLineUserCnt, VisitOnline.VOAction.Add1);
            VisitOnline.VisitCountFactory.GetInstance().GetSetApplicationValue(AppKey.AllVisitorCnt, VisitOnline.VOAction.Add1);
        }

        /// <summary>
        /// 在线人数减1
        /// </summary>
        public static void ReduceOnlineNum()
        {
            VisitOnline.VisitCountFactory.GetInstance().GetSetApplicationValue(AppKey.OnLineUserCnt, VisitOnline.VOAction.Sub1);
        }

        /// <summary>
        /// 记录初次访问日志
        /// </summary>
        public static void WriteFirstVisitLog()
        {
            if (!bRegister)
            {
                return;
            }
            string firstReqKey = AppConfig.GetCacheKey(CacheConst.firstRequest);
            if (dataCache.GetCache(firstReqKey) as string == "0")
            {
                return;
            }
            lock (lockObj)
            {
                var client = ClientServerInfo.ClientInfo.GetClientInfo();
                string userIP = client.IP + "[" + client.Host + "]";
                string detail = string.Format("服务器(IIS版本{0})启动后，初次被访问，用户为{1}", ClientServerInfo.ServerInfo.GetIISVerson(), userIP);
                LogTraceEdm first = new LogTraceEdm()
                {
                    Detail = detail,
                    LogType = LogType.初次访问,
                    Remark = "访问时间" + DateTime.Now,
                    TabOrModu = "初次请求",
                };
                dataCache.SetCache(firstReqKey, "0", expireType: Expire.Month);
                WriteLog(first);
            }


        }

        /// <summary>
        /// .NET平台处理异常情况：先写日志再清除异常
        /// </summary>
        /// <param name="url">错误处理完成后跳转到的网址，为空则不跳转</param>
        public static void HandAndWriteException(string url = null)
        {
            try
            {
                if (System.Web.HttpContext.Current.Server.GetLastError() == null) return;
                Exception ex = System.Web.HttpContext.Current.Server.GetLastError().GetBaseException();
                string serMsg = string.Format("服务器环境：操作系统{0}，IIS版本[{1}]，.Net CLR版本[{2}]，运行时长[{3}]", ClientServerInfo.ServerInfo.GetServerOS(), ClientServerInfo.ServerInfo.GetIISVerson(), ClientServerInfo.ServerInfo.GetCLRVersion(), ClientServerInfo.ServerInfo.GetRunningTime());
                string detail = string.Format("URl：{0}\n引发异常的方法：{1}\n错误信息：{2}\n错误堆栈：{3}\n{4}", System.Web.HttpContext.Current.Request.RawUrl, ex.TargetSite, ex.Message, ex.StackTrace, serMsg);
                LogTraceEdm exLog = new LogTraceEdm()
                {
                    Detail = detail,
                    LogType = LogType.异常,
                    Remark = "异常时间" + DateTime.Now,
                    TabOrModu = "异常模块",
                };
                WriteLog(exLog);
            }
            catch
            {

            }
            finally
            {
                try
                {
                    System.Web.HttpContext.Current.Server.ClearError();
                    if (!string.IsNullOrEmpty(url))
                    {
                        System.Web.HttpContext.Current.Response.Redirect(url);
                    }
                }
                catch
                {

                }
            }
        }
#else
        /// <summary>
        /// .NETCore中添加Configure
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder AddLog2netConfigure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AppConfig.webName = env.ApplicationName;
            app = app.UseErrorHandling();
            app.UseSession();
            app.UseStaticHttpContext();

            app.Use(async (context, next) =>
            {
                //模拟session_start事件，session_end事件无法模拟
                int visits = context.Session.GetInt32("curSessionActive") ?? 0;
                if (visits == 0 /*&& CheckIfThisRequestNeedsToUseSession(context)*/)
                {
                    // New session, do any initial setup and then mark the session as ready
                    context.Session.SetInt32("curSessionActive", 1);
                    Log2Net.DNCMiddleware.HttpContext.GetOnlineVisitNum(0);
                    // ...
                }
                await next();
            });

            return app;
        }


        /// <summary>
        /// dotCore中添加services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        public static void AddLog2netService(IServiceCollection services, IConfiguration Configuration)
        {
            AppConfig.Configuration = Configuration;
            var mins = AppConfig.GetDncSessionTimeoutMins();
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromSeconds(mins * 60);
            });
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

#endif

        #endregion 对外公开的方法


        #region 私有方法  
        static string WriteLog(params LogTraceEdm[] model)
        {
            if (model == null || model.Length <= 0)
            {
                return "";
            }
            try
            {
                List<Log_OperateTrace> list = new List<Log_OperateTrace>();
                foreach (var item in model)
                {
                    Log_OperateTraceR cur = new Log_OperateTraceR()
                    {
                        Detail = item.Detail,
                        LogType = item.LogType,
                        Remark = item.Remark,
                        TabOrModu = item.TabOrModu,
                        UserID = !string.IsNullOrEmpty(item.UserId) ? item.UserId : item.UserName,
                        UserName = !string.IsNullOrEmpty(item.UserName) ? item.UserName : item.UserId,
                    };
                    Log_OperateTrace log_OperateTrace = //AutoMapperExtension.MapTo<Log_OperateTrace>(cur);
                    AutoMapperConfig.GetLog_OperateTraceModel(cur);
                    list.Add(log_OperateTrace);
                }

                appender.WriteLogAndHandFail(list);
                // SendLogToQueue(list, MQType.TraceLog);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw ex;
            }
            return "";
        }

        static string WriteLog(params LogMonitorEdm[] model)
        {
            if (model == null || model.Length <= 0)
            {
                return "";
            }

            try
            {
                List<Log_SystemMonitorMQ> list = new List<Log_SystemMonitorMQ>();
                foreach (var item in model)
                {
                    Log_SystemMonitorR cur = new Log_SystemMonitorR()
                    {
                        Remark = item.Remark,
                        PageViewNumR = XmlSerializeHelper.ToSqlXml(item.PagesView),
                    };
                    Log_SystemMonitorMQ log_SystemMonitorMQ = AutoMapperConfig.GetLog_SystemMonitorMQModel(cur);

                    list.Add(log_SystemMonitorMQ);
                }
                appender.WriteLogAndHandFail(list);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw ex.InnerException;
                }
                throw ex;
            }
            return "";
        }

        static Dictionary<SysCategory, string> GetLogWebApplicationsNameFromFile()
        {
            List<KVEdm> kVEdms = Log2NetConfig.GetSectionVal("userSystemNames");
            if (kVEdms.Count > 0)
            {
                Dictionary<SysCategory, string> dic = kVEdms.ToDictionary(k => StringEnum.GetEnumValue<SysCategory>(k.Key), v => v.Value);
                return dic;
            }
            else
            {
                var enumDic = StringEnum.GetDicFromEnumType(new SysCategory());
                return enumDic.ToDictionary(k => (SysCategory)k.Value, v => v.Key);
            }
        }

        //开启定时任务，写监控日志
        static void WriteMonitorLogThread()
        {
            int sleepMillSec = 600000;//10分钟
            try
            {
                int sleepMillSecTemp = AppConfig.GetFinalConfig("logMonitorIntervalMins", 10, LogApi.GetLogMonitorIntervalMins()) * 60000;
                if (sleepMillSecTemp > 0)
                {
                    sleepMillSec = sleepMillSecTemp;
                }
            }
            catch
            {

            }
            if (sleepMillSec <= 0)
            {
                return;
            }

            int num = 0;
            Thread thread = new Thread(() =>
            //     thread = new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(1000 * 30);
                while (true)
                {
                    try
                    {
                        num++;
                        LogMonitorEdm monitor = new LogMonitorEdm() { Remark = "定时监控" };
                        WriteLog(monitor);
                        WriteModelToFileForDebug(new { 内容 = DateTime.Now.ToString("HH:mm:ss.fff") + " , 第" + num + "次记录监控日志" });
                    }
                    catch
                    {

                    }
                    finally
                    {
                        Thread.Sleep(sleepMillSec);

                    }

                }
            });
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.AboveNormal;
            thread.Start();
            LogTraceEdm logModel = new LogTraceEdm() { Detail = "定时监控服务启动，服务执行周期为" + (sleepMillSec / 60000.0) + "分钟" };
            WriteLog(logModel);
        }

        //开启线程，将备份日志写到Appender中
        static void StartThreadToWriteFileToAppender()
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
            LogTraceEdm logModel = new LogTraceEdm() { Detail = "读备份日志服务启动，服务执行周期为" + logReadThreadSleepNum + "秒" };
            // new MyLogApp().
            WriteLog(LogLevel.Info, logModel);
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

        //记录系统启动日志
        static void WriteServerStartupLog()
        {
#if NET
            string vsVer = ".Net";
#else
            string vsVer = ".Net Core";
#endif
            string detail = string.Format("服务器启动，操作系统为{0}，IIS版本为{1}，.NET CLR为{2}，开发框架为{3}", ClientServerInfo.ServerInfo.GetServerOS(), ClientServerInfo.ServerInfo.GetIISVerson(), ClientServerInfo.ServerInfo.GetCLRVersion(), vsVer);
            LogTraceEdm start = new LogTraceEdm()
            {
                Detail = detail,
                LogType = LogType.启动,
                Remark = "启动时间" + DateTime.Now,
            };
            WriteLog(start);
        }

        #endregion 私有方法



        #region 获取代码中进行的配置

        //获取用户代码中的配置
        static UserCfg GetUserConfigItem()
        {
            UserCfg userCfg = new HttpCacheHelper().GetCache(AppConfig.GetCacheKey(CacheConst.userCfgInCode)) as UserCfg;
            return userCfg;
        }

        internal static LogLevel GetLog2NetLevel()
        {
            return userCfg.LogLevel;
        }

        internal static LogAppendType GetLogAppendType()
        {
            return userCfg.LogAppendType;
        }

        internal static int GetLogMonitorIntervalMins()
        {
            return userCfg.LogMonitorIntervalMins;
        }

        internal static bool IsWriteInfoToDebugFile()
        {
            return userCfg.IsWriteInfoToDebugFile;
        }

        internal static string GetLogToFilePath()
        {
            return userCfg.LogToFilePath;
        }

        internal static DBAccessType GetDBAccessType()
        {
            return userCfg.DBAccessType;
        }

        internal static DataBaseType GetUserCfg_TraceDBTypeKey()
        {
            return userCfg.TraceDataBaseType;
        }

        internal static DataBaseType GetUserCfg_MonitorDBTypeKey()
        {
            return userCfg.MonitorDataBaseType;
        }

        internal static string GetUserCfg_TraceDBConKey()
        {
            return userCfg.TraceDBConKey;
        }

        internal static string GetUserCfg_MonitorDBConKey()
        {
            return userCfg.MonitorDBConKey;
        }

        internal static bool IsInitTraceDBWhenOracle()
        {
            return userCfg.IsInitTraceDBWhenOracle;
        }

        internal static bool IsInitMonitorDBWhenOracle()
        {
            return userCfg.IsInitMonitorDBWhenOracle;
        }

        internal static OracleDriverType GetOracleDriverType()
        {
            return userCfg.OracleDriverType;
        }

        internal static string GetRabbitMQServer_Log()
        {
            return userCfg.RabbitMQServer;
        }

        internal static bool IsWriteToInfluxDB()
        {
            return userCfg.IsWriteToInfluxDB;
        }

        internal static string GetInfluxDBServer_Log()
        {
            return userCfg.InfluxDBServer;
        }

        internal static CacheType GetCacheStrategy()
        {
            if (userCfg == null)
            {
                throw new Exception("您尚未注册Log2Net日志组件");
            }
            return userCfg.CacheType;
        }

        internal static string GetMemCacheServer()
        {
            return userCfg.MemCacheServer;
        }
        internal static string GetRedisCacheServer()
        {
            return userCfg.RedisCacheServer;
        }

        internal static bool IsConnectStrInCode()
        {
            return userCfg.IsConnectStrInCode;
        }
        //   
        internal static string GetTraceDBConnectionString()
        {
            return userCfg.RedisCacheServer;
        }

        internal static string GetMonitorDBConnectionString()
        {
            return userCfg.RedisCacheServer;
        }

        #endregion 获取代码中进行的配置


    }
}
