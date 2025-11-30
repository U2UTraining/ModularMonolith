using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Common.Commands;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

/// <summary>
/// Update the price of a board game.
/// </summary>
/// <param name="Game">The Game</param>
/// <param name="PriceInEuro">New price</param>
public sealed record UpdateGamePriceCommand(
  BoardGame Game
, Money PriceInEuro) 
: ICommand<bool>
{ }
