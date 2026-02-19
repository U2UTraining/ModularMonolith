namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

public sealed record class CurrencyDto(
  string CurrencyName
, decimal ValueInEuro
);

public static class CurrencyDtoExtensions
{
  public static CurrencyDto ToDto(this Currency currency)
    => new CurrencyDto(
      CurrencyName: currency.Id.Key.ToString()
    , ValueInEuro: currency.ValueInEuro.Value
    );

  public static Currency ToDomain(this CurrencyDto dto)
  {
    if (!Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName))
    {
      throw new ArgumentException(
        message: $"Invalid currency name: {dto.CurrencyName}"
      , paramName: nameof(dto));
    }
    return new Currency(
      id: new PK<CurrencyName>(currencyName)
    , valueInEuro: new PositiveDecimal(dto.ValueInEuro));
  }
}
