namespace ModularMonolithBoundedContexts.Shopping.Infra;

public sealed partial class ShoppingDb : DbContext
{

  public const string SchemaName = "shopping";

  public ShoppingDb()
  : base() { }

  public ShoppingDb(DbContextOptions<ShoppingDb> options)
  : base(options) { }

  public DbSet<ShoppingBasket> Baskets => Set<ShoppingBasket>();

  private void ApplyShoppingConfiguration(ModelBuilder modelBuilder)
  {
    _ = modelBuilder.ApplyConfiguration(new CustomerConfiguration())
                    .ApplyConfiguration(new ShoppingBasketConfiguration())
                    .ApplyConfiguration(new BasketItemConfiguration())
    ;
  }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    modelBuilder.HasDefaultSchema(ShoppingDb.SchemaName);
    ApplyShoppingConfiguration(modelBuilder);
  }

  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.ConfigureValueObjectValueConverters();
    configurationBuilder.ConfigureShoppingValueObjectValueConverters();
  }
}