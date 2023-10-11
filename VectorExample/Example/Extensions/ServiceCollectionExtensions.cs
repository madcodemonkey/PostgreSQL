using Example.Repository;

namespace Example;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddExampleDependencies(this IServiceCollection sc, IConfiguration config)
    {
        
        sc.AddRepositories(config);


        return sc;
    }
}