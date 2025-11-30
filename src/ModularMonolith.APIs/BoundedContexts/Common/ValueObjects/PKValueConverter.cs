namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

public sealed class PKIntValueConverter
: ValueConverter<PK<int>, int>
{
  public PKIntValueConverter()
  : base(
    id => id.Key
  , key => new PK<int>(key)
  ) { }
}
