using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace orders.shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddShared(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AllSettings>(configuration.GetSection("MySettings"));
            services.AddSingleton<IAllSettingsWrapper, AllSettingsWrapper>();

            services.Configure<DbSettings>(configuration.GetSection("MySettings:DbSettings"));
            services.AddSingleton<IDbSettingsWrapper, DbSettingsWrapper>();

            services.Configure<AppSettings>(configuration.GetSection("MySettings:AppSettings"));
            services.AddSingleton<IAppSettingsWrapper, AppSettingsWrapper>();

            return services;
        }
    }
}
