namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEvents;

internal sealed class CurrencyValueInEuroHasChangedEmailDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICommandSender _commandSender;

  public CurrencyValueInEuroHasChangedEmailDomainEventHandler(
    ICommandSender commandSender)
  {
    _commandSender = commandSender;
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
      From: "peter@u2u.be",
      To: ["peter@u2u.be"],
      CC: [],
      Subject: $"Currency {@event.CurrencyName} has been updated",
      Body: $"Currency {@event.CurrencyName} has been updated to {@event.NewValueInEuro}"
    );

    await _commandSender.ExecuteAsync(cmd);
  }
}
