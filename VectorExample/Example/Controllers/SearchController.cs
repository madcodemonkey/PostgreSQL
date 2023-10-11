using Example.Model;
using Microsoft.AspNetCore.Mvc;

namespace Example.Controllers;

[ApiController]
[Route("[controller]")]
public class SearchController : ControllerBase
{
    private readonly ILogger<SearchController> _logger;

    public SearchController(ILogger<SearchController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{query}")]
    public IEnumerable<CloudResourceDto> SearchAsync(string query)
    {
        return new List<CloudResourceDto>();

    }
}