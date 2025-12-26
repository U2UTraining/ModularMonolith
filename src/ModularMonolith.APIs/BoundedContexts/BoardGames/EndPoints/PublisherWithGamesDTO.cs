namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public record class PublisherWithGamesDto(
  int Id
, string PublisherName
, List<ContactDto> Contacts
, List<GameDto> Games
);
