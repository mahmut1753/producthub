using ProductHub.Domain.Entity.Product;

namespace ProductHub.Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(int id);
    Task<int> InsertAsync(Product product);
    Task UpdateAsync(Product product);
    Task MatchExternalAsync(int productId, int externalProductId);
    Task UnMatchExternalAsync(int productId);
}
