namespace ModularMonolith.APIs.BoundedContexts.Currencies.Commands;

/// <summary>
/// Update Currency Euro Value.
/// </summary>
public record struct UpdateCurrencyValueInEuroCommand
: ICommand<Currency>
{
  public UpdateCurrencyValueInEuroCommand(
    PK<CurrencyName> name
  , PositiveDecimal newValue)
  {
    // Fail Fast
    if (name.Key == CurrencyName.EUR)
    {
      throw new ArgumentException(
        message: $"Currency EUR cannot be modified"
      , paramName: nameof(name)
      );
    }
    Name = name;
    NewValue = newValue;
  }

  public PK<CurrencyName> Name { get; }
  public PositiveDecimal NewValue { get; }
}
