using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DevopsIntelli.Infrastructure.Caching;

public class RedisConnectionService
{
    private readonly Lazy<ConnectionMultiplexer> _connection;

    public RedisConnectionService( IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("Redis") ?? "localhost:6379";
        _connection = new Lazy<ConnectionMultiplexer>(() =>
        
            ConnectionMultiplexer.Connect(connString)
        );
    }
    public IDatabase Database => _connection.Value.GetDatabase();
}
