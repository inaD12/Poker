using MediatR;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class PlayerLeftLobbyDomainEventHandler : INotificationHandler<PlayerLeftLobbyDomainEvent>
{
    private readonly ILobbyNotifier _notifier;

    public PlayerLeftLobbyDomainEventHandler(ILobbyNotifier notifier)
    {
        _notifier = notifier;
    }

    public async Task Handle(PlayerLeftLobbyDomainEvent notification, CancellationToken cancellationToken)
    {
        await _notifier.NotifyPlayerLeftAsync(notification.TableId, notification.PlayerId);
    }
}