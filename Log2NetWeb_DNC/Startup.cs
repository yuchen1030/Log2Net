using Log2Net;
using Log2Net.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Log2NetWeb_DNC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LogApi.AddLog2netService(services, Configuration);
            services.AddSession();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            app = LogApi.AddLog2netConfigure(app, env);
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            appLifetime.ApplicationStarted.Register(() => StartFunction());
            appLifetime.ApplicationStopped.Register(() => StopFunction());
        }


        //应用启动事件
        void StartFunction()
        {
            LogApi.RegisterLogInitMsg(Log2Net.Models.SysCategory.SysI_01, null, GetUserConfigItemByCode(), GetLogWebApplicationsNameByCode());
        }

        //应用停止事件
        void StopFunction()
        {
            LogApi.WriteServerStopLog();
        }


        #region 用户对日志系统的代码配置
        Dictionary<SysCategory, string> GetLogWebApplicationsNameByCode()
        {
            Dictionary<SysCategory, string> kvInCode = new Dictionary<SysCategory, string>()
            {
                {   SysCategory.SysA_01,"淘宝网" }
            };
            return kvInCode;
        }

        UserCfg GetUserConfigItemByCode()
        {
            UserCfg cfg = new UserCfg()
            {
                LogLevel = LogLevel.Info,
                LogAppendType = LogAppendType.DB,
                LogMonitorIntervalMins = 10,
                IsWriteInfoToDebugFile = false,
                LogToFilePath = "App_Data/Log_Files",
                DBAccessType = DBAccessType.ADONET,
                TraceDataBaseType = DataBaseType.SqlServer,
                MonitorDataBaseType = DataBaseType.SqlServer,
                TraceDBConKey = "logTraceSqlStr",
                MonitorDBConKey = "logMonitorSqlStr",
                IsConnectStrInCode = false,
                IsInitTraceDBWhenOracle = false,
                IsInitMonitorDBWhenOracle = false,
                OracleDriverType = OracleDriverType.Oracle,
                RabbitMQServer = "localhost:5672;oawxAdmin1;admin123.123",
                IsWriteToInfluxDB = false,
                InfluxDBServer = "http://127.0.0.1:8086/;logAdmin;sa123.123",
                CacheType = CacheType.MSHttp,
                MemCacheServer = "127.0.0.1:11211;127.0.0.2:11211",
                RedisCacheServer = "127.0.0.1:6379;127.0.0.2:6379",
                TraceDBConStr = "data source=.;initial catalog=LogTraceW;user id=sa;password=sa123.123;multipleactiveresultsets=True;application name=EntityFramework",
                MonitorDBDBConStr = "data source=.;initial catalog=LogMonitorW;user id=sa;password=sa123.123;MultipleActiveResultSets=True;application name=EntityFramework",
            };
            return cfg;
        }
        #endregion 用户对日志系统的代码配置



    }
}
