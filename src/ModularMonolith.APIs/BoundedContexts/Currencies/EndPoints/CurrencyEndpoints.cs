namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public static class CurrencyEndpoints
{
  extension(RouteGroupBuilder group)
  {
#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
    public RouteGroupBuilder GetWithCurrencyEndpoints()
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
    {
      group.MapGet("/", async (GetAllCurrencies handler)
        => await handler.ExecuteAsync())
        .WithName(nameof(GetAllCurrencies))
        .Produces<List<CurrencyDto>>(StatusCodes.Status200OK);

      group.MapPut("/", async (UpdateCurrencyValue handler
        , CurrencyDto dto
        , CancellationToken cancellationToken)
        => await handler.ExecuteAsync(dto, cancellationToken))
        .WithName(nameof(UpdateCurrencyValue))
        .Produces<CurrencyDto>(StatusCodes.Status200OK);
 
      return group;
    }

    ///// <summary>
    ///// Execute simple query to retrieve all currencies
    ///// </summary>
    ///// <param name="querySender"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    ///// <remarks>
    ///// Uses the Query Sender to ask for all currencies
    ///// </remarks>
    //public static async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> GetAllCurrencies1(
    //  [FromServices] IQuerySender querySender
    //, CancellationToken cancellationToken = default)
    //{
    //  List<Currency> currencies =
    //     await querySender.AskAsync(GetAllCurrenciesQuery.All, cancellationToken);
    //  List<CurrencyDto> allCurrencies =
    //    currencies.Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro)).ToList();
    //  return TypedResults.Ok(allCurrencies);
    //}

    /// <summary>
    /// Execute simple query to retrieve all currencies
    /// </summary>
    /// <param name="querySender"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// Uses the Query Handler to ask for all currencies
    /// </remarks>
    public static async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> GetAllCurrencies2(
      [FromServices] IQueryHandler<GetAllCurrenciesQuery, List<Currency>> queryHandler
    , CancellationToken cancellationToken = default)
    {
      List<Currency> currencies =
         await queryHandler.HandleAsync(GetAllCurrenciesQuery.All, cancellationToken);
      List<CurrencyDto> allCurrencies =
        currencies.Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro)).ToList();
      return TypedResults.Ok(allCurrencies);
    }

    ///// <summary>
    ///// Execute simple query to retrieve all currencies
    ///// </summary>
    ///// <param name="querySender"></param>
    ///// <param name="cancellationToken"></param>
    ///// <returns></returns>
    ///// <remarks>
    ///// Uses the DbContext to ask for all currencies
    ///// </remarks>
    //public static async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> GetAllCurrencies3(
    //  [FromServices] CurrenciesDb db
    //, CancellationToken cancellationToken = default)
    //{
    //  // ✅ Keep EF close to the query
    //  List<CurrencyDto> allCurrencies =
    //    await db.Currencies
    //  // ✅ Dont' track entities for read-only queries
    //      .AsNoTracking()
    //      .Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro))
    //      .ToListAsync();
    //  // ✅ Materialize in Infrastructure; return DTOs or domain objects
    //  return TypedResults.Ok(allCurrencies);
    //}

    /// <summary>
    /// Execute simple query to retrieve all currencies
    /// </summary>
    /// <param name="querySender"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <remarks>
    /// Uses the DbContext with repository extension method
    /// </remarks>
    public static async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> GetAllCurrencies4(
      [FromServices] CurrenciesDb db
    , CancellationToken cancellationToken = default)
    {
      // ✅ Keep EF close to the query
      List<CurrencyDto> allCurrencies = await db.GetAllCurrenciesAsync();
      return TypedResults.Ok(allCurrencies);
    }

    //public static async Task<Results<Ok<CurrencyDto>, BadRequest<string>>> UpdateCurrencyValue(
    //  [FromBody] CurrencyDto dto
    //, [FromServices] ICommandSender commandSender
    //, CancellationToken cancellationToken)
    //{
    //  try
    //  {
    //    if (!Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName))
    //    {
    //      return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
    //    }
    //    Currency updated = await commandSender.ExecuteAsync(
    //      new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro), cancellationToken);
    //    return TypedResults.Ok(updated.ToDto());
    //  }
    //  catch (Exception ex)
    //  {
    //    return TypedResults.BadRequest(error: ex.Message);
    //  }
    //}
  }
}
