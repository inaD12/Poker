namespace Poker.Game.Domain.Notifications;

public abstract record PlayerActionNotification(string Type);

public record PlayerBetNotification(decimal Amount) : PlayerActionNotification("Bet");
public record PlayerFoldNotification() : PlayerActionNotification("Fold");
public record PlayerTurnNotification(int TimeLeftSeconds) : PlayerActionNotification("Turn");
