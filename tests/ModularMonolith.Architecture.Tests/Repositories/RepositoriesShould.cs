using ModularMonolith.APIs.BoundedContexts.Common.Repositories;

namespace ModularMonolith.Architecture.Tests.Repositories;

public class RepositoriesShould
{
  [Test]
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

    if (!result.IsSuccessful)
    {
      string failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      Assert.Fail($"The following Repositories are public: {failedTypes}");
    }
  }
}
