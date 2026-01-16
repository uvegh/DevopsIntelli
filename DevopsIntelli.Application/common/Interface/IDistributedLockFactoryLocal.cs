

namespace DevopsIntelli.Application.common.Interface;

public  interface IDistributedLockFactoryLocal : IDisposable
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="expirtyTime"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<IDistributedLock> CreateLockAsync(string resource,
        TimeSpan expirtyTime,
        CancellationToken ct = default
        );

    
}

public interface IDistributedLock : IAsyncDisposable
{
    /// <summary>
    /// Gets a value indicating whether the resource has been successfully acquired.
    /// </summary>
    bool IsAcquired { get; }
}