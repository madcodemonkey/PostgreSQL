using Example.Model;
using System.Text.Json;
using VectorExample.Services;

namespace Example.Repository;

public class SeedTableCloudResource
{
    
    public static async Task SeedAsync(AppDbContext context )
    {
        if (context.CloudResources.Any()) return;

        var stringData = DataFileLoader.GetFileDataAsString("AzureCatalog.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var dataList = JsonSerializer.Deserialize<List<AzureCatalogData>>(stringData, options) ?? new List<AzureCatalogData>();

        foreach (AzureCatalogData item in dataList)
        {
            context.CloudResources.Add(new CloudResource()
            {
                Category = item.Category,
                Content = item.Content,
                Title = item.Title,
                VectorEmbeddingVersion = 0
            });
        }

        await context.SaveChangesAsync();
    }
}