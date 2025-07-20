using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Extensions;

namespace Poker.Game.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

        serviceCollection
            .AddMediatR(currentAssembly)
            .AddMapper(currentAssembly)
            .AddValidatorsFromAssembly(currentAssembly)
            .AddCaching();

        return serviceCollection;
    }
}