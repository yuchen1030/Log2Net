using Log2Net;
using Log2Net.LogInfo;
using Log2Net.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Log2NetWeb_net45.Controllers
{


    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GetCurrentUser();
            base.OnActionExecuting(filterContext);
        }
        void GetCurrentUser()
        {
            try
            {
                System.Web.HttpContext.Current.Session["curUserID"] = "UserID123";
                System.Web.HttpContext.Current.Session["curUserName"] = "UserName123";
            }
            catch
            {

            }
        }


    }

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            LogTraceVM model = new LogTraceVM()
            {
                Detail = "所有的程序员都是天才编剧，所有的计算机都是烂演员~",
                LogType = LogType.审核,
                Remark = "同意嘛",
                TabOrModu = "关于页面",
            };
            var logRes = new ComClass().WriteLog(LogLevel.Info, model);

            Dictionary<SysCategory, string> dic = LogApi.GetLogWebApplicationsName();
            LogApi.WriteLoginLog();
            return View();
        }




        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}