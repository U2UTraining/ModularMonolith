namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

[Register(
  lifetime: ServiceLifetime.Singleton
, methodNameHint: "AddCurrencyServices")]
public sealed class CurrenciesQuery(IQuerySender _querySender)
{
  public async Task<List<Currency>> GetAllCurrenciesAsync(GetAllCurrenciesQuery query)
    => await _querySender.AskAsync(query);

  public async Task<PositiveDecimal[]> GetValueForCurrencyAsync(GetValueForCurrencyQuery query)
    => await _querySender.AskAsync(query);
}
