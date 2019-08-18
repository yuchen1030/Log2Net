using Log2Net;
using Log2Net.Models;
using Log2Net.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Log2NetWeb_net45
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //第一步：注册日志系统
            LogApi.RegisterLogInitMsg(SysCategory.SysB_01, Application, GetUserConfigItemByCode(), GetLogWebApplicationsNameByCode());//日志系统注册

        }

        #region 用户对日志系统的代码配置  
        Dictionary<SysCategory, string> GetLogWebApplicationsNameByCode()
        {
            Dictionary<SysCategory, string> kvInCode = new Dictionary<SysCategory, string>()
            {
                {   SysCategory.SysA_01,"淘宝网" }
            };
            return kvInCode;
        }

        UserCfg GetUserConfigItemByCode()
        {
            UserCfg cfg = new UserCfg()
            {
                LogLevel = LogLevel.Info,
                LogAppendType = LogAppendType.DB,
                LogMonitorIntervalMins = 10,
                IsWriteInfoToDebugFile = false,
                LogToFilePath = "App_Data/Log_Files",
                DBAccessType = DBAccessType.ADONET,
                TraceDataBaseType = DataBaseType.SqlServer,
                MonitorDataBaseType = DataBaseType.SqlServer,
                TraceDBConKey = "logTraceSqlStr",
                MonitorDBConKey = "logMonitorSqlStr",
                IsConnectStrInCode = false,
                IsInitTraceDBWhenOracle = false,
                IsInitMonitorDBWhenOracle = false,
                OracleDriverType = OracleDriverType.Oracle,
                RabbitMQServer = "localhost:5672;oawxAdmin1;admin123.123",
                IsWriteToInfluxDB = false,
                InfluxDBServer = "http://127.0.0.1:8086/;logAdmin;sa123.123",
                CacheType = CacheType.MSHttp,
                MemCacheServer = "127.0.0.1:11211;127.0.0.2:11211",
                RedisCacheServer = "127.0.0.1:6379;127.0.0.2:6379",
                TraceDBConStr = "data source=.;initial catalog=LogTraceW;user id=sa;password=sa123.123;multipleactiveresultsets=True;application name=EntityFramework",
                MonitorDBDBConStr = "data source=.;initial catalog=LogMonitorW;user id=sa;password=sa123.123;MultipleActiveResultSets=True;application name=EntityFramework",
            };
            return cfg;
        }
        #endregion 用户对日志系统的代码配置


        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                LogApi.HandAndWriteException();
            }
            catch
            {
                System.Web.HttpContext.Current.Server.ClearError();
            }

        }

        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                LogApi.WriteServerStopLog();//写停止日志
            }
            catch
            {

            }

        }


        List<string> hcSessions = new List<string>();  //健康检查导致的seesion。这种session在人数统计时要忽略。
        protected void Session_Start(Object sender, EventArgs e)//客户端一连接到服务器上，这个事件就会发生
        {
            var bHC = System.Web.HttpContext.Current.Request["bHC"];
            if (bHC == "1")
            {
                hcSessions.Add(Session.SessionID);
                return;
            }
            //  ComClass.GetWXWebRootName();
            Application.Lock();//锁定后，只有这个Session能够会话        
            try
            {
                LogApi.IncreaseOnlineVisitNum();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Application.UnLock();//会话完毕后解锁
            }

            //Session["SessionId"] = Session.SessionID;
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (hcSessions.Contains(Session.SessionID))
            {
                hcSessions.Remove(Session.SessionID);
                return;
            }
            Application.Lock();
            LogApi.ReduceOnlineNum();
            Application.UnLock();
            GlobalSessionEnd();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            LogApi.WriteFirstVisitLog();//写初次访问日志
        }


        void GlobalSessionEnd()
        {
            try
            {
                Hashtable hOnline = (Hashtable)System.Web.HttpContext.Current.Application["Online"];//保存所有在线的人员
                if (hOnline[System.Web.HttpContext.Current.Session.SessionID] != null)
                {
                    hOnline.Remove(System.Web.HttpContext.Current.Session.SessionID);
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application["Online"] = hOnline;
                    System.Web.HttpContext.Current.Application.UnLock();
                }
            }
            catch
            {

            }
        }





    }
}
