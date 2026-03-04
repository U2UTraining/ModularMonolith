namespace ModularMonolith.APIs.EFCore.Auditability;

/// <summary>
/// Auditability default column names
/// </summary>
/// <remarks>
/// You could also keep track of the user who created or modified the entity, 
/// but that is out of scope for this example.
/// </remarks>
public static class Auditability
{
  public const string UtcCreated = nameof(UtcCreated);
  public const string UtcModified = nameof(UtcModified);
}
