using Example.Repository;
using Example.Services;

namespace Example;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExampleDependencies(this IServiceCollection sc, IConfiguration config)
    {
        
        sc.AddRepositories(config);
        sc.AddServices(config);

        return sc;
    }
}