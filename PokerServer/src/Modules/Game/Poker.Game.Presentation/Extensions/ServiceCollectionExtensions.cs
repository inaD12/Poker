using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Extensions;
using Poker.Common.Presentation.Endpoints;
using Poker.Game.Application.Extensions;
using Poker.Game.Infrastructure.Extensions;
using Poker.Game.Presentation.Features.Game.Service;
using Poker.Game.Presentation.Features.Lobby.Service;

namespace Poker.Game.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameModule(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;
        
        serviceCollection
            .AddApplicationLayer(configuration)
            .AddInfrastructureLayer(configuration);

        serviceCollection
            .AddMapper(currentAssembly)
            .AddEndpoints(currentAssembly);
        
        serviceCollection
            .AddTransient<IGameService, GameService>()
            .AddTransient<ILobbyService, LobbyService>();

        return serviceCollection;
    }
}