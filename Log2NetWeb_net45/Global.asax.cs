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
            Log2Net.LogApi.RegisterLogInitMsg(SysCategory.SysB_01, Application);//日志系统注册

        }


        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                Log2Net.LogApi.HandAndWriteException();
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
                Log2Net.LogApi.WriteServerStopLog();//写停止日志
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
                Log2Net.LogApi.IncreaseOnlineVisitNum();
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
            Log2Net.LogApi.ReduceOnlineNum();
            Application.UnLock();
            GlobalSessionEnd();
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Log2Net.LogApi.WriteFirstVisitLog();//写初次访问日志
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
