using Assignment4;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
  private readonly DataService _service = new DataService();

  [HttpGet("{id}")]
  public IActionResult GetProduct(int id)
  {
    var product = _service.GetProductById(id);
    if (product == null) return NotFound();
    return Ok(product);
  }

  [HttpGet("category/{id}")]
  public IActionResult GetProductsByCategory(int id)
  {
    var products = _service.GetProductsByCategory(id);
    if (!products.Any()) return NotFound();
    return Ok(products);
  }

  [HttpPost]
  public IActionResult GetProductByName([FromQuery] string name)
    {
    var products = _service.GetProductByName(name);
    if (!products.Any()) return NotFound();
    return Ok(products);
    }
}