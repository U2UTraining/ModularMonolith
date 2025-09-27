using ModularMonolith.BoundedContexts.Common.Entities;

namespace Common.Specifications.Tests;

public record class Student(
  string FirstName
, string LastName
, int Age
)
: IAggregateRoot;
