using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Abstractions;

namespace Poker.Common.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediatR(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddMediatR(cf =>
        {
            cf.RegisterServicesFromAssembly(assembly);

            //cf.AddOpenBehavior(typeof(ValidationPipelineBehavior<,>));
        });

        return serviceCollection;
    }
    
    public static IServiceCollection AddMapper(this IServiceCollection serviceCollection, Assembly assembly)
    {
        serviceCollection.AddAutoMapper(assembly);

        serviceCollection.AddScoped<IPokerMapper, PokerMapper>();

        return serviceCollection;
    }
}