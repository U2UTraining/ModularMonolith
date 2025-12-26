namespace ModularMonolith.APIs.BoundedContexts.BoardGames.ValueObjects;

[DebuggerDisplay("{Amount}{Currency,nq}")]
public readonly record struct Money
{
  public Money(decimal amount, CurrencyName currency)
  {
    Amount = amount;
    Currency = currency;
  }

  public Money(decimal amount)
  : this(amount, CurrencyName.EUR) { }

  public decimal Amount { get; }
  public CurrencyName Currency { get; }

  public override string ToString()
  {
    string currency = Currency.ToString();
    CultureInfo ci = CultureInfo
      .GetCultures(CultureTypes.SpecificCultures)
      .First(x => new RegionInfo(x.Name).ISOCurrencySymbol == currency)
      ;
    return Amount.ToString("C", ci);
  }

  /// <summary>
  /// Return current Money object's price, rounded commercially
  /// </summary>
  public Money Rounded
  {
    get
    {
      decimal amount = Amount * 100 + 49;
      int rounded = (int)amount;
      rounded = rounded - (rounded % 50);
      amount = (decimal)(rounded - 1) / 100;
      return new Money(amount, Currency);
    }
  }

  // Subsitute for with syntax
  public Money WithAmount(decimal amount)
  => new Money(amount, this.Currency);

  public int CompareTo(Money other)
=> Comparer<decimal>.Default.Compare(this.Amount, other.Amount);


  public static bool operator <=(Money left, Money right)
  => Comparer<Money>.Default.Compare(left, right) <= 0;

  public static bool operator <(Money left, Money right)
  => Comparer<Money>.Default.Compare(left, right) < 0;

  public static bool operator >=(Money left, Money right)
  => Comparer<Money>.Default.Compare(left, right) >= 0;

  public static bool operator >(Money left, Money right)
  => Comparer<Money>.Default.Compare(left, right) > 0;

  public static Money Add(Money left, Money right)
  => left + right;

  public static Money operator +(Money m1, Money m2)
  {
    Debug.Assert(m1.Currency == m2.Currency);
    return new Money(m1.Amount + m2.Amount, m1.Currency);
  }

  public static Money Subtract(Money left, Money right)
  => left - right;

  public static Money operator -(Money m1, Money m2)
  {
    Debug.Assert(m1.Currency == m2.Currency);
    return new Money(m1.Amount - m2.Amount, m1.Currency);
  }
}
