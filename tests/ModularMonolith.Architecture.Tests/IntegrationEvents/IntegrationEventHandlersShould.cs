using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace ModularMonolith.Architecture.Tests.IntegrationEvents;

public sealed class IntegrationEventHandlersShould
{
  [Test]
  public void UseIntegrationEventHandlerSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEventHandler))
      .And()
      .AreNotInterfaces()
      .Should()
      .HaveNameEndingWith("IntegrationEventHandler")
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
      .ImplementInterface(typeof(IIntegrationEventHandler))
      .And()
      .AreNotInterfaces()
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Integration Event Handlers are not sealed: {failedTypes}");
    }
  }

  [Test]
  public void BeNotBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IIntegrationEventHandler))
      .And()
      .AreNotInterfaces()
      .Should()
      .NotBePublic()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Integration Event Handlers are public: {failedTypes}");
    }
  }
}
