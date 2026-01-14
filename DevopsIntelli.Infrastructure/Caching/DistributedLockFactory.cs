

using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;

namespace DevopsIntelli.Infrastructure.Caching;

public class DistributedLockFactory:IDistributedLockFactory
{
    private readonly RedLockFactory _redlockFactory;
    public DistributedLockFactory(RedisConnectionService redisConnection)
    {
        //create multiplexer- mulitple parallel connections
        var multiplexer = new List<RedLockMultiplexer>
        {
            redisConnection.Connection

        };
        

    }
}
