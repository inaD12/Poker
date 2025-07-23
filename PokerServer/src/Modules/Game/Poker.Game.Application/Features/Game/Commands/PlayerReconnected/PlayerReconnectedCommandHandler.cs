using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Application.Features.Game.Commands.PlayerDisconnected;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerReconnected;

internal sealed class PlayerReconnectedCommandHandler: ICommandHandler<PlayerReconnectedCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerReconnectedCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result> Handle(PlayerReconnectedCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerReconnected(request.PlayerId);

        return result;
    }
}
