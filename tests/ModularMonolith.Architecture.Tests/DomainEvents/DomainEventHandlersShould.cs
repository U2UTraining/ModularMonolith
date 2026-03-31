using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.Architecture.Tests.DomainEvents;

public sealed class DomainEventHandlersShould
{
  [Test]
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
      Assert.Fail($"The following classes do not follow conventions: {failedTypes}");
    }
  }

  [Test]
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
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Domain Event Handlers are not sealed: {failedTypes}");
    }
  }

  [Test]
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
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Domain Event Handlers are public: {failedTypes}");
    }
  }
}
