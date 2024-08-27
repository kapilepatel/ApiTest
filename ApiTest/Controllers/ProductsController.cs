using Microsoft.AspNetCore.Mvc;
using ApiTest.Contracts;
using Microsoft.AspNetCore.JsonPatch;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        if (await _repository.ProductExistsAsync(product.Id))
        {
            return Conflict(new { message = $"A product already exists with the id '{product.Id}'"});
        }

        product.Created = DateTime.UtcNow;
        product.LastUpdated = DateTime.UtcNow;

        int newId = await _repository.AddProductAsync(product);
        product.Id = newId;

        return CreatedAtAction(nameof(GetProduct), new { id = newId }, product);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _repository.GetProductsAsync();
        return Ok(products);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] JsonPatchDocument<Product> product)
    {
        if (!await _repository.ProductExistsAsync(id))
        {
            return NotFound();
        }

        await _repository.UpdateProductAsync(id, product);
        return NoContent();
    }
}
