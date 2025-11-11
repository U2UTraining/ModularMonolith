namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public sealed class FirstNameValueConverter
: ValueConverter<FirstName, string>
{
  public FirstNameValueConverter()
  : base(
    cn => cn.Value,
    value => new FirstName(value)
  ) { }
}
