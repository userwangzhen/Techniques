using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Errors.Middleware;
using Microsoft.AspNetCore.Builder;
using Ocelot.Responder.Middleware;
using Ocelot.DownstreamRouteFinder.Middleware;
using Ocelot.Request.Middleware;
using Ocelot.RequestId.Middleware;
using Ocelot.Headers.Middleware;
using Ocelot.LoadBalancer.Middleware;
using Ocelot.DownstreamUrlCreator.Middleware;
using Ocelot.Cache.Middleware;
using Ocelot.Requester.Middleware;

using Microsoft.Net.Http.Headers;

namespace Ocelot.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config
                        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddJsonFile("ocelot.json")
                        .AddEnvironmentVariables();
                })
               .ConfigureLogging((hostingContext, logging) =>
               {
                   //add your logging
                   //logging.AddConsole();
                   logging.AddConsole();
               })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
