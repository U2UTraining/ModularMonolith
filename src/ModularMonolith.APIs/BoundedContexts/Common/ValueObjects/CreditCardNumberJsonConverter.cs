using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="CreditCardNumber"/> as a plain JSON string value.
/// </summary>
public sealed class CreditCardNumberJsonConverter : JsonConverter<CreditCardNumber>
{
  public override CreditCardNumber Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new CreditCardNumber(value);
  }

  public override void Write(Utf8JsonWriter writer, CreditCardNumber value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
