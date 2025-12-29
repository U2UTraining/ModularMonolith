namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class PublisherEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder WithPublisherEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      group
        .MapGet("/", PublisherEndpoints.GetAllPublishers)
        .WithName(nameof(GetAllPublishers))
        ;
      group
        .MapGet("/{id:int}", PublisherEndpoints.GetPublisherById)
        .WithName(nameof(GetPublisherById))
        ;
      group
        .MapPut("/game/{id:int}", PublisherEndpoints.UpdateGame);
      return group;
    }

    public static async Task<Results<Ok<List<PublisherDto>>, BadRequest>> GetAllPublishers(
      [FromServices] IQuerySender querySender
    , [FromServices] GamesDb db
    , CancellationToken cancellationToken)
    {
      IEnumerable<Publisher> publishers =
          await querySender.AskAsync(new GetAllPublishersQuery(false), cancellationToken);
      List<PublisherDto> allPublishers = publishers
              .Select(p => new PublisherDto(
                Id: p.Id
              , PublisherName: p.Name))
              .ToList();
      return TypedResults.Ok(allPublishers);
    }

    public static async Task<Results<Ok<PublisherWithGamesDto>, NotFound>> GetPublisherById(
      [FromServices] IQuerySender querySender
    , [FromRoute] int id
    , CancellationToken cancellationToken)
    {
      GetPublisherWithGamesQuery query = new(id);
      Publisher? publisher = await querySender.AskAsync(query, cancellationToken);
      if (publisher is not null)
      {
        PublisherWithGamesDto publisherDto = new(
          Id: publisher.Id
        , PublisherName: publisher.Name
        , Games: publisher.Games.Select(g => new GameDto(
            Id: g.Id
          , GameName: g.Name
          , Price: g.Price.Amount
          , ImageURL: g.ImageURL
          , PublisherName: publisher.Name)
          ).ToList()
        , Contacts: publisher.Contacts.Select(c => new ContactDto(c.FirstName, c.LastName, c.Email)).ToList()
          );
        return TypedResults.Ok(publisherDto);
      }
      else
      {
        return TypedResults.NotFound();
      }
    }

    public static async Task<Results<Ok<GameDto>, NotFound, BadRequest>> UpdateGame(
      [FromRoute] int id
    , [FromBody] GameDto gameDto
    , [FromServices] GamesDb db
    , CancellationToken cancellationToken)
    {
      if (gameDto.Id == id)
      {
        BoardGame? game = await db.Games
          .Include(g => g.Publisher)
          .FirstOrDefaultAsync(g => g.Id == id, cancellationToken);
        if (game is null)
          return TypedResults.NotFound();
        game.Rename(new BoardGameName(gameDto.GameName));
        game.SetPrice(new Money(gameDto.Price));
        await db.SaveChangesAsync(cancellationToken);
        return TypedResults.Ok(new GameDto(
          Id: game.Id
        , GameName: game.Name
        , Price: game.Price.Amount
        , ImageURL: game.ImageURL
        , PublisherName: game.Publisher.Name));
      }
      else
      {
        return TypedResults.BadRequest();
      }
    }
  }
}
