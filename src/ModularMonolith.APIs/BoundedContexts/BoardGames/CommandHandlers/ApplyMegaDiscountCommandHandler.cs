namespace U2U.ModularMonolith.BoundedContexts.BoardGames.CommandHandlers;

internal sealed class ApplyMegaDiscountCommandHandler
: ICommandHandler<Commands.ApplyMegaDiscountCommand, bool>
{
  private readonly IBoardGameRepository _repo;

  public ApplyMegaDiscountCommandHandler(IBoardGameRepository repo) 
  => _repo = repo;

  public async Task<bool> HandleAsync(
    Commands.ApplyMegaDiscountCommand request
  , CancellationToken cancellationToken)
  {
    decimal discount = 
      request.GiveDiscount 
        ? 1.0M - request.Discount.Factor 
        : 1.0M + request.Discount.Factor;
    await _repo.ApplyMegaDiscountAsync(discount, cancellationToken).ConfigureAwait(true);
    return true;
  }
}
