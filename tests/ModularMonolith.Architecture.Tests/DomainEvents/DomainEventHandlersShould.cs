using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.Architecture.Tests.DomainEvents;

public sealed class DomainEventHandlersShould
{
  [Fact]
  public void UseDomainEventHandlerSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEventHandler<>))
      .Should()
      .HaveNameEndingWith("DomainEventHandler")
      .GetResult();

    if (!result.IsSuccessful)
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
      .ImplementInterface(typeof(IDomainEventHandler<>))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Domain Event Handlers are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void NotBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEventHandler<>))
      .Should()
      .NotBePublic()
      .GetResult();

    if (!result.IsSuccessful)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Domain Event Handlers are public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
