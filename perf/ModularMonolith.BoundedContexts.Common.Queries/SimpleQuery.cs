using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.BoundedContexts.Common.Queries;

public record class SimpleQuery
  : IQuery<int>;

public sealed class SimpleQueryHandler
  : IQueryHandler<SimpleQuery, int>
{
  public Task<int> HandleAsync(
    SimpleQuery query
  , CancellationToken cancellationToken)
    => Task.FromResult(42);
}
