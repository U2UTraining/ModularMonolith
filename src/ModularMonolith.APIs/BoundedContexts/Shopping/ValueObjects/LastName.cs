namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public readonly record struct LastName
{
  public const int LastNameMaxLength = 128;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  [SetsRequiredMembers]
  public LastName(string value)
  {
    _value = new NonEmptyString(value);
    if (value is string { Length: > LastNameMaxLength })
    {
      throw new ArgumentException(
        message: $"LastName should not exceed length of {LastNameMaxLength}"
      , paramName: nameof(value));
    }
  }

  public static implicit operator string(LastName fn) => fn.Value;

}
