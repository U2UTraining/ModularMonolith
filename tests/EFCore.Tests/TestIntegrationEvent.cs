using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;

namespace EFCore.Tests;

public class TestIntegrationEvent
  : IIntegrationEvent
{
  public Guid EventId
  {
    get;
    init;
  }
}
