using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

public sealed class GetPublisherWithGamesQuery
  : IQuery<Publisher?>
{
  public GetPublisherWithGamesQuery(int publisherId)
  {
    PublisherId = publisherId;
  }

  public int PublisherId
  {
    get;
  }
}
