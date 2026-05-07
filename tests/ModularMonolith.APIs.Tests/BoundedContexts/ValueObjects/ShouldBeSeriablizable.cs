using System.Text.Json;

using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

namespace ModularMonolith.APIs.Tests.BoundedContexts.ValueObjects;

[TUnit.Core.Category("UnitTests")]

public sealed class ShouldBeSeriablizable
{
  [Test]
  public async Task NonEmptyStringShouldBeJsonSerializable()
  {
    NonEmptyString s = "Hello";

    string json = JsonSerializer.Serialize(s);

    NonEmptyString ds = JsonSerializer.Deserialize<NonEmptyString>(json);

    _ = await Assert.That(ds).EqualTo(s);
  }

  [Test]
  public async Task PositiveDecimalShouldBeJsonSerializable()
  {
    PositiveDecimal p1 = 1.2M;

    string json = JsonSerializer.Serialize(p1);

    PositiveDecimal p2 = JsonSerializer.Deserialize<PositiveDecimal>(json);

    _ = await Assert.That(p2).EqualTo(p1);
  }

  [Test]
  public async Task EmailAddressShouldBeJsonSerializable()
  {
    EmailAddress email = "test@example.com";

    string json = JsonSerializer.Serialize(email);

    EmailAddress deserialized = JsonSerializer.Deserialize<EmailAddress>(json);

    _ = await Assert.That(deserialized).EqualTo(email);
  }

  [Test]
  public async Task CreditCardNumberShouldBeJsonSerializable()
  {
    CreditCardNumber ccn = new CreditCardNumber("4111111111111111");

    string json = JsonSerializer.Serialize(ccn);

    CreditCardNumber deserialized = JsonSerializer.Deserialize<CreditCardNumber>(json);

    _ = await Assert.That(deserialized).EqualTo(ccn);
  }

  [Test]
  public async Task PercentShouldBeJsonSerializable()
  {
    Percent percent = new Percent(42.5M);

    string json = JsonSerializer.Serialize(percent);

    Percent deserialized = JsonSerializer.Deserialize<Percent>(json);

    _ = await Assert.That(deserialized).EqualTo(percent);
  }

  [Test]
  public async Task PKShouldBeJsonSerializable()
  {
    PK<int> pk = new PK<int>(42);

    string json = JsonSerializer.Serialize(pk);

    PK<int> deserialized = JsonSerializer.Deserialize<PK<int>>(json);

    _ = await Assert.That(deserialized).EqualTo(pk);
  }
}
