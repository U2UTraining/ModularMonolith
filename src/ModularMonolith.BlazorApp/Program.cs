using System.Globalization;

using BlazorSseClient.Server;

using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;

using ModularMonolith.APIs.BoundedContexts.BoardGames.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Common.IntegrationEvents;
using ModularMonolith.APIs.BoundedContexts.Currencies.IntegrationEvents;
using ModularMonolith.BlazorApp.Components;
using ModularMonolith.BlazorApp.Components.BoardGames;
using ModularMonolith.BlazorApp.Components.Currencies;
using ModularMonolith.BlazorApp.Components.IntegrationEvents;
using ModularMonolith.BlazorApp.Components.Shopping;
using ModularMonolith.ServiceDefaults;

using OpenTelemetryDemo.ServiceDefaults.Meters;

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

    // Localization: supported cultures for the culture selector
    builder.Services.AddLocalization();
    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
      CultureInfo[] supportedCultures =
      [
        new("en-US"), new("en-GB"),
        new("nl-NL"), new("nl-BE"),
        new("fr-FR"), new("fr-BE"),
        new("de-DE"), new("de-AT"),
        new("es-ES"), new("pt-BR"),
        new("ja-JP"), new("zh-CN"), new("ko-KR"),
      ];
      options.DefaultRequestCulture = new RequestCulture("en-US");
      options.SupportedCultures = supportedCultures;
      options.SupportedUICultures = supportedCultures;
    });

    builder.Services
           .AddDistributedMemoryCache()
           .AddSession();

    // State
    builder.Services.AddHttpContextAccessor(); // Singleton
    builder.Services.AddScoped<State>();
    builder.Services.AddScoped<U2UBlazorIntegrationEventProcessor>();
    builder.Services.AddScoped<IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>, ClientCurrencyHasChangedIntegrationEventHandler>();
    builder.Services.AddScoped<IIntegrationEventHandler<CurrencyHasChangedIntegrationEvent>, CurrencyHasChangedIntegrationEventHandler>();
    builder.Services.AddSingleton<IntegrationEventsMetrics>();
    builder.Services.AddScoped<IIntegrationEventHandler<BoardGameSelectedForShoppingBasketIntegrationEvent>, BoardGameSelectedForShoppingBasketIntegrationEventHandler>();
    builder.Services.AddScoped<IIntegrationEventHandler<GamesHaveChangedIntegrationEvent>, GamesHaveChangedIntegrationEventHandler>();

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

    app.UseRequestLocalization();

    app.UseSession();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Endpoint to set the culture cookie and redirect back
    app.MapGet("/culture/set", (string culture, string redirectUri, HttpContext httpContext) =>
    {
      RequestLocalizationOptions localizationOptions = httpContext.RequestServices
        .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;

      // Only accept cultures from the supported list
      bool isSupported = localizationOptions.SupportedCultures?
        .Any(c => c.Name.Equals(culture, StringComparison.OrdinalIgnoreCase)) ?? false;

      if (isSupported)
      {
        httpContext.Response.Cookies.Append(
          CookieRequestCultureProvider.DefaultCookieName,
          CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
          new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
        );
      }

      return Results.LocalRedirect(redirectUri);
    });

    await app.RunAsync();
  }
}