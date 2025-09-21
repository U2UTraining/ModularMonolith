using System.ComponentModel;

namespace U2U.ModularMonolith;

/// <summary>
/// Singleton class holding the application's state
/// </summary>
public sealed record class State
: INotifyPropertyChanged
{
  private State()
  { }

  public static State Instance
  {
    get;
  } = new();

  public IQueryable<GameDTO>? Games
  {
    get; set;
  }

  //public PK<int> ShoppingBasketId { get; set; } = default;

  //public PK<int> ShipmentId
  //{
  //  get; set;
  //}

  //public HandlingEventName HandlingEventName { get; set; } = HandlingEventName.Open;

  public event PropertyChangedEventHandler? PropertyChanged;

  /// <summary>
  /// Flyweight used during triggering of the event
  /// </summary>
  private readonly PropertyChangedEventArgs _placeholder = new(string.Empty);

  public void StateHasChanged() 
  => PropertyChanged?.Invoke(this, _placeholder);
}
