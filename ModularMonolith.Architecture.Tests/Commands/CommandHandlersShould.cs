using ModularMonolith.APIs.BoundedContexts.Common.Commands;

namespace ModularMonolith.Architecture.Tests.Commands;

public sealed class CommandHandlersShould
{
  [Fact]
  public void UseCommandHandlerSuffix()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(ICommandHandler<,>))
      .Should()
      .HaveNameEndingWith("CommandHandler")
      .GetResult();

    if (result.IsSuccessful is false)
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
      .ImplementInterface(typeof(ICommandHandler<,>))
      .Should()
      .BeSealed()
      .GetResult();

    if( result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Command Handlers are not sealed: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }

  [Fact]
  public void BeNotBePublic()
  {
    NetArchTest.Rules.TestResult result = Types
      .InAssembly(AssembliesUnderTest.ApiAssembly)
      .That()
      .ImplementInterface(typeof(ICommandHandler<,>))
      .Should()
      .NotBePublic()
      .GetResult();

    if (result.IsSuccessful == false)
    {
      var failedTypes = string.Join(", ", result.FailingTypes.Select(t => t.FullName));
      throw new Xunit.Sdk.XunitException($"The following Command Handlers are public: {failedTypes}");
    }
    Assert.True(result.IsSuccessful);
  }
}
