namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public record class ShoppingBasketDTO(
  int ShoppingBasketId
, List<BoardGameDTO> Games
);

public record class BoardGameDTO(

);
