using MediatR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class NewHostDomainEventHandler : INotificationHandler<NewHostDomainEvent>
{
    private readonly ITableNotifier _notifier;

    public NewHostDomainEventHandler(ITableNotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(NewHostDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyNewHostAsync(notification.PlayerId);
    }
}