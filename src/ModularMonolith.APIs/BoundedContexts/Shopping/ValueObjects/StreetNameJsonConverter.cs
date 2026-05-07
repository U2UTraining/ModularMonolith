using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="StreetName"/> as a plain JSON string value.
/// </summary>
public sealed class StreetNameJsonConverter : JsonConverter<StreetName>
{
  public override StreetName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new StreetName(value);
  }

  public override void Write(Utf8JsonWriter writer, StreetName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
