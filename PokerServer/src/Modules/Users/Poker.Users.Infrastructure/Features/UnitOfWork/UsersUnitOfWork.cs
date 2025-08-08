using MediatR;
using Microsoft.EntityFrameworkCore;
using Poker.Common.Domain;
using Poker.Common.Domain.Exceptions;
using Poker.Users.Domain.Abstractions;
using Poker.Users.Infrastructure.Features.DBContexts;

namespace Poker.Users.Infrastructure.Features.UnitOfWork;

internal class UsersUnitOfWork : IUsersUnitOfWork
{
    private readonly UsersDbContext _dbContext;
    private readonly IMediator _notificationPublisher;

    public UsersUnitOfWork(UsersDbContext dbContext, IMediator notificationPublisher)
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
