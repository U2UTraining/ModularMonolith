namespace ModularMonolith.APIs.BoundedContexts.Common.Results;

// For a more complete implementation of Result, see e.g.
// https://github.com/altmann/FluentResults

public readonly record struct Result
{
  private Result(bool success, Error error)
  {
    _ = (success, error) switch
    {
      (false, Error e) when e.Code is null
        => throw new InvalidOperationException(
          "Failure Result must have a code"),
      (false, Error e) when e.Message is null
        => throw new InvalidOperationException(
        "Failure Result must have a message"),
      (true, Error e) when e != Error.None
        => throw new InvalidOperationException(
          "Success Result must not have an Error"),
      _ => 0
    };
    this._success = success;
    this._error = error;
  }

  public static Result Failure(
    string code
  , string errorMessage)
  => new Result(false, Error.FromMessage(code, errorMessage));

  public static Result Success()
  => new Result(true, Error.None);

  private readonly bool _success;
  private readonly Error _error;

  public bool IsSuccess
  => _success;

  public bool IsFailure
  => !IsSuccess;

  public Error Error
  => _error;
}

public readonly record struct Result<T>
{
  private Result(bool success, T value, Error error)
  {
    _success = success;
    _value = value;
    _error = error;
  }

  public static Result<T> Failure(Error error)
  => new Result<T>(false, default!, error);

  public static Result<T> Failure(string code, string errorMessage)
  => Failure(Error.FromMessage(code, errorMessage));

  public static Result<T> Success(T value)
  => new Result<T>(true, value, Error.None);

  private readonly bool _success;
  private readonly T _value;
  private readonly Error _error;

  public bool IsSuccess
  => _success;

  public bool IsFailure
  => !IsSuccess;

  public T Value
  {
    get
    {
      if (IsFailure)
      {
        throw new InvalidOperationException("Cannot access Value on a failed Result.");
      }
      return _value;
    }
  }

  public Error Error
  => _error;
}
