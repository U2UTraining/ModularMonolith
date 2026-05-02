using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.Tests.Common;

[TUnit.Core.Category("UnitTests")]

public class PositiveDecimalShould
{
  [Test]
  [Arguments(4.89, 4.99)]
  [Arguments(0.01, 0.49)]
  [Arguments(1.00, 0.99)]
  [Arguments(3.49, 3.49)]
  [Arguments(5.50, 5.49)]
  [Arguments(7.51, 7.99)]
  [Arguments(9.99, 9.99)]
  [Arguments(10.00, 9.99)]
  [Arguments(10.01, 10.49)]
  [Arguments(15.25, 15.49)]
  [Arguments(25.75, 25.99)]
  [Arguments(99.95, 99.99)]
  public async Task RoundCorrectly(double inputValue, double expectedValue)
  {
    decimal input = (decimal)inputValue;
    decimal expected = (decimal)expectedValue;
    PositiveDecimal d = new PositiveDecimal(input).Rounded();
    await Assert.That(d.Value).IsEqualTo(expected);
  }
}
