namespace Example.Repository;

public class RepositorySettings
{
    public static string SectionName = "Repository";
    public string DatabaseConnectionString { get; set; }
    public bool RunMigrationsOnStartup { get; set; } = true;
}