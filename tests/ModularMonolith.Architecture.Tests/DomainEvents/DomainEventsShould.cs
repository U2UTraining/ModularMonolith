using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.DomainEvents;

namespace ModularMonolith.Architecture.Tests.DomainEvents;

public sealed class DomainEventsShould
{
  [Test]
  public void UseDomainEventSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .HaveNameEndingWith("DomainEvent")
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
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Domain Events are not sealed: {failedTypes}");
    }
  }

  [Test]
  public void BePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IDomainEvent))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Domain Events are not public: {failedTypes}");
    }
  }
}
