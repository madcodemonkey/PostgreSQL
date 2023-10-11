using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Example.Repository;

// Requires Microsoft.Extensions.Hosting.Abstractions
public static class HostExtensions
{
    public static async Task ApplyMigrationsAsync(this IServiceProvider serviceProvider)
    {
        var connectionsConfig = serviceProvider.GetRequiredService<IOptions<RepositorySettings>>();

        // Should the migration be run?
        if ((connectionsConfig.Value?.RunMigrationsOnStartup ?? false) == false) return;

        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            {
                var dbContext = services.GetRequiredService<AppDbContext>();

                await dbContext.Database.MigrateAsync();

               
                await SeedDatabaseMyStuff.SeedDataAsync(dbContext);
            }
        }
    }

}