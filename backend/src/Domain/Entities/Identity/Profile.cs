namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class Profile
{
    public Guid Id { get; private set; }
    public string? DisplayName { get; private set; }
    public string? Bio { get; private set; }
    public string? AvatarUrl { get; private set; }
    
    protected Profile()
    {}

    public Profile(string? displayName, string? bio, string? avatarUrl)
    {
        Id = Guid.CreateVersion7();
        DisplayName = displayName;
        Bio = bio;
        AvatarUrl = avatarUrl;
    }
}