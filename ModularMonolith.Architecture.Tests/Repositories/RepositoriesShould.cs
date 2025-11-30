using ModularMonolith.APIs.BoundedContexts.Common.Repositories;

namespace ModularMonolith.Architecture.Tests.Repositories;

public class RepositoriesShould
{
  [Fact]
  public void NotBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(IReadonlyRepository<>))
      .And()
      .AreNotInterfaces()
      .And()
      .AreNotGeneric()
      .Should()
      .NotBePublic()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Repositories are public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
