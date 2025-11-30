using ModularMonolith.APIs.BoundedContexts.Common.Commands;

namespace ModularMonolith.APIs.BoundedContexts.Mailing.CommandHandlers;

public sealed class SendEmailCommandHandler
: ICommandHandler<SendEmailCommand, bool>
{
  private readonly EmailConfig _emailConfig;

  public SendEmailCommandHandler(
    EmailConfig emailConfig
  )
  {
    _emailConfig = emailConfig;
  }

  public async Task<bool> HandleAsync(
    SendEmailCommand command
  , CancellationToken cancellationToken = default)
  {
    try
    {
      using (SmtpClient client = new())
      {
        client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);
        MimeMessage message = new();
        message.From.Add(new MailboxAddress(name: "You", address: command.From));
        foreach (EmailAddress addr in command.To ?? [])
        {
          message.To.Add(new MailboxAddress(name: "Recipient", address: addr));
        }
        //foreach (EmailAddress addr in command.CC ?? [])
        //{
        //  message.CC.Add(new MailboxAddress(addr));
        //}
        message.Subject = command.Subject;
        message.Body = new TextPart("plain") { Text = command.Body };
        client.Send(message);
        client.Disconnect(true);
      }
      return true;
    }
    catch
    {
      return false;
    }
  }
}
