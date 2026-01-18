using DevopsIntelli.Application.common.Interface;
using DevopsIntelli.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace DevopsIntelli.Infrastructure
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddSingleton<IDistributedLockFactoryLocal, DistributedLockFactory>();
            return services;
        }
    }
}
