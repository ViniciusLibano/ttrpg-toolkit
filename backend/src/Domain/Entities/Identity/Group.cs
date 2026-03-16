namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class Group
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid OwnerId { get; private set; }
    
    // navigation properties
}