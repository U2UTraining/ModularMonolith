namespace ModularMonolith.BoundedContexts.BoardGames.Entities;

/// <summary>
/// Entity representing a contact person for a publisher.
/// </summary>
public sealed class Contact
: EntityBase<PK<int>>
, IAggregate<Publisher>
, IHistory
, ISoftDeletable
{
  // Contacts can only be created through publisher
  internal Contact(
    PK<int> id
  , NonEmptyString firstName
  , NonEmptyString lastName
  , EmailAddress email
  )
  : base(id)
  {
    FirstName = firstName;
    LastName = lastName;
    Email = email;
  }

  public NonEmptyString FirstName { get; set; }
  public NonEmptyString LastName { get; set; }
  public EmailAddress Email { get; set; }
}
