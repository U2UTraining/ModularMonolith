using ModularMonolith.APIs.BoundedContexts.Common.Entities;

using ModularMonolithEFCore.History;
using ModularMonolithEFCore.SoftDelete;

namespace ModularMonolith.Architecture.Tests.Entities;

public class EntitiesShould
{
  [Fact]
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

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
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

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following entity classed do not inherit from EntityBase: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void ImplementIHistory()
  {
    NetArchTest.Rules.TestResult result = Types
    .InAssembly(AssembliesUnderTest.ApiAssembly)
    .That()
    .ImplementInterface(typeof(IAggregateRoot))
    .Or()
    .ImplementInterface(typeof(IAggregate<>))
    .Should()
    .ImplementInterface(typeof(IHistory))
    .GetResult();

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following entity classed do not implement IHistory: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
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

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following entity classed do not implement ISoftDeletable: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
