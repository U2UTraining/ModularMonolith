[assembly: TUnit.Core.Category("ArchitectureTests")]

namespace ModularMonolith.Architecture.Tests.BoundedContexts;


public class CurrencyBoundedContextShould
{
  [Test]
  public void HavePublicEndpoints()
  {
    NetArchTest.Rules.TestResult result = Types
     .InNamespace("ModularMonolith.APIs.BoundedContexts.Currencies")
     .That()
     .ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.Endpoints")
     .Should()
     .BePublic()
     .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }

  [Test]
  public void HaveNonPublicHandlers()
  {
    NetArchTest.Rules.TestResult result = Types
     .InNamespace("ModularMonolith.APIs.BoundedContexts.Currencies")
     .That()
     .ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.CommandHandlers")
     .Or()
     .ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.QueryHandlers")
     .Or()
     .ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.DomainEventHandlers")
     //.Or()
     //.ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEventHandlers")
     .Should()
     .NotBePublic()
     .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }
}
