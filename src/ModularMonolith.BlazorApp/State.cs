using System.ComponentModel;

using ModularMonolith.BlazorApp.Components;

namespace ModularMonolith.BlazorApp;

/// <summary>
/// Singleton class holding the application's state
/// </summary>
public sealed record class State
: INotifyPropertyChanged
{
  private const string ShoppingBasketIdSessionKey = "SBK";
  private ComponentWithState? _currentPage;

  public State(
    IHttpContextAccessor contextAccessor
  )
  {
    _contextAccessor = contextAccessor;
  }

  //public static State Instance
  //{
  //  get;
  //} = new();

  public int? ShoppingBasketId
  {
    get;
    //{
    //  HttpContext? context = _contextAccessor.HttpContext;
    //  if (context is not null)
    //  {
    //    return context.Session.GetInt32(ShoppingBasketIdSessionKey);
    //  }
    //  return null;
    //}
    set;
    //{
    //  HttpContext? context = _contextAccessor.HttpContext;
    //  if (context is not null)
    //  {
    //    if (value is not null)
    //    {
    //      context.Session.SetInt32(ShoppingBasketIdSessionKey, value.Value);
    //    }
    //    else
    //    {
    //      context.Session.Remove(ShoppingBasketIdSessionKey);
    //    }
    //  }
    //}
  }

  public IQueryable<GameDto>? Games
  {
    get; set;
  }

  /// <summary>
  /// The current page path being shown in the UI.
  /// </summary>
  public ComponentWithState CurrentPage
  {
    get => _currentPage;
  }

  //public Dispatcher Dispatcher
  //{
  //  get;
  //}

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
  private readonly IHttpContextAccessor _contextAccessor;

  /// <summary>
  /// Sets the current page and notifies subscribers when it changes.
  /// </summary>
  /// <param name="currentPage">The current route path.</param>
  public void SetCurrentPage(ComponentWithState? currentPage)
  {
    _currentPage = currentPage;
  }

  public void StateHasChanged()
  => PropertyChanged?.Invoke(this, _placeholder);
}
