namespace ModularMonolith.APIs.BoundedContexts.Shopping.Endpoints;

public static class ShoppingBasketEndpoints
{
  extension(RouteGroupBuilder group)
  {
    public RouteGroupBuilder WithShoppingBasketEndpoints()
    {
      var unused2 = group.MapGet("/{id:int}", async (
        [FromServices] GetShoppingBasketWithId handler
      , [FromRoute] int id
      , CancellationToken cancellationToken)
      => await handler.ExecuteAsync(id, cancellationToken)
      )
      .WithName(nameof(GetShoppingBasketWithId))
      .Produces<List<Currency>>(StatusCodes.Status200OK);

      var unused1 = group.MapPost("/", async (
        [FromServices] CreateShoppingBasket handler
      , CancellationToken cancellationToken)
      => await handler.ExecuteAsync(cancellationToken)
      )
      .WithName(nameof(CreateShoppingBasket));

      var unused = group.MapPut("/", async (
        [FromServices] AddBoardGameToShoppingBasket handler
      , [FromBody] AddBoardGameToShoppingBasketDto dto
      , CancellationToken cancellationToken)
      => await handler.ExecuteAsync(dto, cancellationToken)
      )
      .WithName(nameof(AddBoardGameToShoppingBasket));
      return group;
    }
  }
}
