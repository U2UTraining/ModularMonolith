namespace ModularMonolith.APIs.BoundedContexts.Shopping.IntegrationEvents;

public sealed record class ShoppingBasketHasCheckedOutIntegrationEvent
(
  int ShoppingBasketId,
  string CustomerFirstName,
  string CustomerLastName,
  string CustomerStreet,
  string CustomerCity,
  int[] Games
)
: IIntegrationEvent
{ }
