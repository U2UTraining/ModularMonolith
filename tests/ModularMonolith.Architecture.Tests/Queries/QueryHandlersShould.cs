using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.Architecture.Tests.Queries;

public sealed class QueryHandlersShould
{
  [Test]
  public void UseQueryHandlerSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IQueryHandler<,>))
      .Should()
      .HaveNameEndingWith("QueryHandler")
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Query Handlers do not follow conventions: {failedTypes}");
    }
  }

  [Test]
  public void BeNotBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IQueryHandler<,>))
      .Should()
      .NotBePublic()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Query Handlers are public: {failedTypes}");
    }
  }

  [Test]
  public void BeSealed()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IQueryHandler<,>))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Query Handlers are not sealed: {failedTypes}");
    }
  }
}
