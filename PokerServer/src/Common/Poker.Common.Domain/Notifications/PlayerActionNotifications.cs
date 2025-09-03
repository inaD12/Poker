namespace Poker.Common.Domain.Notifications;

public abstract record PlayerActionNotification(string Type);

public record PlayerBetNotification(decimal Amount) : PlayerActionNotification("Bet");

public record PlayerFoldNotification() : PlayerActionNotification("Fold");

public record PlayerAllInNotification() : PlayerActionNotification("AllIn");

public record PlayerCheckNotification() : PlayerActionNotification("Check");

public record PlayerTurnNotification() : PlayerActionNotification("Turn");

public record PlayerDisconnectNotification() : PlayerActionNotification("Disconnect");

public record PlayerReconnectNotification() : PlayerActionNotification("Reconnect");