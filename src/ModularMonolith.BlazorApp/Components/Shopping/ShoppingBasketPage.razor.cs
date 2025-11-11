using ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

namespace ModularMonolith.BlazorApp.Components.Shopping;

public sealed partial class ShoppingBasketPage
{
  [Inject]
  public required State State
  {
    get; set;
  }

  [Inject]
  public required ShoppingBasketClient ShoppingBasketClient
  {
    get; set;
  }

  //[Inject]
  //public required IIntegrationEventPublisher integrationEventPublisher { get; set; }

  private ShoppingBasketDTO? ShoppingBasket
  {
    get; set;
  }

  private IQueryable<BoardGameDTO> _games = default!;

  protected override async Task OnInitializedAsync()
  {

    State.PropertyChanged += (_, __) => InvokeAsync(() => StateHasChanged());

    if (State.ShoppingBasketId is not null)
    {
      ShoppingBasket =
        await ShoppingBasketClient.GetShoppingBasket(State.ShoppingBasketId.Value);
      //  await CQSender.Ask(new GetShoppingBasketQuery(State.ShoppingBasketId), default);

      //if (ShoppingBasket is not null)
      //{
      //  PK<int>[] gameIds = ShoppingBasket.Items.Select(item => item.BoardGameId).ToArray();
      //  _games = await CQSender.Ask(new GetGamesFromListQuery(gameIds));
      //}
    }
  }

  private async Task RemoveGameFromBasket(BoardGameDTO game)
  {
    await Task.CompletedTask;
  }

  //  private string BoardGameImageURL(BoardGameDTO game)
  //=> game.ImageURL ?? "https://u2ublogimages.blob.core.windows.net/cleanarchitecture/GamesStore_BoardGame.jpg";

  //private async Task CheckOut() 
  //=> await CQSender.Execute(new CheckOutShoppingBasketCommand(ShoppingBasket!));

  //public bool ShowCheckOutButton
  //=> ShoppingBasket is not null && State.HandlingEventName == HandlingEventName.Open;

  //private async Task Boxed()
  //  => await CQSender.Execute(new BoxShipmentCommand(State.ShipmentId));

  //public bool ShowBoxedButton => State.HandlingEventName == HandlingEventName.Created;

  //private async Task Move()
  //    => await CQSender.Execute(new MoveShipmentCommand(State.ShipmentId, "Molendries 1", "Aalst"), default);

  //=> await integrationEventPublisher.PublishIntegrationEventAsync(
  //    new ShippingHasMovedIntegrationEvent(State.ShipmentId, "Molendries 1", "Aalst"), default);

  //public bool ShowMoveButton => State.HandlingEventName is HandlingEventName.Boxed or HandlingEventName.Moved;

  //private async Task Deliver()
  //  => await CQSender.Execute(new MoveShipmentCommand(State.ShipmentId, "ResearchPark 110", "Zellik"), default);

  //=> await integrationEventPublisher.PublishIntegrationEventAsync(
  //    new ShippingHasMovedIntegrationEvent(State.ShipmentId, "ResearchPark 110", "Zellik"), default);

  //public bool ShowDeliverButton => ShowMoveButton;
}
