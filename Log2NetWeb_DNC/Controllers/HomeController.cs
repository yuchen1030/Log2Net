using Log2Net;
using Log2Net.Models;
using Log2NetWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Log2NetWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Microsoft.AspNetCore.Http.HttpRequest httpRequest = Request;

            var TTTT = Request.HttpContext.Connection.RemoteIpAddress;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            var dic = LogApi.GetLogWebApplicationsName();
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            Log_OperateTraceBllEdm logModel = new Log_OperateTraceBllEdm() { Detail = "进入了关于页面" };
            LogApi.WriteLog(LogLevel.Info, logModel);
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
