using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Responses;
using Poker.Users.Presentation.Features.Services;

namespace Poker.Game.Application.Features.Lobby.Commands.AddFundsToPlayer;

public sealed class AddFundsToPlayerCommandHandler : ICommandHandler<AddFundsToPlayerCommand>
{
    private readonly IUserService _userService;
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;

    public AddFundsToPlayerCommandHandler(IUserService userService, IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _userService = userService;
        _lobbyStore = lobbyStore;
    }

    public async Task<Result> Handle(AddFundsToPlayerCommand request, CancellationToken cancellationToken)
    {
        var userResponse = await _userService.GetUserDataById(request.PlayerId, cancellationToken);
        if (userResponse.IsFailure)
            return Result.Failure(userResponse.Response);

        var lobby = await _lobbyStore.GetAsync(request.LobbyId, cancellationToken);
        if (lobby is null)
            return Result.Failure(ResponseList.LobbyNotFound);

        var result = lobby.AddFundsToPlayer(request.PlayerId, request.Funds);
        if (result.IsFailure)
            return Result.Failure(result.Response!);

        await _lobbyStore.SaveAsync(lobby, cancellationToken);

        return Result.Success();
    }
}