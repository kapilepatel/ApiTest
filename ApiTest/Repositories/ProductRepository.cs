using ApiTest.Contracts;
using ApiTest.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        var productEntity = await _context.Products.FindAsync(id);
        if (productEntity == null) return null;
        return new Product
        {
            Id = productEntity.Id,
            Name = productEntity.Name,
            Price = productEntity.Price,
            Created = productEntity.Created,
            LastUpdated = productEntity.LastUpdated
        };
    }

    public async Task<IEnumerable<Product>> GetProductsAsync()
    {
        return await _context.Products.Select(p => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Created = p.Created,
            LastUpdated = p.LastUpdated
        }).ToListAsync();
    }

    public async Task<int> AddProductAsync(Product product)
    {
        var productEntity = new ProductEntity
        {
            Name = product.Name,
            Price = product.Price,
            Created = product.Created,
            LastUpdated = product.LastUpdated
        };
        _context.Products.Add(productEntity);
        await _context.SaveChangesAsync();
        return productEntity.Id;
    }

    public async Task UpdateProductAsync(int id, JsonPatchDocument<Product> patchProduct)
    {
        var product = await GetProductByIdAsync(id);
        if (product == null)
        {
            return;
        }
        //keeping a copy, this will be used after patch operation
        var originalProduct = new Product(product);

        //apply patch
        patchProduct.ApplyTo(product);
        
        // Ensuring id value is taken from request param and not the payload 
        product.Id = id;
        
        //Updating "Created" is never allowed, hence updating as per existing value 
        product.Created = originalProduct.Created;

        //update value of LastUpdated only when a fruitful change was made
        bool changesMade = !originalProduct.Equals(product);
        if (changesMade)
        {
            product.LastUpdated = DateTime.UtcNow;
        }

        var productEntity = new ProductEntity{
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Created = product.Created,
            LastUpdated = product.LastUpdated
        };
        _context.Products.Update(productEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ProductExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
}
