using System.Net;

namespace Poker.Common.Domain.Results;

public class Response
{
    private Response(string message, HttpStatusCode statusCode)
    {
        Message = new MessageDTO(message);
        StatusCode = statusCode;
    }

    public MessageDTO Message { get; }
    public HttpStatusCode StatusCode { get; }

    public static Response Ok => Create("Operation successful", HttpStatusCode.OK);

    public static Response Create(string message, HttpStatusCode statusCode)
    {
        return new Response(message, statusCode);
    }
}