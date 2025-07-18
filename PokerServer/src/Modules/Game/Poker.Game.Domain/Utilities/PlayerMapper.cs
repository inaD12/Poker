using Poker.Common.Domain.Dtos;
using Poker.Game.Domain.Entities.TableAggregate;

namespace Poker.Game.Domain.Utilities;

public static class PlayerMapper
{
    public static PlayerInfoDto ToDto(this Player player) =>
        new PlayerInfoDto(
            Id: player.Id,
            Username: player.Username
        );
}
