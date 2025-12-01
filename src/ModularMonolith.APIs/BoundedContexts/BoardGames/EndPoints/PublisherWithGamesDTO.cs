namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public record class PublisherWithGamesDTO(
  int Id
, string PublisherName
, List<ContactDTO> Contacts
, List<GameDTO> Games
);
