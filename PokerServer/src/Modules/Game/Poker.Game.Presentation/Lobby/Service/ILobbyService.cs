using Poker.Common.Domain.Results;
using Poker.Game.Application.Lobby.Models;

namespace Poker.Game.Presentation.Lobby.Service;

public interface ILobbyService
{
    Task<Result<LobbyCommandViewModel>> CreateLobbyAsync(string startingPlayerId, CancellationToken cancellationToken);
    Task<Result> AddPlayerToLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken);
    Task<Result> RemovePlayerFromLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken);
}