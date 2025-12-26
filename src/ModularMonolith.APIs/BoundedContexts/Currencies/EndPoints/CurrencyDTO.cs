namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public sealed record class CurrencyDto(
  string CurrencyName
, decimal ValueInEuro
);
