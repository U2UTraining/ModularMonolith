namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

internal sealed class BoardGameConfiguration
: IEntityTypeConfiguration<BoardGame>
{
  public void Configure(EntityTypeBuilder<BoardGame> boardGame)
  {
    _ = boardGame.HasKey(g => g.Id);

    _ = boardGame
      .Property(g => g.Id)
      .HasColumnOrder(0)
      .ValueGeneratedOnAdd();

    _ = boardGame.HasIndex(g => g.Name)
      .IsUnique();

    _ = boardGame.Property(g => g.Name)
      .HasColumnOrder(1)
      .IsRequired()
      ;

    _ = boardGame.ComplexProperty(game => game.Price, complexProperty =>
    {
      _ = complexProperty.IsRequired();
      _ = complexProperty.Property(m => m.Amount)
            .HasColumnOrder(2)
            .HasColumnType("decimal(4,2)")
            .IsRequired()
            ;
      _ = complexProperty.Property(m => m.Currency)
            .HasColumnOrder(3)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(3)
            ;
    });

    _ = boardGame
      .HasOne(g => g.Image)
      .WithOne()    // No need for inverse property
      .HasForeignKey<GameImage>(gi => gi.Id)
      .OnDelete(DeleteBehavior.Cascade)
      ;

    _ = boardGame
      .HasOne(g => g.Publisher)
      .WithMany(p => p.Games);

    //_ = boardGame.Property<int>("PublisherId")
    //  .IsRequired();

    _ = boardGame
      .HasHistory()
      .HasSoftDelete();
  }
}
