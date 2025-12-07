namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

public static class PublisherEndpoints
{
  public static void AddPublishersEndpoints(this WebApplication app)
  {
    RouteGroupBuilder publishers = app.MapGroup("/publishers")
      .WithTags("Publishers");

    _ = publishers.MapGet("/",
          async (
            [FromServices] IQuerySender querySender
          , [FromServices] GamesDb db
          , CancellationToken cancellationToken) =>
          {
            IEnumerable<Publisher> publishers =
                    //  await db.Games.ToListAsync(cancellationToken);
                    await querySender.AskAsync(GetAllPublishersQuery.Default, cancellationToken);
            List<PublisherDTO> allPublishers = publishers
                    .Select(p => new PublisherDTO(
                      Id: p.Id
                    , PublisherName: p.Name))
                    .ToList();
            return TypedResults.Ok(allPublishers);
          })
          .WithName("GetAllPublishers")
          .Produces<List<PublisherDTO>>(StatusCodes.Status200OK);

    _ = publishers.MapGet("/{id:int}",
          async Task<Results<Ok<PublisherWithGamesDTO>, NotFound>> (
            [FromServices] IQuerySender querySender
          , [FromRoute] int id
          , CancellationToken cancellationToken) =>
          {
          GetPublisherWithGamesQuery query = new(id);
          Publisher? publisher = await querySender.AskAsync(query, cancellationToken);
          if (publisher is not null)
          {
            PublisherWithGamesDTO publisherDto = new(
              Id: publisher.Id
            , PublisherName: publisher.Name
            , Games: publisher.Games.Select(g => new GameDTO(
                Id: g.Id
              , GameName: g.Name
              , Price: g.Price.Amount
              , ImageURL: g.ImageURL
              , PublisherName: publisher.Name)
              ).ToList()
            , Contacts: publisher.Contacts.Select(c => new ContactDTO
                {
                  FirstName= c.FirstName
                , LastName= c.LastName
                  , Email= c.Email
                }).ToList()
              );
              return TypedResults.Ok(publisherDto);
            }
            else
            {
              return TypedResults.NotFound();
            }
          })
          .WithName("GetPublisherById")
          .Produces<PublisherWithGamesDTO>(StatusCodes.Status200OK)
          .Produces(StatusCodes.Status404NotFound);
  }
}
