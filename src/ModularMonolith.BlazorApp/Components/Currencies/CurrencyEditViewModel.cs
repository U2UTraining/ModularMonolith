namespace ModularMonolith.BlazorApp.Components.Currencies;

public record class CurrencyEditViewModel
{
  public CurrencyEditViewModel(string name, decimal valueInEuro)
  {
    Name = name;
    ValueInEuro = valueInEuro;
  }

  public string Name { get; }
  public decimal ValueInEuro { get; set; }
}
