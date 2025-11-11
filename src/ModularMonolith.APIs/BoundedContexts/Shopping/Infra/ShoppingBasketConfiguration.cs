namespace ModularMonolith.APIs.BoundedContexts.Shopping.Infra;

internal sealed class ShoppingBasketConfiguration
: IEntityTypeConfiguration<ShoppingBasket>
{
  public void Configure(EntityTypeBuilder<ShoppingBasket> shoppingBasket)
  {
    _ = shoppingBasket.ToTable("ShoppingBaskets");

    _ = shoppingBasket
      .HasKey(sb => sb.Id)
      ;

    _ = shoppingBasket
      .Property(sb => sb.Id)
      .HasColumnOrder(0)
      .ValueGeneratedOnAdd()
      //.HasConversion(
      //  id => id.Key,
      //  k => new PK<int>(k)
      //)
      ;

    //builder.Ignore(sb => sb.Games);

    _ = shoppingBasket
      .HasOne(basket => basket.Customer)
      .WithOne()
      .HasForeignKey<Customer>()
      .IsRequired(false);

    _ = shoppingBasket
      .HasMany(sb => sb.Items)
      .WithOne();

    Microsoft.EntityFrameworkCore.Metadata.IMutableNavigation? gamesInBasketNav =
      shoppingBasket.Metadata.FindNavigation(nameof(ShoppingBasket.Items));
    gamesInBasketNav!.SetPropertyAccessMode(PropertyAccessMode.Field);
    gamesInBasketNav!.SetField("gamesInBasket");

    _ = shoppingBasket.HasHistory().HasSoftDelete();
  }
}
