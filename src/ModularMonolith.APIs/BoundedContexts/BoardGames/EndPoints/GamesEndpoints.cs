namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class GamesEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder WithBoardGameEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      group.MapPost(pattern: "/"
        , GamesEndpoints.GetGames)
        .WithName(nameof(GetGames))
        .Produces<List<GameDto>>(StatusCodes.Status200OK);

      return group;
    }

    public static async Task<Results<Ok<List<GameDto>>, BadRequest>> GetGames(
        [FromBody] GetGamesQuery query
      , [FromServices] IQuerySender querySender
      , [FromServices] GamesDb db
      , CancellationToken cancellationToken)
    {
      IEnumerable<BoardGame> games =
        await querySender.AskAsync(query, cancellationToken);
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
