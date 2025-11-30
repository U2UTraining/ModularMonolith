using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.BoundedContexts.Common.Commands;
using ModularMonolith.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.Architecture.Tests.DomainEvents;

public sealed class DomainEventsShould
{
  [Fact]
  public void UseDomainEventSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .HaveNameEndingWith("DomainEvent")
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
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Domain Events are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
