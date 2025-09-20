namespace ModularMonolithBoundedContexts.Shopping.ValueObjects;

public sealed class LastNameValueConverter
: ValueConverter<LastName, string>
{
  public LastNameValueConverter()
  : base(
    cn => cn.Value,
    value => new LastName(value)
  ) { }
}
