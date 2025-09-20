namespace ModularMonolithBoundedContexts.BoardGames.Specifications;

public sealed class PublisherWithIdSpecification
: Specification<Publisher>
{
  public PublisherWithIdSpecification(PK<int> id)
  : base(pub => pub.Id == id)
  { }
}
