using MediatR;
using Microsoft.EntityFrameworkCore;
using Poker.Common.Domain;
using Poker.Common.Domain.Exceptions;
using Poker.Game.Domain.Abstractions.Interfaces;
using Poker.Game.Infrastructure.Features.DBContexts;

namespace Poker.Game.Infrastructure.Features.UnitOfWork;

internal class TablesUnitOfWork : ITablesUnitOfWork
{
    private readonly GameDbContext _dbContext;
    private readonly IMediator _notificationPublisher;

    public TablesUnitOfWork(GameDbContext dbContext, IMediator notificationPublisher)
    {
        _dbContext = dbContext;
        _notificationPublisher = notificationPublisher;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            await PublishDomainEventsAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occurred.", ex);
        }
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        var domainEvents = _dbContext.ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e =>
            {
                var events = e.GetDomainEvents();
                e.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
            await _notificationPublisher.Publish(domainEvent, cancellationToken);
    }
}
