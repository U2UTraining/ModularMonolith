namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public record class ShoppingBasketDTO(
  int ShoppingBasketId
, List<GameDTO> Games
)
{
  public static ShoppingBasketDTO ToDTO(ShoppingBasket basket, IQueryable<BoardGame>? games)
  {

    ShoppingBasketDTO dto = new ShoppingBasketDTO(
      ShoppingBasketId: basket.Id.Key,
      Games: basket.Items.Select(item =>
      {
        BoardGame? game = games?.FirstOrDefault(g => g.Id == item.BoardGameId);
        return new GameDTO(
          Id: item.BoardGameId.Key,
          GameName: game?.Name ?? string.Empty,
          Price: item.Price.Amount,
          ImageURL: game?.ImageURL ?? string.Empty
        , PublisherName: game?.Publisher?.Name ?? string.Empty
        );
      }).ToList()
    );
    return dto;
  }
}


