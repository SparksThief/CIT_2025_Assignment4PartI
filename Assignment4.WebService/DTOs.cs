namespace WebServiceLayer.DTOs;

/// <summary>
/// Data Transfer Object for product search by name results.
/// Provides a simplified view with just product and category names.
/// Used by GET /api/products/name/{name} endpoint.
/// </summary>
public class ProductByNameDto
{
  /// <summary>
  /// Name of the product.
  /// Required ensures this property must be set when creating instances.
  /// </summary>
  public required string ProductName { get; set; }
  
  /// <summary>
  /// Name of the category this product belongs to.
  /// Nullable because category might not be loaded or might not exist.
  /// </summary>
  public string? CategoryName { get; set; }
}

/// <summary>
/// Data Transfer Object for products with category information.
/// Used by GET /api/products/category/{id} endpoint.
/// Similar to ProductByNameDto but uses "Name" instead of "ProductName".
/// </summary>
public class ProductWithCategoryDto
{
    /// <summary>
    /// Name of the product.
    /// Required ensures this property must be set when creating instances.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    /// Name of the category this product belongs to.
    /// Nullable because category might not be loaded or might not exist.
    /// </summary>
    public string? CategoryName { get; set; }
}