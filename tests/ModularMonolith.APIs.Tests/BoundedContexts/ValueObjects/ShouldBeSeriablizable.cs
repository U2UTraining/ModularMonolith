using System.Text.Json;

using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Currencies.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

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

  [Test]
  public async Task MoneyShouldBeJsonSerializable()
  {
    Money money = new Money(19.99M, CurrencyName.EUR);

    string json = JsonSerializer.Serialize(money);

    Money deserialized = JsonSerializer.Deserialize<Money>(json);

    _ = await Assert.That(deserialized).EqualTo(money);
  }

  [Test]
  public async Task BoardGameNameShouldBeJsonSerializable()
  {
    BoardGameName name = new BoardGameName("Catan");

    string json = JsonSerializer.Serialize(name);

    BoardGameName deserialized = JsonSerializer.Deserialize<BoardGameName>(json);

    _ = await Assert.That(deserialized).EqualTo(name);
  }

  [Test]
  public async Task PublisherNameShouldBeJsonSerializable()
  {
    PublisherName name = new PublisherName("Kosmos");

    string json = JsonSerializer.Serialize(name);

    PublisherName deserialized = JsonSerializer.Deserialize<PublisherName>(json);

    _ = await Assert.That(deserialized).EqualTo(name);
  }

  [Test]
  public async Task FirstNameShouldBeJsonSerializable()
  {
    FirstName firstName = new FirstName("Alice");

    string json = JsonSerializer.Serialize(firstName);

    FirstName deserialized = JsonSerializer.Deserialize<FirstName>(json);

    _ = await Assert.That(deserialized).EqualTo(firstName);
  }

  [Test]
  public async Task LastNameShouldBeJsonSerializable()
  {
    LastName lastName = new LastName("Smith");

    string json = JsonSerializer.Serialize(lastName);

    LastName deserialized = JsonSerializer.Deserialize<LastName>(json);

    _ = await Assert.That(deserialized).EqualTo(lastName);
  }

  [Test]
  public async Task CityNameShouldBeJsonSerializable()
  {
    CityName cityName = new CityName("Brussels");

    string json = JsonSerializer.Serialize(cityName);

    CityName deserialized = JsonSerializer.Deserialize<CityName>(json);

    _ = await Assert.That(deserialized).EqualTo(cityName);
  }

  [Test]
  public async Task StreetNameShouldBeJsonSerializable()
  {
    StreetName streetName = new StreetName("Main Street 42");

    string json = JsonSerializer.Serialize(streetName);

    StreetName deserialized = JsonSerializer.Deserialize<StreetName>(json);

    _ = await Assert.That(deserialized).EqualTo(streetName);
  }

  [Test]
  public async Task AddressShouldBeJsonSerializable()
  {
    Address address = new Address(new StreetName("Main Street 42"), new CityName("Brussels"));

    string json = JsonSerializer.Serialize(address);

    Address deserialized = JsonSerializer.Deserialize<Address>(json);

    _ = await Assert.That(deserialized).EqualTo(address);
  }
}
