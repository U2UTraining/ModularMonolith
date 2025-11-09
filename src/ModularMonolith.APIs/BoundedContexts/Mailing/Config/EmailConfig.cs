namespace ModularMonolith.APIs.BoundedContexts.Mailing.Config;

public sealed record class EmailConfig(
  string SmtpServer = "localhost"
, int Port = 25
, string UserName = ""
, string Password = ""
);
