using Microsoft.Extensions.Caching.Hybrid;
using Restaurant.Application.Common.Interfaces;
using Restaurant.Application.Common.Interfaces.Services;

namespace Restaurant.Infrastructure.Services;

public sealed class HybridCacheService(HybridCache cache) : ICacheService
{
    private readonly HybridCache _cache = cache;

    public async Task<T?> GetAsync<T>(
        string key,
        CancellationToken cancellationToken = default)
    {
        return await _cache.GetOrCreateAsync<T?>(
            key,
            _ => ValueTask.FromResult<T?>(default),
            cancellationToken: cancellationToken);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan expiration,
        string[]? tags = null,
        CancellationToken ct = default) => 
    
        await _cache.SetAsync(key,
            value,
            new HybridCacheEntryOptions
            {
                Expiration = expiration
            },
            tags,
            ct);

    public async Task RemoveAsync(
        string key,
        CancellationToken ct = default) => await _cache.RemoveAsync(key, ct);

    public async Task RemoveByTagAsync(
        string tag,
        CancellationToken ct = default) => await _cache.RemoveByTagAsync(tag, ct);
}