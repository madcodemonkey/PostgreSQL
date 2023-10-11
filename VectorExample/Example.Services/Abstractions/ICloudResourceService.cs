using Example.Model;

namespace Example.Services;

public interface ICloudResourceService
{
    /// <summary>
    /// Finds the items nearest to the query text.
    /// </summary>
    /// <param name="query">Query text</param>
    /// <param name="numberOfNeighbors">Number of neighbors to return</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<List<CloudResource>> FindNearestNeighborAsync(string query, int numberOfNeighbors, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the embeddings for vector fields on any document that has the incorrect Embedding version number.
    /// Documents that are created without embeddings have a version number of zero and will be updated by calling this method.
    /// </summary>
    /// <param name="batchSize">The number of items to update</param>
    /// <param name="cancellationToken">A cancellation token</param>
    Task<int> UpdateEmbeddingsAsync(int batchSize, CancellationToken cancellationToken = default);
}