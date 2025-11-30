namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Repositories;

public interface IBoardGameRepository 
: IRepository<BoardGame>
{
  ValueTask ApplyMegaDiscountAsync(
    decimal discount
  , CancellationToken cancellationToken = default
  );

  ValueTask<IQueryable<BoardGame>> GetBoardGamesFromList
  (PK<int>[] gameIds, CancellationToken cancellationToken);
}
