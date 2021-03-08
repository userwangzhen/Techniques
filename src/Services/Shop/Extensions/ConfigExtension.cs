using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WangZhen.Techniques.Shop.Api.Settings;

namespace WangZhen.Techniques.Shop.Api.Extensions
{
    public static class ConfigExtension
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<HostSetting>(configuration.GetSection(nameof(HostSetting)));
            services.Configure<DbConnectionSetting>(configuration.GetSection(nameof(DbConnectionSetting)));
            services.Configure<JwtSetting>(configuration.GetSection(nameof(JwtSetting)));
            return services;
        }
    }
}
