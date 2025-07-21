using Poker.Common.Domain.Notifications;
using Poker.Game.Domain.Enums;

namespace Poker.Game.Application.Extensions;

public static class PlayerActionTypeExtensions
{
    public static PlayerActionNotification ToNotification(this PlayerActionType action, int? amount)
    {
        return action switch
        {
            PlayerActionType.PlaceBet => new PlayerBetNotification(amount!.Value),
            PlayerActionType.Fold => new PlayerFoldNotification(),
            PlayerActionType.AllIn => new PlayerAllInNotification(),
            PlayerActionType.Check => new PlayerCheckNotification(),
            _ => throw new InvalidOperationException("Unknown player action")
        };
    }
}