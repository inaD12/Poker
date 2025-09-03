using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Commands.PlayerLeave;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerDisconnected;

internal sealed class PlayerDisconnectedCommandHandler: ICommandHandler<PlayerDisconnectedCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerDisconnectedCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result> Handle(PlayerDisconnectedCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerDisconnected(request.PlayerId);

        await _tableStore.SaveAsync(game, cancellationToken);
        return result;
    }
}
