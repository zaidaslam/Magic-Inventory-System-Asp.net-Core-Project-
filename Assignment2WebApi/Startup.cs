using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Assignment2WebApi.Data;
using Assignment2WebApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Assignment2WebApi
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
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("ApplicationDbContext")));

           // services.AddTransient<OrderHistory>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        //public static void Register(HttpConfiguration config)
        //{
        //    config.Routes.MapHttpRoute(
        //    name: "ApiById",
        //    routeTemplate: "api/{controller}/{id}",
        //    defaults: new { id = RouteParameter.Optional },
        //    constraints: new { id = @"^[0-9]+$" }
        //    );

        //    config.Routes.MapHttpRoute(
        //    name: "ApiByName",
        //    routeTemplate: "api/{controller}/{action}/{name}",
        //    defaults: null,
        //    constraints: new { name = @"^[a-z]+$" }
        //    );

        //    config.Routes.MapHttpRoute(
        //    name: "ApiByAction",
        //    routeTemplate: "api/{controller}/{action}",
        //    defaults: new { action = "Get" }
        //    );
        //}
    }
}
