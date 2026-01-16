using System;
using System.Collections.Generic;
using System.Text;

namespace DevopsIntelli.Application.common.Interface;



/// <summary>
/// Cache service for distributed caching with Redis
/// WHY: Abstraction allows us to swap cache implementations (Redis, in-memory, etc.)
/// INTERVIEW: "I use distributed caching to reduce database load and improve response times"
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Get cached value by key
    /// </summary>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Set cached value with expiration
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken cancellationToken = default) where T : class;

    /// <summary>
    /// Remove cached value by key
    /// </summary>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if key exists in cache
    /// </summary>
    Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
}
