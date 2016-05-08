using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnetliberty.DependencyInjection.Demo.Factories;
using dotnetliberty.DependencyInjection.Demo.Models;
using dotnetliberty.DependencyInjection.Demo.Services;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace dotnetliberty.DependencyInjection.Demo
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        { 
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();
            services.Configure<WidgetClientSettings>(Configuration.GetSection("WidgetClient"));

            services.AddSingleton<WidgetValidator>();
            services.AddSingletonFactory<WidgetClient, WidgetClientFactory>();
            services.AddSingleton<WidgetService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();

            app.Run(async (context) =>
            {
                if (context.Request.Method == "DEBUG") return; // Ignore debug request to keep logs clean
                var widgetService = context.RequestServices.GetRequiredService<WidgetService>();
                widgetService.UpdateWidgetName(1, "New Widget Name");
                await context.Response.WriteAsync("Widget has been updated!");
            });
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
