namespace U2U.ModularMonolith.BoundedContexts.BoardGames.Specifications;

public static class PublisherSpecification
{
  public static ISpecification<Publisher> WithId(PK<int> id)
  {
    return new Specification<Publisher>(pub => pub.Id == id);
  }
}
