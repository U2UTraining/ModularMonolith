namespace ModularMonolith.APIs.BoundedContexts.Mailing.Commands;

public record class SendEmailCommand(
  EmailAddress From,
  EmailAddress[] To,
  NonEmptyString Subject,
  NonEmptyString Body,
  EmailAddress[] CC
) 
: ICommand<bool>
;
