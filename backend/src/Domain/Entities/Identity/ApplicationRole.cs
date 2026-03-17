using Microsoft.AspNetCore.Identity;

namespace TTRPG.Toolkit.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole()
    {
        Id = Guid.CreateVersion7();
    }
    
    public ApplicationRole(string name) : base(name)
    {
        Id = Guid.CreateVersion7();
    }
}