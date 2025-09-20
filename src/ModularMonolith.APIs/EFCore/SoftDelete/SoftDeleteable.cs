namespace ModularMonolithEFCore.SoftDelete;

/// <summary>
/// Soft delete default column names
/// </summary>
public static class SoftDeleteable
{
  public const string IsDeleted = nameof(IsDeleted);
  public const string UtcDeleted = nameof(UtcDeleted);
}
