namespace WebServiceLayer.DTOs;

public class ProductByNameDto
{
  public required string ProductName { get; set; }
  public string? CategoryName { get; set; }
}

public class ProductWithCategoryDto
{
    public required string Name { get; set; }
    public string? CategoryName { get; set; }
}