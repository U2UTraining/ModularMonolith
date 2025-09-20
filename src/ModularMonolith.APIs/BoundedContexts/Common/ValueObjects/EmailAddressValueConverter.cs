namespace ModularMonolith.BoundedContexts.Common.ValueObjects;

public sealed class EmailAddressValueConverter
: ValueConverter<EmailAddress, string>
{
  public EmailAddressValueConverter()
  : base(
    email => email.Value,
    value => new EmailAddress(value)
  )
  { }
}
