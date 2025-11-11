namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public sealed class StreetNameValueConverter
: ValueConverter<StreetName, string>
{
  public StreetNameValueConverter()
  : base(
    cn => cn.Value,
    value => new StreetName(value)
  ) { }
}
