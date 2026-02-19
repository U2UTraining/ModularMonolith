namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
public sealed class ApplyMegaDiscount(
  GamesDb db
, IIntegrationEventPublisher integrationEventPublisher)
{
  public async Task<Results<Ok, BadRequest>> ExecuteAsync(
    decimal factor
  , CancellationToken cancellationToken)
  {
    // Nooooo! 😖
    //DbSet<BoardGame> games = DbContext.Games;
    //foreach (BoardGame game in games)
    //{
    //  game.SetPrice(game.Price.WithAmount(game.Price.Amount * discount));
    //}
    //await SaveChangesAsync(cancellationToken);

    // ✅ Use Set-based update (no materialization)
    await db.Games.ExecuteUpdateAsync(s =>
      s.SetProperty(bg => bg.Price.Amount,
                    bg => bg.Price.Amount * factor)
    , cancellationToken
    )
    .ConfigureAwait(false);
    // Now we need to update the entities that are in memory
    // We can do this using an Integration Event
    await integrationEventPublisher.PublishIntegrationEventAsync(
      new GamesHaveChangedIntegrationEvent(), cancellationToken
    ).ConfigureAwait(false);

    return TypedResults.Ok();
  }
}
