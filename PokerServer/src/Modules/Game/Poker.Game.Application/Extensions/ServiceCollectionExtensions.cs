using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Extensions;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

        serviceCollection
            .AddMediatR(currentAssembly)
            .AddMapper(currentAssembly)
            .AddCaching();

        serviceCollection
            .AddEntityStore<Table, ITableRepository>("table_")
            .AddEntityStore<Lobby, ILobbyRepository>("lobby_");

        return serviceCollection;
    }
}