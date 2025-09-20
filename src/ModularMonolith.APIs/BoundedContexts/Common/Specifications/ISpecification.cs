namespace ModularMonolithBoundedContexts.Common.Specifications;

/// <summary>
/// A ISpecification represents the filter (Where) and includes of a query,
/// to be used with an IReadonlyRepository for finding entities.
/// </summary>
/// <typeparam name="T">The entity class' type.</typeparam>
public interface ISpecification<T>
: IQuery<IQueryable<T>>
where T 
: class
, IAggregateRoot
{
  /// <summary>
  /// The filter criteria.
  /// </summary>
  Expression<Func<T, bool>> Criteria { get; }

  /// <summary>
  /// True if the instance passes the specification
  /// </summary>
  /// <param name="t">The instance to test</param>
  /// <returns></returns>
  bool Test(in T t);

  /// <summary>
  /// Other entities to include, max. 1 level deep.
  /// </summary>
  IEnumerable<Expression<Func<T, object>>> Includes { get; }

  /// <summary>
  /// Return a new specification that includes a list.
  /// </summary>
  /// <param name="includes"></param>
  /// <returns></returns>
  ISpecification<T> Include(
    IEnumerable<Expression<Func<T, object>>> includes
  , bool withQuerySplitting = false);

  /// <summary>
  /// Return a new specification that includes the extra entity.
  /// </summary>
  /// <param name="include"></param>
  /// <returns></returns>
  ISpecification<T> Include(
    Expression<Func<T, object>> include
  , bool withQuerySplitting = false);

  /// <summary>
  /// This method extends the IQueryable with the specification.
  /// </summary>
  /// <param name="q">The IQueryable<typeparamref name="T"/> to query.</param>
  /// <returns>The IQueryable<typeparamref name="T"/>A new IQueryable with Where(spec) added.</returns>
  IQueryable<T> BuildQueryable(IQueryable<T> q);

  /// <summary>
  /// Stop query from tracking changes.
  /// </summary>
  /// <returns></returns>
  ISpecification<T> AsNoTracking();
}
