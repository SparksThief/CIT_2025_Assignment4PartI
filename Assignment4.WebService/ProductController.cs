using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs;
using Assignment4;

namespace WebServiceLayer.Controllers;
[ApiController]
[Route("api/products")]
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
      Category = product.Category != null ? new
      {
        product.Category.Id,
        product.Category.Name
      } : null
    });
  }

  [HttpGet("category/{id:int}")]
  public IActionResult GetByCategory(int id)
  {
    var products = _service.GetProductByCategory(id);
    var dto = products.Select(p => new ProductWithCategoryDto
    {
      Name = p.Name,
      CategoryName = p.Category?.Name
    }).ToList();
    
    if (dto.Count == 0)
      return NotFound(dto);
    
    return Ok(dto);
  }
  [HttpGet("name/{name}")]
  public IActionResult GetByName(string name)
  {
    if (string.IsNullOrWhiteSpace(name))
      return BadRequest("Name parameter is required.");
    var products = _service.GetProductByName(name);
    var dto = products.Select(p => new ProductByNameDto
    {
      ProductName = p.Name,
      CategoryName = p.Category?.Name
    }).ToList();
    
    if (dto.Count == 0)
      return NotFound(dto);
    
    return Ok(dto);
  }
}