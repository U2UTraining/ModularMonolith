using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class GamesEndpoints
{
  public static void AddGamesEndpoints(this WebApplication app)
  {
    RouteGroupBuilder games = app.MapGroup("/games")
      .WithTags("Games");

    _ = games.MapGet("/", 
      async (
        [FromServices] IQuerySender querySender
      , [FromServices] GamesDb db
      , CancellationToken cancellationToken) =>
    {
      IEnumerable<BoardGame> games = 
        await db.Games.ToListAsync(cancellationToken);
        //await querySender.AskAsync(GetAllGamesQuery.Instance, cancellationToken);
      //List<CurrencyDTO> allCurrencies = await db.Currencies
      //  .AsNoTracking()
      //  .Select(c => new CurrencyDTO(c.Id.ToString(), c.ValueInEuro))
      //  .ToListAsync(cancellationToken);
      return TypedResults.Ok(games);
    })
    .WithName("GetAllGames")
    .Produces<List<BoardGame>>(StatusCodes.Status200OK);

    //  games.MapPut("/", 
    //    async Task<Results<Ok<CurrencyDTO>, BadRequest<string>>>(
    //      [FromBody] CurrencyDTO dto
    //    , [FromServices] ICommandSender commandSender
    //    , CancellationToken cancellationToken) =>
    //  {
    //    if (Enum.TryParse<CurrencyName>(dto.CurrencyName, out CurrencyName currencyName) == false)
    //    {
    //      return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
    //    }

    //    await commandSender.ExecuteAsync(
    //      new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro));
    //    return TypedResults.Ok(dto);
    //  })
    //  .WithName("UpdateCurrencyValue")
    //  .Produces<CurrencyDTO>(StatusCodes.Status200OK);
  }
}
