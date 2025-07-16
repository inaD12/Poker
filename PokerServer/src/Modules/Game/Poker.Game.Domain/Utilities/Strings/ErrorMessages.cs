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
	public const string TwoPlayersRequired = "A game requires at least two players";
	public const string SixPlayersMaximum = "A game can have a maximum of six players";
	public const string InsufficientFunds = "Insufficient funds";
	public const string NotYourTurn = "It's not your turn";
	public const string AmountCantBeNegative = "Amount cannot be negative";
	public const string TableNotFound = "Table not found";
	public const string MustMatchBet = "Player must match bet";
}
