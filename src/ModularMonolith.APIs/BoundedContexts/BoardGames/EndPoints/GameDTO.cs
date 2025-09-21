namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public sealed record class GameDTO(
  int Id
, string GameName
, decimal Price
, string? ImageUrl
, string PublisherName
);
