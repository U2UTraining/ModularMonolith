namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

/// <summary>
/// Update the price of a board game.
/// </summary>
/// <param name="Game">The Game</param>
/// <param name="PriceInEuro">New price</param>
public sealed record UpdateGamePriceCommand(
  PK<int> BoardGameId
, Money PriceInEuro) 
: ICommand<bool>
;
