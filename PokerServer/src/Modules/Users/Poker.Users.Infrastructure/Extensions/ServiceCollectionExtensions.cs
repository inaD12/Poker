using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Infrastructure.Extensions;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Domain.Abstractions.Auth;
using Poker.Users.Infrastructure.Features.Auth;
using Poker.Users.Infrastructure.Features.DBContexts;
using Poker.Users.Infrastructure.Features.Helpers;
using Poker.Users.Infrastructure.Features.Repositories;

namespace Poker.Users.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddTransient<IPasswordManager, PasswordManager>()
			.AddTransient<ITokenFactory, TokenFactory>()
			.AddScoped<IUserRepository, UserRepository>()
			.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

		services
			.AddUnitOfWork<UsersDBContext>();


		return services;
	}
}
