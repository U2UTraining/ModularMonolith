using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="Percent"/> as a plain JSON number (the percentage value).
/// </summary>
public sealed class PercentJsonConverter : JsonConverter<Percent>
{
  public override Percent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    decimal value = reader.GetDecimal();

    return new Percent(value);
  }

  public override void Write(Utf8JsonWriter writer, Percent value, JsonSerializerOptions options)
  {
    writer.WriteNumberValue(value.Percentage);
  }
}
