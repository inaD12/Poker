using Microsoft.Extensions.DependencyInjection;

namespace Poker.Common.Domain.Abstractions.Interfaces;

public interface IDatabaseInitializer
{
	Task ApplyMigrationsAsync(IServiceScope scope);
}