using Spestqnko.Data;

namespace Spestqnko.Api.Configurations
{
    public static class MigrationsConfiguration
    {
        public static IApplicationBuilder ConfigureMigrations(this IApplicationBuilder app)
        {
            return app.EnsureMigrationOfContext<SpestqnkoDbContext>();
        }
    }
}
