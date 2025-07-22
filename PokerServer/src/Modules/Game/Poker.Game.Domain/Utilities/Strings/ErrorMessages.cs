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
    public const string LobbyFull = "Lobby is full";
    public const string PlayerAlreadyInTheLobby = "Player is already in the lobby";
    public const string LobbyNotFound = "Lobby not found";
    public const string PlayerNotInLobby = "Player is not in lobby";
    public const string HostNotFromPlayers = "Host must be one of the players";
    public const string OnlyHostCanStartNextHand = "Only host can start next hand";
    public const string HandNotFinished = "Hand hasn't finished yet";
    public const string NotHost = "Only the host can do this";
    public const string UsernameEmpty = "Username cannot be empty";
    public const string BalanceNegative = "Balance cannot be negative";
}