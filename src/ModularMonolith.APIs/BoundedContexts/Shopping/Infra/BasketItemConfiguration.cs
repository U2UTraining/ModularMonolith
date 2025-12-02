using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;
using ModularMonolith.APIs.BoundedContexts.Shopping.Entities;

namespace ModularMonolith.APIs.BoundedContexts.Shopping.Infra;

internal sealed class BasketItemConfiguration
: IEntityTypeConfiguration<BasketItem>
{
  public void Configure(EntityTypeBuilder<BasketItem> gameInBasket)
  {
    _ = gameInBasket.ToTable("BasketItems");

    _ = gameInBasket
      .HasKey(g => g.Id);

    _ = gameInBasket
      .Property(c => c.Id)
      .HasColumnOrder(0)
      .ValueGeneratedOnAdd()
      ;

    _ = gameInBasket
      .Property(c => c.BoardGameId)
      .HasColumnOrder(1)
      ;

    _ = gameInBasket.ComplexProperty(game => game.Price, complexProperty =>
    {
      _ = complexProperty.IsRequired();
      _ = complexProperty.Property(m => m.Amount)
            .HasColumnOrder(2)
            .HasColumnType("decimal(4,2)")
            .IsRequired()
            ;
      _ = complexProperty.Property(nameof(Money.Currency))
            .HasColumnOrder(3)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(3)
            ;
    });

    _ = gameInBasket.HasHistory().HasSoftDelete();

  }
}
