namespace ModularMonolith.APIs.BoundedContexts.Currencies.DI;

/// <summary>
/// Configuration options for currency change email notifications.
/// Bind from the "Currencies:Notifications" section in appsettings.json.
/// </summary>
public sealed class CurrencyNotificationOptions
{
  public const string SectionName = "Currencies:Notifications";

  /// <summary>Sender email address for currency change notifications.</summary>
  public string From { get; init; } = string.Empty;

  /// <summary>List of recipient email addresses for currency change notifications.</summary>
  public string[] To { get; init; } = [];
}
