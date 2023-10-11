using Example.Repository;

public class SeedDatabaseMyStuff
{
    public static async Task SeedDataAsync(AppDbContext context)
    {
        await SeedTableCloudResource.SeedAsync(context);
    }
}