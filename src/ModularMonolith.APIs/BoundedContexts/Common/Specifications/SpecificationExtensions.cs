namespace ModularMonolith.APIs.BoundedContexts.Common.Specifications;

/// <summary>
/// Extensions methods for ISpecification.
/// </summary>
public static class SpecificationExtensions
{
  /// <summary>
  /// Turn specification into a cached specification.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <typeparam name="K"></typeparam>
  /// <param name="spec">Inner specification</param>
  /// <param name="duration">Cache duration</param>
  /// <param name="key">Cache key</param>
  /// <returns></returns>
  public static ISpecification<T> AsCached<T, K>(
    this ISpecification<T> spec
  , TimeSpan duration
  , K key)
  where T
  : class
  , IAggregateRoot 
  => new CachedSpecification<T, K>(spec.Criteria, spec.Includes, duration, key);

  /// <summary>
  /// Include an entity in the specification.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="spec"></param>
  /// <param name="include"></param>
  /// <returns></returns>
  public static ISpecification<T> Including<T>(
    this ISpecification<T> spec
  , Expression<Func<T, object>> include)
  where T
  : class
  , IAggregateRoot 
  => spec.Include(include);

  /// <summary>
  /// Combine two specifications with an AND operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="left">ISpecification</param>
  /// <param name="right">ISpecification</param>
  /// <returns>ISpecification</returns>

  public static ISpecification<T> And<T>(
    this ISpecification<T> left
  , ISpecification<T> right)
  where T
  : class
  , IAggregateRoot 
  => left.And(right.Criteria);

  /// <summary>
  /// Combine two specifications with an AND operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="left">ISpecification</param>
  /// <param name="right">ISpecification</param>
  /// <returns>ISpecification</returns>
  public static ISpecification<T> And<T>(
    this ISpecification<T> left
  , Expression<Func<T, bool>> rightExpression)
  where T 
  : class
  , IAggregateRoot
  {
    Expression<Func<T, bool>> leftExpression = left.Criteria;

    SwapVisitor visitor = new(leftExpression.Parameters[0], rightExpression.Parameters[0]);
    BinaryExpression lazyAnd = Expression.AndAlso(visitor.Visit(leftExpression.Body)!, rightExpression.Body);
    Expression<Func<T, bool>> and = Expression.Lambda<Func<T, bool>>(lazyAnd, rightExpression.Parameters);
    return new Specification<T>(and);
  }

  /// <summary>
  /// Combine two specifications with an OR operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="left">ISpecification</param>
  /// <param name="right">ISpecification</param>
  /// <returns>ISpecification</returns>
  public static ISpecification<T> Or<T>(
    this ISpecification<T> left
  , ISpecification<T> right)
  where T
  : class
  , IAggregateRoot 
  => left.Or(right.Criteria);

  /// <summary>
  /// Combine two specifications with an OR operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="left">ISpecification</param>
  /// <param name="right">ISpecification</param>
  /// <returns>ISpecification</returns>
  public static ISpecification<T> Or<T>(
    this ISpecification<T> left
  , Expression<Func<T, bool>> rightExpression)
  where T 
  : class
  , IAggregateRoot
  {
    Expression<Func<T, bool>> leftExpression = left.Criteria;
    SwapVisitor visitor = new(leftExpression.Parameters[0], rightExpression.Parameters[0]);
    BinaryExpression lazyOr = Expression.OrElse(visitor.Visit(leftExpression.Body)!, rightExpression.Body);
    Expression<Func<T, bool>> or = Expression.Lambda<Func<T, bool>>(lazyOr, rightExpression.Parameters);
    return new Specification<T>(or);
  }

  /// <summary>
  /// Combine two specifications with a NOT operation.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="left">ISpecification</param>
  /// <param name="right">ISpecification</param>
  /// <returns>ISpecification</returns>
  public static ISpecification<T> Not<T>(
    this ISpecification<T> left)
  where T 
  : class
  , IAggregateRoot
  {
    Expression<Func<T, bool>> leftExpression = left.Criteria;
    UnaryExpression notExpression = 
      Expression.Not(leftExpression.Body);
    Expression<Func<T, bool>> not = 
      Expression.Lambda<Func<T, bool>>(notExpression, leftExpression.Parameters);
    return new Specification<T>(not);
  }

  /// <summary>
  /// Helper Visitor that replaces the parameter of the right expression
  /// with the parameter of the left expression. This way we can combine both
  /// expressions in a single expression tree.
  /// </summary>
  private class SwapVisitor 
  : ExpressionVisitor
  {
    private readonly Expression from, to;

    public SwapVisitor(Expression from, Expression to)
    {
      this.from = from;
      this.to = to;
    }

    public override Expression? Visit(Expression? node) 
    => node == from ? to : base.Visit(node);
  }
}

