using Microsoft.AspNetCore.Identity;

namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; private set; }
    public string? AvatarUrl { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    
    // user preference
    // subscriptions tier
    
    // navigation properties
}