using MediatR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Consumers;

public sealed class GamePhaseUpdatedDomainEventHandler : INotificationHandler<GamePhaseUpdatedDomainEvent>
{
    private readonly ITableNotifier _notifier;

    public GamePhaseUpdatedDomainEventHandler(ITableNotifier notifier)
    {
        _notifier = notifier;
    }
    
    public async Task Handle(GamePhaseUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyGamePhaseUpdateAsync(notification.TableId, notification.Phase, notification.Cards);
    }
}

