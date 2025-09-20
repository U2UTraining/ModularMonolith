namespace U2U.ModularMonolith.BoundedContexts.Currencies.Infra;

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

    _ = currency
      .HasHistory()
      .HasSoftDelete();

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
