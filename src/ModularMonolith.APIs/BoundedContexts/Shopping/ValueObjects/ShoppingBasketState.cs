namespace ModularMonolith.APIs.BoundedContexts.Shopping.ValueObjects;

public enum ShoppingBasketState
{
  Open = 0,
  CheckedOut = 10,
  Boxed = 20,
  Shipped = 30,
  Delivered = 40,
  // Closed States
  Cancelled = 90,
  Fulfilled = 100
}
