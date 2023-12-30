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

    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="input">Input texts to get embeddings for, encoded as a an array of strings. Each input must not exceed 2048 tokens in length.
    /// Unless you are embedding code, we suggest replacing newlines (\n) in your input with a single space, as we have observed inferior results when newlines are present.
    /// </param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    Task<Vector> GenerateEmbeddingAsync(IEnumerable<string> input, CancellationToken cancellationToken = default);

    int EmbeddingVersion { get; set; }
}