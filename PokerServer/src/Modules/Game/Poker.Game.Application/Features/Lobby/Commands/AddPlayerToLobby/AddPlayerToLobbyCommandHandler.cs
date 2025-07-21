using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;

public sealed class AddPlayerToLobbyCommandHandler : ICommandHandler<AddPlayerToLobbyCommand>
{
    private readonly IPokerMapper _pokerMapper;
    private readonly IUserService _userService;
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;

    public AddPlayerToLobbyCommandHandler(IPokerMapper  pokerMapper, IUserService  userService, IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _pokerMapper = pokerMapper;
        _userService = userService;
        _lobbyStore = lobbyStore;
    }
    
    public async Task<Result> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.PlayerId, cancellationToken);
        if(userResponse.IsFailure)
            return Result.Failure(userResponse.Response);
        
        var player = _pokerMapper.Map<Player>(userResponse.Value!);

        var lobby = await _lobbyStore.GetAsync(request.LobbyId, cancellationToken);
        if (lobby is null)
            return Result.Failure(ResponseList.LobbyNotFound);
        
        var addPlayerResult = lobby.AddPlayer(player);
        if(addPlayerResult.IsFailure)
            return addPlayerResult;
        
        await _lobbyStore.SaveAsync(lobby, cancellationToken);
        return Result.Success();
    }
}