using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Abstractions;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Behaviours;
using Poker.Common.Application.Services;
using Poker.Common.Domain;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Infrastructure.Caching;

namespace Poker.Common.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddMediatR(cf =>
        {
            cf.RegisterServicesFromAssembly(assembly);

            cf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        serviceCollection
            .AddValidatorsFromAssembly(assembly, includeInternalTypes: true);

        return serviceCollection;
    }

    public static IServiceCollection AddMapper(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddAutoMapper(assembly);

        serviceCollection.AddScoped<IPokerMapper, PokerMapper>();

        return serviceCollection;
    }

    public static IServiceCollection AddCaching(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMemoryCache();
        serviceCollection.AddScoped<ICacheService, MemoryCacheService>();

        return serviceCollection;
    }

    public static IServiceCollection AddEntityStore<T, TRepository>(
        this IServiceCollection services,
        string cacheKeyPrefix
    )
        where T : Entity
        where TRepository : class, IRepository<T>
    {
        services.AddScoped<IEntityStore<T>>(sp =>
        {
            var repository = sp.GetRequiredService<TRepository>();
            var cache = sp.GetRequiredService<ICacheService>();
            var unitOfWork = sp.GetRequiredService<IUnitOfWork>();

            return new EntityStore<T>(repository, cache, unitOfWork, cacheKeyPrefix);
        });

        return services;
    }
}