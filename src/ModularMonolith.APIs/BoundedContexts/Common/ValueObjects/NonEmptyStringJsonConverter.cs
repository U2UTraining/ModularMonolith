using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="NonEmptyString"/> as a plain JSON string value.
/// </summary>
public sealed class NonEmptyStringJsonConverter 
  : JsonConverter<NonEmptyString>
{
  public override NonEmptyString Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new NonEmptyString(value);
  }

  public override void Write(Utf8JsonWriter writer, NonEmptyString value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
