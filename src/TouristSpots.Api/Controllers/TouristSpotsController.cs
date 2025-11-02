using Microsoft.AspNetCore.Mvc;
using TouristSpots.Application.Requests;
using TouristSpots.Application.Services;

namespace TouristSpots.Api.Controllers;

[ApiController]
[Route("api/tourist-spots")]
public class TouristSpotsController : ControllerBase
{
    private readonly ITouristSpotService _service;
    public TouristSpotsController(ITouristSpotService service) => _service = service;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTouristSpotRequest req, CancellationToken ct)
    {
        var id = await _service.CreateAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken ct)
    {
        var dto = await _service.GetAsync(id, ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Search([FromQuery] string? term, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
    {
        var (items, total) = await _service.SearchAsync(term, page, pageSize, ct);
        return Ok(new { items, total, page, pageSize });
    }
}
