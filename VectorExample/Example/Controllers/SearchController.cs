using Example.Model;
using Example.Services;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;
    private readonly ICloudResourceService _cloudResourceService;

    public SearchController(ILogger<SearchController> logger, ICloudResourceService cloudResourceService)
    {
        _logger = logger;
        _cloudResourceService = cloudResourceService;
    }

    [HttpGet]
    public async Task<IEnumerable<CloudResourceDto>> SearchAsync([FromQuery] string query, CancellationToken cancellationToken)
    {
        // TODO: Use Automapper
        var dbResult = await _cloudResourceService.FindNearestNeighborAsync(query, 3, cancellationToken);

        var result = new List<CloudResourceDto>();
        foreach (var item in dbResult)
        {
            result.Add(new CloudResourceDto()
            {
                Category = item.Category,
                Id = item.Id,
                Content = item.Content,
                Title = item.Title
            });
        }

        return result;

    }
}