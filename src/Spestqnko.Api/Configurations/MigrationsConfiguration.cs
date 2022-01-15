using Spestqnko.Data;

namespace Spestqnko.Api.Configurations
{
    public static class MigrationsConfiguration
    {
        public static void ConfigureMigrations(this IApplicationBuilder app)
        {
            app.EnsureMigrationOfContext<SpestqnkoDbContext>();
        }
    }
}
