namespace ModularMonolith.BoundedContexts.BoardGames.ValueObjects;

public sealed class PublisherNameValueConverter
: ValueConverter<PublisherName, string>
{
  public PublisherNameValueConverter()
  : base(
    pn => pn.Value,
    value => new PublisherName(value)
  )
  { }
}
