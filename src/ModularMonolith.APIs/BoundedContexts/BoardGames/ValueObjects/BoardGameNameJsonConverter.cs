using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="BoardGameName"/> as a plain JSON string value.
/// </summary>
public sealed class BoardGameNameJsonConverter : JsonConverter<BoardGameName>
{
  public override BoardGameName Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new BoardGameName(value);
  }

  public override void Write(Utf8JsonWriter writer, BoardGameName value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
