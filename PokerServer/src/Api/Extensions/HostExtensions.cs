using Poker.Common.Domain.Abstractions.Interfaces;
using Serilog;

namespace PokerServer.Extensions;

public static class HostExtensions
{
	public static void ConfigureSerilog(this IHostBuilder hostBuilder)
	{
		hostBuilder.UseSerilog((context, configuration) =>
			configuration
				.ReadFrom.Configuration(context.Configuration)
				.Enrich.FromLogContext()
		);
	}
}
