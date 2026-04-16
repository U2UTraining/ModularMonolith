using System;
using System.Collections.Generic;
using System.Text;

using ModularMonolith.APIs.BoundedContexts.Currencies.Entities;
using ModularMonolith.APIs.BoundedContexts.Currencies.Queries;

namespace ModularMonolith.APIs.Tests.BoundedContexts.Currencies;

public sealed class CurrencyTests
  : IAsyncDisposable
{
  private readonly MsSqlContainer _sqlContainer =
    new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
    .Build();

  [Before(Test)]
  public async Task Setup()
  {
    await _sqlContainer.StartAsync();
  }

  [After(Test)]
  public async Task Teardown()
  {
    await _sqlContainer.StopAsync();
  }

  public async ValueTask DisposeAsync()
  {
    await _sqlContainer.DisposeAsync();
  }

  [Test]
  public async Task GetAllCurrenciesShouldReturnAllCurrenciesInDb()
  {
    CurrenciesDb db = await new CurrencyDbFactory()
      .CreateAsync(_sqlContainer.GetConnectionString());
    GetAllCurrencies2QueryHandler sut = new(db);
    List<Currency> result = await sut.HandleAsync(GetAllCurrenciesQuery.All);
    await Assert.That(result.Count).EqualTo(3);
  }
}
