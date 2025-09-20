namespace U2U.ModularMonolith.BoundedContexts.BoardGames.ValueObjects;

/// <summary>
/// PublisherName ensures that the Publisher name contraints are met
/// </summary>
public readonly record struct PublisherName
{
  public const int PublisherNameMaxLength = 128;

  public readonly NonEmptyString _value;

  public string Value => _value.Value;

  [SetsRequiredMembers]
  public PublisherName(string value)
  {
    _value = new NonEmptyString(value);
    if (value is string { Length: > PublisherNameMaxLength })
    {
      throw new ArgumentException(
        message: $"PublisherName should not exceed length of {PublisherNameMaxLength}"
      , paramName: nameof(value));
    }
  }
}
