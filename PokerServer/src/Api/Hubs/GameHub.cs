using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Responses;
using Poker.Common.Domain.Results;
using Poker.Common.Presentation.Abstractions;
using Poker.Game.Presentation.Features.Game.Service;
using Serilog;

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

        var tableResult = await _gameService.GetTableAsync(tableId, playerId, Context.ConnectionAborted);
        
        var gameState = tableResult.Value!;
        var player = gameState.Players.FirstOrDefault(p => p.Id == playerId);
        
        if (tableResult.IsFailure || player is null)
        {
            Context.Abort();
            return;
        }
        Context.Items["tableId"] = tableId;

        await Groups.AddToGroupAsync(Context.ConnectionId, tableId);

        if (player.isDisconnected)
        {
            var reconnectedResult = await _gameService.PlayerReconnectedAsync(tableId, playerId, Context.ConnectionAborted);

            if (reconnectedResult.IsFailure)
            {
                Log.Error("Reconnection Failure in GameHub");
                Context.Abort();
            }
        }
        
        await Clients.Caller.ReceiveGameState(gameState);
        
        await base.OnConnectedAsync();
    }

    public async Task<Result> PlaceBet(int amount)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerPlacedBetAsync(tableId, playerId, amount, Context.ConnectionAborted);
        
        return result;
    }

    public async Task<Result> Fold()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerFoldedAsync(tableId, playerId, Context.ConnectionAborted);

        return result;
    }

    public async Task<Result> AllIn()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerAllInAsync(tableId, playerId, Context.ConnectionAborted);
        
        return result;
    }

    public async Task<Result> Check()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerCheckedAsync(tableId, playerId, Context.ConnectionAborted);

        return result;
    }

    public async Task<Result> StartNextHand()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.StartNextHandAsync(tableId, playerId, Context.ConnectionAborted);
        
        return result;
    }
    
    public async Task<Result> CloseGame()
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is null || tableId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.GameCloseAsync(tableId, playerId, Context.ConnectionAborted);
        
        return result;
    }
    
    public async Task<Result> Disconnect(string tableId)
    {
        var playerId = _claimsExtractor.GetUserId();
        if (string.IsNullOrWhiteSpace(playerId))
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerDisconnectedAsync(tableId, playerId, Context.ConnectionAborted);
        if (result.IsFailure)
            return result;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, tableId, Context.ConnectionAborted);
        return Result.Success();
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var (playerId, tableId) = GetUserAndGameId();
        if (playerId is not null && tableId is not null)
            await _gameService.PlayerDisconnectedAsync(tableId, playerId, CancellationToken.None);

        await base.OnDisconnectedAsync(exception);
    }

    private (string? userId, string? gameId) GetUserAndGameId()
    {
        var userId = _claimsExtractor.GetUserId();
        //var gameId = Context.Items.TryGetValue("tableId", out var id) && id is string gid ? gid : null;
        var httpContext = Context.GetHttpContext();
        var gameId = httpContext?.Request.Query["tableId"].FirstOrDefault();
        return (userId, gameId);
    }
}