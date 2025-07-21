using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerPlacedBet;

internal sealed class PlayerPlacedBetCommandHandler : ICommandHandler<PlayerPlacedBetCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerPlacedBetCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result> Handle(PlayerPlacedBetCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);
        
        var result = game.PlayerPlaceBet(request.PlayerId, request.Amount);
        if (result.IsFailure)
            return result;
        
        await _tableStore.SaveAsync(game,  cancellationToken);
        return result;
    }
}