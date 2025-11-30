using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEvents;

/// <summary>
/// Domain event triggered when a currency has its valueInEuro changed
/// </summary>
/// <param name="CurrencyName">CurrencyName</param>
/// <param name="ValueInEuro"></param>
public sealed record class CurrencyValueInEuroHasChangedDomainEvent(
  CurrencyName CurrencyName
, PositiveDecimal OldValueInEuro
, PositiveDecimal NewValueInEuro
)
: IDomainEvent
{ }
