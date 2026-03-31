using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.Architecture.Tests.Queries;

public sealed class QueriesShould
{
  [Test]
  public void UseQuerySuffix()
  {
    NetArchTest.Rules.TestResult result = Types
    .InAssembly(AssembliesUnderTest.ApiAssembly)
    .That()
    .ImplementInterface(typeof(IQuery<>))
    .Should()
    .HaveNameEndingWith("Query")
    .Or()
    .HaveNameEndingWith("Specification`1")
    .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }
}
