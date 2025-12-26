namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public record class AddBoardGameToShoppingBasketDto(
  int ShoppingBasketId
, int BoardGameId
, decimal PriceInEuro
);
