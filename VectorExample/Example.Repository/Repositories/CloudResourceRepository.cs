using Example.Model;
using Microsoft.EntityFrameworkCore;

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
}