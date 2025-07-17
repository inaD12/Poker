using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Lobby.Commands.RemovePlayerFromLobby;

public sealed class RemovePlayerFromLobbyCommandHandler : ICommandHandler<RemovePlayerFromLobbyCommand>
{
    private readonly ICacheService _cache;

    public RemovePlayerFromLobbyCommandHandler(ICacheService cache)
    {
        _cache = cache;
    }
    
    public async Task<Result> Handle(RemovePlayerFromLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = _cache.Get<Domain.Entities.Lobby>(request.LobbyId);
        if (lobby is null)
            return Result.Failure(ResponseList.LobbyNotFound);
        
        var removePlayerResult = lobby.RemovePlayer(request.PlayerId);
        if(removePlayerResult.IsFailure)
            return removePlayerResult;
        
        _cache.Set(lobby.Id, lobby);
        return Result.Success();
    }
}