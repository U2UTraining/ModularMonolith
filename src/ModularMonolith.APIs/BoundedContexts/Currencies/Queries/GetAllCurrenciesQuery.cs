namespace ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

/// <summary>
/// Get all currencies query
/// </summary>
/// <remarks>
/// This is a perfect example for a Flyweight  
/// </remarks>
public record class GetAllCurrenciesQuery
: IQuery<List<Currency>> 
{
  private GetAllCurrenciesQuery() { }

  public static GetAllCurrenciesQuery All { get; } = new();
}
