
//namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Services;

//public class CurrencyConverter

//{
//  private readonly IQuerySender _querySender;

//  public CurrencyConverter(IQuerySender querySender)
//  {
//    _querySender = querySender;
//  }

//  public async Task<decimal> ConvertAsync(
//    decimal amount
//  )
//  {
//    GetAllCurrenciesQuery query = GetAllCurrenciesQuery.All;
//    List<Currency> currencies = await _querySender.AskAsync(query);
//    //..
//  } 
//}
