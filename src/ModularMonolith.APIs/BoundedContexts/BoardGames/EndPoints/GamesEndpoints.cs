namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class GamesEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder WithBoardGameEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      group.MapGet("/", GamesEndpoints.GetAllGames)
        .WithName(nameof(GetAllGames))
        .Produces<List<GameDto>>(StatusCodes.Status200OK);

      return group;
    }

    public static async Task<Results<Ok<List<GameDto>>, BadRequest>> GetAllGames(
        [FromServices] IQuerySender querySender
      , [FromServices] GamesDb db
      , CancellationToken cancellationToken)
    {
      IEnumerable<BoardGame> games =
        await querySender.AskAsync(GetAllGamesQuery.WithPublisher, cancellationToken);
      List<GameDto> allGames = games
        .Select(g => new GameDto(
          Id: g.Id
        , GameName: g.Name.Value
        , Price: g.Price.Amount
        , ImageURL: g.ImageURL
        , PublisherName: g.PublisherName))
        .ToList();
      return TypedResults.Ok(allGames);
    }
  }
}
