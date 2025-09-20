namespace U2U.ModularMonolith.BoundedContexts.Shopping.ValueObjects;

public readonly record struct CityName
{
  public const int CityNameMaxLength = 128;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  [SetsRequiredMembers]
  public CityName(string value)
  {
    _value = new NonEmptyString(value);
    if (value is string { Length: > CityNameMaxLength })
    {
      throw new ArgumentException(
        message: $"CityName should not exceed length of {CityNameMaxLength}"
      , paramName: nameof(value));
    }
  }

  public static implicit operator string(CityName fn) => fn.Value;

}
