namespace ModularMonolith.APIs.BoundedContexts.Common.DI;

/// <summary>
/// Method to create an instance of type I.
/// </summary>
/// <typeparam name="I">Type of instance being created</typeparam>
public interface IFactory<I> 
where I : class
{
  I Create(IServiceProvider serviceProvider);
}
