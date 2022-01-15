using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;
using Spestqnko.Data;

namespace Spestqnko.Api.Configurations
{
    public static  class DbContextConfiguration
    {
        public static void ConfigureDbContexts(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SpestqnkoDbContext>(options =>
                options.UseNpgsql(
                    connectionString,
                    b => b.MigrationsAssembly("Spestqnko.Data")));

            services.AddDatabaseDeveloperPageExceptionFilter();

            services
                .AddIdentity<User, Role>(options => 
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1d);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                })
                .AddEntityFrameworkStores<SpestqnkoDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
