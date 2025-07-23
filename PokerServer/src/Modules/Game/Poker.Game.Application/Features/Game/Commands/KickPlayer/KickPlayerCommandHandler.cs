using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.KickPlayer;

internal sealed class KickPlayerCommandHandler: ICommandHandler<KickPlayerCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public KickPlayerCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result> Handle(KickPlayerCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        if(game.HostPlayerId != request.CallingPlayerId)
            return Result.Failure(ResponseList.NotHost);
        
        var result = game.KickPlayer(request.PlayerId);

        return result;
    }
}
