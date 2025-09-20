namespace U2U.ModularMonolith.BoundedContexts.BoardGames.Specifications;

public static class BoardGameSpecification
{
  public static ISpecification<BoardGame> WithId(PK<int> id) 
  => new Specification<BoardGame>(g => g.Id == id);

  public static ISpecification<BoardGame> WithPublisherId(PK<int> pubId)
  => new Specification<BoardGame>(game
    => EF.Property<PK<int>>(game, "PublisherId") == pubId);

  public static ISpecification<BoardGame> WithPriceLowerThan(decimal maxPrice)
  => new Specification<BoardGame>(game => game.Price.Amount <= maxPrice);

  public static ISpecification<BoardGame> WithPriceHigherThan(decimal minPrice)
=> new Specification<BoardGame>(game => game.Price.Amount >= minPrice);

  public static ISpecification<BoardGame> WithPriceBetween(
    decimal minPrice
  , decimal maxPrice)
  => WithPriceHigherThan(minPrice).And(WithPriceLowerThan(maxPrice));
}
