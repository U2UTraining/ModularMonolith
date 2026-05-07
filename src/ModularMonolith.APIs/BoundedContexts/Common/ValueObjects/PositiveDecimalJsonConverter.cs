using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="PositiveDecimal"/> as a plain JSON number value.
/// </summary>
public sealed class PositiveDecimalJsonConverter : JsonConverter<PositiveDecimal>
{
  public override PositiveDecimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    decimal value = reader.GetDecimal();

    return new PositiveDecimal(value);
  }

  public override void Write(Utf8JsonWriter writer, PositiveDecimal value, JsonSerializerOptions options)
  {
    writer.WriteNumberValue(value.Value);
  }
}
