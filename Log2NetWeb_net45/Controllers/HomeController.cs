using Log2Net;
using Log2Net.LogInfo;
using Log2Net.Models;
using System.Web.Mvc;

namespace Log2NetWeb_net45.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            //VisitOnline.IVisitCount IVisitCount = VisitOnline.VisitCountFactory.GetInstance();

            //IVisitCount.SetVisitNumWhenInit();
            //int OnlineCnt = IVisitCount.GetOnlineNum(); //System.Web.HttpContext.Current.Session.Contents.Count;

            ViewBag.Message = "Your application description page.";

            Log_OperateTraceBllEdm model = new Log_OperateTraceBllEdm()
            {
                Detail = "Log2 dll test",
                LogType = LogType.业务记录,
                Remark = "test",
                TabOrModu = "ce shi",
                UserID = "A7895",
                UserName = "hanmeimei",
            };
            //AdoNetAppender appender = new AdoNetAppender();
            //appender.WriteLogAndHandFail(model);
            LogApi.WriteLog( LogLevel .Info, model);
            
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}