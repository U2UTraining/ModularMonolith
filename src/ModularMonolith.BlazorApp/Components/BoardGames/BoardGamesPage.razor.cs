using System.ComponentModel;

using ModularMonolith.APIs.BoundedContexts.BoardGames.Queries;
using ModularMonolith.BlazorApp.Components.Shopping;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed partial class BoardGamesPage
: IDisposable
{
  [Inject]
  public required State State
  {
    get; init;
  }

  [Inject]
  public required BoardGamesClient BoardGamesClient
  {
    get; init;
  }

  [Inject]
  public required ShoppingBasketClient ShoppingBasketClient
  {
    get; init;
  }

  // Easy Access
  private IQueryable<GameDto>? Games => State.Games;

  private GetGamesQuery filter = new(decimal.Zero, 1000M, false);

  private async Task<IQueryable<GameDto>> GetBoardGames(GetGamesQuery query)
  {
    IEnumerable<GameDto> result =
      await BoardGamesClient.GetGamesAsync(query);
    return result.AsQueryable();
  }

  protected override async Task OnInitializedAsync()
  {
    State.Games = await GetBoardGames(filter).ConfigureAwait(true);

    State.PropertyChanged += OnPropertyChanged;
  }

  public void Dispose()
  {
    State.PropertyChanged += OnPropertyChanged;
  }

  private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
  => this.InvokeAsync(StateHasChanged);

  private async Task AddBoardGameToBasket(GameDto game)
  {
    if (State.ShoppingBasketId is null)
    {
      int? shoppingBasketId = await ShoppingBasketClient.CreateShoppingBasket();
      if (shoppingBasketId is null)
      {
        // TODO Show error
        return;
      }
      else
      {
        State.ShoppingBasketId = shoppingBasketId;
      }
    }
    await ShoppingBasketClient.SelectBoardGame(
      shoppingBasketId: State.ShoppingBasketId!.Value
    , boardGameId: game.Id
    , priceInEuro: game.Price);
  }

  //public async Task UpdateGame()
  //{
  //  if (_games is not null)
  //  {
  //    UpdateGamePriceCommand cmd = new(_games[0], new Money(10));
  //    await Sender.Execute(cmd);
  //  }
  //}

  private static string BoardGameImageURL(GameDto game)
  => game.ImageURL ?? "https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_BoardGame.jpg";

  private async Task GiveGlobalDiscount()
  {
    await BoardGamesClient.ApplyMegaDiscountAsync();
    State.Games = await GetBoardGames(filter).ConfigureAwait(true);
  }

  private async Task Filter()
  {
    filter = new GetGamesQuery(10M, 30M, false);
    State.Games = await GetBoardGames(filter).ConfigureAwait(true);
  }
}
