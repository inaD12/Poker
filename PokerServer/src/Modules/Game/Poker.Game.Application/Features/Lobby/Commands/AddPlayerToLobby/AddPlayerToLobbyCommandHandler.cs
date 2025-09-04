using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Game.Application.Features.Lobby.Commands.AddPlayerToLobby;

public sealed class AddPlayerToLobbyCommandHandler : ICommandHandler<AddPlayerToLobbyCommand, LobbyViewModel>
{
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;
    private readonly IPokerMapper _pokerMapper;
    private readonly IUserService _userService;

    public AddPlayerToLobbyCommandHandler(IPokerMapper pokerMapper, IUserService userService,
        IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _pokerMapper = pokerMapper;
        _userService = userService;
        _lobbyStore = lobbyStore;
    }

    public async Task<Result<LobbyViewModel>> Handle(AddPlayerToLobbyCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.PlayerId, cancellationToken);
        if (userResponse.IsFailure)
            return Result<LobbyViewModel>.Failure(userResponse.Response);

        var player = _pokerMapper.Map<Player>(userResponse.Value!);

        var lobby = await _lobbyStore.GetAsync(request.LobbyId, cancellationToken);
        if (lobby is null)
            return Result<LobbyViewModel>.Failure(ResponseList.LobbyNotFound);

        var addPlayerResult = lobby.AddPlayer(player);
        if (addPlayerResult.IsFailure)
        {
            if (addPlayerResult.Response!.Message != ResponseList.PlayerAlreadyInTheLobby.Message)
            {
                return Result<LobbyViewModel>.Failure(addPlayerResult.Response!);
            }
        }
        else
        {
            try
            {
                await _lobbyStore.SaveAsync(lobby, cancellationToken);
            }
            catch
            {
                _lobbyStore.DeleteFromCacheAsync(request.LobbyId);
                return Result<LobbyViewModel>.Failure(ResponseList.PlayerAlreadyIsInAnotherLobby);
            }
        }

        var lobbyViewModel = _pokerMapper.Map<LobbyViewModel>(lobby);
        lobbyViewModel.Players.Find(p => p.Id == player.Id)!.IsSelf = true;
        return Result<LobbyViewModel>.Success(lobbyViewModel);
    }
}