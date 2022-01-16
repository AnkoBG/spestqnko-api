using Microsoft.EntityFrameworkCore;

namespace Spestqnko.Api
{
    public static class EnsureMigration
    {
        public static IApplicationBuilder EnsureMigrationOfContext<T>(this IApplicationBuilder app) where T : DbContext
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var context = serviceScope.ServiceProvider.GetService<T>();

            if(context?.Database?.GetPendingMigrations().Count() > 0)
                context?.Database?.Migrate();

            return app;
        }
    }
}
