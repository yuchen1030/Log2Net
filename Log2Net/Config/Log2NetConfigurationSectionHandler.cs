using Log2Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Log2Net.Config
{

#if NET

    //用法：<section name="log2netCfg" type="Log2Net.Config.Log2NetConfigurationSectionHandler, Log2Net" /> 。
    //注意: 此处section name 的值必须跟 Log2NetCfg.config 中保持一致；本组件中默认为 log2netCfg。
    public class Log2NetConfigurationSectionHandler : System.Configuration.IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            var list = new List<KVEdm>();
            for (int i = 0; i < section.ChildNodes.Count; i++)   //xml 文件节点最多两级，且key不能重复
            {
                var attr = section.ChildNodes[i].Attributes;
                if (attr == null)
                {
                    continue;
                }
                var sonSections = section.ChildNodes[i].ChildNodes;
                if (sonSections != null && sonSections.Count > 0)
                {
                    var secName = section.ChildNodes[i].Name;
                    foreach (XmlNode item in sonSections)
                    {
                        if (item.Attributes == null)
                        {
                            continue;
                        }
                        try
                        {
                            list.Add(new KVEdm()
                            {
                                Key = item.Attributes["key"].InnerText.Trim(),
                                Value = item.Attributes["value"].InnerText.Trim()
                            });
                            list.Add(new KVEdm()
                            {
                                Key = secName + "." + item.Attributes["key"].InnerText.Trim(),
                                Value = item.Attributes["value"].InnerText.Trim()
                            });
                        }
                        catch { }
                    }
                }
                else
                {
                    try
                    {
                        list.Add(new KVEdm()
                        {
                            Key = attr["key"].InnerText.Trim(),
                            Value = attr["value"].InnerText.Trim()
                        });
                    }
                    catch { }
                }
            }
            return list;
            // return section;
        }

    }

#endif
    internal class KVEdm
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    internal class Log2NetConfig
    {
        internal static string GetConfigVal(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "";
            }
            try
            {
#if NET
                var values = System.Configuration.ConfigurationManager.GetSection(GetLog2netCfgSectionName()) as List<KVEdm>;
                return values.Where(a => string.Equals(a.Key, key.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().Value.Trim();
#else
                return AppConfig.GetConfigValue(key);
#endif
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal static List<KVEdm> GetSectionVal(string sectionKey)
        {
            List<KVEdm> kv = new List<KVEdm>();
            if (string.IsNullOrEmpty(sectionKey))
            {
                return kv;
            }
            try
            {
#if NET
                var values = System.Configuration.ConfigurationManager.GetSection(GetLog2netCfgSectionName()) as List<KVEdm>;
                string secFlag = sectionKey + ".";
                var secKVs = values.Where(a => a.Key.StartsWith(secFlag, StringComparison.OrdinalIgnoreCase)).Select(a => new KVEdm() { Key = a.Key.Substring(secFlag.Length), Value = a.Value }).ToList();
                return secKVs;
#else
                var dic = AppConfig.GetValue(sectionKey) as Dictionary<string, string>;
                kv = dic.Select(a => new KVEdm() { Key = a.Key, Value = a.Value }).ToList();
                return kv;
#endif
            }
            catch { return kv; }
        }



#if NET
        static string GetLog2netCfgSectionName()
        {
            string name = System.Configuration.ConfigurationManager.AppSettings["log2netCfgSectionName"];
            name = string.IsNullOrEmpty(name) ? "log2netCfg" : name;
            return name;
        }

#endif

    }
}
