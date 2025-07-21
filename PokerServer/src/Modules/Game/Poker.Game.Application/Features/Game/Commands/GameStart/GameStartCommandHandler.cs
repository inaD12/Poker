using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Models;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.GameStart;

internal sealed class GameStartCommandHandler : ICommandHandler<GameStartCommand, GameCommandViewModel>
{
    private readonly IEntityStore<Domain.Entities.Lobby> _lobbyStore;
    private readonly IPokerMapper _pokerMapper;
    private readonly IEntityStore<Table> _tableStore;

    public GameStartCommandHandler(IPokerMapper pokerMapper, IEntityStore<Table> tableStore,
        IEntityStore<Domain.Entities.Lobby> lobbyStore)
    {
        _pokerMapper = pokerMapper;
        _tableStore = tableStore;
        _lobbyStore = lobbyStore;
    }

    public async Task<Result<GameCommandViewModel>> Handle(GameStartCommand request,
        CancellationToken cancellationToken)
    {
        var lobby = await _lobbyStore.GetAsync(request.LobbyId, cancellationToken);
        if (lobby is null)
            return Result<GameCommandViewModel>.Failure(ResponseList.LobbyNotFound);

        var players = lobby.Players;

        var gameResponse = Table.StartGame(players);
        if (gameResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(gameResponse.Response);
        var game = gameResponse.Value!;

        await _tableStore.SaveNewAsync(game, cancellationToken);

        var gameViewModel = _pokerMapper.Map<GameCommandViewModel>(game.Id);
        return Result<GameCommandViewModel>.Success(gameViewModel);
    }
}