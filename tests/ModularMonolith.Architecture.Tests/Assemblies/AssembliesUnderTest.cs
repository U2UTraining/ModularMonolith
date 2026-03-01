namespace ModularMonolith.Architecture.Tests.Assemblies;

internal static class AssembliesUnderTest
{
  public static Assembly ApiAssembly = 
    typeof(ModularMonolith.APIs.Program).Assembly;

  public static Assembly BlazorAssembly =
    typeof(ModularMonolith.BlazorApp.Program).Assembly;
}
