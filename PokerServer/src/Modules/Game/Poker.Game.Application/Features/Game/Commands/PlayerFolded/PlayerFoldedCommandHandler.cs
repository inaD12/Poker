using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Commands.PlayerFolded;

internal sealed class PlayerFoldedCommandHandler : ICommandHandler<PlayerFoldedCommand>
{
    private readonly IEntityStore<Table> _tableStore;

    public PlayerFoldedCommandHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }

    public async Task<Result> Handle(PlayerFoldedCommand request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result.Failure(ResponseList.TableNotFound);

        var result = game.PlayerFold(request.PlayerId);
        if (result.IsFailure)
            return result;

        await _tableStore.SaveAsync(game, cancellationToken);
        return result;
    }
}