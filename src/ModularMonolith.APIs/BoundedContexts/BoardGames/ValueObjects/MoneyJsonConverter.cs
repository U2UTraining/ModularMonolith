using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="Money"/> as a JSON object with Amount and Currency properties.
/// </summary>
public sealed class MoneyJsonConverter : JsonConverter<Money>
{
  public override Money Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException("Expected start of JSON object for Money.");
    }

    decimal amount = 0;
    CurrencyName currency = CurrencyName.EUR;
    bool hasAmount = false;
    bool hasCurrency = false;

    while (reader.Read())
    {
      if (reader.TokenType == JsonTokenType.EndObject)
      {
        break;
      }

      if (reader.TokenType != JsonTokenType.PropertyName)
      {
        throw new JsonException("Expected property name.");
      }

      string propertyName = reader.GetString()!;
      reader.Read();

      if (string.Equals(propertyName, nameof(Money.Amount), StringComparison.OrdinalIgnoreCase))
      {
        amount = reader.GetDecimal();
        hasAmount = true;
      }
      else if (string.Equals(propertyName, nameof(Money.Currency), StringComparison.OrdinalIgnoreCase))
      {
        string? currencyValue = reader.GetString();
        ArgumentException.ThrowIfNullOrEmpty(currencyValue);
        currency = Enum.Parse<CurrencyName>(currencyValue, ignoreCase: true);
        hasCurrency = true;
      }
    }

    if (!hasAmount || !hasCurrency)
    {
      throw new JsonException("Money JSON must contain both Amount and Currency properties.");
    }

    return new Money(amount, currency);
  }

  public override void Write(Utf8JsonWriter writer, Money value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WriteNumber(nameof(Money.Amount), value.Amount);
    writer.WriteString(nameof(Money.Currency), value.Currency.ToString());
    writer.WriteEndObject();
  }
}
