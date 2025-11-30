using ModularMonolith.APIs.BoundedContexts.Common.Commands;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.Mailing.Commands;

public sealed record class SendEmailCommand(
  EmailAddress From,
  EmailAddress[] To,
  NonEmptyString Subject,
  NonEmptyString Body,
  EmailAddress[] CC
)
: ICommand<bool>
;
