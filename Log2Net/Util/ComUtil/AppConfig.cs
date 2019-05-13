using Log2Net.Models;

#if NET
using System.Web.Configuration;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
#endif

namespace Log2Net.Util
{

    internal class AppConfig
    {

#if NET
#else
        public static IConfiguration Configuration { get; set; }
#endif
        public static string webName = "log2netBase";

        static AppConfig()
        {
#if NET
            webName = System.Web.Hosting.HostingEnvironment.ApplicationHost.GetSiteName();
#else
            //   webName = hostingEnvironment.ApplicationName;
#endif
        }
        public static string GetConfigValue(string key)
        {
#if NET
            if (!string.IsNullOrEmpty(key))
            {
                var str = WebConfigurationManager.AppSettings[key];  //System.Configuration.ConfigurationManager.AppSettings[key];
                return !string.IsNullOrEmpty(str) ? str : "";
            }
            return "";
#else
            key = GetRealKey(key);
            if( string.IsNullOrEmpty(key))
            {
                return "";
            }
            return GetValue(key) as string;
#endif
        }



        public static string GetDBConnectString(string sqlStrKey)
        {
            try
            {
#if NET
                string conStr = System.Configuration.ConfigurationManager.ConnectionStrings[sqlStrKey].ConnectionString;
#else
                string conStr = Configuration.GetConnectionString(sqlStrKey);
#endif
                return conStr;
            }
            catch
            {
                return "";
            }
        }


#if NET
#else

        //获取netCore中session过期的分钟数，默认值为20
        public static double GetDncSessionTimeoutMins()
        {
            var minsText = GetConfigValue("dncSessionTimeoutMins");
            double minsNum = 0;
            double.TryParse(minsText, out minsNum);
            minsNum = minsNum <= 0 ? 20 : minsNum;
            return minsNum;
        }
        //key值是子节点的话，子节点前以:分开,例如 Logging:LogLevel:Default
        static object GetValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            key = key ?? "";
            var temp = key.Split(new char[] { ':', '.', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            key = string.Join(":", temp);
            object res = Configuration[key];
            if (res == null)
            {
                //  Configuration. GetSection("AllowCallers").Get<string[]>() ；Configuration["AllowCallers:0"];
                res = Configuration.GetSection(key).Value;
                if (res == null)
                {
                    res = GetValue_2<string[]>(key);
                }
            }
            res = res ?? "";
            return res;
        }

        static Dictionary<string, string> keysDic = new Dictionary<string, string>();

        static string GetRealKey(string key)
        {
            if (keysDic.Count <= 0)
            {
                var fir = Configuration.GetChildren();
                foreach (var item in fir)
                {
                    try { keysDic.Add(item.Key, item.Key); } catch { }

                    var child = item.GetChildren();
                    foreach (var ch in child)
                    {
                        try { keysDic.Add(ch.Key, item.Key + ":" + ch.Key); } catch { }
                    }
                }
            }
            var realKey = keysDic.Where(a => key.Equals(a.Key, StringComparison.OrdinalIgnoreCase)).LastOrDefault();
            if (string.IsNullOrEmpty(realKey.Key))
            {
                return "";
            }
            return realKey.Value;

        }

        static object GetValue_2<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            key = key ?? "";
            var temp = key.Split(new char[] { ':', '.', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            key = string.Join(":", temp);
            object res = Configuration[key];
            if (res == null)
            {
                //  Configuration. GetSection("AllowCallers").Get<string[]>() ；Configuration["AllowCallers:0"];
                res = Configuration.GetSection(key).Value;
                if (res == null)
                {
                    res = Configuration.GetSection(key);//.Get<T>();
                }
            }
            res = res ?? "";
            return res;
        }



#endif



        public static string GetCacheKey(CacheConst cacheConst)
        {
            var text = webName + "." + cacheConst.ToString();
            int length = text.Length;
            return text;
        }



    }


}
