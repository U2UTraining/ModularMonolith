namespace ModularMonolith.APIs.Tests.EFCore.OutboxPattern;

public class TestIntegrationEvent
  : IIntegrationEvent
{
  public Guid EventId
  {
    get;
    init;
  }
}
