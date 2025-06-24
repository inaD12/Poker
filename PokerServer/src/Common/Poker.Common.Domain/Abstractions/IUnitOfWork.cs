namespace Poker.Common.Domain.Abstractions;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}