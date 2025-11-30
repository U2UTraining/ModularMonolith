namespace ModularMonolith.APIs.BoundedContexts.BoardGames.DI;

public static class ServiceCollectionExtensions
{
  public static IHostApplicationBuilder AddBoardGames(
    this IHostApplicationBuilder builder)
  {
    builder.Services
             .AddBoardGamesQueries()
             .AddBoardGamesCommands()
             .AddBoardGamesIntegrationEventHandlers()
             ;
    builder.AddSqlServerDbContext<GamesDb>(GamesDb.DatabaseName,
    sqlServerOptions => {
    },
    optionsBuilder =>
    {
      optionsBuilder.AddInterceptors(
        new SoftDeleteInterceptor(),
        new HistoryInterceptor()
      );
      optionsBuilder.EnableDetailedErrors(true);
#if DEBUG
      optionsBuilder.EnableSensitiveDataLogging(true);
#endif
    });
    _ = builder.Services.AddScoped<IBoardGameRepository, BoardGamesRepository>();
    _ = builder.Services.AddMultiScoped<
          BoardGamesRepository
        , IReadonlyRepository<BoardGame>
        , IRepository<BoardGame>>();
    _ = builder.Services.AddMultiScoped<
          PublisherRepository
        , IReadonlyRepository<Publisher>
        , IRepository<Publisher>>();
    return builder;
  }

  private static IServiceCollection AddBoardGamesQueries(
    this IServiceCollection services)
  => services
     .AddScoped<
      IQueryHandler<GetAllGamesQuery, IQueryable<BoardGame>>
     , GetAllGamesQueryHandler>()
     .AddScoped<
       IQueryHandler<GetAllPublishersQuery, IQueryable<Publisher>>
     , GetAllPublishersQueryHandler>()
     .AddScoped<
       IQueryHandler<GetPublisherWithGamesQuery, Publisher?>
     , GetPublisherWithGamesQueryHandler>()
     .AddScoped<
       IQueryHandler<GetGamesFromListQuery, IQueryable<BoardGame>>
     , GetGamesFromListQueryHandler>()
     .AddScoped<
       IQueryHandler<Specification<BoardGame>, IQueryable<BoardGame>>
     , BoardGameSpecificationQueryHandler>()
     ;

  private static IServiceCollection AddBoardGamesCommands(
    this IServiceCollection services)
  => services
  .AddScoped<
    ICommandHandler<ApplyMegaDiscountCommand, bool>
  , ApplyMegaDiscountCommandHandler>()
  .AddScoped<
    ICommandHandler<UpdateGamePriceCommand, bool>
  , UpdateGamePriceCommandHandler>()
  .AddScoped<
    ICommandHandler<AddBoardGameToPublisherCommand, Publisher>
  , AddBoardGameToPublisherCommandHandler>()
  ;

  private static IServiceCollection AddBoardGamesIntegrationEventHandlers(
    this IServiceCollection services)
  => services
  //.AddTransient<
  //  IIntegrationEventHandler<GamesHaveChanged>,
  //  GamesHaveChangedHandler>()
  ;

  //private static IServiceCollection AddBoardGamesInfra(
  //  this IServiceCollection services
  //, string connectionString)
  //{
  //  _ = services.AddDbContext<GamesDb>((serviceProvider, optionsBuilder) =>
  //  {
  //    _ = optionsBuilder.UseSqlServer(connectionString, sqlServerOptions =>
  //    {
  //      sqlServerOptions.EnableRetryOnFailure(3);
  //      sqlServerOptions.UseCompatibilityLevel(160);
  //      sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
  //      // Migrations
  //      sqlServerOptions.MigrationsHistoryTable(
  //        tableName: HistoryRepository.DefaultTableName
  //      , schema: GamesDb.SchemaName);
  //    })
  //    .AddInterceptors(
  //      serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
  //      serviceProvider.GetRequiredService<HistoryInterceptor>()
  //    );
  //    // Workaround for EF bug...
  //    // https://github.com/dotnet/efcore/issues/35110
  //    optionsBuilder.ConfigureWarnings(warnings =>
  //    {
  //      warnings.Log(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning);
  //    });
  //  });
  //  //.UseInternalServiceProvider(serviceProvider)
  //  //);
  //  _ = services.AddScoped<IBoardGameRepository, BoardGamesRepository>();
  //  _ = services.AddMultiScoped<
  //        BoardGamesRepository
  //      , IReadonlyRepository<BoardGame>
  //      , IRepository<BoardGame>>();
  //  _ = services.AddMultiScoped<
  //        PublisherRepository
  //      , IReadonlyRepository<Publisher>
  //      , IRepository<Publisher>>();

  //  return services;
  //}
}
