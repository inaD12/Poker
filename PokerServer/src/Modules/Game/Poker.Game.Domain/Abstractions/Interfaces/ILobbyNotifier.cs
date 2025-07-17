using Poker.Game.Domain.DTOs;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Domain.Abstractions.Interfaces;

public interface ILobbyNotifier
{
    Task NotifyPlayerJoinedAsync(string lobbyId, Player player);
    Task NotifyPlayerLeftAsync(string lobbyId, string playerId);
    Task NotifyLobbyUpdatedAsync(string lobbyId, Lobby lobby);
    Task NotifyLobbyClosedAsync(string lobbyId);
    Task NotifyGameStartingAsync(string lobbyId, GameStateDto gameInfo);
}