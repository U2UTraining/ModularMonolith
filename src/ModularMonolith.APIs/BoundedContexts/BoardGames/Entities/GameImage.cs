using ModularMonolith.APIs.BoundedContexts.Common.Entities;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;

/// <summary>
/// Entity representing an image of a board game.
/// </summary>
/// <remarks>
/// This could also have been a property on the BoardGame entity.
/// Here it is used to demo a one-to-one relationship
/// </remarks>
public sealed class GameImage
: EntityBase<PK<int>>
, IAggregate<BoardGame>
, IHistory
, ISoftDeletable
{
  internal GameImage(PK<int> id, Uri imageLocation)
  : base(id) 
  => ImageLocation = imageLocation;

  public Uri ImageLocation { get; private set; }

  public void SetImageUri(Uri url) 
  => ImageLocation = url;
}
