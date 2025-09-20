namespace U2U.ModularMonolith.BoundedContexts.BoardGames.Commands;

/// <summary>
/// Apply discount by changing the price of every game in the database
/// </summary>
/// <param name="GiveDiscount">Give or reduce the discount</param>
/// <param name="Discount"></param>
public sealed record class ApplyMegaDiscountCommand(
  bool GiveDiscount
, Percent Discount)
: ICommand<bool>
{ }
