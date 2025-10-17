using Microsoft.AspNetCore.Mvc;
using WebServiceLayer.DTOs;
using Assignment4;

namespace WebServiceLayer.Controllers;

/// <summary>
/// REST API controller for Product read operations.
/// Provides endpoints for retrieving products by ID, category, or name search.
/// Base route: /api/products
/// Note: This is read-only - no create/update/delete operations for products.
/// </summary>
[ApiController]  // Enables automatic model validation and API-specific behaviors
[Route("api/products")]  // Explicit route (not using [controller] token)
public class ProductController : ControllerBase
{
  private readonly IDataService _service;
  
  /// <summary>
  /// Constructor with dependency injection.
  /// ASP.NET Core automatically injects the IDataService implementation.
  /// </summary>
  public ProductController(IDataService service) => _service = service;

  /// <summary>
  /// GET /api/products/{id}
  /// Retrieves a specific product by ID with its category information.
  /// </summary>
  /// <param name="id">The product ID</param>
  /// <returns>
  /// 200 OK with product object including nested category, or
  /// 404 Not Found if product doesn't exist
  /// </returns>
  [HttpGet("{id:int}")]  // :int constraint ensures only integer values are routed here
  public IActionResult GetById(int id)
  {
    // GetProduct eagerly loads the Category navigation property
    var product = _service.GetProduct(id);
    if (product == null)
      return NotFound();
    
    // Return anonymous object with selected properties
    // Nested anonymous object for Category prevents circular reference issues in JSON serialization
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

  /// <summary>
  /// GET /api/products/category/{id}
  /// Retrieves all products in a specific category.
  /// Returns a simplified DTO with just product name and category name.
  /// </summary>
  /// <param name="id">The category ID to filter by</param>
  /// <returns>
  /// 200 OK with array of ProductWithCategoryDto objects, or
  /// 404 Not Found with empty array if no products found
  /// </returns>
  [HttpGet("category/{id:int}")]
  public IActionResult GetByCategory(int id)
  {
    var products = _service.GetProductByCategory(id);
    
    // Map to DTO objects for cleaner API response
    var dto = products.Select(p => new ProductWithCategoryDto
    {
      Name = p.Name,
      CategoryName = p.Category?.Name  // Null-conditional in case Category wasn't loaded
    }).ToList();
    
    // Return 404 if no products found in this category
    if (dto.Count == 0)
      return NotFound(dto);
    
    return Ok(dto);
  }
  
  /// <summary>
  /// GET /api/products/name/{name}
  /// Searches for products by name using case-insensitive substring matching.
  /// Example: /api/products/name/chai will find "Chai", "CHAI TEA", "Vegie-spread chai" etc.
  /// </summary>
  /// <param name="name">The substring to search for in product names</param>
  /// <returns>
  /// 200 OK with array of ProductByNameDto objects,
  /// 404 Not Found with empty array if no matching products,
  /// 400 Bad Request if name parameter is missing/empty
  /// </returns>
  [HttpGet("name/{name}")]
  public IActionResult GetByName(string name)
  {
    // Validate that search term is provided
    if (string.IsNullOrWhiteSpace(name))
      return BadRequest("Name parameter is required.");
    
    // GetProductByName uses ILIKE for case-insensitive pattern matching
    var products = _service.GetProductByName(name);
    
    // Map to DTO objects for cleaner API response
    var dto = products.Select(p => new ProductByNameDto
    {
      ProductName = p.Name,
      CategoryName = p.Category?.Name  // Null-conditional in case Category wasn't loaded
    }).ToList();
    
    // Return 404 if no products match the search term
    if (dto.Count == 0)
      return NotFound(dto);
    
    return Ok(dto);
  }
}