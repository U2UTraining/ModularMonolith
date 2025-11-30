using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.Architecture.Tests.IntegrationEvents;

public sealed class IntegrationEventsShould
{
  [Fact]
  public void UseIntegrationEventSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .HaveNameEndingWith("IntegrationEvent")
      .GetResult();

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed do not follow conventions: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void BeSealed()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Integration Events are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void BePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Integration Events are not public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
