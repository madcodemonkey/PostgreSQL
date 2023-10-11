using Example.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.AI.Embeddings.VectorOperations;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace Example.Repository;

public class CloudResourceRepository : RepositoryBase<CloudResource, int>, ICloudResourceRepository
{
    /// <summary>
    /// Constructor
    /// </summary>
    public CloudResourceRepository(AppDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Finds records that don't have the specified Embedding Version  
    /// </summary>
    /// <param name="batchSize">The size of the batch to process</param>
    /// <param name="embeddingVersion">The current embedding version number</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public Task<List<CloudResource>> FindDocsThatNeedEmbeddingAsync(int batchSize, int embeddingVersion, CancellationToken cancellationToken = default)
    {
        var result = Context.CloudResources
            .Where(w => w.VectorEmbeddingVersion != embeddingVersion)
            .Take(batchSize)
            .ToListAsync(cancellationToken);

        return result;
    }

    /// <summary>
    /// Finds the items nearest to the query text.
    /// </summary>
    /// <param name="queryVector">The query embedding to find</param>
    /// <param name="numberOfNeighbors">Number of neighbors to return</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <remarks>
    /// This method demonstrates merging two searches and then picking the best answer from the combined items.
    /// </remarks>
    public async Task<List<CloudResource>> FindNearestNeighborAsync(Vector queryVector, int numberOfNeighbors, CancellationToken cancellationToken = default)
    {
        var combinedList = new List<OneVectorAndData<CloudResource>>();

        // Find all the title and content items
        var titleItems = await FindNearestNeighborByTitleAsync(queryVector, numberOfNeighbors, cancellationToken);
        var contentItems = await FindNearestNeighborByContentAsync(queryVector, numberOfNeighbors, cancellationToken);

        // Combine them but assign TheVector field based on the source.
        combinedList.AddRange(
            titleItems.Select(s =>
                new OneVectorAndData<CloudResource>
                {
                    TheVector = s.TitleVector?.ToArray() ?? Array.Empty<float>(), // Title is source, so use title vector
                    Data = s
                }).ToList()
        );
        combinedList.AddRange(
            contentItems.Select(s =>
                new OneVectorAndData<CloudResource>
                {
                    TheVector = s.ContentVector?.ToArray() ?? Array.Empty<float>(),  // Content is the source, so use content vector
                    Data = s
                }).ToList()
        );

        // From these top items find the ones that are closest to the query 
        // Note 1: To use the CosineSimilarity extension method it requires the Microsoft.SemanticKernel.Core NuGet package (1.0.0-beta1 Oct 2023)
        // Note 2: Docs for CosineSimilarity https://learn.microsoft.com/en-us/dotnet/api/microsoft.semantickernel.ai.embeddings.vectoroperations.cosinesimilarityoperation.cosinesimilarity?view=semantic-kernel-dotnet
        // Note 3: We are ordering from 1 to -1 (descending order) because items that are closer to 1 are more like each other.
        var topItems = combinedList.OrderByDescending(x => x.TheVector?.CosineSimilarity(queryVector.ToArray()) ?? -1.0)
            .Take(numberOfNeighbors)
            .ToList();

        var result = topItems.Select(s => s.Data).ToList();

        return result;
    }

    /// <summary>
    /// Finds the Content column items nearest to the query text by querying the ContentVector column.
    /// </summary>
    /// <param name="queryVector">The query embedding to find</param>
    /// <param name="numberOfNeighbors">Number of neighbors to return</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<List<CloudResource>> FindNearestNeighborByContentAsync(Vector queryVector, int numberOfNeighbors, CancellationToken cancellationToken = default)
    {
        var contentItems = await Context.CloudResources
            .OrderBy(x => x.ContentVector!.L2Distance(queryVector))
            .Take(numberOfNeighbors)
            .ToListAsync(cancellationToken);

        return contentItems;
    }
    /// <summary>
    /// Finds the Title column items nearest to the query text by querying the TitleVector column.
    /// </summary>
    /// <param name="queryVector">The query embedding to find</param>
    /// <param name="numberOfNeighbors">Number of neighbors to return</param>
    /// <param name="cancellationToken">The cancellation token</param>
    public async Task<List<CloudResource>> FindNearestNeighborByTitleAsync(Vector queryVector, int numberOfNeighbors, CancellationToken cancellationToken = default)
    {
        var titleItems = await Context.CloudResources
            .OrderBy(x => x.TitleVector!.L2Distance(queryVector))
            .Take(numberOfNeighbors)
            .ToListAsync(cancellationToken);

        return titleItems;
    }
}