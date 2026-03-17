using Microsoft.EntityFrameworkCore;
using TTRPG.Toolkit.Domain.Entities.Identity;
using TTRPG.Toolkit.Infrastructure.Database;

namespace TTRPG.Toolkit.Shared.Extensions;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDbContext()
        {
            services.AddDbContext<AppDbContext>(options => 
                options.UseSqlite("Data source=app.db"));

            return services;
        }
        
        public IServiceCollection AddIdentityModule()
        {
            services.AddAuthorization();

            services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            return services;
        }
    }
}