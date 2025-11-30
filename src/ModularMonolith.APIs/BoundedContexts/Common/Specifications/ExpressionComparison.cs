namespace ModularMonolith.APIs.BoundedContexts.Common.Specifications;

internal class ExpressionComparison
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
  private T? MakeCandidateMatch<T>([NotNull] T original) where T : Expression
  {
    return (T?)candidate;
  }

  public override Expression? Visit(Expression? expression)
  {
    if (expression == null || !AreEqual)
    {
      return expression;
    }

    candidate = PeekCandidate();
    if (!CheckNotNull(candidate) || !CheckAreOfSameType(candidate!, expression))
    {
      return expression;
    }

    _ = PopCandidate();

    return base.Visit(expression);
  }

  protected override Expression VisitConstant(ConstantExpression constant)
  {
    ConstantExpression? candidate = MakeCandidateMatch(constant);
    _ = CheckEqual(constant.Value, candidate?.Value);
    return base.VisitConstant(constant);
  }

  protected override Expression VisitMember(MemberExpression member)
  {
    MemberExpression? candidate = MakeCandidateMatch(member);
    _ = CheckEqual(member.Member, candidate?.Member);
    return base.VisitMember(member);
  }

  protected override Expression VisitMethodCall(MethodCallExpression methodCall)
  {
    MethodCallExpression? candidate = MakeCandidateMatch(methodCall);
    _ = CheckEqual(methodCall.Method, candidate?.Method);
    return base.VisitMethodCall(methodCall);
  }

  protected override Expression VisitParameter(ParameterExpression parameter)
  {
    ParameterExpression? candidate = MakeCandidateMatch(parameter);
    _ = CheckEqual(parameter.Name, candidate?.Name);
    return base.VisitParameter(parameter);
  }

  protected override Expression VisitTypeBinary(TypeBinaryExpression type)
  {
    TypeBinaryExpression? candidate = MakeCandidateMatch(type);
    _ = CheckEqual(type.TypeOperand, candidate!.TypeOperand);
    return base.VisitTypeBinary(type);
  }

  protected override Expression VisitBinary(BinaryExpression binary)
  {
    BinaryExpression? candidate = MakeCandidateMatch(binary);
    _ = CheckEqual(binary.Method, candidate!.Method);
    _ = CheckEqual(binary.IsLifted, candidate!.IsLifted);
    _ = CheckEqual(binary.IsLiftedToNull, candidate!.IsLiftedToNull);
    return base.VisitBinary(binary);
  }

  protected override Expression VisitUnary(UnaryExpression unary)
  {
    UnaryExpression? candidate = MakeCandidateMatch(unary);
    _ = CheckEqual(unary.Method, candidate!.Method);
    _ = CheckEqual(unary.IsLifted, candidate.IsLifted);
    _ = CheckEqual(unary.IsLiftedToNull, candidate.IsLiftedToNull);
    return base.VisitUnary(unary);
  }

  protected override Expression VisitNew(NewExpression nex)
  {
    NewExpression? candidate = MakeCandidateMatch(nex);
    _ = CheckEqual(nex.Constructor, candidate!.Constructor);
    CompareList(nex.Members, candidate.Members);
    return base.VisitNew(nex);
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
