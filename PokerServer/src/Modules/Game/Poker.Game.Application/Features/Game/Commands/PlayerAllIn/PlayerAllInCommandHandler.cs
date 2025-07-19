using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerAllIn;

public sealed class PlayerAllInCommandHandler : ICommandHandler<PlayerAllInCommand>
{
    private readonly ICacheService _cache;

    public PlayerAllInCommandHandler(ICacheService  cache)
    {
        _cache = cache;
    }
    
    public async Task<Result> Handle(PlayerAllInCommand request, CancellationToken cancellationToken)
    {
        var game = _cache.Get<Table>(request.TableId);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerAllIn(request.PlayerId);
        if (result.IsFailure)
            return result;
        
        _cache.Set(game.Id, game);
        return result;
    }
}