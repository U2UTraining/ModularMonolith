using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Common.Queries;

namespace ModularMonolith.Architecture.Tests.Queries;

public sealed class QueriesShould
{
  public void UseQuerySuffix()
  {
      NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IQuery<>))
      .Should()
      .HaveNameEndingWith("Query")
      .GetResult();

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed do not follow conventions: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }
}
