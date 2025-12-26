namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// Percentage, with normal value between 0% and 100%
/// </summary>
/// <remarks>
/// Allows for smaller and bigger percentages, like 120%
/// </remarks>

[DebuggerDisplay("{Percentage}%")]
public readonly record struct Percent
: IEquatable<Percent>
{
  public static Percent Zero { get; } = new Percent(0M);
  public static Percent Hundred { get; } = new Percent(100M);

  public Percent(decimal percentage) 
  => Percentage = percentage;

  public decimal Percentage { get; }

  public decimal Factor => Percentage / 100M;

  public override string ToString() 
  => $"{Percentage}%";

  // operators - any missing?
  public static Percent operator+(Percent a, Percent b)
  => new Percent(a.Percentage + b.Percentage);
  public static Percent operator -(Percent a, Percent b)
  => new Percent(a.Percentage - b.Percentage);
  public static Percent operator *(Percent a, Percent b)
  => new Percent(a.Percentage * b.Percentage);
  public static Percent operator /(Percent a, Percent b)
  => new Percent(a.Percentage / b.Percentage);
  public static Percent operator %(Percent a, Percent b)
  => new Percent(a.Percentage % b.Percentage);

  public int CompareTo(Percent other)
  => Comparer<decimal>.Default.Compare(this.Percentage, other.Percentage);

  // operator== and operator!= are done with record syntax

  public static bool operator <=(Percent left, Percent right)
  => Comparer<Percent>.Default.Compare(left, right) <= 0;
  public static bool operator >=(Percent left, Percent right)
  => Comparer<Percent>.Default.Compare(left, right) >= 0;
  public static bool operator <(Percent left, Percent right)
  => Comparer<Percent>.Default.Compare(left, right) < 0;
  public static bool operator >(Percent left, Percent right)
  => Comparer<Percent>.Default.Compare(left, right) > 0;
}
