using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Presentation.Extensions;
using PokerServer.Notifiers;

namespace PokerServer.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddApiLayer(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		serviceCollection
			.AddTransient<ILobbyNotifier, LobbyNotifier>()
			.AddTransient<ITableNotifier, TableNotifier>()
			.AddSignalR();

		serviceCollection
			.AddAuthentication(configuration)
			.AddSwagger()
			.ConfigureCors(configuration)
			.AddEndpointsApiExplorer()
			.AddHttpContextAccessor()
			.AddControllers();

		return serviceCollection;
	}
}
