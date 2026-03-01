namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

public record class GetGameByIdQuery(
  int GameId
)
: IQuery<BoardGame?>;
