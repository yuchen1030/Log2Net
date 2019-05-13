
using Log2Net.Config;
using Log2Net.Models;
using Log2Net.Util;
using Log2Net.Util.DBUtil;
using Log2Net.Util.DBUtil.Models;
using System;
using System.Linq;

#if NET
using System.Web;
#else
using Log2Net.DNCMiddleware;
#endif

namespace Log2Net.LogInfo
{
    //在线人数和访客统计
    internal class VisitOnline
    {
        //获取访问人数统计的实例（简单工厂）
        public class VisitCountFactory
        {
            public static IVisitCount GetInstance()
            {
                //简单工厂使用方法如下：
                string type = Log2NetConfig.GetConfigVal("VisitCountStrategy");  //获取类型：默认为0缓存，为1则使用Application

#if NET
                if (type == "1")
                {
                    return new ApplicationVisitCount();
                }
                else
                {
                    return new CacheVisitCount();
                }
#else
                return new CacheVisitCount();
#endif




            }

        }

#if NET


        //使用 Application 实现的 在线人数和访客统计类，要事先把他们记录到 到 Application 的 OnLineUserCnt 和 AllVisitorCnt 中。此方法对性能有影响，建议以后使用缓存等实现
        public class ApplicationVisitCount : IVisitCount
        {
            public static object ApplicationObj = null;
            HttpApplicationState _Application;
            public ApplicationVisitCount()
            {
                try
                {
                    _Application = (HttpApplicationState)ApplicationObj;
                }
                catch (Exception ex)
                {
                    LogCom.WriteExceptToFile(ex, "ApplicationVisitCount构造函数");
                }

            }

            public override int GetSetApplicationValue(AppKey key, VisitOnline.VOAction act, int value = 0)
            {
                try
                {
                    if (_Application == null)
                    {
                        return 0;
                    }
                    string keyName = key.ToString();
                    switch (act)
                    {
                        case VisitOnline.VOAction.Get: return Convert.ToInt32(_Application[keyName]);
                        case VisitOnline.VOAction.Set: _Application[keyName] = value; return value;
                        case VisitOnline.VOAction.Add1: value = GetSetApplicationValue(key, VisitOnline.VOAction.Get) + 1; _Application[keyName] = value; return value;
                        case VisitOnline.VOAction.Sub1: value = GetSetApplicationValue(key, VisitOnline.VOAction.Get) - 1; value = value <= 0 ? 1 : value; _Application[keyName] = value; return value;
                        default: return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }

        }



#else
        //netCore下不实现 ApplicationVisitCount
#endif


        //用缓存实现的 在线人数和访客统计类
        public class CacheVisitCount : IVisitCount
        {
            static int initVisitNum = 0;
            public CacheVisitCount()
            {
                if (initVisitNum <= 0)
                {
                    initVisitNum = GetAllVisitNumWhenInit();
                }
            }
            static ICache dataCache = CacheFac.CacheFactory();

            public override int GetSetApplicationValue(AppKey key, VisitOnline.VOAction act, int value = 0)
            {
                try
                {
                    string keyName = key.ToString();
                    switch (act)
                    {
#if NET                        
                        case VisitOnline.VOAction.Get: return Convert.ToInt32(dataCache.GetCache(keyName));
#else
                        case VisitOnline.VOAction.Get:
                            var num = HttpContext.GetOnlineVisitNum(initVisitNum);
                            if (key == AppKey.AllVisitorCnt)
                            {
                                return num.VisitNum;
                            }
                            else
                            {
                                return num.OnlineNum;
                            }
#endif
                        case VisitOnline.VOAction.Set: dataCache.SetCache(keyName, value, expireType: Expire.Month); return value;
                        case VisitOnline.VOAction.Add1: value = GetSetApplicationValue(key, VisitOnline.VOAction.Get) + 1; dataCache.SetCache(keyName, value, expireType: Expire.Month); return value;
                        case VisitOnline.VOAction.Sub1: value = GetSetApplicationValue(key, VisitOnline.VOAction.Get) - 1; value = value <= 0 ? 1 : value; dataCache.SetCache(keyName, value, expireType: Expire.Month); return value;
                        default: return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }

        }

        //在线人数和访客统计抽象类
        public abstract class IVisitCount
        {
            public int GetOnlineNum()//获取在线人数
            {
                //int num = System.Web.HttpContext.Current.Session.Count;              
                return GetSetApplicationValue(AppKey.OnLineUserCnt, VisitOnline.VOAction.Get);
            }

            public int GetCurVisitorNum()//获取历史访客
            {
                return GetSetApplicationValue(AppKey.AllVisitorCnt, VisitOnline.VOAction.Get);
            }

            public abstract int GetSetApplicationValue(AppKey key, VisitOnline.VOAction act, int value = 0);//获取或设置某个AppKey值

            public void SetVisitNumWhenInit()
            {
                int dbVisitor = GetAllVisitNumWhenInit();
                int onLineNum = dbVisitor > 0 ? 1 : 0;
                GetSetApplicationValue(AppKey.OnLineUserCnt, VisitOnline.VOAction.Set, onLineNum);
                GetSetApplicationValue(AppKey.AllVisitorCnt, VisitOnline.VOAction.Set, dbVisitor);
            }


            public static int GetAllVisitNumWhenInit()
            {
                int sysID = (int)LogCom.GetSystemID();
                string serverIP = ClientServerInfo.ClientInfo.GetServerIPHost().IP;
                PageSerach<Log_SystemMonitor> baseSerach = new PageSerach<Log_SystemMonitor>()
                {
                    Filter = a => (int)a.SystemID == sysID && a.ServerIP == serverIP && a.AllVisitors > 0,
                    OrderBy = a => a.OrderByDescending(m => m.LogTime),
                    PageIndex = 1,
                    PageSize = 1
                };
                var dbAccessFac = new Log_SystemMonitorDBAccessFac().DBAccessFactory();
                var res = dbAccessFac.GetAll(baseSerach);
                if (res.ErrCode != 0)
                {
                    LogCom.WriteExceptToFile(res.ExBody, res.ErrMsg + "," + res.Module);
                    return 0;
                }
                var one = (res.ExeModel as IQueryable<Log_SystemMonitor>).FirstOrDefault();
                if (one != null)
                {
                    return one.AllVisitors;
                }
                return 0;
            }


        }

        public class VOEdm //Visit Online edm
        {
            public int VisitNum { get; set; }
            public int OnlineNum { get; set; }
        }

        public enum VOAction   //Visit Online  action
        {
            Get, //获取
            Set,//设置
            Add1,//加1
            Sub1//减1
        }


    }


}
