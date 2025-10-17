using Microsoft.AspNetCore.Mvc;
using Assignment4;

namespace WebServiceLayer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IDataService _service;
    public CategoriesController(IDataService service) => _service = service;

    // GET /api/categories
    [HttpGet]
    public IActionResult GetAll()
    {
        var cats = _service.GetCategories();
        return Ok(cats.Select(c => new { c.Id, c.Name, c.Description }));
    }

    // GET /api/categories/3
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var cat = _service.GetCategory(id);
        if (cat is null) return NotFound();
        return Ok(new { cat.Id, cat.Name, cat.Description });
    }

    // POST /api/categories
    // Body: { "name": "New name", "description": "..." }
    public record CategoryCreateRequest(string Name, string Description);

    [HttpPost]
    public IActionResult Create([FromBody] CategoryCreateRequest req)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.Name))
            return BadRequest("Name is required.");

        var created = _service.CreateCategory(req.Name, req.Description ?? string.Empty);
        return Created($"/api/categories/{created.Id}", new { created.Id, created.Name, created.Description });
    }

    // PUT /api/categories/3
    // Body: { "name": "Updated", "description": "..." }
    public record CategoryUpdateRequest(string Name, string Description);

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CategoryUpdateRequest req)
    {
        if (req is null || string.IsNullOrWhiteSpace(req.Name))
            return BadRequest("Name is required.");

        var ok = _service.UpdateCategory(id, req.Name, req.Description ?? string.Empty);
        if (!ok) return NotFound();
        return Ok(); // assignment table says "Ok" on valid update
    }

    // DELETE /api/categories/3
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var ok = _service.DeleteCategory(id);
        if (!ok) return NotFound();
        return Ok(); // assignment table says "Ok" on delete
    }
}
