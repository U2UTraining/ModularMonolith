using OpenTelemetryDemo.ServiceDefaults.Meters;

namespace ModularMonolith.APIs.BoundedContexts.Common.DI;

/// <summary>
/// Extension methods for IServiceCollection to add common services.
/// </summary>
public static class ServiceCollectionExtensions
{
  /// <summary>
  /// Adds a singleton using Lazy&lt;IT&gt;.
  /// </summary>
  /// <typeparam name="IT">Interface for singleton</typeparam>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazySingleton<IT, T>(
    this IServiceCollection services)
  where T 
  : class, IT
  where IT 
  : class
  => services.AddSingleton<T>()
             .AddSingleton<IT, T>()
             .AddSingleton(provider =>
               new Lazy<IT>(() => provider.GetRequiredService<T>()))
             ;

  /// <summary>
  /// Adds a singleton using Lazy&lt;T&gt;.
  /// </summary>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazySingleton<T>(
    this IServiceCollection services)
  where T 
  : class
  => services.AddSingleton<T>()
             .AddSingleton(provider =>
               new Lazy<T>(() => provider.GetRequiredService<T>()))
             ;

  /// <summary>
  /// Adds a transient using Lazy&lt;IT&gt;.
  /// </summary>
  /// <typeparam name="IT">Interface for transient</typeparam>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazyTransient<IT, T>(
    this IServiceCollection services)
  where T 
  : class, IT
  where IT 
  : class
    => services.AddTransient<T>()
               .AddTransient<IT, T>()
               .AddTransient(provider =>
                 new Lazy<IT>(() => provider.GetRequiredService<T>()))
               ;

  /// <summary>
  /// Adds a transient using Lazy&lt;T&gt;.
  /// </summary>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazyTransient<T>(
    this IServiceCollection services)
  where T 
  : class
  => services.AddTransient<T>()
             .AddTransient(provider =>
               new Lazy<T>(() => provider.GetRequiredService<T>()))
             ;

  /// <summary>
  /// Adds a scoped using Lazy&lt;IT&gt;.
  /// </summary>
  /// <typeparam name="IT">Interface for transient</typeparam>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazyScoped<IT, T>(
    this IServiceCollection services)
  where T 
  : class, IT
  where IT 
  : class
  => services.AddScoped<T>()
             .AddScoped<IT, T>()
             .AddScoped(provider =>
               new Lazy<IT>(() => provider.GetRequiredService<T>()))
             ;

  /// <summary>
  /// Adds a scoped using Lazy&lt;T&gt;.
  /// </summary>
  /// <typeparam name="T">Instance of T</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddLazyScoped<T>(
    this IServiceCollection services)
  where T 
  : class
  => services.AddScoped<T>()
             .AddScoped(provider =>
               new Lazy<T>(() => provider.GetRequiredService<T>()))
             ;

  /// <summary>
  /// Create a instance of I using a factory F
  /// </summary>
  /// <typeparam name="I">The type of the instance created</typeparam>
  /// <typeparam name="F">The type the factory type implementing IFactory&lt;I&gt;</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddFactory<I, F>(
    this IServiceCollection services)
  where F 
  : class, IFactory<I>
  where I 
  : class
  => services.AddSingleton<F>()
             .AddScoped(serviceProvider
               => serviceProvider.GetRequiredService<F>().Create(serviceProvider))
             ;

  /// <summary>
  /// Register T as a scoped service, and both dependencies of I1 and I2
  /// will resolve to the same scoped instance of T.
  /// Typically used with repositories that implement multiple interfaces.
  /// </summary>
  /// <typeparam name="T">Type of service implementing both I1 and I2</typeparam>
  /// <typeparam name="I1">Interface I1</typeparam>
  /// <typeparam name="I2">Interface I2</typeparam>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddMultiScoped<T, I1, I2>(
    this IServiceCollection services)
  where T 
  : class, I1, I2
  where I1 
  : class
  where I2 
  : class
  => services.AddScoped<T>()
             .AddScoped<I1>(sp => sp.GetRequiredService<T>())
             .AddScoped<I2>(sp => sp.GetRequiredService<T>())
             ;

  /// <summary>
  /// Method that configures DI for the Common Bounded Context.
  /// </summary>
  /// <returns>IHostApplicationBuilder</returns>
  /// <remarks>Using IHostApplicationBuilder works better with Aspire</remarks>
  public static IHostApplicationBuilder AddCommon(
    this IHostApplicationBuilder builder
  )
  {
    builder.Services
    .AddEFCoreInterceptors()
    .AddQueries()
    .AddCommands()
    .AddDomainEvents()
    .AddIntegrationEvents()
    ;
    return builder;
  }

  /// <summary>
  /// Register Domain Event Handler Infrastructure.
  /// </summary>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddDomainEvents(
    this IServiceCollection services
  )
  => services
    .AddScoped<IDomainEventPublisher, U2UDomainEventPublisher>()
    ;

  /// <summary>
  /// Register Integration Event Infrastructure.
  /// </summary>
  /// <param name="services">IServiceCollection</param>
  /// <returns>IServiceCollection</returns>
  public static IServiceCollection AddIntegrationEvents(
    this IServiceCollection services
  )
  => services
    .AddSingleton(Channel.CreateUnbounded<IIntegrationEvent>())
    .AddHostedService<U2UIntegrationEventHostedService>()
    .AddSingleton<IIntegrationEventPublisher, U2UIntegrationEventPublisher>()
    .AddSingleton<U2UIntegrationEventProcessor>()
    .AddSingleton<IntegrationEventsMetrics>()
    ;

  public static IServiceCollection AddQueries(
    this IServiceCollection services
  )
  => services
    .AddScoped<IQuerySender, U2UQuerySender>()
    ;

  public static IServiceCollection AddCommands(
    this IServiceCollection services
  )
  => services
    .AddScoped<ICommandSender, U2UCommandSender>()
    ;

  public static IServiceCollection AddEFCoreInterceptors(
    this IServiceCollection services
  )
  => services
    .AddSingleton(new SoftDeleteInterceptor())
    .AddSingleton(new HistoryInterceptor())
    ;
}
