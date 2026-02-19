namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public record class ShoppingBasketDto(
  int ShoppingBasketId
, List<GameDto> Games
)
{
  public static ShoppingBasketDto ToDTO(ShoppingBasket basket, IQueryable<BoardGame>? games)
  {

    ShoppingBasketDto dto = new ShoppingBasketDto(
      ShoppingBasketId: basket.Id.Key,
      Games: basket.Items.Select(item =>
      {
        BoardGame? game = games?.FirstOrDefault(g => g.Id == item.BoardGameId);
        return new GameDto(
          Id: item.BoardGameId.Key,
          GameName: game?.Name ?? string.Empty,
          Price: item.Price.Amount,
          Currency: item.Price.Currency,
          ImageURL: game?.ImageURL ?? string.Empty
        , PublisherName: game?.Publisher?.Name ?? string.Empty
        );
      }).ToList()
    );
    return dto;
  }
}


