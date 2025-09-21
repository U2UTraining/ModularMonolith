namespace ModularMonolith.BlazorApp.Configuration;

public record class ModularMonolithApplicationOptions
{
  public const string SectionName = "Application";

  public required string Title { get; set; }

  public required string Home { get; set; }
}
