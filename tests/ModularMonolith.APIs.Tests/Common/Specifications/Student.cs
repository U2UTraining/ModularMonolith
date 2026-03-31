using ModularMonolith.APIs.BoundedContexts.Common.Entities;

namespace ModularMonolith.APIs.Tests.Common.Specifications;

public record class Student(
  string FirstName
, string LastName
, int Age
)
: IAggregateRoot;
