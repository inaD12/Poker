using Poker.Common.Domain.Results;
using Poker.Users.Domain.Utilities.Strings;
using System.Net;

namespace Poker.Users.Domain.Responses;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response SameUsername => Response.Create(ErrorMessages.SameUsername, HttpStatusCode.BadRequest);
	public static Response UserNotFound => Response.Create(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);
	public static Response IncorrectPassword => Response.Create(ErrorMessages.IncorrectPassword, HttpStatusCode.Unauthorized);
	public static Response EmailTaken => Response.Create(ErrorMessages.EmailTaken, HttpStatusCode.Conflict);
	public static Response TokenNotFound => Response.Create(ErrorMessages.TokenNotFound, HttpStatusCode.NotFound);
	public static Response InvalidVerificationToken => Response.Create(ErrorMessages.InvalidVerificationToken, HttpStatusCode.BadRequest);
}
