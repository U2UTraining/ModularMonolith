using System.Text.Json;
using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Factory that creates a <see cref="JsonConverter"/> for any <see cref="PK{T}"/> closed generic type.
/// Serializes and deserializes <see cref="PK{T}"/> as the underlying key value.
/// </summary>
public sealed class PKJsonConverterFactory : JsonConverterFactory
{
  public override bool CanConvert(Type typeToConvert)
  {
    return typeToConvert.IsGenericType
        && typeToConvert.GetGenericTypeDefinition() == typeof(PK<>);
  }

  public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
  {
    Type keyType = typeToConvert.GetGenericArguments()[0];
    Type converterType = typeof(PKJsonConverter<>).MakeGenericType(keyType);

    return (JsonConverter?)Activator.CreateInstance(converterType);
  }
}

/// <summary>
/// Serializes and deserializes <see cref="PK{T}"/> as the underlying key value.
/// </summary>
public sealed class PKJsonConverter<T> : JsonConverter<PK<T>>
{
  public override PK<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
  {
    T? key = JsonSerializer.Deserialize<T>(ref reader, options);
    if (key is null)
    {
      throw new JsonException($"Cannot deserialize null as PK<{typeof(T).Name}>.");
    }

    return new PK<T>(key);
  }

  public override void Write(Utf8JsonWriter writer, PK<T> value, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, value.Key, options);
  }
}
