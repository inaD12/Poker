using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Common.Infrastructure.Abstractions.Interfaces;
using Poker.Game.Application.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Lobby.Commands.CreateLobby;

public sealed class CreateLobbyCommandHandler : ICommandHandler<CreateLobbyCommand, LobbyCommandViewModel>
{
    private readonly IPokerMapper _pokerMapper;
    private readonly ICacheService _cache;
    private readonly IUserService _userService;

    public CreateLobbyCommandHandler(IPokerMapper  pokerMapper, ICacheService cache, IUserService  userService)
    {
        _pokerMapper = pokerMapper;
        _cache = cache;
        _userService = userService;
    }
    
    public async Task<Result<LobbyCommandViewModel>> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.StartingPlayerId);
        if(userResponse.IsFailure)
            return Result<LobbyCommandViewModel>.Failure(userResponse.Response);
        
        var player = _pokerMapper.Map<Player>(userResponse.Value!);

        var lobbyResponse = Domain.Entities.Lobby.CreateLobby([player]);
        if(lobbyResponse.IsFailure)
            return Result<LobbyCommandViewModel>.Failure(lobbyResponse.Response);
        var lobby = lobbyResponse.Value!;

        _cache.Set(lobby.Id, lobby);
        
        var lobbyViewModel = _pokerMapper.Map<LobbyCommandViewModel>(lobby.Id);
        return Result<LobbyCommandViewModel>.Success(lobbyViewModel);
    }
}