namespace ModularMonolithBoundedContexts.Common.Repositories;

/// <summary>
/// This is an automatic implementation for IReadonlyRepository.
/// </summary>
/// <typeparam name="T">The entity class' type.</typeparam>
/// <typeparam name="D">The DbContext to use.</typeparam>
public class ReadonlyRepository<T, D>
: IReadonlyRepository<T>
where T 
: class
, IAggregateRoot
where D 
: DbContext
{
  protected D DbContext { get; }

  public ReadonlyRepository(D dbContext)
  => DbContext = dbContext;

  protected virtual IQueryable<T> Includes(IQueryable<T> q)
  => q;

  protected internal IQueryable<T> BuildQueryable(
    ISpecification<T> specification) 
  => specification.BuildQueryable(
    Includes(DbContext.Set<T>().AsQueryable()));

  public virtual async ValueTask<IQueryable<T>> ListAsync(
    ISpecification<T> specification
  , CancellationToken token = default) 
  => await ValueTask.FromResult(BuildQueryable(specification));

  public virtual async ValueTask<T?> SingleAsync(
    ISpecification<T> specification
  , CancellationToken token = default) 
  => await BuildQueryable(specification)
    .SingleOrDefaultAsync(token);
}

