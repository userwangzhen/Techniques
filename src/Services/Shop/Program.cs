using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using WangZhen.Techniques.Shop.Api.Extensions;
using WangZhen.Techniques.Shop.Api.Infrastructure;
using Microsoft.Extensions.Logging;

namespace WangZhen.Techniques.Shop.Api
{
    public class Program
    {
        public static readonly string Namespace = typeof(Program).Namespace;
        public static readonly string AppName = Namespace.Substring(Namespace.LastIndexOf('.', Namespace.LastIndexOf('.') - 1) + 1);
        public static void Main(string[] args)
        {

            var configuration = GetConfiguration();

            Log.Logger = CreateSerilogLogger(configuration);

            try
            {

                Log.Information("Configuring web host ({ApplicationContext})...", AppName);

                var host = CreateHostBuilder(configuration, args).Build();

                Log.Information("Applying migrations ({ApplicationContext})...", AppName);

                host.MigrateDbContext<ShopDbContext>((context, services) =>
                {
                    var env = services.GetService<IWebHostEnvironment>();

                    var logger = services.GetService<ILogger<ShopDbContextSeed>>();

                    new ShopDbContextSeed().SeedAsync(context, env, logger).Wait();

                });

                Log.Information("Starting web host ({ApplicationContext})...", AppName);
                host.Run();
                //CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", AppName);
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }


        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext", AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }

        public static IHostBuilder CreateHostBuilder(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .UseSerilog()
            .ConfigureLogging((context, logger) =>
            {
                logger.AddConsole();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                var ip = configuration["HostSetting:IPAddress"];
                var prot = configuration["HostSetting:Port"];

                webBuilder.UseUrls($"http://{ip}:{prot}");
                webBuilder.UseStartup<Startup>();
            });
    }
}
