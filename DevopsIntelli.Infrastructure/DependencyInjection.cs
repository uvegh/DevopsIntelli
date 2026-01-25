
using DevopsIntelli.Infrastructure.Caching;
using DevOpsIntelligence.Infrastructure.AI;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;


namespace DevopsIntelli.Infrastructure;

public static class DependencyInjection
{

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration )
    {

     
        services.AddSingleton<IDistributedLockFactoryLocal, DistributedLockFactory>();
        //rediscache service
        services.AddSingleton<ICacheService, RedisCacheService>();
        //redis connection service
        services.AddSingleton<RedisConnectionService>();
        AddHangfire(services, configuration);
        //distributed locking
        services.AddSingleton<IDistributedLockFactoryLocal, DistributedLockFactory>();
        
        return services;
    }

    public static void AddHangfire(IServiceCollection services, IConfiguration configuration)
    {

        services.AddHangfire(config => config
             .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
             .UseSimpleAssemblyNameTypeSerializer()
             .UseRecommendedSerializerSettings()
             .UsePostgreSqlStorage(options =>
                 options.UseNpgsqlConnection(
                     configuration.GetConnectionString("DefaultConnection")))
         );
    }
}

