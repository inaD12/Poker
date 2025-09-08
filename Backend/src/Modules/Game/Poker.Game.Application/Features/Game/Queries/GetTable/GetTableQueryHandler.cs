using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Queries.GetTable;

internal sealed class GetTableQueryHandler : IQueryHandler<GetTableQuery, GameStateDto>
{
    private readonly IEntityStore<Table> _tableStore;

    public GetTableQueryHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }

    public async Task<Result<GameStateDto>> Handle(GetTableQuery request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result<GameStateDto>.Failure(ResponseList.TableNotFound);

        var resultGameState = game.GetGameState(request.PlayerId);
        
        return resultGameState;
    }
}