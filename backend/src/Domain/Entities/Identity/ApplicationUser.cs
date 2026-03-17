using Microsoft.AspNetCore.Identity;
using TTRPG.Toolkit.Domain.Enums;
using TTRPG.Toolkit.Domain.Shared;

namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? ProfileId { get; private set; }
    public Profile? Profile { get; private set; }
    
    public DateTime? LastLoginAt { get; private set; }
    public string TimeZone { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public ApplicationUser()
    {
        Id = Guid.CreateVersion7();
        TimeZone = null!;
    }
    
    public ApplicationUser(string username, string timeZone) : base(username)
    {
        Id = Guid.CreateVersion7();
        CreatedAt = DateTime.UtcNow;
        TimeZone = timeZone;
    }

    public Result AssignProfile(Guid profileId)
    {
        if (ProfileId is not null)
            return Result.Failure(
                "User.ProfileAlreadyAssigned",
                "Usuário já possui perfil vinculado.",
                ErrorType.Conflict);

        return Result.Success();
    }

    private void SetUpdatedTimestamp() => UpdatedAt = DateTime.UtcNow;

    private void SetUpdatedLastLogin() => LastLoginAt = DateTime.UtcNow;
}