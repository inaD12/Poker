using Poker.Common.Domain.Dtos;

namespace PokerServer.Hubs;

public interface ILobbyClient
{
    Task GameStarted(string gameId);
    Task PlayerJoined(PlayerInfoDto player);
    Task PlayerLeft(string playerId);
    Task LobbyClosed();
}