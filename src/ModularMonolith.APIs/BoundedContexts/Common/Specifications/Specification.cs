namespace ModularMonolithBoundedContexts.Common.Specifications;

public class Specification<T>
: ISpecification<T>
, IEquatable<Specification<T>>
where T 
: class
, IAggregateRoot
{
  private static readonly Lazy<Specification<T>> _all 
    = new(new Specification<T>(static (_) => true));

  // Return a specification for all rows
  public static Specification<T> All() 
  => _all.Value;

  public Specification(Expression<Func<T, bool>> criteria)
    : this(criteria, [])
  { }

  public Specification(
    Expression<Func<T, bool>> criteria
  , Expression<Func<T, object>> include)
  : this(criteria, [include])
  { }

  public Specification(
    Expression<Func<T, bool>> criteria
  , IEnumerable<Expression<Func<T, object>>> includes
  , bool withQuerySplitting = false)
  {
    Criteria = criteria;
    Includes = includes;
    _withQuerySplitting |= withQuerySplitting;
  }

  public Expression<Func<T, bool>> Criteria { get; }

  private Func<T, bool>? compiledCriteria = null;

  private bool _noTracking = false;

  private bool _withQuerySplitting = false;

  public ISpecification<T> AsNoTracking()
  {
    _noTracking = true;
    return this;
  }

  public bool Test(in T t)
  {
    compiledCriteria ??= Criteria.Compile();
    return compiledCriteria.Invoke(t);
  }

  public IEnumerable<Expression<Func<T, object>>> Includes { get; }

  public ISpecification<T> Include(
    IEnumerable<Expression<Func<T, object>>> includes
  , bool withQuerySplitting = false
  )
  {
    _withQuerySplitting |= withQuerySplitting;
    return includes == null ? this : new Specification<T>(Criteria, Includes.Union(includes), _withQuerySplitting);
  }

  public ISpecification<T> Include(
    Expression<Func<T, object>> include
  , bool withQuerySplitting = false
  )
  {
    if (include == null)
    {
      return this;
    }
    _withQuerySplitting |= withQuerySplitting;
    List<Expression<Func<T, object>>> includes = new(Includes)
      {
        include
      };
    return Include(includes, _withQuerySplitting);
  }

  public IQueryable<T> BuildQueryable(IQueryable<T> q)
  {
    IQueryable<T> iq = Includes.Aggregate(seed: q, func: (current, include) => current.Include(include));
    if (Criteria != All().Criteria)
    {
      // Do not apply a Where for All() queries
      iq = iq.Where(Criteria);
    }
    if (_noTracking)
    {
      iq = iq.AsNoTracking();
    }
    if (_withQuerySplitting)
    {
      iq = iq.AsSplitQuery();
    }
    return iq;
  }

  public virtual bool Equals([AllowNull] Specification<T> other)
  {
    if (ReferenceEquals(this, other))
    {
      return true;
    }
    return other is not null && new ExpressionComparison(Criteria, other.Criteria).AreEqual;
  }

  public override bool Equals(object? obj)
  {
    if (ReferenceEquals(this, obj))
    {
      return true;
    }
    if (GetType() == obj?.GetType())
    {
      Specification<T> spec = (Specification<T>)obj;
      return new ExpressionComparison(Criteria, spec.Criteria).AreEqual;
    }
    return false;
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Criteria);
  }
}

