using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;

public sealed record CurrencyHasChangedIntegrationEvent(
  string CurrencyName
, decimal OldValueInEuro
, decimal NewValueInEuro
, string CurrencyString
)
: IIntegrationEvent
{ }
