namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

/// <summary>
/// Query handler for retrieving board games based on a specification.
/// </summary>
/// <param name="boardGameRepository"></param>
internal sealed class BoardGameSpecificationQueryHandler(
  IBoardGameRepository boardGameRepository)
: IQueryHandler<Specification<BoardGame>, IQueryable<BoardGame>>
{

  public async Task<IQueryable<BoardGame>> HandleAsync(
    Specification<BoardGame> request,
    CancellationToken cancellationToken = default)
   => await boardGameRepository.ListAsync(request, cancellationToken)
                                .ConfigureAwait(false);
}
