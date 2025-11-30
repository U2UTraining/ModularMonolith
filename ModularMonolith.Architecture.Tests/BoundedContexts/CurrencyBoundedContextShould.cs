using System;
using System.Collections.Generic;
using System.Text;

namespace ModularMonolith.Architecture.Tests.BoundedContexts;

public class CurrencyBoundedContextShould
{
  [Fact]
  public void HavePublicEndpoints()
  {
    NetArchTest.Rules.TestResult result = Types
     .InNamespace("ModularMonolith.APIs.BoundedContexts.Currencies")
     .That()
     .ResideInNamespace("ModularMonolith.APIs.BoundedContexts.Currencies.Endpoints")
     .Should()
     .BePublic()
     .GetResult();

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed do not follow conventions: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }

  [Fact]
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

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed do not follow conventions: {failedTypes}");
    }


    Assert.True(result.IsSuccessful);
  }
}
