namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

internal sealed class PublisherConfiguration
: IEntityTypeConfiguration<Publisher>
{
  public void Configure(EntityTypeBuilder<Publisher> publisher)
  {
    _ = publisher.HasKey(pub => pub.Id);

    _ = publisher.Property(pub => pub.Id)
     .HasColumnOrder(0)
     //.HasConversion(
     //  pk => pk.Key,
     //  k => new PK<int>(k)
     //)
     .ValueGeneratedOnAdd();

    _ = publisher.HasAlternateKey(p => p.Name);

    _ = publisher.Property(pub => pub.Name)
      .HasColumnOrder(1)
      //.HasMaxLength(PublisherName.PublisherNameMaxLength)
      .IsRequired()
      //.HasConversion(
      //  pbn => pbn.Value,
      //  name => new PublisherName(name))
      ;

    _ = publisher.HasMany(p => p.Games)
      .WithOne(g => g.Publisher)
      .IsRequired()
      .OnDelete(DeleteBehavior.Cascade)
      ;

    // Use the games field instead of the property, because the property has side-effects
    IMutableNavigation? gamesNav = 
    publisher.Metadata.FindNavigation(nameof(Publisher.Games));
    gamesNav!.SetPropertyAccessMode(PropertyAccessMode.Field);


    // Contact is part of the publisher aggregate,
    // so we use OwnsMany.

    publisher
      .OwnsMany(p => p.Contacts, c =>
      {
        c.WithOwner().HasForeignKey("PublisherId");
        c.HasKey("Id");
        c.Property<PK<int>>("Id").ValueGeneratedOnAdd();
        c.Property(c => c.FirstName)
       .HasColumnOrder(1)
       .HasMaxLength(PublisherName.PublisherNameMaxLength)
       .IsRequired()
       ;

        c.Property(c => c.LastName)
        .HasColumnOrder(2)
        .HasMaxLength(PublisherName.PublisherNameMaxLength)
        .IsRequired()
        ;

        c.Property(c => c.Email)
        .HasColumnOrder(3)
        .IsRequired();

        c.HasHistory()
        .HasSoftDelete()
        .HasRowVersion()
          ;
      })
      .UsePropertyAccessMode(PropertyAccessMode.Field);
      ;
      //.WithOne()
      //.IsRequired(false);

    //IMutableNavigation? contactsNav =
    //publisher.Metadata.FindNavigation(nameof(Publisher.Contacts));
    //contactsNav!.SetPropertyAccessMode(PropertyAccessMode.Field);

    _ = publisher
      .HasHistory()
      .HasSoftDelete()
      .HasRowVersion()
      ;
  }
}
