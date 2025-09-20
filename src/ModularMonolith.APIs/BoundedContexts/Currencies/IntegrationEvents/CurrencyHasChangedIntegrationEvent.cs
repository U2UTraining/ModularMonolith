﻿namespace ModularMonolithBoundedContexts.Currencies.IntegrationEvents;

public sealed record CurrencyHasChangedIntegrationEvent(
  string CurrencyName
, decimal OldValueInEuro
, decimal NewValueInEuro
, string CurrencyString
)
: IIntegrationEvent
{ }
