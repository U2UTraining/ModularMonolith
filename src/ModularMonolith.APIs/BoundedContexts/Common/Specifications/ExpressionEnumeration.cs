namespace ModularMonolith.APIs.BoundedContexts.Common.Specifications;

internal sealed class ExpressionEnumeration
: ExpressionVisitor
, IEnumerable<Expression>
{
  private readonly List<Expression> expressions = [];

  public ExpressionEnumeration(Expression expression) 
  => _ = Visit(expression);

  public override Expression? Visit(Expression? node)
  {
    if (node == null)
    {
      return node;
    }
    expressions.Add(node);
    return base.Visit(node);
  }

  public IEnumerator<Expression> GetEnumerator() 
  => expressions.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator() 
  => (this as IEnumerable<Expression>).GetEnumerator();
}

