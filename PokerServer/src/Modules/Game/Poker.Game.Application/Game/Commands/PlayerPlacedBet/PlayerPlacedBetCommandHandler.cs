using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Game.Commands.PlayerPlacedBet;

public sealed class PlayerPlacedBetCommandHandler : ICommandHandler<PlayerPlacedBetCommand>
{
    private readonly ICacheService _cache;

    public PlayerPlacedBetCommandHandler(ICacheService  cache)
    {
        _cache = cache;
    }
    
    public async Task<Result> Handle(PlayerPlacedBetCommand request, CancellationToken cancellationToken)
    {
        var game = _cache.Get<Table>(request.TableId);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerPlaceBet(request.PlayerId, request.Amount);
        if (result.IsFailure)
            return result;
        
        _cache.Set(game.Id, game);
        return result;
    }
}