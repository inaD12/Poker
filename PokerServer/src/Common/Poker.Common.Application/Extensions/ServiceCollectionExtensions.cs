using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Abstractions;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Behaviours;
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
}