
using FluentValidation.Results;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.Commands;

public class UpdateCurrencyValueInEuroValidator 
  : AbstractValidator<UpdateCurrencyValueInEuroCommand>
{
  public UpdateCurrencyValueInEuroValidator()
  {
    RuleFor(x => x.Name)
      .NotEqual(CurrencyName.EUR)
      .WithMessage("EUR exchange rate cannot be modified");
  }
}
