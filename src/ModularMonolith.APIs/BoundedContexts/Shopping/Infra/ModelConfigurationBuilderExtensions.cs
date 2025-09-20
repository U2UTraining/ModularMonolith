namespace ModularMonolith.BoundedContexts.Shopping.Infra;

public static class ModelConfigurationBuilderExtensions
{
  public static void ConfigureShoppingValueObjectValueConverters(this ModelConfigurationBuilder configurationBuilder)
  {
    _ = configurationBuilder
      .Properties<FirstName>()
      .HaveConversion<FirstNameValueConverter>()
      .HaveMaxLength(FirstName.FirstNameMaxLength)
      ;

    _ = configurationBuilder
      .Properties<LastName>()
      .HaveConversion<LastNameValueConverter>()
      .HaveMaxLength(LastName.LastNameMaxLength)
      // .HavePrecision(4,2)
      ;

    _ = configurationBuilder
      .Properties<StreetName>()
      .HaveConversion<StreetNameValueConverter>()
      .HaveMaxLength(StreetName.StreetNameMaxLength)
      ;

    _ = configurationBuilder
      .Properties<CityName>()
      .HaveConversion<CityNameValueConverter>()
      .HaveMaxLength(CityName.CityNameMaxLength)
      ;
  }
}
