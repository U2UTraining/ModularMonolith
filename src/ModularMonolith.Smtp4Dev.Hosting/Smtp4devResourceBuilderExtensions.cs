using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace ModularMonolith.Smtp4Dev.Hosting;

public static class Smtp4devResourceBuilderExtensions
{
  public static IResourceBuilder<Smtp4devResource> AddSmtp4dev(
        this IDistributedApplicationBuilder builder,
        string name,
        int? httpPort = null,
        int? smtpPort = null)
  {
    Smtp4devResource resource = new(name);

    return builder.AddResource(resource)
                  .WithImage(Smtp4devContainerImageTags.Image)
                  .WithImageRegistry(Smtp4devContainerImageTags.Registry)
                  .WithImageTag(Smtp4devContainerImageTags.Tag)
                  .WithHttpEndpoint(
                      targetPort: 80,
                      port: httpPort,
                      name: Smtp4devResource.HttpEndpointName)
                  .WithEndpoint(
                      targetPort: 25,
                      port: smtpPort,
                      name: Smtp4devResource.SmtpEndpointName);
  }
}

internal static class Smtp4devContainerImageTags
{
  internal const string Registry = "docker.io";
  internal const string Image = "rnwood/smtp4dev";
  internal const string Tag = "latest";
}

