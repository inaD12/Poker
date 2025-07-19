using Poker.Common.Domain.Abstractions.Interfaces;
using Serilog;

namespace PokerServer.Extensions;

public static class HostExtensions
{
	public static async Task SetUpDatabaseAsync(this IHost host)
	{
		using var scope = host.Services.CreateScope();

		var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();

		await databaseInitializer.ApplyMigrationsAsync(scope);
	}
	
	public static void ConfigureSerilog(this IHostBuilder hostBuilder)
	{
		hostBuilder.UseSerilog((context, configuration) =>
			configuration
				.ReadFrom.Configuration(context.Configuration)
				.Enrich.FromLogContext()
		);
	}
}
