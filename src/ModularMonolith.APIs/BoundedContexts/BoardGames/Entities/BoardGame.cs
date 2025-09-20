namespace U2U.ModularMonolith.BoundedContexts.BoardGames.Entities;

/// <summary>
/// Entity representing a board game.
/// </summary>
[DebuggerDisplay("Game {Name} - {Price.Amount}.")]
public sealed class BoardGame
: EntityBase<PK<int>>
, IAggregateRoot
, IHistory
, ISoftDeletable
{
  private static Money DefaultGamePrice { get; } = new(50M, CurrencyName.EUR);

  //public BoardGame()
  //: this(new PK<int>(0), new BoardGameName("Unknown"))
  //{ }

  /// <summary>
  /// Ctor for use by EF Core
  /// </summary>
  /// <param name="id">primary key</param>
  /// <param name="name">name</param>
  internal BoardGame(
    PK<int> id
  , BoardGameName name) 
  : base(id)
  {
    Name = name;
  }

  internal BoardGame(
    PK<int> id
  , BoardGameName name
  , Publisher publisher) 
  : this(id, name)
  {
    SetPrice(DefaultGamePrice);
    Publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
    Publisher.AddGame(this);
  }

  public BoardGameName Name { get; private set; } = default!;

  public Publisher Publisher { get; private set; } = default!;

  public Money Price { get; private set; } = default!;

  public GameImage Image { get; private set; } = default!;

  public string? ImageURL => Image?.ImageLocation.AbsoluteUri;

  public string PublisherName
    => Publisher.Name.Value;

  public void Rename(BoardGameName name)
  {
    Name = name;
  }

  public void SetPrice(in Money price)
  {
    Price = price;
    //this.RegisterDomainEvent(new GamePriceHasChanged(this));
  }

  public void SetImage(Uri imageUrl)
  {
    if (Image == null)
    {
      GameImage image = new(default, imageUrl);
      Image = image;
    }
    else
    {
      Image.SetImageUri(imageUrl);
    }
  }

  public void ChangePublisher(Publisher pub)
  {
    // Invariant: A game should always have a publisher!
    if (Publisher == pub)
    {
      return;
    }

    Publisher.RemoveGame(this);
    pub.AddGame(this);
    Publisher = pub;
  }
}

