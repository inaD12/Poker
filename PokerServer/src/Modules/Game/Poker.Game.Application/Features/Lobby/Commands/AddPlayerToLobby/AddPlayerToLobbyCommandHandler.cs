using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;

public sealed class AddPlayerToLobbyCommandHandler : ICommandHandler<AddPlayerToLobbyCommand>
{
    private readonly IPokerMapper _pokerMapper;
    private readonly ICacheService _cache;
    private readonly IUserService _userService;

    public AddPlayerToLobbyCommandHandler(IPokerMapper  pokerMapper, ICacheService cache, IUserService  userService)
    {
        _pokerMapper = pokerMapper;
        _cache = cache;
        _userService = userService;
    }
    
    public async Task<Result> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.PlayerId, cancellationToken);
        if(userResponse.IsFailure)
            return Result.Failure(userResponse.Response);
        
        var player = _pokerMapper.Map<Player>(userResponse.Value!);

        var lobby = _cache.Get<Domain.Entities.Lobby>(request.LobbyId);
        if (lobby is null)
            return Result.Failure(ResponseList.LobbyNotFound);
        
        var addPlayerResult = lobby.AddPlayer(player);
        if(addPlayerResult.IsFailure)
            return addPlayerResult;
        
        _cache.Set(lobby.Id, lobby);
        return Result.Success();
    }
}