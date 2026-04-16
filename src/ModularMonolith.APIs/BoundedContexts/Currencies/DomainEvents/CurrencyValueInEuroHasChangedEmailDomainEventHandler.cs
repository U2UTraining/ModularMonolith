namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEvents;

[Register(
  serviceType: typeof(IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class CurrencyValueInEuroHasChangedEmailDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICommandSender _commandSender;
  private readonly CurrencyNotificationOptions _options;

  public CurrencyValueInEuroHasChangedEmailDomainEventHandler(
    ICommandSender commandSender
  , IOptions<CurrencyNotificationOptions> options)
  {
    _commandSender = commandSender;
    _options = options.Value;
  }

  /// <summary>
  /// Handles the specified domain event by sending a notification email about the currency value change.
  /// </summary>
  /// <param name="event">
  /// The domain event containing information about the updated currency and its new value in euros. 
  /// Cannot be null.
  /// </param>
  /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public async ValueTask HandleAsync(
    CurrencyValueInEuroHasChangedDomainEvent @event
  , CancellationToken cancellationToken)
  {
    SendEmailCommand cmd = new(
      From: _options.From,
      To: [.. _options.To],
      CC: [],
      Subject: $"Currency {@event.CurrencyName} has been updated",
      Body: $"Currency {@event.CurrencyName} has been updated to {@event.NewValueInEuro}"
    );

    await _commandSender.ExecuteAsync(cmd);
  }
}
