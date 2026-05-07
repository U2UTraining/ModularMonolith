using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="PublisherName"/> as a plain JSON string value.
/// </summary>
public sealed class PublisherNameJsonConverter : JsonConverter<PublisherName>
{
  public override PublisherName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new PublisherName(value);
  }

  public override void Write(Utf8JsonWriter writer, PublisherName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
