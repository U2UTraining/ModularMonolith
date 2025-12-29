using ModularMonolithEFCore.RowVersion;

namespace ModularMonolith.APIs.BoundedContexts.Currencies.Infra;

public sealed class CurrencyConfiguration
: IEntityTypeConfiguration<Currency>
{
  public void Configure(
    EntityTypeBuilder<Currency> currency)
  {
    _ = currency.ToTable("Currencies");

    _ = currency.HasKey(cur => cur.Id);

    // I don't want the enumeration value to be used in the database (0,1,2)
    // Instead we use a converter to go back and forth between readable names
    _ = currency
      .Property(cur => cur.Id)
      .HasColumnOrder(1)
      .HasMaxLength(3)
      .HasConversion(
        id => id.Key.ToString()
      , key => new PK<CurrencyName>(Enum.Parse<CurrencyName>(key))
      )
      //.HasConversion<string>()
      .ValueGeneratedNever();

#pragma warning disable S125 // Sections of code should not be commented out
    _ = currency
      .Property(cur => cur.ValueInEuro)
      .HasColumnOrder(2)
      .HasColumnType("decimal(18,4)")
      //.HasConversion(
      //  pd => pd.Value
      //, d => new PositiveDecimal(d)
      //)
      //.HasConversion<PositiveDecimalValueConverter>();
    ;
#pragma warning restore S125 // Sections of code should not be commented out

    _ = currency
      .HasHistory()
      .HasSoftDelete()
      .HasRowVersion()
      ;

    // Add a constraint to keep this number positive
    _ = currency.ToTable(t =>
    {
      _ = t.HasCheckConstraint("CK_Value_Positive",
      """
      ValueInEuro > 0
      """
      );
    });
  }
}
