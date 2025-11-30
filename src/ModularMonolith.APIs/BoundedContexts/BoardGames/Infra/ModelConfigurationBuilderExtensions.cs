namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

public static class ModelConfigurationBuilderExtensions
{
  /// <summary>
  /// Register all Value Objects with their converter
  /// </summary>
  /// <param name="configurationBuilder"></param>
  public static void ConfigureBoardGameValueObjectValueConverters(this ModelConfigurationBuilder configurationBuilder)
  {
    _ = configurationBuilder
      .Properties<BoardGameName>()
      .HaveConversion<BoardGameNameValueConverter>()
      .HaveMaxLength(BoardGameName.BoardGameNameMaxLength)
      ;

    _ = configurationBuilder
      .Properties<PublisherName>()
      .HaveConversion<PublisherNameValueConverter>()
      .HaveMaxLength(PublisherName.PublisherNameMaxLength)
      ;
  }
}
