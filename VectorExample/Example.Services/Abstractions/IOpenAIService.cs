using Pgvector;

namespace Example.Services;

public interface IOpenAIService
{
    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="text">The text to create an embedding with</param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);

    int EmbeddingVersion { get; set; }
}