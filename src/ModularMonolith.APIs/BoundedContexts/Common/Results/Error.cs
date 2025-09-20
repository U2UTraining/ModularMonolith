namespace ModularMonolithBoundedContexts.Common.Results;

/// <summary>
/// Represents application errors.
/// </summary>
/// <param name="Code">Unique name for the error</param>
/// <param name="Message">Developer-friendly message</param>
/// <param name="Inner">Optional mapped exception</param>
public readonly record struct Error(
  string Code
, string Message)
{
  public static Error None { get; } 
  = new Error(string.Empty, string.Empty);

  public static Error FromMessage(
    string code
  , string message) 
  => new Error(code, message);
}
