namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public sealed record class GameDto(
  int Id
, string GameName
, decimal Price
, string? ImageURL
, string PublisherName
);
