namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

public sealed record class GetPublisherWithGamesQuery(
  int PublisherId
) 
: IQuery<Publisher?>
;
