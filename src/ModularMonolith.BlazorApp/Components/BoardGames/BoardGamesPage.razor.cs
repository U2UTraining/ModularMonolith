using System.ComponentModel;

using U2U.ModularMonolith;

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

  // Easy Access
  private IQueryable<GameDTO>? Games => State.Games;

  private async Task<IQueryable<GameDTO>> GetBoardGames()
  {
    IEnumerable<GameDTO> result =
      await BoardGamesClient.GetGamesAsync(default);
    //await Sender.AskAsync(GetAllGamesQuery.WithPublisher, default)
    //            .ConfigureAwait(true);
    return result.AsQueryable();
  }

  protected override async Task OnInitializedAsync()
  {
    State.Games = await GetBoardGames().ConfigureAwait(true);

    State.PropertyChanged += OnPropertyChanged;
  }

  public void Dispose()
  {
    State.PropertyChanged += OnPropertyChanged;
  }

  private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
  => this.StateHasChanged();

  private async Task AddBoardGameToBasket(GameDTO game)
  {
    //AddGameToShoppingBasketCommand cmd = new(State.ShoppingBasketId, game.Id, game.Price);
    //State.ShoppingBasketId = await Sender.Execute(cmd, default);
    await Task.CompletedTask.ConfigureAwait(true);
  }

  //public async Task UpdateGame()
  //{
  //  if (_games is not null)
  //  {
  //    UpdateGamePriceCommand cmd = new(_games[0], new Money(10));
  //    await Sender.Execute(cmd);
  //  }
  //}

  private string BoardGameImageURL(GameDTO game)
  => game.ImageUrl ?? "https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_BoardGame.jpg";

  private async Task GiveGlobalDiscount()
  {
    //await Commander.ExecuteAsync(
    //  new Commands.ApplyMegaDiscountCommand(true, new Percent(10M))
    //).ConfigureAwait(true);
    await Task.CompletedTask;
  }

  private async Task Filter()
  {
    //Money minPrice = new Money(10);
    //Money maxPrice = new Money(30);
    //IQueryable<BoardGame> filteredGames = 
    //  await Sender.AskAsync(BoardGameSpecification.WithPriceBetween(10, 30)).ConfigureAwait(true);
    ////IQueryable<BoardGame> filteredGames = await Sender.Ask(GameSpecification.WithPriceHigherThan(30));
    //Console.WriteLine(filteredGames.Count());
    await Task.CompletedTask;
  }
}
