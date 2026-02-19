using ModularMonolith.APIs.BoundedContexts.BoardGames.Entities;

namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed partial class PublishersPage
{
  [Inject]
  public required IToastService ToastService
  {
    get; set;
  }

  [Inject]
  public required IDialogService DialogService
  {
    get; set;
  }

  [Inject]
  public required PublishersClient PublishersClient
  {
    get; set;
  }

  private IQueryable<PublisherDto>? _publishers;

  private async Task<IQueryable<PublisherDto>> GetPublishersAsync()
  {
    IEnumerable<PublisherDto> publishers =
      await PublishersClient.GetPublishersAsync();
    return publishers.AsQueryable();
  }

  protected override async Task OnInitializedAsync()
  {
    _publishers = await GetPublishersAsync()
                      .ConfigureAwait(false);
  }

  public PublisherDto? SelectedPublisher
  {
    get; set;
  } = default;

  public IEnumerable<PublisherDto> SelectedItems
  {
    get; set
    {
      field = value;
      if (field.Any())
      {
        _ = OnSelectedPublisherChanged(value.First());
      }
    }
  } = [];

  public IQueryable<GameDto>? Games
  {
    get; set;
  } = default;

  public async Task OnSelectedPublisherChanged(PublisherDto selectedPublisher)
  {
    PublisherWithGamesDto? pub = await PublishersClient.GetPublisherWithGamesAsync(selectedPublisher.Id);
    Games = pub?.Games.AsQueryable();
    Contacts = pub?.Contacts.AsQueryable();
    await this.InvokeAsync(() => StateHasChanged());
  }

  public IQueryable<ContactDto>? Contacts
  {
    get; set;
  } = default!;

  public async Task AddBoardGame(PublisherDto publisher)
  {
    BoardGameEditorViewModel tempGame = new("", 1.0M);
    DialogParameters parameters = new()
    {
      Height = "240px",
      Title = $"Add board game",
      PreventDismissOnOverlayClick = true,
      PreventScroll = true,
    };

    try
    {
      IDialogReference dialog = await DialogService.ShowDialogAsync<BoardGameEditorDialog>(
        tempGame
      , parameters)
      .ConfigureAwait(true);
      DialogResult result = await dialog.Result.ConfigureAwait(true);
      if (!result.Cancelled)
      {
        //AddBoardGameToPublisherCommand cmd = new(
        //  PublisherId: publisher.Id
        //, Name: new BoardGameName(tempGame.Name)
        //, PriceInEuro: new Money(tempGame.Price));
        //await Commander.ExecuteAsync(cmd, default)
        //               .ConfigureAwait(false);
      }
    }
    catch (ArgumentException ex)
    {
      ToastService.ShowError(
        title: ex.Message);
    }
  }

  public async Task EditBoardGame(GameDto game)
  {
    BoardGameEditorViewModel tempGame = new(game.GameName, game.Price);
    DialogParameters parameters = new()
    {
      Height = "240px",
      Title = $"Edit board game",
      PreventDismissOnOverlayClick = true,
      PreventScroll = true,
    };

    try
    {
      IDialogReference dialog = await DialogService.ShowDialogAsync<BoardGameEditorDialog>(
        tempGame
      , parameters)
      .ConfigureAwait(true);
      DialogResult result = await dialog.Result.ConfigureAwait(true);
      if (!result.Cancelled)
      {
        GameDto gameDto = new GameDto(
          Id: game.Id
        , GameName: tempGame.Name
        , Price: tempGame.Price
        , Currency: CurrencyName.EUR
        , ImageURL: game.ImageURL
        , PublisherName: string.Empty
        );
        await PublishersClient.UpdateGameAsync(gameDto);
#pragma warning disable CA2245 // Do not assign a property to itself
#pragma warning disable S1656 // Variables should not be self-assigned
        SelectedItems = SelectedItems;
#pragma warning restore S1656 // Variables should not be self-assigned
#pragma warning restore CA2245 // Do not assign a property to itself
      }
    }
    catch (ArgumentException ex)
    {
      ToastService.ShowError(
        title: ex.Message);
    }
  }

}
