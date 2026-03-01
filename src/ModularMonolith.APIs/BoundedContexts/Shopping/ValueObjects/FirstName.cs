namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public readonly record struct FirstName
{
  public const int FirstNameMaxLength = 128;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  [SetsRequiredMembers]
  public FirstName(string value)
  {
    _value = new NonEmptyString(value);
    if (value is string { Length: > FirstNameMaxLength })
    {
      throw new ArgumentException(
        message: $"FirstName should not exceed length of {FirstNameMaxLength}"
      , paramName: nameof(value));
    }
  }

  public static implicit operator string(FirstName fn) => fn.Value;
}
