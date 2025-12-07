namespace ModularMonolith.APIs.BoundedContexts.Shopping.Repositories;

/// <summary>
/// Shopping repository using extension method style.
/// </summary>
/// <remarks>
/// By using extension methods we can avoid repeating ourselves while having the power of a repository
/// with methods to retrieve items from the database.
/// </remarks>
internal static class ShoppingRepostory
{
  extension(ShoppingDb db)
  {
    private IQueryable<ShoppingBasket> ShoppingBasketAggregate(
      bool includeGames = false
    , bool includeCustomer = false
    )
    {
      IQueryable<ShoppingBasket> query = db.Baskets;
      if (includeGames)
      {
        query = query.Include(sb => sb.Items);
      }
      if (includeCustomer)
      {
        query = query.Include(sb => sb.Customer);
      }
      return query;
    }

    /// <summary>
    /// Retrieves a shopping basket by its identifier asynchronously.
    /// </summary>
    /// <param name="shoppingBasketId"></param>
    /// <param name="includeGames"></param>
    /// <param name="includeCustomer"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async ValueTask<ShoppingBasket?> GetShoppingBasketAsync(
      PK<int> shoppingBasketId
    , bool includeGames = false
    , bool includeCustomer = false
    , CancellationToken cancellationToken = default)
    {
      return await db.ShoppingBasketAggregate(includeGames: includeGames, includeCustomer: includeCustomer)
        .AsNoTracking()
        .Where(sb => sb.Id == shoppingBasketId)
        .SingleOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes all baskets that are marked as deleted from the database asynchronously.
    /// </summary>
    /// <remarks>
    /// This method permanently removes baskets that have been soft-deleted. The operation is
    /// performed in the database and may affect multiple records. If the cancellation token is triggered before
    /// completion, the operation will be canceled.
    /// </remarks>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the delete operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of baskets deleted.</returns>

    public async ValueTask<int> DeleteOldBasketsAsync(
      CancellationToken cancellationToken)
    {
      return await db
        .Baskets
        .Where(sb => EF.Property<bool>(sb, SoftDeleteable.IsDeleted) == true)
        .ExecuteDeleteAsync(cancellationToken);
    }
  }
}
