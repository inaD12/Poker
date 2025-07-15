using Poker.Common.Application.Abstractions;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Common.Infrastructure.Abstractions;
using Poker.Game.Application.Game.Models;
using Poker.Game.Domain.Entities;

namespace Poker.Game.Application.Game.Commands.GameStart;

public sealed class GameStartCommandHandler : ICommandHandler<GameStartCommand, GameCommandViewModel>
{
    private readonly IUserService _userService;
    private readonly IPokerMapper _pokerMapper;

    public GameStartCommandHandler(IUserService userService, IPokerMapper  pokerMapper)
    {
        _userService = userService;
        _pokerMapper = pokerMapper;
    }
    
    public async Task<Result<GameCommandViewModel>> Handle(GameStartCommand request, CancellationToken cancellationToken)
    {
        var usersResponse = await _userService.GetUserDataByIds(request.PlayerIds);
        if(usersResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(usersResponse.Response);
        
        var players = _pokerMapper.Map<List<Player>>(usersResponse.Value!);
        
        var gameResponse = Domain.Entities.Game.StartGame(players);
        if(gameResponse.IsFailure)
            return Result<GameCommandViewModel>.Failure(gameResponse.Response);
        var game = gameResponse.Value!;

        //TODO: Game caching and db saving(?)
        
        var gameViewModel = _pokerMapper.Map<GameCommandViewModel>(game.Id);
        return Result<GameCommandViewModel>.Success(gameViewModel);
    }
}