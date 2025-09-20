namespace ModularMonolithBoundedContexts.Common.Repositories;

/// <summary>
/// IRepository<typeparamref name="T"/> supports all actions of a IReadonlyRepository<typeparamref name="T"/>
/// adding mutation methods to insert, update, and delete.
/// </summary>
/// <typeparam name="T">The entity class' type.</typeparam>
public interface IRepository<T>
: IReadonlyRepository<T>
where T 
: class
, IAggregateRoot
{
  /// <summary>
  /// Insert a new instance, asynchronously
  /// </summary>
  /// <param name="entity">A new instance to insert.</param>
  ValueTask InsertAsync(T entity, CancellationToken token);

  /// <summary>
  /// Delete an instance, asynchronously.
  /// </summary>
  /// <param name="entity">The instance that needs to disappear.</param>
  ValueTask DeleteAsync(T entity, CancellationToken token);

  /// <summary>
  /// Update an instance, asynchronously.
  /// </summary>
  /// <param name="entity">Some instance to update.</param>
  ValueTask UpdateAsync(T entity, CancellationToken token);

  /// <summary>
  /// Asynchronously saves all changes made in the current context 
  /// to the underlying data store.
  /// </summary>
  /// <remarks>
  /// This method commits all tracked changes in the context to 
  /// the data store.  If the operation is canceled via the 
  /// provided <paramref name="token"/>,  the task will be 
  /// marked as canceled.
  /// </remarks>
  /// <param name="token">
  /// A <see cref="CancellationToken"/> that can be used to 
  /// cancel the save operation.
  /// </param>
  /// <returns>
  /// A <see cref="ValueTask"/> that represents the asynchronous 
  /// save operation.
  /// </returns>
  ValueTask SaveChangesAsync(CancellationToken token);
}

