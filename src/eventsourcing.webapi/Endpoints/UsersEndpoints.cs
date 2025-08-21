using Eventsourcing.WebApi.Documents;
using Marten;
using Microsoft.AspNetCore.Mvc;

namespace Eventsourcing.WebApi.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        app.MapGet("/users", async ([FromServices] IDocumentSession session) =>
        {
            var users = await session.Query<User>().ToListAsync();
            return Results.Ok(users);
        })
        .WithName("GetUsers")
        .Produces<List<User>>(StatusCodes.Status200OK);

        app.MapPost("/users", async (CreateUserRequest create, [FromServices] IDocumentSession session) =>
        {
            var user = new User
            {
                FirstName = create.FirstName,
                LastName = create.LastName,
                Internal = create.Internal
            };

            session.Store(user);

            await session.SaveChangesAsync();
            return Results.Created($"/users/{user.Id}", user);
        })
        .WithName("CreateUser")
        .Produces<User>(StatusCodes.Status201Created);
    }
}

public record CreateUserRequest(string FirstName, string LastName, bool Internal);