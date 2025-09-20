﻿namespace U2U.ModularMonolith.BoundedContexts.Currencies.Queries;

/// <summary>
/// Get all currencies query
/// </summary>
/// <remarks>
/// This is a perfect example for a Flyweight  
/// </remarks>
public record class GetCurrenciesQuery
: IQuery<IQueryable<Currency>> 
{
  private GetCurrenciesQuery() { }

  public static GetCurrenciesQuery All { get; } = new();
}
