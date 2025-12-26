namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

/// <summary>
/// Get all currencies query
/// </summary>
/// <remarks>
/// This is a perfect example for a Flyweight  
/// </remarks>
public record class GetCurrenciesQuery
: IQuery<List<Currency>> 
{
  private GetCurrenciesQuery() { }

  public static GetCurrenciesQuery All { get; } = new();
}
