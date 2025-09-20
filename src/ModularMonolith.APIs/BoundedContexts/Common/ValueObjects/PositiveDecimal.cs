namespace ModularMonolith.BoundedContexts.Common.ValueObjects;

/// <summary>
/// PositiveDecimal represents a decimal number which > 0M
/// </summary>
/// <remarks>
/// Make Value Objects readonly record struct!
/// This make the value object immutable.
/// Any properties should only have a getter, and the constructor initializes them.
/// This way using with-syntax is prevented.
/// For more complex initialization you can use a factory method instead of ctor.
/// Do avoid using default, e.g. PositiveDecimal dec = default will assign 0, bypassing ctor!
/// The database uses a constraint to ensure positive!
/// </remarks>

[DebuggerDisplay("Positive Decimal {Value}")]
public readonly record struct PositiveDecimal
: IEquatable<PositiveDecimal>    // handled by record 
, IComparable<PositiveDecimal>
{
  public PositiveDecimal(decimal value) 
  => _value = ThrowIfNonPositiveDecimal(value);

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private readonly decimal _value;

  // This will slow things down just a tiny bit,
  // but better fail fast!
  public decimal Value 
  => ThrowIfNonPositiveDecimal(_value);

  private decimal ThrowIfNonPositiveDecimal(decimal value)
  {
    if (value <= 0M)
    {
      throw new ArgumentException(
        message: $"Positive decimal has to be positive (and zero is not positive)"
      , paramName: nameof(value));
    }
    return value;
  }

  public override string ToString() 
  => Value.ToString();

  public int CompareTo(PositiveDecimal other)
  => Comparer<decimal>.Default.Compare(this.Value, other.Value);

  // operator== and operator!= are done with record syntax

  public static bool operator <=(PositiveDecimal left, PositiveDecimal right)
  => Comparer<PositiveDecimal>.Default.Compare(left, right) <= 0;
  public static bool operator >=(PositiveDecimal left, PositiveDecimal right)
  => Comparer<PositiveDecimal>.Default.Compare(left, right) >= 0;
  public static bool operator <(PositiveDecimal left, PositiveDecimal right)
  => Comparer<PositiveDecimal>.Default.Compare(left, right) < 0;
  public static bool operator >(PositiveDecimal left, PositiveDecimal right)
  => Comparer<PositiveDecimal>.Default.Compare(left, right) > 0;

  // Implicit conversion to decimal
  public static implicit operator decimal(PositiveDecimal pd)
  => pd.Value;

  public static implicit operator PositiveDecimal(decimal d)
  => new PositiveDecimal(d);
}
