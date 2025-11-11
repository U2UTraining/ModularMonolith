using ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

namespace ModularMonolith.APIs.BoundedContexts.BoardGames.Infra;

internal sealed class ContactConfiguration
: IEntityTypeConfiguration<Contact>
{
  public void Configure(EntityTypeBuilder<Contact> contact)
  {
    _ = contact.HasKey(c => c.Id);

    _ = contact.Property(c => c.Id)
     .HasColumnOrder(0)
    // .HasConversion(
    //   pk => pk.Key,
    //   k => new PK<int>(k)
    //)
    .ValueGeneratedOnAdd();

    _ = contact.Property(c => c.FirstName)
     .HasColumnOrder(1)
     .HasMaxLength(PublisherName.PublisherNameMaxLength)
     .IsRequired()
     //.HasConversion(
     //  pbn => pbn.Value,
     //  name => new NonEmptyString(name)
     //)
     ;

    _ = contact.Property(c => c.LastName)
     .HasColumnOrder(2)
     .HasMaxLength(PublisherName.PublisherNameMaxLength)
     .IsRequired()
     //.HasConversion(
     //  pbn => pbn.Value,
     //  name => new NonEmptyString(name)
     //)
     ;

    _ = contact.Property(c => c.Email)
      .HasColumnOrder(3)
      //.HasMaxLength(EmailAddress.EmailMaxLength)
      .IsRequired()
      //.HasConversion(
      //  pbn => pbn.Value,
      //  value => new EmailAddress(value)
      //)
      ;
    _ = contact
      .HasHistory()
      .HasSoftDelete();
  }
}
