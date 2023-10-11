using AutoMapper;
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
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor
    /// </summary>
    public SearchController(ILogger<SearchController> logger, 
        ICloudResourceService cloudResourceService,
        IMapper mapper)
    {
        _logger = logger;
        _cloudResourceService = cloudResourceService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CloudResourceResponse>> SearchAsync([FromQuery] string query, CancellationToken cancellationToken)
    {
        var dbResult = await _cloudResourceService.FindNearestNeighborAsync(query, 3, cancellationToken);

        var result = _mapper.Map<List<CloudResourceResponse>>(dbResult);

        return result;
    }
}