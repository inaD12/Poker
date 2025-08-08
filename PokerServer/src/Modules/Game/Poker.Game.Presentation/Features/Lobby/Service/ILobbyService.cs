using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Presentation.Features.Lobby.Service;

public interface ILobbyService
{
    Task<Result<LobbyCommandViewModel>> CreateLobbyAsync(string startingPlayerId, string lobbyName, CancellationToken cancellationToken);
    Task<Result<LobbyViewModel>> AddPlayerToLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken);
    Task<Result> RemovePlayerFromLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken);
}