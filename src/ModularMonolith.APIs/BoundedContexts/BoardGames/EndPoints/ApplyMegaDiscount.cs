using Microsoft.EntityFrameworkCore.Storage;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.EndPoints;

[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
public sealed class ApplyMegaDiscount(
  BoardGamesDb db
, [FromKeyedServices(nameof(BoardGamesDb))] IOutboxSignal outboxSignal
//, IIntegrationEventPublisher integrationEventPublisher
)
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

    IExecutionStrategy strategy = db.Database.CreateExecutionStrategy();
    
    await strategy.ExecuteAsync(async () =>
    {
      using IDbContextTransaction tx = await db.Database
        .BeginTransactionAsync(cancellationToken)
        .ConfigureAwait(false);
      
      try
      {
        // ✅ Use Set-based update (no materialization)
        await db.BoardGames.ExecuteUpdateAsync(s =>
          s.SetProperty(bg => bg.Price.Amount,
                        bg => bg.Price.Amount * factor)
        , cancellationToken
        )
        .ConfigureAwait(false);
        
        // Now we need to update the entities that are in memory
        // We can do this using an Integration Event
        GamesHaveChangedIntegrationEvent @event =
          new GamesHaveChangedIntegrationEvent(
            EventId: Guid.NewGuid()
          );
        await db.SaveChangesAsync(@event, outboxSignal, cancellationToken)
          .ConfigureAwait(false);

        await tx.CommitAsync(cancellationToken);
      }
      catch
      {
        await tx.RollbackAsync(cancellationToken).ConfigureAwait(false);
        throw; // Re-throw to allow the execution strategy to retry
      }
    });

    return TypedResults.Ok();
  }
}
