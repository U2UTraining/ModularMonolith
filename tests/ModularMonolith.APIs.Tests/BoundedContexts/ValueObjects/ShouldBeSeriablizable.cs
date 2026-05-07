using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.Tests.BoundedContexts.ValueObjects;

public sealed class ShouldBeSeriablizable
{
  [Test]
  public async Task NonEmptyStringShouldBeSerializable()
  {
    NonEmptyString s = "Hello";

    string json = JsonSerializer.Serialize(s);

    NonEmptyString ds = JsonSerializer.Deserialize<NonEmptyString>(json);

    await Assert.That(ds).EqualTo(s);
  }
}
