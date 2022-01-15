using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Spestqnko.Core.Models;

namespace Spestqnko.Data
{
    public class SpestqnkoDbContext : IdentityDbContext<User, Role, Guid>
    {
        public SpestqnkoDbContext(DbContextOptions<SpestqnkoDbContext> options)
            : base(options)
        {
            
        }
    }
}