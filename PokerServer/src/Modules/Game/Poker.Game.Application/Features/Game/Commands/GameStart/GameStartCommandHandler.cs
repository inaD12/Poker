using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Models;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.GameStart;

public sealed class GameStartCommandHandler : ICommandHandler<GameStartCommand, GameCommandViewModel>
{
    private readonly IPokerMapper _pokerMapper;
    private readonly ICacheService _cache;

    public GameStartCommandHandler(IPokerMapper  pokerMapper, ICacheService cache)
    {
        _pokerMapper = pokerMapper;
        _cache = cache;
    }
    
    public async Task<Result<GameCommandViewModel>> Handle(GameStartCommand request, CancellationToken cancellationToken)
    {
        var lobby = _cache.Get<Domain.Entities.Lobby>(request.LobbyId);
        if (lobby is null)
            return Result<GameCommandViewModel>.Failure(ResponseList.LobbyNotFound);

        var players = lobby.Players;
        
        var gameResponse = Table.StartGame(players);
        if(gameResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(gameResponse.Response);
        var game = gameResponse.Value!;

        _cache.Set(game.Id, game);
        
        //TODO: db saving(?)
        
        var gameViewModel = _pokerMapper.Map<GameCommandViewModel>(game.Id);
        return Result<GameCommandViewModel>.Success(gameViewModel);
    }
}