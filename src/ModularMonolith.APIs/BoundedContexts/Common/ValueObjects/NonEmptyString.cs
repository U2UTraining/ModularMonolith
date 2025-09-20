namespace ModularMonolithBoundedContexts.Common.ValueObjects;

/// <summary>
/// String whose value cannot be empty
/// </summary>
/// <exception cref="ArgumentException">
///   Thrown when invalid (string.Empty)
/// </exception>
/// <exception cref="ArgumentNullException">
///   Thrown when invalid (null)
/// </exception>

[DebuggerDisplay("NonEmpty String {Value,nq}")]
public readonly record struct NonEmptyString
: IEquatable<NonEmptyString>
, IComparable<NonEmptyString>
{
  public NonEmptyString(string value)
  => _value = ThrowIfNonEmptyString(value);

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private readonly string _value;

  public string Value
  => ThrowIfNonEmptyString(_value);

  // Implicit conversion from string to NonEmptyString
  public static implicit operator NonEmptyString(string value)
  => new NonEmptyString(value);

  public static implicit operator string(NonEmptyString nes)
  => nes.Value;

  public override string ToString()
  => Value;

  private string ThrowIfNonEmptyString(string? value)
  {
    ArgumentException.ThrowIfNullOrEmpty(value);
    return value;
  }

  // operator== and operator!= are done with record syntax

  public int CompareTo(NonEmptyString other)
  => Comparer<string>.Default.Compare(this, other);

  //=> Value.CompareTo(other.Value);

  public static bool operator <(NonEmptyString nes1, NonEmptyString nes2)
  => nes1.CompareTo(nes2) < 0;

  public static bool operator >(NonEmptyString nes1, NonEmptyString nes2)
  => nes1.CompareTo(nes2) > 0;

  public static bool operator <=(NonEmptyString nes1, NonEmptyString nes2)
  => nes1.CompareTo(nes2) <= 0;

  public static bool operator >=(NonEmptyString nes1, NonEmptyString nes2)
  => nes1.CompareTo(nes2) >= 0;
}
