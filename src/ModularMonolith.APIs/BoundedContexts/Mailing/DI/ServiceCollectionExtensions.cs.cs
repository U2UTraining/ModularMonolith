namespace ModularMonolith.APIs.BoundedContexts.Mailing.DI;

public static class ServiceCollectionExtensions
{
  extension(IServiceCollection services)
  {
    public IServiceCollection AddMailings()
    => services.AddMailingCommands();

    public IServiceCollection AddMailingCommands()
    => services
      .AddScoped<
        ICommandHandler<SendEmailCommand, bool>
      , SendEmailCommandHandler>()
      ;
  }
}
