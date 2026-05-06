namespace ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

public interface IIntegrationEvent
{
  public Guid EventId { get;init; }
}
