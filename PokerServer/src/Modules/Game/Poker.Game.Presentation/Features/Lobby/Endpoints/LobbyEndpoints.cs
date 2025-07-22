using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Poker.Common.Application.Abstractions.Interfaces;
using Poker.Common.Presentation.Endpoints;
using Poker.Common.Presentation.Helpers;
using Poker.Game.Application.Features.Lobby.Models;
using Poker.Game.Application.Features.Lobby.Queries.GetAllLobbiesPaged;
using Poker.Game.Presentation.Features.Lobby.Models;

namespace Poker.Game.Presentation.Features.Lobby.Endpoints;

internal class LobbyEndpoints : IEndpoints
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/lobbies");

        group.MapGet("get-all", GetAll)
            .Produces<LobbyPaginatedQueryViewModel>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError)
            .RequireAuthorization();
    }

    private async Task<IResult> GetAll(
        [AsParameters] GetAllLobbiesRequest request,
        [FromServices] ISender sender,
        [FromServices] IPokerMapper mapper,
        CancellationToken cancellationToken)
    {
        var query = mapper.Map<GetAllLobbiesPagedQuery>(request);
        var res = await sender.Send(query, cancellationToken);
        if (res.IsFailure)
            return ControllerResponse.ParseAndReturnMessage(res);

        var userCommandResponse = mapper.Map<LobbyPaginatedQueryViewModel>(res.Value!);
        return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
    }
}