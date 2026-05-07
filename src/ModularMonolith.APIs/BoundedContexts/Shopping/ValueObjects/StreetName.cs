using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

[JsonConverter(typeof(StreetNameJsonConverter))]
public readonly record struct StreetName
{
  public const int StreetNameMaxLength = 100;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  [SetsRequiredMembers]
  public StreetName(string value)
  {
    _value = new NonEmptyString(value);
    if (value is string { Length: > StreetNameMaxLength })
    {
      throw new ArgumentException(
        message: $"StreetName should not exceed length of {StreetNameMaxLength}"
      , paramName: nameof(value));
    }
  }

  public static implicit operator string(StreetName fn) => fn.Value;
}
