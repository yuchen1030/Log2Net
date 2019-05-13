#if NET
#else
using Log2Net.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static Log2Net.LogInfo.VisitOnline;

namespace Log2Net.DNCMiddleware
{


    internal static class HttpContext
    {
        public class SessionEdm
        {
            public string Key { get; set; }
            public string Val { get; set; }
            public DateTime ExpiresAtTime { get; set; }
        }


        public static Microsoft.AspNetCore.Http.HttpContext Current => _accessor.HttpContext;

        static ConcurrentDictionary<string, SessionEdm> sessionMaps = new ConcurrentDictionary<string, SessionEdm>();

        static double dncSessionMins = AppConfig.GetDncSessionTimeoutMins();

        private static IHttpContextAccessor _accessor;
        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static VOEdm GetOnlineVisitNum(int preVisitNum)
        {          
            if (_accessor.HttpContext != null)
            {
                var curSession = _accessor.HttpContext.Session;
                SessionEdm sessionEdm = new SessionEdm() { Key = curSession.Id, Val = "1", ExpiresAtTime = DateTime.Now.AddMinutes(dncSessionMins) };
                sessionMaps.TryAdd(curSession.Id, sessionEdm);
            }
            int visitorsNum = sessionMaps.Count;
            VOEdm vOEdm = new VOEdm() { VisitNum = preVisitNum + visitorsNum };
            //将过期session的值变为0，未过期的session的数量为在线人数
            var keys = sessionMaps.Keys.ToArray();
            for (int i = 0; i < sessionMaps.Count; i++)
            {
                var cur = sessionMaps[keys[i]];
                if (cur.Val == "1" && cur.ExpiresAtTime <= DateTime.Now) //已过期
                {
                    cur.Val = "0";
                }
            }
            var onlineNums = sessionMaps.Where(a => a.Value.Val == "1").Count();
            vOEdm.OnlineNum = onlineNums;
            return vOEdm;
        }

    }

    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            HttpContext.Configure(httpContextAccessor);
            return app;
        }
    }


}
#endif
