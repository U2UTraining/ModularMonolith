namespace U2U.ModularMonolith.BoundedContexts.Shopping.Infra;

internal sealed class CustomerConfiguration
: IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> customer)
  {
    _ = customer.ToTable("Customers");

    _ = customer
      .HasKey(c => c.Id);

    _ = customer
      .Property(c => c.Id)
      .HasColumnOrder(0)
      .ValueGeneratedOnAdd()
      //.HasConversion(
      //  id => id.Key,
      //  k => new PK<int>(k)
      //)
      ;

    _ = customer.Property(c => c.FirstName)
      .HasColumnOrder(1)
      //.HasMaxLength(FirstName.FirstNameMaxLength)
      .IsRequired()
      //.HasConversion(
      //  fn => fn.Value,
      //  v => new FirstName(v)
      //)
      ;

    _ = customer.Property(c => c.LastName)
      .HasColumnOrder(2)
      //.HasMaxLength(LastName.LastNameMaxLength)
      .IsRequired()
      //.HasConversion(
      //  ns => ns.Value,
      //  v => new LastName(v)
      //)
      ;

    _ = customer.ComplexProperty(cust => cust.Address, complexProperty =>
    {
      _ = complexProperty.Property(a => a.Street)
      .HasColumnOrder(3)
      //.HasMaxLength(StreetName.StreetNameMaxLength)
      //.HasConversion(
      //  ns => ns.Value,
      //  v => new StreetName(v)
      //)
      ;

      _ = complexProperty.Property(a => a.City)
      .HasColumnOrder(4)
      //.HasMaxLength(CityName.CityNameMaxLength)
      //.HasConversion(
      //  ns => ns.Value,
      //  v => new CityName(v)
      //)
      ;

    });

    _ = customer.HasHistory().HasSoftDelete();
  }
}
