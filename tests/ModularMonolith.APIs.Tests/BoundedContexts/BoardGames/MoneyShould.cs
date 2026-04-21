using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Currencies.ValueObjects;

namespace ModularMonolith.APIs.Tests.BoundedContexts.BoardGames;

public class MoneyShould
{
  [Test]
  public async Task ImplementFactor()
  {
    decimal factor = 0.5M;
    Money m1 = new Money(6, CurrencyName.EUR);
    Money m2 = m1 * factor;
    await Assert.That(m2.Currency).EqualTo(m1.Currency);
    await Assert.That(m2.Amount).EqualTo(m1.Amount * factor);
  }
}
