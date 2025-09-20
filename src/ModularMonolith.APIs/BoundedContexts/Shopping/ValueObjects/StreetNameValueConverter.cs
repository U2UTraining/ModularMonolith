namespace U2U.ModularMonolith.BoundedContexts.Shopping.ValueObjects;

public sealed class StreetNameValueConverter
: ValueConverter<StreetName, string>
{
  public StreetNameValueConverter()
  : base(
    cn => cn.Value,
    value => new StreetName(value)
  ) { }
}
