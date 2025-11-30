using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

/// <summary>
/// A board game name should be a non-empty string with a maximum length of 128.
/// </summary>
public readonly record struct BoardGameName 
{
  public const int BoardGameNameMaxLength = 128;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  public BoardGameName(string value)
  {
    _value = new NonEmptyString(value);
    if(value is string {  Length: > BoardGameNameMaxLength })
    {
      throw new ArgumentException(
        message: $"BoardGameName should not exceed length of {BoardGameNameMaxLength}"
      , paramName: nameof(value));
    }
  }

  public static implicit operator string(BoardGameName nes)
  => nes.Value;

}
