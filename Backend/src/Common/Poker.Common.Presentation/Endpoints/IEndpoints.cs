using Microsoft.AspNetCore.Routing;

namespace Poker.Common.Presentation.Endpoints;

public interface IEndpoints
{
    void RegisterEndpoints(IEndpointRouteBuilder app);
}