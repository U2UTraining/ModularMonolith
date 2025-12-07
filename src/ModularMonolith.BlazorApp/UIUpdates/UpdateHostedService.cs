//using System.Net.ServerSentEvents;

//namespace ModularMonolith.BlazorApp.UIUpdates;

//public class UpdateHostedService
//: BackgroundService
//{
//  private readonly IServiceProvider _serviceProvider;
//  private readonly State _state;

//  public UpdateHostedService(IServiceProvider serviceProvider, State state)
//  {
//    _serviceProvider = serviceProvider;
//    _state = state;
//  }

//  protected override async Task ExecuteAsync(CancellationToken cancellationToken)
//  {
//    using var scope = _serviceProvider.CreateScope();
//    UpdateClient _updateClient = scope.ServiceProvider.GetRequiredService<UpdateClient>();

//    HttpResponseMessage response =
//      await _updateClient.GetTokens(cancellationToken);
//    await using var stream =
//      await response.Content.ReadAsStreamAsync(cancellationToken);

//    await foreach (SseItem<string> hb in SseParser
//      .Create(stream)
//      .EnumerateAsync(cancellationToken))
//    {
//      _state.StateHasChanged();
//    }
//  }
//}
