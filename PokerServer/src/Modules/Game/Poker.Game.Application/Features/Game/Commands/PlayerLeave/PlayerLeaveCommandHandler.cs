using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerLeave;

internal sealed class PlayerLeaveCommandHandler: ICommandHandler<PlayerLeaveCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerLeaveCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result> Handle(PlayerLeaveCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerLeave(request.PlayerId);

        return result;
    }
}
