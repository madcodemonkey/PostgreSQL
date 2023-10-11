 
using Example.Repository;

namespace Example.Services;

public class CustomDataService : ICustomDataService
{
    private readonly ICloudResourceRepository _cloudResourceRepository;
    private readonly IOpenAIService _openAIService;

    /// <summary>
    /// Constructor
    /// </summary>
    public CustomDataService(ICloudResourceRepository cloudResourceRepository,
        IOpenAIService openAIService)
    {
        _cloudResourceRepository = cloudResourceRepository;
        _openAIService = openAIService;
    } 

    /// <summary>
    /// Updates the embeddings for vector fields on any document that has the incorrect Embedding version number.
    /// Documents that are created without embeddings have a version number of zero and will be updated by calling this method.
    /// </summary>
    /// <param name="batchSize">The number of items to update</param>
    /// <param name="cancellationToken">A cancellation token</param>
    /// <returns></returns>
    public async Task<int> UpdateEmbeddingsAsync(int batchSize, CancellationToken cancellationToken = default)
    {
        int numberUpdated = 0;
        int numberToSave = 0;

        var batchOfRecords = await _cloudResourceRepository.FindDocsThatNeedEmbeddingAsync(
            batchSize, _openAIService.EmbeddingVersion, cancellationToken);

        foreach (var oneRecord in batchOfRecords)
        {
            oneRecord.TitleVector = await _openAIService.GenerateEmbeddingAsync(oneRecord.Title, cancellationToken);
            oneRecord.ContentVector = await _openAIService.GenerateEmbeddingAsync(oneRecord.Content, cancellationToken);
            oneRecord.VectorEmbeddingVersion = _openAIService.EmbeddingVersion;

            await _cloudResourceRepository.UpdateAsync(oneRecord, false, cancellationToken);
            numberToSave++;

            if (numberToSave > 9)
            {
                await _cloudResourceRepository.SaveChangesAsync(cancellationToken);
                numberUpdated += numberToSave;
                numberToSave = 0;
            }
        }

        if (numberToSave > 0)
        {
            await _cloudResourceRepository.SaveChangesAsync(cancellationToken);
            numberUpdated += numberToSave;
        }

        return numberUpdated;
    }
}