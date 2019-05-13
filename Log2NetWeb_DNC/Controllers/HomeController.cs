using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Log2NetWeb.Models;
using Microsoft.AspNetCore.HttpOverrides;
using Log2Net.Models;
using Log2Net;

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

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            Log_OperateTraceBllEdm logModel = new Log_OperateTraceBllEdm() { Detail = "进入了关于页面" };
            LogApi.WriteLog( LogLevel.Info,  logModel);
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
