using ModularMonolith.BlazorApp.Components;
using ModularMonolith.BlazorApp.Components.BoardGames;
using ModularMonolith.BlazorApp.Components.Currencies;
using ModularMonolith.ServiceDefaults;

using U2U.ModularMonolith;

namespace ModularMonolith.BlazorApp;

public partial class Program
{
  private static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    // Aspire
    builder.AddServiceDefaults();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    // Add support for MS FluentUI
    builder.Services
      .AddFluentUIComponents();

    // State
    builder.Services.AddSingleton<State>((_) => State.Instance);

    // API Services
    builder.Services.AddHttpClient<CurrencyClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis");
    });
    builder.Services.AddHttpClient<BoardGamesClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/games");
    });
    builder.Services.AddHttpClient<PublishersClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/publishers");
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
      app.UseExceptionHandler("/Error", createScopeForErrors: true);
      // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
      app.UseHsts();
    }
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    await app.RunAsync();
  }
}