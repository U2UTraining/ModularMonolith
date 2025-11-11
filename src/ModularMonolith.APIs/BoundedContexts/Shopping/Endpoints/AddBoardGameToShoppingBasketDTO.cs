namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public record class AddBoardGameToShoppingBasketDTO(
  int ShoppingBasketId
, int BoardGameId
, decimal PriceInEuro
);
