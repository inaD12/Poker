using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Responses;
using Poker.Common.Domain.Results;
using Poker.Game.Presentation.Game.Service;

namespace PokerServer.Hubs;

//[Authorize]
public class GameHub : Hub<IGameClient>
{
    private readonly IGameService _gameService;

    public GameHub(IGameService  gameService)
    {
        _gameService = gameService;
    }
    
    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        var gameId = httpContext?.Request.Query["gameId"].ToString();

        if (string.IsNullOrWhiteSpace(gameId))
        {
            Context.Abort();
            return;
        }
        //TODO: Check if player is in the game
        Context.Items["gameId"] = gameId;

        await Groups.AddToGroupAsync(Context.ConnectionId,gameId);
        await base.OnConnectedAsync();
    }

    public async Task<Result> PlaceBet(int amount, CancellationToken cancellationToken)
    {
        var (userId, gameId) = GetUserAndGameId();
        if (userId is null || gameId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerPlacedBetAsync(gameId, userId, amount, cancellationToken);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }
    
    public async Task<Result> Fold(CancellationToken cancellationToken)
    {
        var (userId, gameId) = GetUserAndGameId();
        if (userId is null || gameId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerFoldedAsync(gameId, userId, cancellationToken);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }
    
    public async Task<Result> AllIn(CancellationToken cancellationToken)
    {
        var (userId, gameId) = GetUserAndGameId();
        if (userId is null || gameId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerAllInAsync(gameId, userId, cancellationToken);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }
    
    public async Task<Result> Check(CancellationToken cancellationToken)
    {
        var (userId, gameId) = GetUserAndGameId();
        if (userId is null || gameId is null)
            return Result.Failure(SharedResponses.InternalError);

        var result = await _gameService.PlayerCheckedAsync(gameId, userId, cancellationToken);
        if (result.IsFailure)
            return result;

        return Result.Success();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var gameId = httpContext?.Request.Query["gameId"].ToString();

        if (!string.IsNullOrWhiteSpace(gameId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId,gameId);
        }

        await base.OnDisconnectedAsync(exception);
    }
    
    private (string? userId, string? gameId) GetUserAndGameId()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var gameId = Context.Items.TryGetValue("gameId", out var id) && id is string gid ? gid : null;
        return (userId, gameId);
    }
}