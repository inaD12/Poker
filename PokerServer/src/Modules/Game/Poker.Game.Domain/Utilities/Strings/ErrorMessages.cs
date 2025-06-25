namespace Poker.Game.Domain.Utilities.Strings;

public static class ErrorMessages
{
	public const string PlayerNotInGame = "Player is not in the game";
	public const string PlayerFolded = "Player has folded";
	public const string PlayerAllIn = "Player is all-in";
	public const string PlayerAlreadyFolded = "Player has already folded";
	public const string PlayerAlreadyAllIn = "Player is already all-in";
	public const string BetTooSmall = "Bet is too small, must at least call the current bet";
	public const string MinimumRaiseNotMet = "Minimum raise not met";
}
