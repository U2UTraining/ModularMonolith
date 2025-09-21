namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed partial class Publishers
{
  [Inject]
  public required IToastService ToastService { get; set; }

  [Inject]
  public required IDialogService DialogService { get; set; }

  private IQueryable<PublisherDTO>? _publishers;

  private async Task<IQueryable<PublisherDTO>> GetPublishersAsync()
  {
    IEnumerable<PublisherDTO> result = new List<PublisherDTO>();
    //await Sender.AskAsync(GetAllPublishersQuery.WithGames, default)
    //            .ConfigureAwait(false);
    await Task.CompletedTask; // Remove warning
    return result.AsQueryable();
    ;
  }

  protected override async Task OnInitializedAsync()
  {
    _publishers = await GetPublishersAsync()
                      .ConfigureAwait(false);
  }

  //public Publisher? SelectedPublisher { get; set; }

  //public IEnumerable<Publisher> SelectedItems {get;set;} = [];

  //public IQueryable<BoardGame> Games 
  //=> SelectedItems.SelectMany(pub => pub.Games).AsQueryable();

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
