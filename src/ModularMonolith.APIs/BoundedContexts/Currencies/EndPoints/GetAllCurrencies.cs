namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

// =====================================================================================
///// <summary>
///// Version that uses the query sender indirection to decouple 
///// the endpoint from the query handler and the data access layer.
///// 🤔 Maybe a little too much de-coupling?
///// </summary>
///// <param name="querySender"></param>
//[Register(
//  lifetime: ServiceLifetime.Scoped
//, methodNameHint: "AddCurrencyServices")]
//internal sealed class GetAllCurrencies(IQuerySender querySender)
//{
//  public async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> ExecuteAsync(
//    CancellationToken cancellationToken = default)
//  {
//    List<Currency> currencies =
//       await querySender.AskAsync(GetAllCurrenciesQuery.All, cancellationToken);
//    List<CurrencyDto> allCurrencies =
//      currencies.Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro)).ToList();
//    return TypedResults.Ok(allCurrencies);
//  }
//}
// =====================================================================================

// =====================================================================================
///// <summary>
///// Version that uses the query handler directly, which is simpler
///// and more straightforward, but also more tightly coupled to 
///// the data access layer.
///// </summary>
///// <param name="queryHandler"></param>
//[Register(
//  lifetime: ServiceLifetime.Scoped
//, methodNameHint: "AddCurrencyServices")]
//internal sealed class GetAllCurrencies(
//  IQueryHandler<GetAllCurrenciesQuery, List<Currency>> queryHandler)
//{
//  public async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> ExecuteAsync(
//    CancellationToken cancellationToken = default)
//  {
//    List<Currency> currencies =
//       await queryHandler.HandleAsync(GetAllCurrenciesQuery.All, cancellationToken);
//    List<CurrencyDto> allCurrencies =
//      currencies.Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro)).ToList();
//    return TypedResults.Ok(allCurrencies);
//  }
//}
// =====================================================================================

// =====================================================================================
///// <summary>
///// Version that skips the query handler and the query sender indirection, 
///// and uses the DbContext directly in the endpoint.
///// 😊 Good for speed
///// </summary>
///// <param name="db"></param>
//[Register(
//  lifetime: ServiceLifetime.Scoped
//, methodNameHint: "AddCurrencyServices")]
//internal sealed class GetAllCurrencies(CurrenciesDb db)
//{
//  public async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> ExecuteAsync(
//    CancellationToken cancellationToken = default)
//  {
//    // ✅ Keep EF close to the query
//    List<CurrencyDto> allCurrencies =
//      await db.Currencies
//        // ✅ Dont' track entities for read-only queries
//        .AsNoTracking()
//        .Select(c => new CurrencyDto(c.Id.ToString(), c.ValueInEuro))
//        .ToListAsync();
//    // ✅ Materialize in Infrastructure; return DTOs or domain objects
//    return TypedResults.Ok(allCurrencies);
//  }
//}
// =====================================================================================

// =====================================================================================
/// <summary>
/// Here the "repository" is implemented as an extension method
/// See BoundedContexts\Currencies\Repositories\CurrencyRepository.cs
/// </summary>
/// <param name="db"></param>
[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class GetAllCurrencies(CurrenciesDb db)
{
  public async Task<Results<Ok<List<CurrencyDto>>, BadRequest>> ExecuteAsync(
    CancellationToken cancellationToken = default)
  {
    // ✅ Keep EF close to the query
    List<CurrencyDto> allCurrencies = await db.GetAllCurrenciesAsync();
    return TypedResults.Ok(allCurrencies);
  }
}

