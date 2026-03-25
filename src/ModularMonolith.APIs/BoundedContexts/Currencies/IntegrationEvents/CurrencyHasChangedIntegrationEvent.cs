namespace ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;

public sealed record CurrencyHasChangedIntegrationEvent(
  Guid EventId
, string CurrencyName
, decimal OldValueInEuro
, decimal NewValueInEuro
, string CurrencyString
)
: IIntegrationEvent
{ }
