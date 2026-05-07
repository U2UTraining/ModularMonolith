using System.Text.Json.Serialization;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

[JsonConverter(typeof(AddressJsonConverter))]
public record struct Address(
  StreetName Street
, CityName City
) { }
