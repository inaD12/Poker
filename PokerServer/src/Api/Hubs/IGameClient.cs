using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Notifications;

namespace PokerServer.Hubs;

public interface IGameClient
{
    Task GameInfo(GameStateDto state);
    Task PlayerAction(string playerId, PlayerActionNotification action);
    Task GamePhaseUpdate(GamePhase gamePhase, List<CardDto> cards);
    Task Showdown(List<string> winnerPlayerIds, int winningsEach);
    Task YourTurn();
}