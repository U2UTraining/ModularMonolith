namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// CreditCardNumber represents a credit card number
/// </summary>
/// <remarks>
/// Supports VISA, Master Card, American Express, Diners Club, Discover and JCB
/// </remarks>
/// <exception cref="ArgumentException">Thrown when invalid</exception>

[DebuggerDisplay("CCN {Value,nq}")]
public readonly record struct CreditCardNumber
{
  public const int CreditCardNumberMaxLength = 19;

  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  public CreditCardNumber(string value)
  {
    _value = new NonEmptyString(value);
    if (!CCN.RegEx().IsMatch(value))
    {
      throw new ArgumentException(
        message: $"Value {value} is not a valid credit card number",
        paramName: nameof(value));
    }
  }

  public override string ToString() 
  => Value;
}

internal partial class CCN
{
  // https://www.regular-expressions.info/creditcard.html
  public const string CCNPattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?|(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|6(?:011|5[0-9]{2})[0-9]{12}|(?:2131|1800|35\d{3})\d{11})$";

  [GeneratedRegex(CCNPattern, RegexOptions.IgnoreCase, "en-US")]
  public static partial Regex RegEx();
}
