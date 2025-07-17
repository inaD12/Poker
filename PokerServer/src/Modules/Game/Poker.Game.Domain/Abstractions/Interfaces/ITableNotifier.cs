using Poker.Game.Domain.DTOs;
using Poker.Game.Domain.Notifications;

namespace Poker.Game.Domain.Abstractions.Interfaces;

public interface ITableNotifier
{
    Task NotifyPlayerGameStartedAsync(string playerId, GameStateDto state);
    Task NotifyPlayerActionAsync(string playerId, PlayerActionNotification action);
}
