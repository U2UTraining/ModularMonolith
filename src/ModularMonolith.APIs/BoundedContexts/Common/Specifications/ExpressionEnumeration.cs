namespace ModularMonolith.BoundedContexts.Common.Specifications;

internal class ExpressionEnumeration
: ExpressionVisitor
, IEnumerable<Expression>
{
  private readonly List<Expression> expressions = [];

  public ExpressionEnumeration(Expression expression) 
  => _ = Visit(expression);

  public override Expression? Visit(Expression? expression)
  {
    if (expression == null)
    {
      return expression;
    }
    expressions.Add(expression);
    return base.Visit(expression);
  }

  public IEnumerator<Expression> GetEnumerator() 
  => expressions.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() 
  => (this as IEnumerable<Expression>).GetEnumerator();
}

