using MediatR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Application.Extensions;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class PlayerTookActionDomainEventHandler : INotificationHandler<PlayerTookActionDomainEvent>
{
    private readonly ITableNotifier _notifier;

    public PlayerTookActionDomainEventHandler(ITableNotifier notifier)
    {
        _notifier = notifier;
    }
    
    public async Task Handle(PlayerTookActionDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyPlayerActionAsync(notification.TableId, notification.PlayerId, notification.Action.ToNotification(notification.Amount));
        await _notifier.NotifyNextPlayer(notification.NextPlayerId);
    }
}

