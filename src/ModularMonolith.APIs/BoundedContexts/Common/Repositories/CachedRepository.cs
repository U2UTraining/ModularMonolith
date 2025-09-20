namespace ModularMonolith.BoundedContexts.Common.Repositories;

/// <summary>
/// This is an automatic implementation for IReadonlyRepository, with caching.
/// It uses the ReadonlyRepository to retrieve actual data, and stores it in the cache.
/// </summary>
/// <typeparam name="T">The entity class' type.</typeparam>
/// <typeparam name="D">The DbContext to use.</typeparam>
//public class CachedRepository<T, D> : IReadonlyRepository<T>
//  where T : class, IAggregateRoot
//  where D : DbContext
//{
//  protected ReadonlyRepository<T, D> innerRepo;
//  protected IMemoryCache cache;

//  public CachedRepository(D dbContext, IMemoryCache cache)
//  {
//    innerRepo = new ReadonlyRepository<T, D>(dbContext);
//    this.cache = cache;
//  }

//  public async ValueTask<IEnumerable<T>> ListAsync(
//    ISpecification<T> specification
//  , CancellationToken token = default)
//  {
//    return await cache.GetOrCreateAsync<IEnumerable<T>>(specification,
//      async spec => await innerRepo.ListAsync(specification, token)) ?? [];
//  }

//  public async ValueTask<T?> SingleAsync(
//    ISpecification<T> specification
//  , CancellationToken token = default)
//  {
//    return await cache.GetOrCreateAsync<T?>(specification,
//      async (spec) => await innerRepo.SingleAsync(specification, token));
//  }
//}

