namespace ModularMonolith.APIs.BoundedContexts.Currencies.EndPoints;

// =====================================================================================
/// <summary>
/// Uses the command sender to validate and execute the command, which is more decoupled and allows for better separation of concerns, 
/// but also adds some overhead.
/// </summary>
/// <param name="handler"></param>
[Register(
  lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddCurrencyServices")]
internal sealed class UpdateCurrencyValue(ICommandSender commandSender)
{
  public async Task<Results<Ok<CurrencyDto>, BadRequest<string>>> ExecuteAsync(
    CurrencyDto dto
  , CancellationToken cancellationToken = default)
  {
    try
    {
      if (!Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName))
      {
        return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
      }
      Currency updated = await commandSender.ExecuteAsync(
        new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro)
      , cancellationToken);
      return TypedResults.Ok(updated.ToDto());
    }
    catch (Exception ex)
    {
      return TypedResults.BadRequest(error: ex.Message);
    }
  }
}
// =====================================================================================


// =====================================================================================
///// <summary>
///// Bypasses the command sender and the command handler indirection, which is simpler
///// this bypasses command validation 😒
///// </summary>
///// <param name="handler"></param>  
//[Register(
//  lifetime: ServiceLifetime.Scoped
//, methodNameHint: "AddCurrencyServices")]
//internal sealed class UpdateCurrencyValue(UpdateCurrencyValueInEuroCommandHandler handler)
//{
//  public async Task<Results<Ok<CurrencyDto>, BadRequest<string>>> ExecuteAsync(
//    CurrencyDto dto
//  , CancellationToken cancellationToken = default)
//  {
//    try
//    {
//      if (!Enum.TryParse(dto.CurrencyName, out CurrencyName currencyName))
//      {
//        return TypedResults.BadRequest(error: $"Currency '{dto.CurrencyName}' is not valid.");
//      }
//      Currency updated  = await handler.HandleAsync(
//        new UpdateCurrencyValueInEuroCommand(currencyName, dto.ValueInEuro), cancellationToken);
//      return TypedResults.Ok(updated.ToDto());
//    }
//    catch (Exception ex)
//    {
//      return TypedResults.BadRequest(error: ex.Message);
//    }
//  }
//}
// =====================================================================================

