namespace ModularMonolith.BoundedContexts.BoardGames.Infra;

public sealed partial class GamesDb 
: DbContext
{
  public const string SchemaName = "games";
  public const string DatabaseName = "mm-games-db";

  public GamesDb() : base() { }

  public GamesDb(DbContextOptions<GamesDb> options)
      : base(options) { }

  // Only aggregate root entities are registered in the DbSet properties.
  public DbSet<BoardGame> Games => Set<BoardGame>();
  public DbSet<Publisher> Publishers => Set<Publisher>();

  private void ApplyGamesConfiguration(ModelBuilder modelBuilder)
  {
    _ = modelBuilder.ApplyConfiguration(new BoardGameConfiguration())
                    .ApplyConfiguration(new GameImageConfiguration())
                    .ApplyConfiguration(new PublisherConfiguration())
                    .ApplyConfiguration(new ContactConfiguration())
    ;
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(GamesDb.SchemaName);
    ApplyGamesConfiguration(modelBuilder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.ConfigureValueObjectValueConverters();
    configurationBuilder.ConfigureBoardGameValueObjectValueConverters();
  }
}