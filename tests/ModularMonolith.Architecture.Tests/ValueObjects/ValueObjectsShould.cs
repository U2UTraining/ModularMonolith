namespace ModularMonolith.Architecture.Tests.ValueObjects;

public sealed class ValueObjectsShould
{
  [Fact]
  public void BeSealed()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ResideInNamespaceContaining("ValueObjects")
      .And()
      .AreNotStatic()
      .Should()
      .BeSealed()
      .GetResult();

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following classed should be sealed: {failedTypes}");
    }

    Assert.True(result.IsSuccessful);
  }
}
