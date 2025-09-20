namespace U2U.ModularMonolith.BoundedContexts.Shopping.ValueObjects;

public record struct Address(
  StreetName Street
, CityName City
) { }
