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

  private IQueryable<PublisherDTO>? _publishers;

  private async Task<IQueryable<PublisherDTO>> GetPublishersAsync()
  {
    IEnumerable<PublisherDTO> publishers =
      await PublishersClient.GetPublishersAsync();
    return publishers.AsQueryable();
  }

  protected override async Task OnInitializedAsync()
  {
    _publishers = await GetPublishersAsync()
                      .ConfigureAwait(false);
  }

  public PublisherDTO? SelectedPublisher
  {
    get; set;
  } = default;

  public IEnumerable<PublisherDTO> SelectedItems
  {
    get; set
    {
      field = value;
      if (field.Any())
      {
        OnSelectedPublisherChanged(value.First());
      }
    }
  } = [];

  public IQueryable<GameDTO>? Games
  {
    get; set;
  } = default;

  public async Task OnSelectedPublisherChanged(PublisherDTO selectedPublisher)
  {
    PublisherWithGamesDTO? pub = await PublishersClient.GetPublisherWithGamesAsync(selectedPublisher.Id);
    Games = pub?.Games.AsQueryable();
    await this.InvokeAsync(() => StateHasChanged());
  }

  //public IQueryable<Contact> Contacts 
  //=> SelectedItems.SelectMany(pub => pub.Contacts).AsQueryable();

  //public async Task AddBoardGame(Publisher publisher)
  //{
  //  BoardGameEditorViewModel tempGame = new("", 1.0M);
  //  DialogParameters parameters = new()
  //  {
  //    Height = "240px",
  //    Title = $"Add board game",
  //    PreventDismissOnOverlayClick = true,
  //    PreventScroll = true,
  //  };

  //  try
  //  {
  //    IDialogReference dialog = await DialogService.ShowDialogAsync<BoardGameEditorDialog>(
  //      tempGame
  //    , parameters)
  //    .ConfigureAwait(true) ;
  //    DialogResult result = await dialog.Result.ConfigureAwait(true);
  //    if (!result.Cancelled)
  //    {
  //      AddBoardGameToPublisherCommand cmd = new(
  //        PublisherId: publisher.Id
  //      , Name: new BoardGameName(tempGame.Name)
  //      , PriceInEuro: new Money(tempGame.Price));
  //      await Commander.ExecuteAsync(cmd, default)
  //                     .ConfigureAwait(false);
  //    }
  //  }
  //  catch (ArgumentException ex)
  //  {
  //    ToastService.ShowError(
  //      title: ex.Message);
  //  }
  //}
}
