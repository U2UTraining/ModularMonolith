using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Common.Commands;

namespace ModularMonolith.BoundedContexts.Common.Queries;

public record class SimpleCommand
  : ICommand<int>;

public sealed class SimpleCommandHandler
  : ICommandHandler<SimpleCommand, int>
{
  public Task<int> HandleAsync(
    SimpleCommand command
  , CancellationToken cancellationToken = default)
    => Task.FromResult(42);
}
