using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Poker.Common.Domain.Responses;
using Poker.Common.Domain.Results;
using Poker.Common.Presentation.Abstractions;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Presentation.Features.Game.Service;
using Poker.Game.Presentation.Features.Lobby.Service;

namespace PokerServer.Hubs;

[Authorize]
public class LobbyHub : Hub<ILobbyClient>
{
    private readonly IClaimsExtractor _claimsExtractorService;
    private readonly IGameService _gameService;
    private readonly ILobbyService _lobbyService;

    public LobbyHub(ILobbyService lobbyService, IGameService gameService, IClaimsExtractor claimsExtractorService)
    {
        _lobbyService = lobbyService;
        _gameService = gameService;
        _claimsExtractorService = claimsExtractorService;
    }

    public async Task<Result<LobbyCommandViewModel>> CreateLobby(string lobbyName)
    {
        var playerId = _claimsExtractorService.GetUserId();
        if (string.IsNullOrWhiteSpace(playerId))
            return Result<LobbyCommandViewModel>.Failure(SharedResponses.InternalError);

        var result = await _lobbyService.CreateLobbyAsync(playerId, lobbyName, Context.ConnectionAborted);
        if (result.IsFailure)
            return Result<LobbyCommandViewModel>.Failure(result.Response);

        string lobbyId = result.Value!.Id;
        Context.Items["lobbyId"] = lobbyId;
        
        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId, Context.ConnectionAborted);
        return result;
    }

    public async Task<Result> JoinLobby(string lobbyId)
    {
        var playerId = _claimsExtractorService.GetUserId();
        if (string.IsNullOrWhiteSpace(playerId))
            return Result.Failure(SharedResponses.InternalError);

        var result = await _lobbyService.AddPlayerToLobbyAsync(lobbyId, playerId, Context.ConnectionAborted);
        if (result.IsFailure)
            return result;

        Context.Items["lobbyId"] = lobbyId;
        await Groups.AddToGroupAsync(Context.ConnectionId, lobbyId, Context.ConnectionAborted);
        return Result.Success();
    }

    public async Task<Result> LeaveLobby(string lobbyId)
    {
        var playerId = _claimsExtractorService.GetUserId();
        if (string.IsNullOrWhiteSpace(playerId))
            return Result.Failure(SharedResponses.InternalError);

        var result = await _lobbyService.RemovePlayerFromLobbyAsync(lobbyId, playerId, Context.ConnectionAborted);
        if (result.IsFailure)
            return result;

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, lobbyId, Context.ConnectionAborted);
        return Result.Success();
    }

    public async Task<Result> StartGame(string lobbyId)
    {
        var gameResult = await _gameService.StartGameAsync(lobbyId, Context.ConnectionAborted);
        if (gameResult.IsFailure)
            return Result.Failure(gameResult.Response);

        var tableId = gameResult.Value!.Id;

        await Clients.Group(lobbyId).GameStarted(tableId);
        return Result.Success();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpContext = Context.GetHttpContext();
        var lobbyId = httpContext?.Request.Query["lobbyId"].ToString();

        var playerId = _claimsExtractorService.GetUserId();
        if (!string.IsNullOrWhiteSpace(lobbyId) && !string.IsNullOrWhiteSpace(playerId))
        {
            await _lobbyService.RemovePlayerFromLobbyAsync(lobbyId, playerId, CancellationToken.None);
        }

        await base.OnDisconnectedAsync(exception);
    }
}