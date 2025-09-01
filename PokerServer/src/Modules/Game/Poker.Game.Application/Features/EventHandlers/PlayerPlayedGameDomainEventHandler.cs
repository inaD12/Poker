using MediatR;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Game.Domain.Events;
using Poker.Users.Presentation.Features.Services;
using Serilog;

namespace Poker.Game.Application.Features.EventHandlers;

public sealed class PlayerPlayedGameDomainEventHandler : INotificationHandler<PlayerPlayedGameDomainEvent>
{
    private readonly IUserService  _userService;

    public PlayerPlayedGameDomainEventHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task Handle(PlayerPlayedGameDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await _userService.UserPlayedGame(notification.Id, notification.Won, notification.Earnings, cancellationToken);

        if (result.IsFailure)
            Log.Error("Failure in PlayerPlayedGameDomainEventHandler");
    }
}