using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="Address"/> as a JSON object with Street and City properties.
/// </summary>
public sealed class AddressJsonConverter : JsonConverter<Address>
{
  public override Address Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    if (reader.TokenType != JsonTokenType.StartObject)
    {
      throw new JsonException("Expected start of JSON object for Address.");
    }

    StreetName? street = null;
    CityName? city = null;

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

      if (string.Equals(propertyName, nameof(Address.Street), StringComparison.OrdinalIgnoreCase))
      {
        string? streetValue = reader.GetString();
        ArgumentException.ThrowIfNullOrEmpty(streetValue);
        street = new StreetName(streetValue);
      }
      else if (string.Equals(propertyName, nameof(Address.City), StringComparison.OrdinalIgnoreCase))
      {
        string? cityValue = reader.GetString();
        ArgumentException.ThrowIfNullOrEmpty(cityValue);
        city = new CityName(cityValue);
      }
    }

    if (street is null || city is null)
    {
      throw new JsonException("Address JSON must contain both Street and City properties.");
    }

    return new Address(street.Value, city.Value);
  }

  public override void Write(Utf8JsonWriter writer, Address value, JsonSerializerOptions options)
  {
    writer.WriteStartObject();
    writer.WriteString(nameof(Address.Street), value.Street.Value);
    writer.WriteString(nameof(Address.City), value.City.Value);
    writer.WriteEndObject();
  }
}
