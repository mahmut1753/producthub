using Dapper;
using ProductHub.Domain.Repositories;
using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Products;
using ProductHub.Domain.Entity.Product;
using ProductHub.Infrastructure.Persistence;
using System.Data;

namespace ProductHub.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly IDapperContext _context;

    public ProductRepository(IDapperContext context)
        => _context = context;

    public async Task<List<Product>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();

        var products = await connection.QueryAsync<Product>(
            "CATALOG.sp_Product_GetAll",
            commandType: CommandType.StoredProcedure);

        return products.ToList();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        using var connection = _context.CreateConnection();

        return await connection.QueryFirstOrDefaultAsync<Product>(
            "CATALOG.sp_Product_GetById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> InsertAsync(Product product)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(
            "CATALOG.sp_Product_Insert",
            new
            {
                product.Name,
                product.ExternalProductId,
                product.Description,
                product.Image,
                product.Category,
                product.Price,
                product.IsActive
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UpdateAsync(Product product)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "CATALOG.sp_Product_Update",
            new
            {
                product.Id,
                product.Description,
                product.Image,
                product.Category,
                product.Price
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task MatchExternalAsync(int productId, int externalProductId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "CATALOG.sp_Product_MatchExternal",
            new
            {
                Id = productId,
                ExternalProductId = externalProductId
            },
            commandType: CommandType.StoredProcedure);
    }

    public async Task UnMatchExternalAsync(int productId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(
            "CATALOG.sp_Product_UnMatchExternal",
            new
            {
                Id = productId
            },
            commandType: CommandType.StoredProcedure);
    }
}
