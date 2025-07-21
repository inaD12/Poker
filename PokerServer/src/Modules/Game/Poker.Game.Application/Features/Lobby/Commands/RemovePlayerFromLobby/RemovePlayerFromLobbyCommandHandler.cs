using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Lobby.Commands.RemovePlayerFromLobby;

public sealed class RemovePlayerFromLobbyCommandHandler : ICommandHandler<RemovePlayerFromLobbyCommand>
{
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;

    public RemovePlayerFromLobbyCommandHandler(IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _lobbyStore = lobbyStore;
    }
    
    public async Task<Result> Handle(RemovePlayerFromLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = await _lobbyStore.GetAsync(request.LobbyId, cancellationToken);
        if (lobby is null)
            return Result.Failure(ResponseList.LobbyNotFound);
        
        var removePlayerResult = lobby.RemovePlayer(request.PlayerId);
        if(removePlayerResult.IsFailure)
            return removePlayerResult;
        
        await _lobbyStore.SaveAsync(lobby, cancellationToken);
        return Result.Success();
    }
}