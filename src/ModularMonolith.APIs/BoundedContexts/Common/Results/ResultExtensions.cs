namespace ModularMonolith.BoundedContexts.Common.Results;

public static class ResultExtensions
{
  public static Result<R> Switch<T, R>(
    this Result<T> source
  , Func<T, Result<R>> success
  , Func<Error, Error> failure)
  {
    if (source.IsSuccess)
    {
      // Process the success case
      return success(source.Value); 
    }
    else
    {
      // Map error to Result<R>
      return Result<R>.Failure(failure(source.Error));
    }
  }

  public static Result<R> Switch<T, R>(
  this Result<T> source
  , Func<T, Result<R>> success)
  => source.Switch(
        success
      , error => error);


  public static Result<R> Select<T, R>(
    this Result<T> source
  , Func<T, Result<R>> selector)
  {
    return source.Switch(
      result => selector(result)
    , error => source.Error);
  }

  public static Result<R> SelectMany<T, M, R>(
    this Result<T> source,
    Func<T, Result<M>> bind,
    Func<T, M, Result<R>> resultSelector)
  {
  return source.Switch(
    success: r =>
    {
      Result<M> bindResult = bind(r);
      return bindResult.Select(m => resultSelector(r, m));
    },
    failure: error => error);
  }

  public static Result<T> Where<T>(
    this Result<T> source,
    Func<T, bool> predicate,
    string errorCode,
    string errorMessage)
  {
    return source.IsSuccess && predicate(source.Value)
        ? source
        : Result<T>.Failure(errorCode, errorMessage);
  }
}
