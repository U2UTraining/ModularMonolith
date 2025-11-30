using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.Architecture.Tests.IntegrationEvents;

public sealed class IntegrationEventHandlersShould
{
  [Fact]
  public void UseIntegrationEventHandlerSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEventHandler))
      .Should()
      .HaveNameEndingWith("IntegrationEventHandler")
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
      .ImplementInterface(typeof(IIntegrationEventHandler))
      .Should()
      .BeSealed()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Integration Events Handlers are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
