using Example.Model;
using Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("[controller]")]
public class DataManipulationController : ControllerBase
{
    private readonly ILogger<DataManipulationController> _logger;
    private readonly ICustomDataService _customDataService;

    /// <summary>
    /// Constructor
    /// </summary>
    public DataManipulationController(ILogger<DataManipulationController> logger,
        ICustomDataService customDataService)
    {
        _logger = logger;
        _customDataService = customDataService;
    }

    [HttpPost("Update-Vector-Fields")]
    public async Task<string> UpdateVectorFieldsAsync(int batchSize, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Update embeddings in the {nameof(CloudResource)} table!");

        int numberUpdated = await _customDataService.UpdateEmbeddingsAsync(batchSize, cancellationToken);

        return $"Updated {numberUpdated} records";
    }
}