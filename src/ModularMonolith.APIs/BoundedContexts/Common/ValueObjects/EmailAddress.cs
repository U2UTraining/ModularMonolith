namespace ModularMonolith.APIs.BoundedContexts.Common.ValueObjects;

/// <summary>
/// EmailAddress
/// </summary>
/// <exception cref="ArgumentException">Thrown when invalid</exception>

[DebuggerDisplay("Email {Value,nq}")]
public readonly record struct EmailAddress
{
  // https://en.wikipedia.org/wiki/Email_address
  // The format of an email address is local-part@domain, where the local-part
  // may be up to 64 octets long and the domain may have a maximum of 255 octets.[5]
  // The formal definitions are in RFC 5322 (sections 3.2.3 and 3.4.1) and
  // RFC 5321—with a more readable form given in the informational RFC 3696
  // (written by J. Klensin, the author of RFC 5321[6]) and the associated errata.
  public const int EmailMaxLength = 320;

  //private static readonly Regex EmailRegEx =
  //    new(pattern: EmailPattern,
  //        options: RegexOptions.Compiled | RegexOptions.IgnoreCase);

  [DebuggerBrowsable(DebuggerBrowsableState.Never)]
  private readonly NonEmptyString _value;

  public string Value => _value.Value;

  public EmailAddress(string value)
  {
    _value = new NonEmptyString(value);
    if (!EmailHelper.RegEx().IsMatch(value) || value is not string { Length: <= EmailMaxLength })
    {
      throw new ArgumentException(
        message: $"Value {value} is not a valid e-mail address",
        paramName: nameof(value));
    }
  }

  public override string ToString() 
  => Value;

  public static implicit operator EmailAddress(string value) 
  => new EmailAddress(value);

  public static implicit operator string(EmailAddress fn) 
  => fn.Value;
}

internal partial class EmailHelper
{
  // https://www.regular-expressions.info/
  public const string EmailPattern = @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

  // Using the regular expression source generator
  // https://learn.microsoft.com/dotnet/standard/base-types/regular-expression-source-generators
  [GeneratedRegex(EmailPattern, RegexOptions.IgnoreCase, "en-US")]
  public static partial Regex RegEx();
}
