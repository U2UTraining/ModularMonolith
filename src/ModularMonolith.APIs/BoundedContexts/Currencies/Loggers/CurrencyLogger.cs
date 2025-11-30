namespace ModularMonolith.APIs.BoundedContexts.Currencies.Loggers;

public sealed partial class CurrencyLogger
{
  [LoggerMessage(
        EventId = 123
      , Level = LogLevel.Information
      , Message = "Updated currency at {timestamp}")]
  public static partial void UpdateCurrencyValueInEuroCommandInvoked(
        ILogger logger
      , DateTime timestamp
      , [LogProperties] UpdateCurrencyValueInEuroCommand command);
}