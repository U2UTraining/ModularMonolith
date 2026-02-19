namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query to retrieve all board games.
/// </summary>
public sealed record class GetGamesQuery(
  decimal MinAmount = decimal.Zero
, decimal MaxAmount = decimal.MaxValue
, bool IncludePublisher = false
, CurrencyName AsCurrency = CurrencyName.EUR
)
: IQuery<IQueryable<BoardGame>>
;
