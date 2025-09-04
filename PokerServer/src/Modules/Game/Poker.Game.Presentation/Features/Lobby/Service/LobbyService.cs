using MediatR;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Lobby.Commands.AddFundsToPlayer;
using Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;
using Poker.Game.Application.Features.Lobby.Commands.CreateLobby;
using Poker.Game.Application.Features.Lobby.Commands.RemovePlayerFromLobby;
using Poker.Game.Application.Features.Lobby.Models;

namespace Poker.Game.Presentation.Features.Lobby.Service;

internal class LobbyService : ILobbyService
{
    private readonly ISender _sender;

    public LobbyService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<LobbyCommandViewModel>> CreateLobbyAsync(string startingPlayerId, string lobbyName, CancellationToken cancellationToken)
    {
        var command = new CreateLobbyCommand(startingPlayerId,  lobbyName);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result<LobbyViewModel>> AddPlayerToLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken)
    {
        var command = new AddPlayerToLobbyCommand(lobbyId, playerId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    public async Task<Result> RemovePlayerFromLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken)
    {
        var command = new RemovePlayerFromLobbyCommand(lobbyId, playerId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
    
    public async Task<Result> AddFundsToPlayer(string lobbyId, string playerId, int funds, CancellationToken cancellationToken)
    {
        var command = new AddFundsToPlayerCommand(playerId, lobbyId, funds);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
}