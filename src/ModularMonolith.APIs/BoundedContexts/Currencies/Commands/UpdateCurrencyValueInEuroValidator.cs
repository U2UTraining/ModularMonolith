namespace ModularMonolith.APIs.BoundedContexts.Currencies.Commands;

public class UpdateCurrencyValueInEuroValidator 
  : AbstractValidator<UpdateCurrencyValueInEuroCommand>
{
  public UpdateCurrencyValueInEuroValidator()
  {
    RuleFor(x => x.Name.Key)
      .IsInEnum()
      .WithMessage(x => $"Currency '{x.Name}' is not valid");

    RuleFor(x => x.Name)
      .NotEqual(CurrencyName.EUR)
      .WithMessage("EUR exchange rate cannot be modified");

    RuleFor(x => x.NewValue.Value)
      .InclusiveBetween(0.01m, 1000000m)
      .WithMessage(x => $"Currency value '{x.NewValue}' must be between 0.01 and 1,000,000");
  }
}
