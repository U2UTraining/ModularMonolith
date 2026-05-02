namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

/// <summary>
/// Handler for GetGamesQuery, implementing the query pattern
/// </summary>
[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
public sealed class GetGames(BoardGamesDb db, IQuerySender querySender)
{
  public async Task<Results<Ok<List<GameDto>>, BadRequest>> ExecuteAsync(
    GetGamesQuery query
  , CancellationToken cancellationToken)
  {
    // Run query in current bounded context
    IQueryable<BoardGame> gamesQuery =
      db.BoardGames
        .AsNoTracking()
        .Include(g => g.Image);
    if (query.MinAmount > 0)
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount >= query.MinAmount);
    }
    if ((query.MaxAmount < decimal.MaxValue))
    {
      gamesQuery = gamesQuery.Where(g => g.Price.Amount <= query.MaxAmount);
    }
    if (query.IncludePublisher)
    {
      gamesQuery = gamesQuery.Include(g => g.Publisher);
    }
    List<BoardGame> boardGames = await gamesQuery.ToListAsync(cancellationToken);

    // ==============================================================================

    // Run query in other bounded context
    GetValueForCurrencyQuery currencyQuery = new(
       FromCurrency: CurrencyName.EUR
     , ToCurrency: query.AsCurrency
     , Amounts: boardGames.Select(g => new PositiveDecimal(g.Price.Amount)).ToArray());

    PositiveDecimal[] convertedAmounts = await querySender.AskAsync(currencyQuery, cancellationToken);

    // ==============================================================================

    List<GameDto> result = [];

    // Convert the price to the requested currency
    for (int i = 0; i < boardGames.Count; i++)
    {
      BoardGame g = boardGames[i];
      result.Add(new GameDto
      (
          Id: g.Id
        , GameName: g.Name.Value
        , Price: convertedAmounts[i]
        , Currency: query.AsCurrency
        , ImageURL: g.ImageURL
        , PublisherName: query.IncludePublisher ? g.PublisherName : string.Empty
      ));
    }
    return TypedResults.Ok(result);
  }
}
