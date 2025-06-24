using Poker.Common.Domain.Results;
using Poker.Common.Domain.Utilities.Strings;
using System.Net;

namespace Poker.Common.Domain.Responses;

public static class SharedResponses
{
	// Success Responses

	// Error Responses
	public static Response ValidationError => Response.Create(ErrorMessages.ValidationError, HttpStatusCode.BadRequest);
	public static Response EntityNotFound => Response.Create(ErrorMessages.EntityNotFound, HttpStatusCode.NotFound);
	public static Response InternalError => Response.Create(ErrorMessages.InternalError, HttpStatusCode.InternalServerError);
}
