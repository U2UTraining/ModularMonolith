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

    if (result.IsSuccessful is false)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed do not follow conventions: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }
}
