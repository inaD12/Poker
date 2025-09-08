using Poker.Common.Domain.Dtos;

namespace Poker.Common.Domain.Abstractions.Interfaces.Notifiers;

public interface ILobbyNotifier
{
    Task NotifyPlayerJoinedAsync(string lobbyId, PlayerInfoDto player);
    Task NotifyPlayerLeftAsync(string lobbyId, string playerId);
    Task NotifyLobbyClosedAsync(string lobbyId);
    Task NotifyGameStartingAsync(string lobbyId, string gameId);
}