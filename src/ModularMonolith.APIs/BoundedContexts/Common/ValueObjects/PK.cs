namespace ModularMonolithBoundedContexts.Common.ValueObjects;

/// <summary>
/// ValueObject to denote primary key
/// </summary>
/// <typeparam name="T">Type of primary key</typeparam>
/// <remarks>
/// For performance, declare structs as readonly to avoid unnecessary copying
/// Also, use default to create a new PK
/// </remarks>
[DebuggerDisplay("PK {Value}")]
public readonly record struct PK<T>
{
  public PK(T key) 
  => Key = key;

  public T Key { get; }

  public static implicit operator T(PK<T> pk) 
  => pk.Key;
  public static implicit operator PK<T>(T key)
  => new(key);
}
