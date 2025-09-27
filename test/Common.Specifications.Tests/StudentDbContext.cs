namespace Common.Specifications.Tests;

public class StudentDbContext
: DbContext
{
  public virtual DbSet<Student> Students
  => Set<Student>();
}
