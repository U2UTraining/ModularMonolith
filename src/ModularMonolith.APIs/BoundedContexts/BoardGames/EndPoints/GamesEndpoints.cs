namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class GamesEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder WithBoardGameEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      //group.MapPost(pattern: "/"
      //  , GamesEndpoints.GetGamesDirect)
      //  .WithName(nameof(GetGamesDirect))
      //  .Produces<List<GameDto>>(StatusCodes.Status200OK)
      //  .Produces(StatusCodes.Status400BadRequest)
      //  ;

      group.MapPost(pattern: "/", (
        GetGamesQuery query
      , GetGames handler
      , CancellationToken cancellationToken) 
      => handler.ExecuteAsync(query, cancellationToken))
        .WithName(nameof(GetGames))
        .Produces<List<GameDto>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        ;

      group.MapPut(pattern: "/apply-discount"
        , async (ApplyMegaDiscount handler, CancellationToken cancellationToken) 
      => await handler.ExecuteAsync(0.90M, cancellationToken))
        .WithName(nameof(ApplyMegaDiscount))
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        ;

      group.MapPut(pattern: "/undo-discount"
        , async (ApplyMegaDiscount handler, CancellationToken cancellationToken)
      => await handler.ExecuteAsync(1.10M, cancellationToken))
        .WithName("UndoMegaDiscount")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        ;

      return group;
    }

    ///// <summary>
    ///// Get Games using a Query Sender
    ///// </summary>
    ///// <param name="query"></param>
    ///// <param name="querySender"></param>
    ///// <param name="db"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    //public static async Task<Results<Ok<List<GameDto>>, BadRequest>> GetGames(
    //    [FromBody] GetGamesQuery query
    //  , [FromServices] IQuerySender querySender
    //  , [FromServices] GamesDb db
    //  , CancellationToken cancellationToken)
    //{
    //  IEnumerable<BoardGame> games =
    //    await querySender.AskAsync(query, cancellationToken);
    //  List<GameDto> allGames = games
    //    .Select(g => new GameDto(
    //      Id: g.Id
    //    , GameName: g.Name.Value
    //    , Price: g.Price.Amount
    //    , ImageURL: g.ImageURL
    //    , PublisherName: g.PublisherName))
    //    .ToList();
    //  return TypedResults.Ok(allGames);
    //}

  //  /// <summary>
  //  /// Get Games using a Query Handler directly
  //  /// </summary>
  //  /// <param name="query"></param>
  //  /// <param name="queryHandler"></param>
  //  /// <param name="db"></param>
  //  /// <param name="cancellationToken"></param>
  //  /// <returns></returns>
  //  public static async Task<Results<Ok<List<GameDto>>, BadRequest>> GetGamesDirect(
  //  [FromBody] GetGamesQuery query
  //, [FromServices] IQueryHandler<GetGamesQuery, IQueryable<BoardGame>> queryHandler
  //, [FromServices] GamesDb db
  //, CancellationToken cancellationToken)
  //  {
  //    IEnumerable<BoardGame> games =
  //      await queryHandler.HandleAsync(query, cancellationToken);
  //    List<GameDto> allGames = games
  //      .Select(g => new GameDto(
  //        Id: g.Id
  //      , GameName: g.Name.Value
  //      , Price: g.Price.Amount
  //      , ImageURL: g.ImageURL
  //      , PublisherName: g.PublisherName))
  //      .ToList();
  //    return TypedResults.Ok(allGames);
  //  }

    /// <summary>
    /// Apply Mega Discount Command
    /// </summary>
    public static async Task<Results<Ok, BadRequest>> ApplyMegaDiscount(
      [FromServices] ICommandHandler<ApplyMegaDiscountCommand, bool> commandHandler
    , CancellationToken cancellationToken)
    {
      ApplyMegaDiscountCommand command = new(GiveDiscount: true, Discount: new Percent(20));
      bool success = await commandHandler.HandleAsync(command, cancellationToken);
      return success ? TypedResults.Ok() : TypedResults.BadRequest();
    }
  }
}
