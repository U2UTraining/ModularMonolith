namespace ModularMonolith.APIs.BoundedContexts.Currencies.Entities;

[DebuggerDisplay("Currency {Name,nq} = {ValueInEuro}EUR")]
public sealed class Currency
: EntityBase<PK<CurrencyName>>
, IAggregateRoot
, IHistory
, ISoftDeletable
{
  public Currency(
    PK<CurrencyName> id
  , PositiveDecimal valueInEuro)
  : base(id) 
  => ValueInEuro = valueInEuro;

  public PositiveDecimal ValueInEuro { get; private set; }

  public void UpdateValueInEuro(PositiveDecimal valueInEuro)
  {
    if (Id.Key is CurrencyName.EUR)
    {
      throw new ArgumentException(
        message: "The value for currency EUR cannot be changed!"
      , paramName: nameof(valueInEuro));
    }
    PositiveDecimal oldValueInEuro = this.ValueInEuro;
    ValueInEuro = valueInEuro;
    RegisterDomainEvent(new CurrencyValueInEuroHasChangedDomainEvent(
      Id.Key, oldValueInEuro, ValueInEuro
    ));
  }

  public static CurrencyName Parse(string currencyAsString) 
  => CurrencyName.Parse<CurrencyName>(currencyAsString);

  public override string ToString()
  {
    CultureInfo ci = CultureInfo
      .GetCultures(CultureTypes.SpecificCultures)
      .Where(x => new RegionInfo(x.Name).ISOCurrencySymbol == Id.Key.ToString())
      .First();
    return ValueInEuro.Value.ToString("C4", ci);
  }

  public string ToEuroString()
  {
    CultureInfo ci = new CultureInfo("nl-BE");
    return ValueInEuro.Value.ToString("C4", ci);
  }
}
