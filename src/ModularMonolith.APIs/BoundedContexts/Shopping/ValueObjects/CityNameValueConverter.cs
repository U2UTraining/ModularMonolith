namespace ModularMonolithBoundedContexts.Shopping.ValueObjects;

public sealed class CityNameValueConverter
: ValueConverter<CityName, string>
{
  public CityNameValueConverter()
  : base(
    cn => cn.Value,
    value => new CityName(value)
  ) { }
}
