using MediatR;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Lobby.Commands.AddPlayerToLobby;
using Poker.Game.Application.Lobby.Commands.CreateLobby;
using Poker.Game.Application.Lobby.Commands.RemovePlayerFromLobby;
using Poker.Game.Application.Lobby.Models;

namespace Poker.Game.Presentation.Lobby.Service;

internal class LobbyService : ILobbyService
{
    private readonly ISender _sender;

    public LobbyService(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Result<LobbyCommandViewModel>> CreateLobbyAsync(string startingPlayerId, CancellationToken cancellationToken)
    {
        var command = new CreateLobbyCommand(startingPlayerId);
        
        var result =  await _sender.Send(command, cancellationToken);
        
        return result;
    }
    
    public async Task<Result> AddPlayerToLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken)
    {
        var command = new AddPlayerToLobbyCommand(lobbyId, playerId);
        
        var result =  await _sender.Send(command, cancellationToken);
        
        return result;
    }
    
    public async Task<Result> RemovePlayerFromLobbyAsync(string lobbyId, string playerId, CancellationToken cancellationToken)
    {
        var command = new RemovePlayerFromLobbyCommand(lobbyId, playerId);
        
        var result =  await _sender.Send(command, cancellationToken);
        
        return result;
    }
}