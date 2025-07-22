using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Domain.Utilities;

public static class PlayerMapper
{
    public static PlayerInfoDto ToDto(this Player player)
    {
        return new PlayerInfoDto(
            player.Id,
            player.Username,
            player.GamesPlayed,
            player.GamesWon,
            player.TotalEarnings
        );
    }
}