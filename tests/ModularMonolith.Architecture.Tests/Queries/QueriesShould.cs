using ModularMonolith.APIs.BoundedContexts.Common.Queries;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace ModularMonolith.Architecture.Tests.Queries;

public sealed class QueriesShould
{
  [Test]
  public void QueriesShouldUseQuerySuffix()
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

  [Test]
  public void QueriesShouldBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
    .InAssembly(AssembliesUnderTest.ApiAssembly)
    .That()
    .ImplementInterface(typeof(IQuery<>))
    .And()
    .AreNotGeneric()
    .Should()
    .BePublic()
    .And()
    .BeSealed()
    .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }
}
