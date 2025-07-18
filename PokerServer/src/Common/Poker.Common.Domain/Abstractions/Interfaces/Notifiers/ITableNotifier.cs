using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Notifications;

namespace Poker.Common.Domain.Abstractions.Interfaces.Notifiers;

public interface ITableNotifier
{
    Task NotifyGameStartedAsync(string tableId, GameStateDto state);
    Task NotifyPlayerActionAsync(string tableId, string playerId, PlayerActionNotification action);
    Task NotifyGamePhaseUpdateAsync(string tableId, GamePhase gamePhase, List<CardDto> cards);
    Task NotifyShowdownAsync(string tableId, List<string> winnerPlayerIds, int winningsEach);
    Task NotifyNextPlayer(string playerId);
}
