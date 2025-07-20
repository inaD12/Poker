using Microsoft.AspNetCore.Routing;
using Poker.Common.Presentation.Endpoints;

namespace Poker.Common.Presentation.Helpers;

public static class EndpointMapper
{
    public static void MapAllEndpoints(IEndpointRouteBuilder endpoints)
    {
        var endpointTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(IEndpoints).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            var endpointInstance = Activator.CreateInstance(endpointType) as IEndpoints;

            endpointInstance?.RegisterEndpoints(endpoints);
        }
    }
}