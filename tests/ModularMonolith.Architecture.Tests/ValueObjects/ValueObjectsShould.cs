namespace ModularMonolith.Architecture.Tests.ValueObjects;

public sealed class ValueObjectsShould
{
  [Test]
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
      Assert.Fail($"The following classes should be sealed: {failedTypes}");
    }
  }
}
