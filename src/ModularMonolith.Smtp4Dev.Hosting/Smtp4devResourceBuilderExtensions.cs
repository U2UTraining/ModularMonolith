using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;

namespace ModularMonolith.Smtp4Dev.Hosting;

public static class Smtp4DevResourceBuilderExtensions
{
  public static IResourceBuilder<Smtp4DevResource> AddSmtp4dev(
        this IDistributedApplicationBuilder builder,
        string name,
        int? httpPort = null,
        int? smtpPort = null)
  {
    Smtp4DevResource resource = new(name);

    return builder.AddResource(resource)
                  .WithImage(Smtp4DevContainerImageTags.Image)
                  .WithImageRegistry(Smtp4DevContainerImageTags.Registry)
                  .WithImageTag(Smtp4DevContainerImageTags.Tag)
                  .WithHttpEndpoint(
                      targetPort: 80,
                      port: httpPort,
                      name: Smtp4DevResource.HttpEndpointName)
                  .WithEndpoint(
                      targetPort: 25,
                      port: smtpPort,
                      name: Smtp4DevResource.SmtpEndpointName);
  }
}

internal static class Smtp4DevContainerImageTags
{
  internal const string Registry = "docker.io";
  internal const string Image = "rnwood/smtp4dev";
  internal const string Tag = "latest";
}

