namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;

public sealed record class GetGameByIdQuery(
  int GameId
)
: IQuery<GameDto?>;
