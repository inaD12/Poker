using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Common.Infrastructure.Abstractions.Interfaces;
using Poker.Game.Application.Game.Models;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Application.Game.Commands.GameStart;

public sealed class GameStartCommandHandler : ICommandHandler<GameStartCommand, GameCommandViewModel>
{
    private readonly IUserService _userService;
    private readonly IPokerMapper _pokerMapper;
    private readonly ICacheService _cacheService;

    public GameStartCommandHandler(IUserService userService, IPokerMapper  pokerMapper, ICacheService cacheService)
    {
        _userService = userService;
        _pokerMapper = pokerMapper;
        _cacheService = cacheService;
    }
    
    public async Task<Result<GameCommandViewModel>> Handle(GameStartCommand request, CancellationToken cancellationToken)
    {
        var usersResponse = await _userService.GetUserDataByIds(request.PlayerIds);
        if(usersResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(usersResponse.Response);
        
        var players = _pokerMapper.Map<List<Player>>(usersResponse.Value!);
        
        var gameResponse = Table.StartGame(players);
        if(gameResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(gameResponse.Response);
        var game = gameResponse.Value!;

        _cacheService.Set(game.Id, game);
        
        //TODO: return GameStateDto to all players, db saving(?)
        
        var gameViewModel = _pokerMapper.Map<GameCommandViewModel>(game.Id);
        return Result<GameCommandViewModel>.Success(gameViewModel);
    }
}