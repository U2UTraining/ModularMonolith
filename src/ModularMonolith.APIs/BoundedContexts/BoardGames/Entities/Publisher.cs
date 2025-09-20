namespace ModularMonolithBoundedContexts.BoardGames.Entities;

/// <summary>
/// Entity representing a publisher of board games.
/// </summary>
[DebuggerDisplay("Publisher {Name,nq}")]
public sealed class Publisher 
: EntityBase<PK<int>>
, IAggregateRoot
, IHistory
, ISoftDeletable
{
  //[SetsRequiredMembers]
  //public Publisher()
  //: this(new PK<int>(0), new PublisherName("Unknown"))
  //{ }

  /// <summary>
  /// Ctor for use by EF Core
  /// </summary>
  /// <param name="id">id</param>
  /// <param name="name">name</param>
  [SetsRequiredMembers]
  public Publisher(PK<int> id, PublisherName name)
  : base(id)
  {
    Name = name;
  }

  //[SetsRequiredMembers]
  //public Publisher(int id, PublisherName name)
  //: this(id, name.Value)
  //{ }

  /// <summary>
  /// Immutable name of the publisher.
  /// </summary>
  public required PublisherName Name { get; init; }

  /// <summary>
  /// A publisher can have multiple board games.
  /// </summary>
  public IEnumerable<BoardGame> Games => GetGamesList();

  private List<BoardGame>? _games;

  private List<BoardGame> GetGamesList() 
  => _games ??= [];

  internal void AddGame(BoardGame g) 
  => GetGamesList().Add(g);

  internal void RemoveGame(BoardGame g) 
  => _ = GetGamesList().Remove(g);

  /// <summary>
  /// Factory method to create a new board game.
  /// </summary>
  /// <param name="name">Board game name</param>
  /// <param name="priceInEuro">Price in Euro</param>
  /// <returns></returns>
  public BoardGame CreateGame(
    BoardGameName name
  , Money priceInEuro)
  {
    // Invariant checked in debug builds only
    Debug.Assert(priceInEuro.Currency == CurrencyName.EUR);
    BoardGame newGame =  new BoardGame(
      id: default
    , name: name
    , publisher: this);
    newGame.SetPrice(priceInEuro);
    return newGame;
  }

  private List<Contact> _contacts = [];

  public IEnumerable<Contact> Contacts => _contacts.AsEnumerable();

  public void AddContact(
    NonEmptyString firstName
  , NonEmptyString lastName
  , EmailAddress email
  )
  => _contacts.Add(new Contact(default, firstName, lastName, email));
}
