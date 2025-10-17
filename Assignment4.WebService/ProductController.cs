using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs;
using Assignment4;

namespace WebServiceLayer.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly IDataService _service;
  public ProductController(IDataService service) => _service = service;

  [HttpGet("{id:int}")]
  public IActionResult GetById(int id)
  {
    var product = _service.GetProduct(id);
    if (product == null)
      return NotFound();
    return Ok(new
    {
      product.Id,
      product.Name,
      product.QuantityPerUnit,
      product.UnitPrice,
      product.UnitsInStock,
      CategoryName = product.Category?.Name
    });
  }

  [HttpGet("category/{id:int}")]
  public IActionResult GetByCategory(int id)
  {
    var products = _service.GetProductByCategory(id);
    if (products.Count == 0) return NotFound();
    var dto = products.Select(p => new ProductWithCategoryDto
    {
      Name = p.Name,
      CategoryName = p.Category?.Name
    }).ToList();
    return Ok(dto);
  }
  [HttpGet]
  public IActionResult GetByName([FromQuery] string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      return BadRequest("Query parameter 'name' is required.");
    var products = _service.GetProductByName(name);
    if (products.Count == 0) return NotFound();
    var dto = products.Select(p => new ProductByNameDto
    {
      ProductName = p.Name,
      CategoryName = p.Category?.Name
    }).ToList();
    return Ok(dto);
  }
}