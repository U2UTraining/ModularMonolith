namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Commands;

[Register(
  serviceType: typeof(ICommandHandler<UpdateGamePriceCommand, bool>)
, lifetime: ServiceLifetime.Scoped
, methodNameHint: "AddBoardGameServices")]
internal sealed class UpdateGamePriceCommandHandler 
: ICommandHandler<UpdateGamePriceCommand, bool>
{
  private readonly BoardGamesDb _db;
  private readonly IOutboxSignal _outboxSignal;
  private readonly ILogger<UpdateCurrencyValueInEuroCommandHandler> _logger;

  public UpdateGamePriceCommandHandler(
    BoardGamesDb db
  , [FromKeyedServices(nameof(BoardGamesDb))] IOutboxSignal outboxSignal
  , ILogger<UpdateCurrencyValueInEuroCommandHandler> logger)
  {
    _db = db;
    _outboxSignal = outboxSignal;
    _logger = logger;
  }

  public async Task<bool> HandleAsync(
    UpdateGamePriceCommand request
  , CancellationToken cancellationToken = default)
  {
    BoardGame? boardGame = _db.BoardGames.Find(request.BoardGameId);
    if (boardGame is not null)
    {
      boardGame.SetPrice(request.PriceInEuro);

      BoardGamePriceUpdateIntegrationEvent @event = new(
        EventId: Guid.NewGuid()
        , BoardGameId: request.BoardGameId
        , Name: boardGame.Name
        , PriceInEuro: request.PriceInEuro
        ); 
      await _db.SaveChangesAsync(@event, _outboxSignal, cancellationToken);

      BoardGamesLogger.UpdateGamePriceCommandInvoked(_logger, DateTime.UtcNow, request);
    }
    return boardGame is not null;
  }
}
