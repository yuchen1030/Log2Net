using Log2Net;
using Log2Net.Models;
using Log2NetWeb_DNC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Log2NetWeb_DNC.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetCurrentUser();
            base.OnActionExecuting(context);
        }
        void GetCurrentUser()
        {
            HttpContext.Session.SetString("curUserID", "UserID123_DNC");// Log2Net.DNCMiddleware.HttpContext.Current.Session.SetString("curUserID","UserID123_DNC");
            HttpContext.Session.SetString("curUserName", "UserName123_DNC");
        }
    }

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var dic = LogApi.GetLogWebApplicationsName();
            var userCnt = LogApi.GetNumOfOnLineAllVisit();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            LogTraceVM logModel = new LogTraceVM() { LogType = LogType.审批, Detail = "人间天堂，最美苏杭", TabOrModu = "联系我们" };
            new ComClass().WriteLog(LogLevel.Info, logModel);
            var logRes = LogApi.WriteLoginLog();
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
