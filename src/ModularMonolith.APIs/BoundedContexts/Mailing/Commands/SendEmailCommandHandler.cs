namespace ModularMonolith.APIs.BoundedContexts.Mailing.Commands;

internal sealed class SendEmailCommandHandler
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
        await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, false, cancellationToken);
        MimeMessage message = new();
        message.From.Add(new MailboxAddress(name: "You", address: command.From));
        foreach (EmailAddress addr in command.To ?? [])
        {
          message.To.Add(new MailboxAddress(name: "Recipient", address: addr));
        }
        message.Subject = command.Subject;
        message.Body = new TextPart("plain") { Text = command.Body };
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
      }
      return true;
    }
    catch
    {
      return false;
    }
  }
}
