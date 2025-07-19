using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Responses;
using Poker.Common.Domain.Results;
using Poker.Game.Presentation.Features.Game.Service;
using Poker.Game.Presentation.Features.Lobby.Service;

namespace PokerServer.Hubs;

//[Authorize]
public class LobbyHub : Hub<ILobbyClient>
{
    private readonly ILobbyService _lobbyService;
    private readonly IGameService _gameService;

    public LobbyHub(ILobbyService  lobbyService, IGameService  gameService)
    {
        _lobbyService = lobbyService;
        _gameService = gameService;
    }
    public async Task<Result> JoinLobby(string lobbyId, CancellationToken cancellationToken)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Failure(SharedResponses.InternalError);

        var result = await _lobbyService.AddPlayerToLobbyAsync(lobbyId, userId, cancellationToken);
        if (result.IsFailure)
            return result;

        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId, cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result> LeaveLobby(string lobbyId, CancellationToken cancellationToken)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userId))
            return Result.Failure(SharedResponses.InternalError);

        var result = await _lobbyService.RemovePlayerFromLobbyAsync(lobbyId, userId, cancellationToken);
        if (result.IsFailure)
            return result;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId, cancellationToken);
        return Result.Success();
    }
    
    public async Task<Result> StartGame(string lobbyId, CancellationToken cancellationToken)
    {
        var gameResult = await _gameService.StartGameAsync(lobbyId, cancellationToken);
        if (gameResult.IsFailure)
            return Result.Failure(gameResult.Response);

        var gameId = gameResult.Value!.Id;

        await Clients.Group(lobbyId).GameStarted(gameId);
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
}