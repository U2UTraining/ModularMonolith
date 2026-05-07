using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Serializes and deserializes <see cref="EmailAddress"/> as a plain JSON string value.
/// </summary>
public sealed class EmailAddressJsonConverter : JsonConverter<EmailAddress>
{
  public override EmailAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    string? value = reader.GetString();
    ArgumentException.ThrowIfNullOrEmpty(value);

    return new EmailAddress(value);
  }

  public override void Write(Utf8JsonWriter writer, EmailAddress value, JsonSerializerOptions options)
  {
    writer.WriteStringValue(value.Value);
  }
}
