using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.Architecture.Tests.IntegrationEvents;

public sealed class IntegrationEventsShould
{
  [Test]
  public void UseIntegrationEventSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .HaveNameEndingWith("IntegrationEvent")
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }

  [Test]
  public void BeSealed()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Integration Events are not sealed: {failedTypes}");
    }
  }

  [Test]
  public void BePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Integration Events are not public: {failedTypes}");
    }
  }
}
