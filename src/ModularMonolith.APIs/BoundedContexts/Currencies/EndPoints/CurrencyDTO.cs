namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public sealed record class CurrencyDTO(
  string CurrencyName
, decimal ValueInEuro
);
