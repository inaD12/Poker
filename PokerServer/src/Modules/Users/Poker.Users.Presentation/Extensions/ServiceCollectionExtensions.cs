using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Application.Extensions;
using Poker.Common.Presentation.Endpoints;
using Poker.Users.Application.Extensions;
using Poker.Users.Infrastructure.Extensions;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Users.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddUsersModule(this IServiceCollection serviceCollection, IConfiguration configuration)
	{
		var currentAssembly = typeof(ServiceCollectionExtensions).Assembly;
		
		serviceCollection
			.AddApplicationLayer(configuration)
			.AddInfrastructureLayer(configuration);

		serviceCollection
			.AddMapper(currentAssembly)
			.AddEndpoints(currentAssembly);
			
		serviceCollection
			.AddTransient<IUserService, UserService>();

		return serviceCollection;
	}
}
