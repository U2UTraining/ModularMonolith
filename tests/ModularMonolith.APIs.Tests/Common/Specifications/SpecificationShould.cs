using ModularMonolith.APIs.BoundedContexts.Common.Specifications;

namespace ModularMonolith.APIs.Tests.Common.Specifications;

public class SpecificationShould
{
  private const string FirstName = "Jefke";
  private const string LastName = "Versmossen";
  private const int Age = 23;

  private Student CreateStudent()
  => new(FirstName, LastName, Age);

  [Test]
  public async Task MatchProperty()
  {
    Specification<Student> spec = new(s => s.FirstName == FirstName);
    await Assert.That(spec.Test(CreateStudent())).IsTrue();
  }

  [Test]
  public async Task SupportAnd()
  {
    Specification<Student> leftTrue = new(s => s.FirstName == FirstName);
    Specification<Student> leftFalse = new(s => s.FirstName == string.Empty);
    Specification<Student> rightTrue = new(s => s.Age == Age);
    Specification<Student> rightFalse = new(s => s.Age == -1);
    ISpecification<Student> andSpec = leftTrue.And(rightTrue);
    await Assert.That(andSpec.Test(CreateStudent())).IsTrue();
    andSpec = leftFalse.And(rightTrue);
    await Assert.That(andSpec.Test(CreateStudent())).IsFalse();
    andSpec = leftTrue.And(rightFalse);
    await Assert.That(andSpec.Test(CreateStudent())).IsFalse();
    andSpec = leftFalse.And(rightFalse);
    await Assert.That(andSpec.Test(CreateStudent())).IsFalse();
  }

  [Test]
  public async Task SupportOr()
  {
    Specification<Student> leftTrue = new(s => s.FirstName == FirstName);
    Specification<Student> leftFalse = new(s => s.FirstName == string.Empty);
    Specification<Student> rightTrue = new(s => s.Age == Age);
    Specification<Student> rightFalse = new(s => s.Age == -1);
    ISpecification<Student> orSpec = leftTrue.Or(rightTrue);
    await Assert.That(orSpec.Test(CreateStudent())).IsTrue();
    orSpec = leftFalse.Or(rightTrue);
    await Assert.That(orSpec.Test(CreateStudent())).IsTrue();
    orSpec = leftTrue.Or(rightFalse);
    await Assert.That(orSpec.Test(CreateStudent())).IsTrue();
    orSpec = leftFalse.Or(rightFalse);
    await Assert.That(orSpec.Test(CreateStudent())).IsFalse();
  }

  [Test]
  public async Task SupportNot()
  {
    Specification<Student> trueSpec = new(s => s.FirstName == FirstName);
    Specification<Student> falseSpec = new(s => s.FirstName == string.Empty);
    ISpecification<Student> notSpec = trueSpec.Not();
    await Assert.That(notSpec.Test(CreateStudent())).IsFalse();
    notSpec = falseSpec.Not();
    await Assert.That(notSpec.Test(CreateStudent())).IsTrue();
  }
}
