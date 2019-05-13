using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Log2Net;
using Log2Net.Util.DBUtil.EF2DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace Log2NetWeb
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
            LogApi.RegisterLogInitMsg(Log2Net.Models.SysCategory.SysI_01, null);
        }

        //应用停止事件
        void StopFunction()
        {
            LogApi.WriteServerStopLog();
        }


    }
}
