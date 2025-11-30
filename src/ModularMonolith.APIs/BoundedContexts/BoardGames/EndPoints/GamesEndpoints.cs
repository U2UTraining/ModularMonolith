using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class GamesEndpoints
{
  public static void AddGamesEndpoints(this WebApplication app)
  {
    RouteGroupBuilder games = app.MapGroup("/games")
      .WithTags("Games");

    _ = games.MapGet("/",
      async (
        [FromServices] IQuerySender querySender
      , [FromServices] GamesDb db
      , CancellationToken cancellationToken) =>
    {
      IEnumerable<BoardGame> games =
      //  await db.Games.ToListAsync(cancellationToken);
        await querySender.AskAsync(GetAllGamesQuery.WithPublisher, cancellationToken);
      List<GameDTO> allGames = games
        .Select(g => new GameDTO(
          Id: g.Id
        , GameName: g.Name.Value
        , Price: g.Price.Amount
        , ImageUrl: g.ImageURL
        , PublisherName: g.PublisherName))
        .ToList();
      return TypedResults.Ok(allGames);
    })
    .WithName("GetAllGames")
    .Produces<List<GameDTO>>(StatusCodes.Status200OK);
  }
}
