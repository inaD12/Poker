using Poker.Common.Domain.Abstractions.Interfaces;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerChecked;

public sealed class PlayerCheckedCommandHandler : ICommandHandler<PlayerCheckedCommand>
{
    private readonly ICacheService _cache;

    public PlayerCheckedCommandHandler(ICacheService  cache)
    {
        _cache = cache;
    }

    public async Task<Result> Handle(PlayerCheckedCommand request, CancellationToken cancellationToken)
    {
        var game = _cache.Get<Table>(request.TableId);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerCheck(request.PlayerId);
        if (result.IsFailure)
            return result;
        
        _cache.Set(game.Id, game);
        return result;
    }
}