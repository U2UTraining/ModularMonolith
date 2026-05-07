using ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.Loggers;

public static partial class BoardGamesLogger
{
  [LoggerMessage(
    EventId = 123
  , Level = LogLevel.Information
  , Message = "[BoardGames] Updated Game Price at {timestamp}")]
  public static partial void UpdateGamePriceCommandInvoked(
    ILogger logger
  , DateTime timestamp
  , [LogProperties] UpdateGamePriceCommand command);
}