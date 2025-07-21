using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Game.Application.Features.Lobby.Commands.CreateLobby;

public sealed class CreateLobbyCommandHandler : ICommandHandler<CreateLobbyCommand, LobbyCommandViewModel>
{
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;
    private readonly IPokerMapper _pokerMapper;
    private readonly IUserService _userService;

    public CreateLobbyCommandHandler(IPokerMapper pokerMapper, IUserService userService,
        IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _pokerMapper = pokerMapper;
        _userService = userService;
        _lobbyStore = lobbyStore;
    }

    public async Task<Result<LobbyCommandViewModel>> Handle(CreateLobbyCommand request,
        CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.StartingPlayerId, cancellationToken);
        if (userResponse.IsFailure)
            return Result<LobbyCommandViewModel>.Failure(userResponse.Response);

        var player = _pokerMapper.Map<Player>(userResponse.Value!);

        var lobbyResponse = Domain.Entities.Lobby.CreateLobby([player]);
        if (lobbyResponse.IsFailure)
            return Result<LobbyCommandViewModel>.Failure(lobbyResponse.Response);
        var lobby = lobbyResponse.Value!;

        await _lobbyStore.SaveNewAsync(lobby, cancellationToken);

        var lobbyViewModel = _pokerMapper.Map<LobbyCommandViewModel>(lobby.Id);
        return Result<LobbyCommandViewModel>.Success(lobbyViewModel);
    }
}