using ModularMonolith.APIs.BoundedContexts.Common.Entities;
using ModularMonolith.APIs.EFCore.Auditability;
using ModularMonolith.APIs.EFCore.SoftDelete;

namespace ModularMonolith.Architecture.Tests.Entities;

public class EntitiesShould
{
  [Test]
  public void BeSealed()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IAggregateRoot))
      .Or()
      .ImplementInterface(typeof(IAggregate<>))
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following classes are not sealed: {failedTypes}");
    }
  }

  [Test]
  public void InheritFromEntityBase()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IAggregateRoot))
      .Or()
      .ImplementInterface(typeof(IAggregate<>))
      .Should()
      .Inherit(typeof(EntityBase<>))
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following entity classes do not inherit from EntityBase: {failedTypes}");
    }
  }

  [Test]
  public void ImplementIAuditability()
  {
    NetArchTest.Rules.TestResult result = Types
    .InAssembly(AssembliesUnderTest.ApiAssembly)
    .That()
    .ImplementInterface(typeof(IAggregateRoot))
    .Or()
    .ImplementInterface(typeof(IAggregate<>))
    .Should()
    .ImplementInterface(typeof(IAuditability))
    .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following entity classes do not implement IAuditability: {failedTypes}");
    }
  }

  [Test]
  public void ImplementSoftDelete()
  {
    NetArchTest.Rules.TestResult result = Types
    .InAssembly(AssembliesUnderTest.ApiAssembly)
    .That()
    .ImplementInterface(typeof(IAggregateRoot))
    .Or()
    .ImplementInterface(typeof(IAggregate<>))
    .Should()
    .ImplementInterface(typeof(ISoftDeletable))
    .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following entity classes do not implement ISoftDeletable: {failedTypes}");
    }
  }
}
