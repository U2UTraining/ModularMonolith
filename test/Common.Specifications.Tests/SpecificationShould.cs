namespace Common.Specifications.Tests;

public class SpecificationShould
{
  private const string FirstName = "Jefke";
  private const string LastName = "Versmossen";
  private const int Age = 23;

  private Student CreateStudent()
  => new(FirstName, LastName, Age);

  [Fact]
  public void MatchProperty()
  {
    Specification<Student> spec = new(s => s.FirstName == FirstName);
    Assert.True(spec.Test(CreateStudent()));
  }

  [Fact]
  public void SupportAnd()
  {
    Specification<Student> leftTrue = new(s => s.FirstName == FirstName);
    Specification<Student> leftFalse = new(s => s.FirstName == string.Empty);
    Specification<Student> rightTrue = new(s => s.Age == Age);
    Specification<Student> rightFalse = new(s => s.Age == -1);
    ISpecification<Student> andSpec = leftTrue.And(rightTrue);
    Assert.True(andSpec.Test(CreateStudent()));
    andSpec = leftFalse.And(rightTrue);
    Assert.False(andSpec.Test(CreateStudent()));
    andSpec = leftTrue.And(rightFalse);
    Assert.False(andSpec.Test(CreateStudent()));
    andSpec = leftFalse.And(rightFalse);
    Assert.False(andSpec.Test(CreateStudent()));
  }

  [Fact]
  public void SupportOr()
  {
    Specification<Student> leftTrue = new(s => s.FirstName == FirstName);
    Specification<Student> leftFalse = new(s => s.FirstName == string.Empty);
    Specification<Student> rightTrue = new(s => s.Age == Age);
    Specification<Student> rightFalse = new(s => s.Age == -1);
    ISpecification<Student> orSpec = leftTrue.Or(rightTrue);
    Assert.True(orSpec.Test(CreateStudent()));
    orSpec = leftFalse.Or(rightTrue);
    Assert.True(orSpec.Test(CreateStudent()));
    orSpec = leftTrue.Or(rightFalse);
    Assert.True(orSpec.Test(CreateStudent()));
    orSpec = leftFalse.Or(rightFalse);
    Assert.False(orSpec.Test(CreateStudent()));
  }

  [Fact]
  public void SupportNot()
  {
    Specification<Student> trueSpec = new(s => s.FirstName == FirstName);
    Specification<Student> falseSpec = new(s => s.FirstName == string.Empty);
    ISpecification<Student> notSpec = trueSpec.Not();
    Assert.False(notSpec.Test(CreateStudent()));
    notSpec = falseSpec.Not();
    Assert.True(notSpec.Test(CreateStudent()));
  }
}
