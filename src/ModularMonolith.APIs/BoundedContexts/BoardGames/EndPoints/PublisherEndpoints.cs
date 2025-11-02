using Microsoft.AspNetCore.Mvc;

using ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class PublisherEndpoints
{
  public static void AddPublishersEndpoints(this WebApplication app)
  {
    RouteGroupBuilder games = app.MapGroup("/publishers")
      .WithTags("Publishers");

    _ = games.MapGet("/",
          async (
            [FromServices] IQuerySender querySender
          , [FromServices] GamesDb db
          , CancellationToken cancellationToken) =>
          {
            IEnumerable<Publisher> publishers =
                    //  await db.Games.ToListAsync(cancellationToken);
                    await querySender.AskAsync(GetAllPublishersQuery.WithGames, cancellationToken);
            List<PublisherDTO> allPublishers = publishers
                    .Select(p => new PublisherDTO(
                      Id: p.Id
                    , PublisherName: p.Name))
                    .ToList();
            return TypedResults.Ok(allPublishers);
          })
          .WithName("GetAllPublishers")
          .Produces<List<BoardGame>>(StatusCodes.Status200OK);

  }
}
