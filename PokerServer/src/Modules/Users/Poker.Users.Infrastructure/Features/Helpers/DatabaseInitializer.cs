using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Users.Infrastructure.Features.DBContexts;

namespace Poker.Users.Infrastructure.Features.Helpers;

internal class DatabaseInitializer : IDatabaseInitializer
{
	public async Task ApplyMigrationsAsync(IServiceScope scope)
	{
		UsersDBContext dBContext =
		   scope.ServiceProvider.GetRequiredService<UsersDBContext>();

		var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

		if (pendingMigrations.Any())
			await dBContext.Database.MigrateAsync();
	}
}
