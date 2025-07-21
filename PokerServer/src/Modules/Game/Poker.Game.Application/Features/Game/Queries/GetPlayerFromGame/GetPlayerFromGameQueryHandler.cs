using Poker.Common.Application.Services;
using Poker.Common.Domain.Abstractions.Messaging;
using Poker.Common.Domain.Dtos;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Entities.TableAggregate;
using Poker.Game.Domain.Responses;

namespace Poker.Game.Application.Features.Game.Queries.GetPlayerFromGame;

internal sealed class GetPlayerFromGameQueryHandler: IQueryHandler<GetPlayerFromGameQuery, PlayerInfoDto>
{
    private readonly IEntityStore<Table> _tableStore;

    public GetPlayerFromGameQueryHandler(IEntityStore<Table> tableStore)
    {
        _tableStore = tableStore;
    }
    
    public async Task<Result<PlayerInfoDto>> Handle(GetPlayerFromGameQuery request, CancellationToken cancellationToken)
    {
        var game = await _tableStore.GetAsync(request.TableId, cancellationToken);
        if (game is null)
            return Result<PlayerInfoDto>.Failure(ResponseList.TableNotFound);
        
        var player = game.GetPlayerDto(request.PlayerId);
        if (player is null)
            return Result<PlayerInfoDto>.Failure(ResponseList.PlayerNotInGame);
        
        return Result<PlayerInfoDto>.Success(player);
    }
}