using Poker.Common.Domain.Results;
using Poker.Game.Application.Game.Models;

namespace Poker.Game.Presentation.Game.Service;

public interface IGameService
{
    Task<Result<GameCommandViewModel>> StartGameAsync(string lobbyId, CancellationToken cancellationToken);
    Task<Result> PlayerAllInAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerCheckedAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerFoldedAsync(string tableId, string playerId, CancellationToken cancellationToken);
    Task<Result> PlayerPlacedBetAsync(string tableId, string playerId, int amount, CancellationToken cancellationToken);
}