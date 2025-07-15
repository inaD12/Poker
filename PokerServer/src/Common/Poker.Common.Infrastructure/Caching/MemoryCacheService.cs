using Microsoft.Extensions.Caching.Memory;
using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Common.Infrastructure.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task RemoveGameAsync(Guid gameId)
    {
        _memoryCache.Remove(gameId);
        return Task.CompletedTask;
    }

    public void Set(string key, object value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        options.SetAbsoluteExpiration(expiration ?? TimeSpan.FromMinutes(5));

        _memoryCache.Set(key, value);
    }

    public T? Get<T>(string key)
    {
        _memoryCache.TryGetValue(key, out T? value);
        return value;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}