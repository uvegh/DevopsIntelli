using DevopsIntelli.Application.common.Interface;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

namespace DevopsIntelli.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{

    private readonly IDatabase _database;
    private readonly ILogger<RedisCacheService> _logger;
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive=true,
        WriteIndented=false
    };

    public RedisCacheService(RedisConnectionService redisConnectionService, ILogger<RedisCacheService> logger)
    {
      _database=  redisConnectionService.Database;

    }
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        //var value = await _database.StringGetAsync(key);

        //return !value.IsNullOrEmpty;
      return  await _database.KeyExistsAsync(key);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var value = await _database.StringGetAsync(key);
            if (value.IsNullOrEmpty)
            {
                _logger.LogDebug("Cache key does not exist,{key}", key);

                return null;
            }
           
            return JsonSerializer.Deserialize<T>(value.ToString()!, _jsonOptions);

        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting value for {ex}", ex);
            return null;

        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _database.KeyDeleteAsync(key);
            _logger.LogDebug("remove cache key :{key}", key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to remove key: {key}", key);

        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
           var res=  JsonSerializer.Serialize<T>(value,_jsonOptions);
           await _database.StringSetAsync(key,res,expiration);
            _logger.LogDebug("key :{key} cached for  timespan:{expiration}", key, expiration);

        }catch(Exception err)
        {
            _logger.LogError(err, "failed to set value for {key}", key);
            
        }
    }
}
