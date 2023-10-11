using Example.Model;
using Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("[controller]")]
public class DataManipulationController : ControllerBase
{
    private readonly ILogger<DataManipulationController> _logger;
    private readonly ICloudResourceService _cloudResourceService;

    /// <summary>
    /// Constructor
    /// </summary>
    public DataManipulationController(ILogger<DataManipulationController> logger,
        ICloudResourceService cloudResourceService)
    {
        _logger = logger;
        _cloudResourceService = cloudResourceService;
    }

    [HttpPost("Update-Vector-Fields")]
    public async Task<string> UpdateVectorFieldsAsync(int batchSize, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Update embeddings in the {nameof(CloudResource)} table!");

        int numberUpdated = await _cloudResourceService.UpdateEmbeddingsAsync(batchSize, cancellationToken);

        return $"Updated {numberUpdated} records";
    }
}