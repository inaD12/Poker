using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Models;

namespace Poker.Game.Presentation.Features.Game.Service;

public interface IGameService
{
    Task<Result<GameCommandViewModel>> StartGameAsync(string lobbyId, CancellationToken cancellationToken);
    Task<Result> PlayerAllInAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerCheckedAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerFoldedAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerPlacedBetAsync(string tableId, string playerId, int amount, CancellationToken cancellationToken);
    Task<Result> StartNextHandAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result<GameStateDto>> GetTableAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> GameCloseAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerDisconnectedAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerReconnectedAsync(string tableId, string playerId, CancellationToken cancellationToken);
}