namespace ModularMonolith.BoundedContexts.BoardGames.Infra;

internal sealed class GameImageConfiguration
:IEntityTypeConfiguration<GameImage>
{
  public void Configure(EntityTypeBuilder<GameImage> gameImage)
  {
    _ = gameImage.HasKey(gi => gi.Id);

    _ = gameImage.Property(gi => gi.Id)
      .HasColumnOrder(0)
      .ValueGeneratedOnAdd()
      ;

    _ = gameImage.Property(gi => gi.ImageLocation)
      .HasColumnOrder(1)
      .HasMaxLength(1024)
      .IsRequired()
      .HasConversion(
        gi => gi.AbsoluteUri,
        uri => new Uri(uri)
      );

    _ = gameImage
      .HasHistory()
      .HasSoftDelete();
  }
}
