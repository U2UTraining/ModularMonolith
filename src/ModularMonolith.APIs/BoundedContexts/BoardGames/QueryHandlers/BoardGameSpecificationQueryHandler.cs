using ModularMonolith.APIs.BoundedContexts.BoardGames.Repositories;
using ModularMonolith.APIs.BoundedContexts.Common.Queries;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.QueryHandlers;

/// <summary>
/// Query handler for retrieving board games based on a specification.
/// </summary>
/// <param name="boardGameRepository"></param>
public sealed class BoardGameSpecificationQueryHandler(
  IBoardGameRepository boardGameRepository)
: IQueryHandler<Specification<BoardGame>, IQueryable<BoardGame>>
{

  public async Task<IQueryable<BoardGame>> HandleAsync(
    Specification<BoardGame> request,
    CancellationToken cancellationToken)
   => await boardGameRepository.ListAsync(request, cancellationToken)
                                .ConfigureAwait(false);
}
