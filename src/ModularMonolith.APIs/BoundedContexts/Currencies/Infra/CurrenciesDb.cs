namespace U2U.ModularMonolith.BoundedContexts.Currencies.Infra;

public sealed partial class CurrenciesDb 
: DbContext
{
  public const string SchemaName = "currencies";
  public const string DatabaseName = "mm-currency-db";

  public CurrenciesDb() 
  : base() 
  { }

  public CurrenciesDb(DbContextOptions<CurrenciesDb> options)
  : base(options) 
  { }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(CurrenciesDb.SchemaName);
    ApplyCurrencyConfiguration(modelBuilder);
  }

  public DbSet<Currency> Currencies
  => Set<Currency>();

  private void ApplyCurrencyConfiguration(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.ConfigureValueObjectValueConverters();
  }
}
