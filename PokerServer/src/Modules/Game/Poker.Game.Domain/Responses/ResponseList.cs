using System.Net;
using Poker.Common.Domain.Results;
using Poker.Game.Domain.Utilities.Strings;

namespace Poker.Game.Domain.Responses;

public static class ResponseList
{
    // Success Responses
    // Error Responses
    public static Response PlayerNotInGame => Response.Create(ErrorMessages.PlayerNotInGame, HttpStatusCode.NotFound);
    public static Response PlayerFolded => Response.Create(ErrorMessages.PlayerFolded, HttpStatusCode.Conflict);
    public static Response PlayerAllIn => Response.Create(ErrorMessages.PlayerAllIn, HttpStatusCode.Conflict);
    public static Response PlayerAlreadyFolded => Response.Create(ErrorMessages.PlayerAlreadyFolded, HttpStatusCode.Conflict);
    public static Response PlayerAlreadyAllIn =>Response.Create(ErrorMessages.PlayerAlreadyAllIn, HttpStatusCode.Conflict);
    public static Response BetTooSmall => Response.Create(ErrorMessages.BetTooSmall, HttpStatusCode.BadRequest);
    public static Response MinimumRaiseNotMet =>Response.Create(ErrorMessages.MinimumRaiseNotMet, HttpStatusCode.BadRequest);
    public static Response TwoPlayersRequired =>Response.Create(ErrorMessages.TwoPlayersRequired, HttpStatusCode.BadRequest);
    public static Response SixPlayersMaximum => Response.Create(ErrorMessages.SixPlayersMaximum, HttpStatusCode.BadRequest);
    public static Response InsufficientFunds =>Response.Create(ErrorMessages.InsufficientFunds, HttpStatusCode.BadRequest);
    public static Response NotYourTurn => Response.Create(ErrorMessages.NotYourTurn, HttpStatusCode.Conflict);
    public static Response AmountCantBeNegative => Response.Create(ErrorMessages.AmountCantBeNegative, HttpStatusCode.BadRequest);
    public static Response TableNotFound => Response.Create(ErrorMessages.TableNotFound, HttpStatusCode.NotFound);
    public static Response MustMatchBet => Response.Create(ErrorMessages.MustMatchBet, HttpStatusCode.Conflict);
    public static Response LobbyFull => Response.Create(ErrorMessages.LobbyFull, HttpStatusCode.Conflict);
    public static Response PlayerAlreadyInTheLobby =>Response.Create(ErrorMessages.PlayerAlreadyInTheLobby, HttpStatusCode.Conflict);
    public static Response LobbyNotFound => Response.Create(ErrorMessages.LobbyNotFound, HttpStatusCode.NotFound);
    public static Response PlayerNotInLobby => Response.Create(ErrorMessages.PlayerNotInLobby, HttpStatusCode.NotFound);
    public static Response HostNotFromPlayers => Response.Create(ErrorMessages.HostNotFromPlayers, HttpStatusCode.Conflict);
    public static Response OnlyHostCanStartNextHand => Response.Create(ErrorMessages.OnlyHostCanStartNextHand, HttpStatusCode.Conflict);
    public static Response HandNotFinished => Response.Create(ErrorMessages.HandNotFinished, HttpStatusCode.Conflict);
}