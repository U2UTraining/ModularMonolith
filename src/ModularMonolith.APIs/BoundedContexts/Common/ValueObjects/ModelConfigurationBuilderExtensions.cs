namespace ModularMonolith.BoundedContexts.Common.ValueObjects;

public static class ModelConfigurationBuilderExtensions
{
  /// <summary>
  /// Register all Value Objects with their converter
  /// </summary>
  /// <param name="configurationBuilder"></param>
  /// <remarks>Using in DbContext's ConfigureConventions method</remarks>
  public static void ConfigureValueObjectValueConverters(
    this ModelConfigurationBuilder configurationBuilder)
  {
    _ = configurationBuilder
      .Properties<NonEmptyString>()
      .HaveConversion<NonEmptyStringConverter>()
      ;

    _ = configurationBuilder
      .Properties<PositiveDecimal>()
      .HaveConversion<PositiveDecimalValueConverter>()
      ;

    _ = configurationBuilder
      .Properties<EmailAddress>()
      .HaveConversion<EmailAddressValueConverter>()
      .HaveMaxLength(EmailAddress.EmailMaxLength)
      ;

    _ = configurationBuilder
      .Properties<CreditCardNumber>()
      .HaveConversion<CreditCardNumberValueConverter>()
      .HaveMaxLength(CreditCardNumber.CreditCardNumberMaxLength)
      ;

    _ = configurationBuilder
      .Properties<PK<int>>()
      .HaveConversion<PKIntValueConverter>()
      ;
  }
}
