namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public sealed record class GameDTO(
  int Id
, string GameName
, decimal Price
, string? ImageURL
, string PublisherName
);
