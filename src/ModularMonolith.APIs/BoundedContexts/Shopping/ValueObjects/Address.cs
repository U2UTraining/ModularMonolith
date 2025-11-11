namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public record struct Address(
  StreetName Street
, CityName City
) { }
