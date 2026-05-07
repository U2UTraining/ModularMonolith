using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="CityName"/> as a plain JSON string value.
/// </summary>
public sealed class CityNameJsonConverter : JsonConverter<CityName>
{
  public override CityName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new CityName(value);
  }

  public override void Write(Utf8JsonWriter writer, CityName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
