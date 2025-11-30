using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

/// <summary>
/// Add a board game to a publisher.
/// </summary>
/// <param name="PublisherId">Id of publisher</param>
/// <param name="Name">Board game name</param>
/// <param name="PriceInEuro">Board game price</param>
public sealed record class AddBoardGameToPublisherCommand(
  PK<int> PublisherId
, BoardGameName Name
, Money PriceInEuro)
: ICommand<Publisher>
{ }
