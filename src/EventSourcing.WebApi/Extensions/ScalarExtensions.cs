using Scalar.AspNetCore;

namespace EventSourcing.WebApi.Extensions;

public static class ScalarExtensions
{
    public static IEndpointConventionBuilder MapScalarConfig(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapScalarApiReference();
    }
}
