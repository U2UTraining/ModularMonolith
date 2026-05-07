using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="FirstName"/> as a plain JSON string value.
/// </summary>
public sealed class FirstNameJsonConverter : JsonConverter<FirstName>
{
  public override FirstName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new FirstName(value);
  }

  public override void Write(Utf8JsonWriter writer, FirstName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
