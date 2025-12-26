namespace ModularMonolith.APIs.BoundedContexts.Common.Specifications;

internal sealed class ExpressionComparison
: ExpressionVisitor
{
  private readonly Queue<Expression> candidates;
  private Expression? candidate;

  public bool AreEqual { get; private set; } = true;

  public ExpressionComparison(Expression a, Expression b)
  {
    candidates = new Queue<Expression>(new ExpressionEnumeration(b));
    candidate = null;

    _ = Visit(a);

    if (candidates.Count > 0)
    {
      Stop();
    }
  }

  private Expression? PeekCandidate()
  {
    return candidates.Count == 0 ? null : candidates.Peek();
  }

  private Expression PopCandidate()
  {
    return candidates.Dequeue();
  }

  private bool CheckAreOfSameType(Expression candidate, Expression expression)
  {
    return CheckEqual(expression.NodeType, candidate.NodeType)
      && CheckEqual(expression.Type, candidate.Type);
  }

  private void Stop()
  {
    AreEqual = false;
  }

  // Change the type of candidate to match original
#pragma warning disable S1172 // Unused method parameters should be removed
  private T? MakeCandidateMatch<T>(T _)
#pragma warning restore S1172 // Unused method parameters should be removed
    where T : Expression
  {
    return (T?)candidate;
  }

  public override Expression? Visit(Expression? node)
  {
    if (node == null || !AreEqual)
    {
      return node;
    }

    candidate = PeekCandidate();
    if (!CheckNotNull(candidate) || !CheckAreOfSameType(candidate!, node))
    {
      return node;
    }

    _ = PopCandidate();

    return base.Visit(node);
  }

  protected override Expression VisitConstant(ConstantExpression node)
  {
    ConstantExpression? ce = MakeCandidateMatch(node);
    _ = CheckEqual(node.Value, ce?.Value);
    return base.VisitConstant(node);
  }

  protected override Expression VisitMember(MemberExpression node)
  {
    MemberExpression? me = MakeCandidateMatch(node);
    _ = CheckEqual(node.Member, me?.Member);
    return base.VisitMember(node);
  }

  protected override Expression VisitMethodCall(MethodCallExpression node)
  {
    MethodCallExpression? mc = MakeCandidateMatch(node);
    _ = CheckEqual(node.Method, mc?.Method);
    return base.VisitMethodCall(node);
  }

  protected override Expression VisitParameter(ParameterExpression node)
  {
    ParameterExpression? pe = MakeCandidateMatch(node);
    _ = CheckEqual(node.Name, pe?.Name);
    return base.VisitParameter(node);
  }

  protected override Expression VisitTypeBinary(TypeBinaryExpression node)
  {
    TypeBinaryExpression? be = MakeCandidateMatch(node);
    _ = CheckEqual(node.TypeOperand, be!.TypeOperand);
    return base.VisitTypeBinary(node);
  }

  protected override Expression VisitBinary(BinaryExpression node)
  {
    BinaryExpression? be = MakeCandidateMatch(node);
    _ = CheckEqual(node.Method, be!.Method);
    _ = CheckEqual(node.IsLifted, be!.IsLifted);
    _ = CheckEqual(node.IsLiftedToNull, be!.IsLiftedToNull);
    return base.VisitBinary(node);
  }

  protected override Expression VisitUnary(UnaryExpression node)
  {
    UnaryExpression? ue = MakeCandidateMatch(node);
    _ = CheckEqual(node.Method, ue!.Method);
    _ = CheckEqual(node.IsLifted, ue.IsLifted);
    _ = CheckEqual(node.IsLiftedToNull, ue.IsLiftedToNull);
    return base.VisitUnary(node);
  }

  protected override Expression VisitNew(NewExpression node)
  {
    NewExpression? ne = MakeCandidateMatch(node);
    _ = CheckEqual(node.Constructor, ne!.Constructor);
    CompareList(node.Members, ne.Members);
    return base.VisitNew(node);
  }

  private void CompareList<T>(ReadOnlyCollection<T>? collection, ReadOnlyCollection<T>? candidates)
  {
    CompareList(collection, candidates, EqualityComparer<T>.Default.Equals);
  }

  private void CompareList<T>(ReadOnlyCollection<T>? collection, ReadOnlyCollection<T>? candidates, Func<T, T, bool> comparer)
  {
    if (!CheckAreOfSameSize(collection, candidates))
    {
      return;
    }

    if (collection is not null && candidates is not null)
    {
      for (int i = 0; i < collection.Count; i++)
      {
        if (!comparer(collection[i], candidates[i]))
        {
          Stop();
          return;
        }
      }
    }
    // ???
  }

  private bool CheckAreOfSameSize<T>(ReadOnlyCollection<T>? collection, ReadOnlyCollection<T>? candidate)
  {
    return CheckEqual(collection?.Count, candidate?.Count);
  }

  private bool CheckNotNull<T>(T? t) where T : class
  {
    if (t == null)
    {
      Stop();
      return false;
    }

    return true;
  }

  private bool CheckEqual<T>(T? t, T? candidate)
  {
    if (!EqualityComparer<T>.Default.Equals(t, candidate))
    {
      Stop();
      return false;
    }

    return true;
  }
}
