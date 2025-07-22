using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Responses;
using Poker.Common.Domain.Results;
using Poker.Common.Presentation.Abstractions;
using Poker.Game.Presentation.Features.Game.Service;

namespace PokerServer.Hubs;

[Authorize]
public class GameHub : Hub<IGameClient>
{
    private readonly IGameService _gameService;
    private readonly IClaimsExtractor _claimsExtractor;

    public GameHub(IGameService gameService, IClaimsExtractor claimsExtractor)
    {
        _gameService = gameService;
        _claimsExtractor = claimsExtractor;
    }

    public override async Task OnConnectedAsync()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
        {
            Context.Abort();
            return;
        }

        var result = await _gameService.GetTableAsync(tableId, playerId, CancellationToken.None);
        if (result.IsFailure || result.Value!.Players.All(p => p.Id != playerId))
        {
            Context.Abort();
            return;
        }
        Context.Items["tableId"] = tableId;

        await Groups.AddToGroupAsync(Context.ConnectionId, tableId);
        
        var gameState = result.Value!;
        await Clients.Caller.ReceiveGameState(gameState);
        
        await base.OnConnectedAsync();
    }

    public async Task<Result> PlaceBet(int amount, CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerPlacedBetAsync(tableId, playerId, amount, cancellationToken);
        
        return result;
    }

    public async Task<Result> Fold(CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerFoldedAsync(tableId, playerId, cancellationToken);

        return result;
    }

    public async Task<Result> AllIn(CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerAllInAsync(tableId, playerId, cancellationToken);
        
        return result;
    }

    public async Task<Result> Check(CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerCheckedAsync(tableId, playerId, cancellationToken);

        return result;
    }

    public async Task<Result> StartNextHand(CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.StartNextHandAsync(tableId, playerId, cancellationToken);
        
        return result;
    }
    
    public async Task<Result> CloseGame(CancellationToken cancellationToken)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.GameCloseAsync(tableId, playerId, cancellationToken);
        
        return result;
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is not null && tableId is not null)
           // await _gameService.PlayerDisconnectedAsync(lobbyId, playerId, CancellationToken.None);

        await base.OnDisconnectedAsync(exception);
    }

    private (string? userId, string? gameId) GetUserAndGameId()
    {
        var userId = _claimsExtractor.GetUserId();
        var gameId = Context.Items.TryGetValue("tableId", out var id) && id is string gid ? gid : null;
        return (userId, gameId);
    }
}