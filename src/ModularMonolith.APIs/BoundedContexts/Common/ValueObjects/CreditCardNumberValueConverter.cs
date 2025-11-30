namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

public sealed class CreditCardNumberValueConverter
: ValueConverter<CreditCardNumber, string>
{
  public CreditCardNumberValueConverter()
  : base(
    ccn => ccn.Value,
    value => new CreditCardNumber(value)
  )
  { }
}
