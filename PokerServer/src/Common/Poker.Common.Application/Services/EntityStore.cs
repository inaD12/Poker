using Poker.Common.Domain;
using Poker.Common.Domain.Abstractions.Interfaces;

namespace Poker.Common.Application.Services;

internal class EntityStore<T> : IEntityStore<T> where T : Entity
{
    private readonly ICacheService _cache;
    private readonly string _cacheKeyPrefix;
    private readonly IRepository<T> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public EntityStore(IRepository<T> repository, ICacheService cache, IUnitOfWork unitOfWork, string cacheKeyPrefix)
    {
        _repository = repository;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _cacheKeyPrefix = cacheKeyPrefix;
    }

    public async Task<T?> GetAsync(string id, CancellationToken cancellationToken)
    {
        var cacheKey = $"{_cacheKeyPrefix}{id}";
        var cached = _cache.Get<T>(cacheKey);
        if (cached is not null)
            return cached;

        var entity = await _repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
            _cache.Set(cacheKey, entity);

        return entity;
    }

    public async Task SaveNewAsync(T entity, CancellationToken cancellationToken)
    {
        await _repository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
        _cache.Set(cacheKey, entity);
    }

    public async Task SaveAsync(T entity, CancellationToken cancellationToken = default)
    {
        _repository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        entity.ClearDomainEvents();
        var cacheKey = $"{_cacheKeyPrefix}{entity.Id}";
        _cache.Set(cacheKey, entity);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIdAsync(id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var cacheKey = $"{_cacheKeyPrefix}{id}";
        _cache.Remove(cacheKey);
    }
    
    public void DeleteFromCacheAsync(string id)
    {
        var cacheKey = $"{_cacheKeyPrefix}{id}";
        _cache.Remove(cacheKey);
    }
}