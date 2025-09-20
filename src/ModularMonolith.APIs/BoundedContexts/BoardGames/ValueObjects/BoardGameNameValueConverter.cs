namespace U2U.ModularMonolith.BoundedContexts.BoardGames.ValueObjects;

public sealed class BoardGameNameValueConverter
: ValueConverter<BoardGameName, string>
{
  public BoardGameNameValueConverter()
  : base(
    bgn => bgn.Value,
    value => new BoardGameName(value)
  ) { }
}
