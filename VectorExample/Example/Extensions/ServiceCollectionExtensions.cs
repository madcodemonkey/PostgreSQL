using Example.Repository;
using Example.Services;

namespace Example;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExampleDependencies(this IServiceCollection sc, IConfiguration config)
    {
        sc.AddAutoMapper(typeof(MappingProfiles).Assembly);

        sc.AddRepositories(config);
        sc.AddServices(config);

        return sc;
    }
}