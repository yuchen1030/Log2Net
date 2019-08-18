using Log2Net.Config;
using Log2Net.Models;
using System;

#if NET
using System.Web.Configuration;
#else
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
#endif

namespace Log2Net.Util
{

    public class AppConfig
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

        //根据键获取配置值
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
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            return GetValue(key) as string;
#endif
        }

        //获取数据库连接字符串
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

        //获取使用哪里的配置
        internal static CfgMode GetConfigMode()
        {
            string val = GetConfigValue("ConfigInWhere");
            try
            {
                var cfgMode = StringEnum.GetEnumValue<CfgMode>(val);
                return cfgMode;
            }
            catch
            {
                return CfgMode.MixF;
            }
        }

        //综合文件和代码中的配置，获取最终的配置结果
        internal static T GetFinalConfig<T>(string keyInFie, T defaultVal, T valInCode)
        {
            T result = defaultVal;
            var cfgWhere = GetConfigMode();
            if (cfgWhere == CfgMode.File)
            {
                try
                {
                    result = GetCfgValueFromFile(keyInFie, defaultVal);
                }
                catch
                {
                    result = defaultVal;
                }
            }
            else if (cfgWhere == CfgMode.Code)
            {
                result = valInCode;
            }
            else//混合模式
            {
                T fileCfgVal = default(T);
                try
                {
                    fileCfgVal = GetCfgValueFromFile(keyInFie, defaultVal);
                    if (cfgWhere == CfgMode.MixC)//冲突时优先使用代码中的
                    {
                        if (valInCode.Equals(default(T)))//代码中的值为空，则使用文件中
                        {
                            return fileCfgVal;
                        }
                        else
                        {
                            return valInCode;
                        }
                    }
                    else //冲突时优先使用文件中的
                    {
                        if (fileCfgVal.Equals(default(T)))//文件中的值为空，则使用代码中
                        {
                            return valInCode;
                        }
                        else
                        {
                            return fileCfgVal;
                        }
                    }

                }
                catch
                {
                    return defaultVal;
                }

            }
            return result;
        }

        //从文件中获取配置值
        static T GetCfgValueFromFile<T>(string keyInFie, T defaultVal)
        {
            T result = defaultVal;
            var curVal = Log2NetConfig.GetConfigVal(keyInFie);
            var curType = typeof(T);

            if (curType.IsEnum)
            {
                result = StringEnum.GetEnumValue<T>(curVal);
                return result;
            }
            if (curType.Name == "Boolean")
            {
                curVal = curVal == "1" || curVal.Equals("true", StringComparison.OrdinalIgnoreCase) ? "true" : "false";
            }
            result = (T)Convert.ChangeType(curVal, typeof(T));
            return result;
        }

        //根据缓存类型获取缓存的键
        internal static string GetCacheKey(CacheConst cacheConst)
        {
            var text = webName + "." + cacheConst.ToString();
            int length = text.Length;
            return text;
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
        public static object GetValue(string key)
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
            if(Configuration == null)
            {
                throw new Exception("请确保您已注册了Log2netService");
            }
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
                    if (res != null)  //key为section
                    {
                        var secKVs = keysDic.Where(a => a.Value.StartsWith(key + ":", StringComparison.OrdinalIgnoreCase)).ToList();
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        foreach (var item in secKVs)
                        {
                            var ttttt = Configuration[item.Value];
                            try { dic.Add(item.Key, Configuration[item.Value] as string); } catch { }
                        }
                        return dic;
                    }
                }


            }
            res = res ?? "";
            return res;
        }



#endif




    }


}
