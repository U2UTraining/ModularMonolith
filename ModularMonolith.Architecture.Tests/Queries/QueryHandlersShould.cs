using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.Architecture.Tests.Queries;

public sealed class QueryHandlersShould
{
  [Fact]
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
      throw new Xunit.Sdk.XunitException($"The following Query Handlers do not follow conventions: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }

  [Fact]
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
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Query Handlers are public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void BePublic()
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
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Query Handlers are not public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
