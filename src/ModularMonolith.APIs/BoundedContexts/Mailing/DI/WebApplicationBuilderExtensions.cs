namespace ModularMonolith.APIs.BoundedContexts.Mailing.DI;

public static class WebApplicationBuilderExtensions
{
  extension(IHostApplicationBuilder builder)
  {
    public IHostApplicationBuilder AddEmailServices()
    {
      EmailConfig? emailConfig = builder.Configuration
            .GetSection("EmailConfig")
            .Get<EmailConfig>();
      if (emailConfig is null)
      {
        throw new ConfigurationErrorsException(message: "Missing EmailConfig");
      }
      else
      {
        _ = builder.Services.AddSingleton(emailConfig!);
        _ = builder.Services.AddMailings();
        return builder;
      }
    }
  }
}
