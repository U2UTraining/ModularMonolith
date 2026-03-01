using ModularMonolith.APIs.BoundedContexts.Common.Repositories;
using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace Common.Specifications.Tests;

public class RepositoryShould
{
  internal static Student[] Students
  => [
    new ("A", "B", 20)
  , new ("E", "F", 45)
  , new ("C", "D", 50)
  ];

  internal static StudentDbContext CreateDbContext()
  {
    StudentDbContext db = new();
    return db.WithTable(db => db.Students, Students.AsQueryable())
             .Build();
  }

  internal static IRepository<Student> CreateRepository()
    => new Repository<Student, StudentDbContext>(CreateDbContext(), default!);

  [Fact]
  public void SupportSpecificationForSingleRow()
  {
    ISpecification<Student> spec =
      new Specification<Student>(s => s.FirstName == "C");

    IQueryable<Student> students = Students.AsQueryable();
    List<Student> result = spec.BuildQueryable(students).ToList();
    _ = Assert.Single(result);
  }

  [Fact]
  public void SupportSpecificationForMultipleRows()
  {
    ISpecification<Student> spec =
      new Specification<Student>(s => s.Age >= 10);
    IQueryable<Student> students = Students.AsQueryable();
    List<Student> result = spec.BuildQueryable(students).ToList();
    Assert.Equal(3, result.Count);

    spec = spec.And(new Specification<Student>(s => s.Age <= 45));
    result = spec.BuildQueryable(students).ToList();
    Assert.Equal(2, result.Count);
  }
}
