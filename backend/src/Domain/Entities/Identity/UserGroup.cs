namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class UserGroup
{
    public Guid UserId { get; private set; }
    public Guid GroupId { get; private set; }
    // group role
    public DateTime JoinedAt { get; private set; }
    // navigation
}