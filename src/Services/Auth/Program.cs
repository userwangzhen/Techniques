using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.IO;

namespace WangZhen.Techniques.Auth.Api
{
    public class Program
    {

        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {
            // CreateHostBuilder(args).Build().Run();

            var configuration = GetConfiguration();
            Serilog.Log.Logger = CreateLogger(configuration);
            try
            {
                Log.Debug($"Application {AppName} Starting...");

                var host = CreateWebHostBuilder(configuration,args).Build();

                host.Run();

                Log.Debug($"Application {AppName} Start Ending");
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message,ex);
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateWebHostBuilder(IConfiguration configuration, string[] args)
        {
            var Ihost = Host.CreateDefaultBuilder(args)
                            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
                            .UseSerilog()
                            .ConfigureWebHostDefaults(webBuilder =>
                            {
                                webBuilder.UseStartup<Startup>();
                                var ip = configuration["HostSetting:IPAddress"];
                                var port = configuration["HostSetting:Port"];

                                webBuilder.UseUrls($"http://{ip}:{port}");
                            });

            return Ihost;

        }
        public static Serilog.ILogger CreateLogger(IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                                          .MinimumLevel.Verbose()
                                          .Enrich.WithProperty("ApplicationContext", AppName)
                                          .Enrich.FromLogContext()
                                          .WriteTo.Console()
                                          .ReadFrom.Configuration(configuration);
            return loggerConfiguration.CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                   
                    webBuilder.UseUrls();
                    webBuilder.UseStartup<Startup>();

                }).ConfigureLogging((ctx, log) =>
                {
                    log.AddConsole();
                });


        private static IConfiguration GetConfiguration()
        {
            var builder =  new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json")
                               .AddEnvironmentVariables();
            return builder.Build();
               
        }

    }
}
