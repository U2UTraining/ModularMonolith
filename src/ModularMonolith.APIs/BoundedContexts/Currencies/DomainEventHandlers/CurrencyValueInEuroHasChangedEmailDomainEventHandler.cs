namespace ModularMonolith.APIs.BoundedContexts.Currencies.DomainEventHandlers;

public class CurrencyValueInEuroHasChangedEmailDomainEventHandler
: IDomainEventHandler<CurrencyValueInEuroHasChangedDomainEvent>
{
  private readonly ICommandSender _commandSender;

  public CurrencyValueInEuroHasChangedEmailDomainEventHandler(
    ICommandSender commandSender)
  {
    _commandSender = commandSender;
  }

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
