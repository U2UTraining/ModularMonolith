using ModularMonolith.BlazorApp.Components;
using ModularMonolith.BlazorApp.Components.BoardGames;
using ModularMonolith.BlazorApp.Components.Currencies;
using ModularMonolith.BlazorApp.Components.Shopping;
using ModularMonolith.BlazorApp.UIUpdates;
using ModularMonolith.ServiceDefaults;

namespace ModularMonolith.BlazorApp;

public static partial class Program
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

    builder.Services
           .AddDistributedMemoryCache()
           .AddSession();

    // State
    builder.Services.AddHttpContextAccessor(); // Singleton
    builder.Services.AddScoped<State>();

    // API Services -- DO NOT FORGET TRAILING SLASH! --
    builder.Services.AddHttpClient<CurrencyClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/currencies/");
    })
      .AddStandardResilienceHandler();

    builder.Services.AddHttpClient<BoardGamesClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/games/");
    })
      .AddStandardResilienceHandler();
    builder.Services.AddHttpClient<PublishersClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/publishers/");
    })
      .AddStandardResilienceHandler();
    builder.Services.AddHttpClient<ShoppingBasketClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/shopping/");
    })
      .AddStandardResilienceHandler();
    builder.Services.AddHttpClient<UpdateClient>(client =>
    {
      client.BaseAddress = new("https+http://modular-monolith-apis/ui/");
    })
      .AddStandardResilienceHandler();

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

    app.UseSession();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    await app.RunAsync();
  }
}