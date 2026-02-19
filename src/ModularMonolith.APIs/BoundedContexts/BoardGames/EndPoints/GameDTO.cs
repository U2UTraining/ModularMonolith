namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public sealed record class GameDto(
  int Id
, string GameName
, decimal Price
, CurrencyName Currency
, string? ImageURL
, string PublisherName
);
