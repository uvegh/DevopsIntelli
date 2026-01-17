



using DevopsIntelli.Application.common.Interface;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;


namespace DevopsIntelli.Infrastructure.Caching;
//using redlock to prevent multiple servers from processing the same task simultaneously
public class DistributedLockFactory:IDistributedLockFactoryLocal
{
    private readonly RedLockFactory _redlockFactory;
    public DistributedLockFactory(RedisConnectionService redisConnection)
    {
        //create multiplexer- mulitple parallel connections
        var multiplexers = new List<RedLockMultiplexer>
        {
            redisConnection.Connection

        };
        _redlockFactory = RedLockFactory.Create(multiplexers);
    }



    public async Task<IDistributedLock> CreateLockAsync(
        string resource, TimeSpan expiryTime, CancellationToken ct = default)
    {
        var redlock = await _redlockFactory.CreateLockAsync(resource, expiryTime,
            waitTime: TimeSpan.FromSeconds(10),//wait 10sec to acquire lock
            retryTime: TimeSpan.FromMilliseconds(100),// if failed retry every 100ms
            ct
            );
        return new DistributedLockWrapper(redlock);

    }

    //shuts down entire factory
    public void Dispose()
    {
        _redlockFactory.Dispose();
    }

    internal class DistributedLockWrapper : IDistributedLock
    {
        private readonly IRedLock _redLock;
        public DistributedLockWrapper( IRedLock redLock)
        {
            _redLock = redLock;
        }
        bool IsAcquired => _redLock.IsAcquired;

        bool IDistributedLock.IsAcquired => IsAcquired;

        public async ValueTask DisposeAsync()
        {
            //releases the lock in redis automatically, this is for cleanup-this dispose is for lock cleanup
          await  _redLock.DisposeAsync();
        }
    }
}
