using EcoTrack.Application.Dtos;
using EcoTrack.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcoTrack.Net.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController(ProductService svc) : ControllerBase
{
	[HttpGet("{id:guid}")]
	public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct)
		=> (await svc.GetAsync(id, ct)) is { } dto ? Ok(dto) : NotFound();

	[HttpGet]
	public async Task<ActionResult> Search([FromQuery] string? q, [FromQuery] string? category, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, CancellationToken ct = default)
	{
		var (items, total) = await svc.SearchAsync(q, category, Math.Max(1, page), Math.Clamp(pageSize, 1, 100), ct);
		return Ok(new { total, page, pageSize, items });
	}

	[HttpPost]
	public async Task<ActionResult> Create([FromBody] CreateProductRequest req, CancellationToken ct)
	{
		var id = await svc.CreateAsync(req, ct);
		return CreatedAtAction(nameof(GetById), new { id }, new { id });
	}

	[HttpPut("{id:guid}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest req, CancellationToken ct)
	{ await svc.UpdateAsync(id, req, ct); return NoContent(); }

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
	{ await svc.DeleteAsync(id, ct); return NoContent(); }
}
