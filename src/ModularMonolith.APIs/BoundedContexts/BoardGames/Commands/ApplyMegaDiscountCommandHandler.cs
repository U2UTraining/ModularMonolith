namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

internal sealed class ApplyMegaDiscountCommandHandler
: ICommandHandler<ApplyMegaDiscountCommand, bool>
{
  private readonly IBoardGameRepository _repo;

  public ApplyMegaDiscountCommandHandler(IBoardGameRepository repo) 
  => _repo = repo;

  public async Task<bool> HandleAsync(
    ApplyMegaDiscountCommand request
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
