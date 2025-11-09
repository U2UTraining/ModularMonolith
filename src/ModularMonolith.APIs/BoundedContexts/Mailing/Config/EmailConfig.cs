namespace ModularMonolith.APIs.BoundedContexts.Mailing.Config;

public record class EmailConfig(
  string SmtpServer = "localhost"
, int Port = 25
, string UserName = ""
, string Password = ""
);
