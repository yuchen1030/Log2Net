using CacheManager.Core;
using System;
using System.Collections.Generic;
using Log2Net.Config;


#if NET
using System.Web;
using Enyim.Caching.Configuration;  //memcached缓存用
#else
//using CacheManager.Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Memory;
#endif

namespace Log2Net.Util
{

    //缓存简单工厂：0、NET缓存；1、CacheManager中的NET系统缓存；2、Memcached缓存；3、Redis缓存；默认为0
    //net下支持这四种配置，netcore下仅支持0和3。
    internal class CacheFac
    {
        public static ICache CacheFactory()
        {
            try
            {
                var type = AppConfig.GetFinalConfig("CacheStrategy", Models.CacheType.MSHttp, LogApi.GetCacheStrategy());//获取缓存类型
                if (type != Models.CacheType.MSHttp)
                {
                    return CMCacheHelper.Instance;  // CacheManager缓存
                }
                else
                {
                    return HttpCacheHelper.dataCache; //原始net缓存
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }


    //使用CacheManager 实现的缓存操作类
    internal class CMCacheHelper : BaseCache, ICache
    {
        static CMCacheHelper m_CacheHelper = null;
        static ICacheManager<object> manager = null;
        static string CacheStrategyInstance = "HttpCache";    //缓存策略    
        static readonly object locker = new object();

        //缓存对象实例
        public static CMCacheHelper Instance
        {
            get
            {
                if (m_CacheHelper == null)
                {
                    lock (locker)
                    {
                        if (m_CacheHelper == null)
                        {
                            m_CacheHelper = new CMCacheHelper();
                        }
                    }
                }

                if (manager == null)
                {
                    lock (locker)
                    {
                        if (manager == null)
                        {
                            bool bNeedHttpCache = false;
                            try
                            {
                                //1：NET系统缓存（CacheManager中）；2：Memcached缓存；3：Redis缓存
                                var type = AppConfig.GetFinalConfig("CacheStrategy",  Models.CacheType.CMHttp, LogApi.GetCacheStrategy());//获取缓存类型
                                Dictionary<string, int> serverList = GetServerPort(type ==  Models.CacheType.Redis);
                                if (serverList == null || serverList.Count <= 0)
                                {
                                    bNeedHttpCache = true;
                                }
                                else
                                {
                                    if (type ==  Models.CacheType.Memcached)
                                    {
#if NET

                                        #region 使用memcache缓存

                                        var cfg = new MemcachedClientConfiguration();
                                        foreach (var item in serverList)
                                        {
                                            cfg.AddServer(item.Key, item.Value);
                                        }

                                        manager = CacheFactory.Build("memcached", settings =>
                                            settings.WithSystemRuntimeCacheHandle("handleName")
                                            .And
                                            .WithMemcachedCacheHandle("default", cfg));  // configurationName  = default 或  enyim.com/memcached ,可省略
                                        CacheStrategyInstance = "Memcached";

                                        #endregion 使用memcache缓存
#endif
                                    }
                                    else if (type ==  Models.CacheType.Redis)
                                    {

                                        #region 使用Redis缓存
                                        manager = CacheFactory.Build("getStartedCache", settings =>
                                        {
                                            settings
#if NET

                                           .WithSystemRuntimeCacheHandle("handleName")
                                            //  .WithExpiration(ExpirationMode.Sliding, TimeSpan.FromSeconds(60))
                                            .And
#else
#endif

                                          .WithRedisConfiguration("redis", config => GetRedisConfig(config)
                                            //{
                                            //    config.WithAllowAdmin()
                                            //    .WithDatabase(0)
                                            //    .WithEndpoint("localhost", 6379)
                                            //        //.WithEndpoint("127.0.0.1", 6379)
                                            //        //.WithPassword("mysupersecret")
                                            //    ;
                                            //}


                                            )
                                            .WithMaxRetries(100)
                                            .WithRetryTimeout(50)
                                            .WithRedisBackplane("redis")
                                            .WithRedisCacheHandle("redis", true)
                                            ;
                                        });
                                        CacheStrategyInstance = "Redis";
                                        #endregion 使用Redis缓存

                                    }
                                    else
                                    {
                                        manager = GetHttpCacheManger();
                                    }
                                }

                            }
                            catch
                            {
                                bNeedHttpCache = true;
                            }

                            if (!bNeedHttpCache)  //确认缓存可用
                            {
                                var testKey = Guid.NewGuid().ToString();
                                var testValue = Guid.NewGuid().ToString();
                                manager.Add(testKey, testValue);

                                var storeValue = manager.Get(testKey);
                                if (storeValue as string != testValue)
                                {
                                    bNeedHttpCache = true;
                                }
                                manager.Remove(testKey);
                            }

                            if (bNeedHttpCache)
                            {
                                manager = GetHttpCacheManger();
                                CacheStrategyInstance = "HttpCache";
                            }

                        }
                    }
                }

                return m_CacheHelper;
            }
        }


        //从配置文件中读取cache服务器地址
        static Dictionary<string, int> GetServerPort(bool bRedis)
        {
            Dictionary<string, int> serverList = new Dictionary<string, int>();
            var serverStr = "";
            if (!bRedis)
            {
                serverStr = AppConfig.GetFinalConfig("MemCacheServer", "", LogApi.GetMemCacheServer());
            }
            else
            {
                serverStr = AppConfig.GetFinalConfig("RedisCacheServer", "", LogApi.GetRedisCacheServer());
            }

            if (string.IsNullOrEmpty(serverStr))
            {
                return serverList;
            }

            var oneServer = serverStr.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in oneServer)
            {
                var ipPort = item.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    serverList.Add(ipPort[0], Convert.ToInt32(ipPort[1]));
                }
                catch
                {

                }
            }
            return serverList;
        }



        static void GetRedisConfig(CacheManager.Redis.RedisConfigurationBuilder config)
        {
            Dictionary<string, int> serverList = GetServerPort(true);
            var redisBuild = config.WithAllowAdmin().WithDatabase(0);
            //.WithPassword("mysupersecret"); 
            foreach (var item in serverList)
            {
                redisBuild.WithEndpoint(item.Key, item.Value);
            }
            //.WithEndpoint("127.0.0.1", 6379)
        }


        //向缓存中写数据(键不存在则添加,键存在则覆盖)
        public void SetCache(string key, object value, int seconds = 0, Expire expireType = Expire.Defalut)
        {
            key = GetFinalKey(key);
            manager.Put(key, value);    //    manager.AddOrUpdate(key, value, v => value);    
            int sec = (seconds != 0 ? seconds : GetExpireTime(expireType));
            if (sec <= 0)
            {
                sec = (int)Expire.Defalut;
            }
            manager.Expire(key, TimeSpan.FromSeconds(sec));   //设置某个键的过期时间                                                                                                          
        }

        //获取缓存值
        public object GetCache(string key)
        {
            key = GetFinalKey(key);
            return manager.Get(key);
        }

        //移除键
        public bool RemoveCache(string key)
        {
            key = GetFinalKey(key);
            return manager.Remove(key);
        }

        //获取缓存策略
        public string GetCacheStrategyInstance()
        {
            return CacheStrategyInstance;
        }

    }


    // 缓存操作类（使用的是net本身缓存）
    internal class HttpCacheHelper : BaseCache, ICache
    {
        public static HttpCacheHelper dataCache = new HttpCacheHelper();

#if NET
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
#else
        static MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
#endif
        // 获取当前应用程序指定CacheKey的Cache值
        public object GetCache(string CacheKey)
        {
            CacheKey = GetFinalKey(CacheKey);
#if NET
            return objCache[CacheKey];
#else
            object val = null;
            if (cache.TryGetValue(CacheKey, out val))
            {
                return val;
            }
            else
            {
                return default(object);
            }
#endif

        }


        // 设置当前应用程序指定CacheKey的Cache值
        public void SetCache(string CacheKey, object objObject, int secondsSpan = 0, Expire expireType = Expire.Defalut)
        {
            if(objObject == null)
            {
                return;
            }
            CacheKey = GetFinalKey(CacheKey);
            int cueSeconds = secondsSpan > 0 ? secondsSpan : GetExpireTime(expireType);
            if (cueSeconds < 0)
            {
                RemoveCache(CacheKey);
            }
#if NET
            else if (cueSeconds > 0)
            {
                // objCache.Insert(CacheKey, objObject, null, DateTime.MaxValue, TimeSpan.FromSeconds(cueSeconds));//相对过期
                objCache.Insert(CacheKey, objObject, null, DateTime.Now.AddSeconds(cueSeconds), TimeSpan.Zero);//绝对过期
            }
            else if (cueSeconds == 0)
            {
                objCache.Insert(CacheKey, objObject);
            }
#else
            if (CacheKey != null)
            {
                secondsSpan = secondsSpan == 0 ? Int32.MaxValue : secondsSpan;
                cache.Set(CacheKey, objObject, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(secondsSpan)
                });
            }
#endif


        }


        //删除指定cache的值
        public bool RemoveCache(string CacheKey)
        {
            CacheKey = GetFinalKey(CacheKey);
#if NET
            return objCache.Remove(CacheKey) != null;
#else
            cache.Remove(CacheKey);
            return true;
#endif
        }


        public string GetCacheStrategyInstance()
        {
            return "NETCache";
        }

    }


    //缓存接口
    internal interface ICache
    {
        void SetCache(string key, object value, int seconds = 0, Expire expireType = Expire.Defalut);
        object GetCache(string key);
        bool RemoveCache(string key);
        string GetCacheStrategyInstance();
    }

    //缓存的公共方法
    internal abstract class BaseCache
    {
        public string DefaultCacheNamespace = "Cache";// "DefaultCache";//缓存键的第一级命名空间,后期需要考虑分布式的情况，后期需要储存在缓存中,或进行全局配置
        public static ICache cacheInstance = null;

        //获取过期时间
        internal int GetExpireTime(Expire expireType)
        {
            int second = (int)expireType;
            return second;
        }

        //获取内存缓存Manager
        internal static ICacheManager<object> GetHttpCacheManger()
        {
            return CacheFactory.Build("getStartedCache", settings =>
            {
#if NET
                settings.WithSystemRuntimeCacheHandle("handleName");
#else
                settings.WithMicrosoftMemoryCacheHandle("handleName");
#endif

            });
        }

        //获取缓存数据库中实际的键值
        internal string GetFinalKey(string key, bool isFullKey = false)
        {
            return isFullKey ? key : String.Format("Log2net.{0}.{1}", DefaultCacheNamespace, key);
        }


    }

    //过期时间枚举定义，注意各个数字不能相同
    internal enum Expire
    {
        Defalut = 45,//默认缓存过期时间 45秒
        Year50 = 1555200000,//3600*24*30*12*50(50年)，
        Year10 = 311040000,//3600*24*30*12*10(10年)，
        Year = 31104000,//3600*24*30*12(1年)，
        Month = 2592000,//3600*24*30(1月)
        Day = 86400,//3600*24秒（1天）
        Min30 = 1800,//1800秒   ，10分钟 
        Min20 = 1200,//1200秒   ，20分钟 
        Min10 = 600,//600秒   ，10分钟 
        Minute = 60,//60秒，1分钟
        Sec30 = 30, //30秒
        Sec20 = 20, //20秒
        Sec10 = 10,//10秒
        Sec5 = 5,//5秒
        ZERO = 0,//会报错，非法
        Below0 = -1,//负值
    }




}
