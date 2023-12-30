using Azure;
using Azure.AI.OpenAI;
using Microsoft.Extensions.Options;
using Pgvector;

namespace Example.Services;

public class OpenAIService : IOpenAIService
{
    private readonly OpenAiSettings _settings;
    private OpenAIClient? _client;

    /// <summary>
    /// Constructor
    /// </summary>
    public OpenAIService(IOptions<OpenAiSettings>  settings)
    {
        _settings = settings.Value;
    }

    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="text">The text to create an embedding with</param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    public async Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        return await GenerateEmbeddingAsync(new List<string> { text }, cancellationToken);
    }

    /// <summary>
    /// Creates an embedding using the model specified in the settings.
    /// </summary>
    /// <param name="input">Input texts to get embeddings for, encoded as a an array of strings. Each input must not exceed 2048 tokens in length.
    /// Unless you are embedding code, we suggest replacing newlines (\n) in your input with a single space, as we have observed inferior results when newlines are present.
    /// </param>
    /// <param name="cancellationToken">The cancellation token to stop the process if necessary</param>
    public async Task<Vector> GenerateEmbeddingAsync(IEnumerable<string> input, CancellationToken cancellationToken = default)
    {
        OpenAIClient client = GetClient();

        var result = await client.GetEmbeddingsAsync(new EmbeddingsOptions(_settings.DeploymentOrModelName, input), cancellationToken);

        float[] embedding = result.Value.Data[0].Embedding.ToArray();

        return new Vector(embedding);
    }

    public int EmbeddingVersion { get; set; } = 1;

    /// <summary>
    /// Creates a client.
    /// </summary>
    private OpenAIClient GetClient()
    {
        if (_client == null)
        {
            if (string.IsNullOrWhiteSpace(_settings.Endpoint) || _settings.Endpoint.StartsWith("---"))
            {
                throw new ArgumentException("Please specify a URL endpoint for the open AI client!  You can obtain it from the Azure Portal.");
            }

            if (string.IsNullOrWhiteSpace(_settings.Key) || _settings.Endpoint.StartsWith("---"))
            {
                throw new ArgumentException("Please specify a KEY for the open AI client!  You can obtain it from the Azure Portal.");
            }

            _client = new OpenAIClient(new Uri(_settings.Endpoint), new AzureKeyCredential(_settings.Key));
        }

        return _client;
    }
}