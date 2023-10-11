using Example.Model;

namespace Example.Repository;

public interface ICloudResourceRepository
{
    /// <summary>
    /// Finds records that don't have the specified Embedding Version  
    /// </summary>
    /// <param name="batchSize">The size of the batch to process</param>
    /// <param name="embeddingVersion">The current embedding version number</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<List<CloudResource>> FindDocsThatNeedEmbeddingAsync(int batchSize, int embeddingVersion, CancellationToken cancellationToken = default);

    /// <summary>Inserts one entity into the database.</summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<CloudResource> AddAsync(CloudResource entity, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>Deletes a list of entity from the database.</summary>
    /// <param name="items">Items to delete</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task DeleteAsync(List<CloudResource> items, bool saveChanges = true, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all entities from the database.</summary>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<List<CloudResource>> GetAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves one entity from the database.</summary>
    /// <param name="id">The primary key</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<CloudResource?> GetAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>Saves Changes.</summary>
    /// <param name="cancellationToken">The cancellation token</param>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>Updates one entity in the database.</summary>
    /// <param name="entity">Entity to update</param>
    /// <param name="saveChanges">Indicates if we should save immediately or not</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task UpdateAsync(CloudResource entity, bool saveChanges = true, CancellationToken cancellationToken = default);
}