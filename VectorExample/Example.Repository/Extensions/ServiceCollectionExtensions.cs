using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Pgvector.EntityFrameworkCore;

namespace Example.Repository;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection sc, IConfiguration config)
    {
        var repoSettings = new RepositorySettings();
        config.GetSection(RepositorySettings.SectionName).Bind(repoSettings);

        sc.AddSingleton(repoSettings);
        
        sc.AddDbContext<AppDbContext>((serviceProvider, dbContextOptions) =>
        {
            // Using Vectors https://github.com/pgvector/pgvector-dotnet#entity-framework-core
            dbContextOptions.UseNpgsql(repoSettings.DatabaseConnectionString, o => o.UseVector());
        });

        sc.AddScoped<ICloudResourceRepository, CloudResourceRepository>();

        return sc;
    }
}