using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerChecked;

internal sealed class PlayerCheckedCommandHandler : ICommandHandler<PlayerCheckedCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerCheckedCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }

    public async Task<Result> Handle(PlayerCheckedCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerCheck(request.PlayerId);
        if (result.IsFailure)
            return result;
        
        await _tableStore.SaveAsync(game, cancellationToken);
        return result;
    }
}