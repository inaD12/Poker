using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Enums;
using Poker.Common.Domain.Notifications;

namespace PokerServer.Hubs;

public interface IGameClient
{
    Task PlayerAction(string playerId, PlayerActionNotification action);
    Task GamePhaseUpdate(GamePhase gamePhase, List<CardDto> cards);
    Task ReceiveGameState(GameStateDto state);
    Task Showdown(List<string> winnerPlayerIds, int winningsEach, List<PlayerStateDto> playerStates);
    Task YourTurn();
    Task GameClose();
}