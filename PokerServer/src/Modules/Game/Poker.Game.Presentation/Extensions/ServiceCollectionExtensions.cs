using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Game.Application.Extensions;
using Poker.Game.Presentation.Features.Game.Service;
using Poker.Game.Presentation.Features.Lobby.Service;

namespace Poker.Game.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameModule(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;

		serviceCollection
			.AddApplicationLayer(configuration);

		serviceCollection
			.AddTransient<IGameService, GameService>()
			.AddTransient<ILobbyService, LobbyService>();

		return serviceCollection;
	}
}
