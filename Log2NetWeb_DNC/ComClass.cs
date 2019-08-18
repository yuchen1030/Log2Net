
using Log2Net;
using Log2Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using Log2Net;
using Log2Net.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace Log2NetWeb_DNC
{
    public class ComClass
    {
        public string WriteLog(LogLevel logLevel, params LogTraceVM[] model)
        {
            var myLogModels = model.Select(a => new LogTraceEdm()
            {
                UserId = GetCurUserID(),
                UserName = GetCurUserName(),
                Detail = a.Detail,
                LogType = a.LogType,
                Remark = a.Remark,
                TabOrModu = a.TabOrModu,
            }).ToArray();

            return LogApi.WriteLog(LogLevel.Info, myLogModels);
        }


        string GetCurUserID()
        {
            try
            {
                ISession _session = Log2Net.DNCMiddleware.HttpContext.Current.Session;
                return _session.GetString("curUserID");
            }
            catch
            {
                return "系统";
            }

        }

        string GetCurUserName()
        {
            try
            {
                ISession _session = Log2Net.DNCMiddleware.HttpContext.Current.Session;
                return _session.GetString("curUserName");
            }
            catch
            {
                return "系统";
            }

        }


    }

    public class LogTraceVM
    {
        public LogType LogType { get { return _LogType; } set { _LogType = value; } } //日志类型
        public string TabOrModu { get { return _TabOrModu; } set { _TabOrModu = value; } }//表名或模块名称
        public string Detail { get; set; } //日志内容
        public string Remark { get; set; } //其他信息

        string _TabOrModu = "启动";
        LogType _LogType = LogType.业务记录;
    }


}