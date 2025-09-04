using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Interfaces.Notifiers;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.GameClose;

internal sealed class GameCloseCommandHandler : ICommandHandler<GameCloseCommand>
{
    private readonly IEntityStore<Table> _tableStore;
    private readonly ITableNotifier _tableNotifier;

    public GameCloseCommandHandler(IEntityStore<Table> tableStore, ITableNotifier tableNotifier)
    {
        _tableStore = tableStore;
        _tableNotifier = tableNotifier;
    }

    public async Task<Result> Handle(GameCloseCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        if(game.HostPlayerId != request.PlayerId)
            return Result.Failure(ResponseList.NotHost);
        if(!game.WaitingForNextHand)
            return Result.Failure(ResponseList.HandNotFinished);
        
        await _tableStore.DeleteAsync(request.TableId, cancellationToken);
        await _tableNotifier.NotifyGameClosingAsync(request.TableId);
        
        return Result.Success();
    }
}