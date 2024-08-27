using ApiTest.Contracts;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(int id);
    Task<IEnumerable<Product>> GetProductsAsync();
    Task<int> AddProductAsync(Product product);
    Task UpdateProductAsync(int id, JsonPatchDocument<Product> product);
    Task<bool> ProductExistsAsync(int id);
}
