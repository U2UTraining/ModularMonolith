namespace ModularMonolithBoundedContexts.Shopping.ValueObjects;

public record struct Address(
  StreetName Street
, CityName City
) { }
