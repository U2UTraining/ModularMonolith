using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="LastName"/> as a plain JSON string value.
/// </summary>
public sealed class LastNameJsonConverter : JsonConverter<LastName>
{
  public override LastName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new LastName(value);
  }

  public override void Write(Utf8JsonWriter writer, LastName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
