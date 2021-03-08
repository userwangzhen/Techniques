using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Auth.Api.Settings;

namespace WangZhen.Techniques.Auth.Api.Extensions
{
    public static class ConsulExtension
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConsulSetting>(configuration.GetSection(nameof(ConsulSetting)));
            services.Configure<HostSetting>(configuration.GetSection(nameof(HostSetting)));
            services.AddSingleton<IConsulClient>(sp => new ConsulClient(config =>
            {
                
                var consulSetting = sp.GetRequiredService<IOptions<ConsulSetting>>().Value;

                if (consulSetting == null)
                {
                    throw new Exception("ConsulSetting Not Found");
                }

                config.Address = new Uri($"http://{consulSetting.IPAddress}:{consulSetting.Port}");

            })
            ); 
            return services;
        }



        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var client = app.ApplicationServices.GetRequiredService<IConsulClient>();
           
            //部署在docker  ip为当前docker主机ip，Port 为docker的映射端口
             var hostSetting = app.ApplicationServices.GetService<IOptions<HostSetting>>().Value;
            
                
         
            client.Agent.ServiceRegister(new AgentServiceRegistration() { 
                ID="Identity:"+hostSetting.IPAddress+":"+hostSetting.Port,
                Name="Identity",
                Address=hostSetting.IPAddress,
                Port=hostSetting.Port,
                Check=new AgentServiceCheck()
                {
                    Interval=TimeSpan.FromSeconds(10),
                    Timeout=TimeSpan.FromSeconds(5),
                    HTTP=$"http://{hostSetting.IPAddress}:{hostSetting.Port}/HealthCheck",
                    DeregisterCriticalServiceAfter=TimeSpan.FromSeconds(20)
                }
            });

            return app;
        }
    }
}
