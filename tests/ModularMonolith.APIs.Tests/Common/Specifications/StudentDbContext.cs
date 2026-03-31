namespace ModularMonolith.APIs.Tests.Common.Specifications;

public class StudentDbContext
: DbContext
{
  public virtual DbSet<Student> Students
  => Set<Student>();
}
