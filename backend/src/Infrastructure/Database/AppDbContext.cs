using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TTRPG.Toolkit.Domain.Entities.Identity;

namespace TTRPG.Toolkit.Infrastructure.Database;

public class AppDbContext(
    DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}