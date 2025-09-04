using MediatR;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Events;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class PlayerJoinedLobbyDomainEventHandler : INotificationHandler<PlayerJoinedLobbyDomainEvent>
{
    private readonly ILobbyNotifier _notifier;
    private readonly IPokerMapper _mapper;

    public PlayerJoinedLobbyDomainEventHandler(ILobbyNotifier notifier, IPokerMapper mapper)
    {
        _notifier = notifier;
        _mapper = mapper;
    }

    public async Task Handle(PlayerJoinedLobbyDomainEvent notification, CancellationToken cancellationToken)
    {
        var playerInfoDto = _mapper.Map<PlayerInfoDto>(notification.Player);
        
        await _notifier.NotifyPlayerJoinedAsync(notification.TableId, playerInfoDto);
    }
}