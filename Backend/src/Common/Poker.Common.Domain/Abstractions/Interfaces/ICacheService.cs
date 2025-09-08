namespace Poker.Common.Domain.Abstractions.Interfaces;

public interface ICacheService
{
    void Set(string key, object value, TimeSpan? expiration = null);
    T? Get<T>(string key);
    void Remove(string key);
}