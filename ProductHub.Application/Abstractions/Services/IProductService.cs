using ProductHub.Application.Common;
using ProductHub.Application.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Abstractions.Services;

public interface IProductService
{
    Task<Result<IEnumerable<ProductDto>>> GetAllAsync();
    Task<Result<ProductDto>> GetByIdAsync(int id);
    Task<Result<int>> CreateAsync(CreateProductRequest request);
    Task<Result> UpdateAsync(int id, UpdateProductRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> MatchExternalAsync(int productId, int externalProductId);
    Task<Result> UnmatchExternalAsync(int productId);
    Task<Result<ProductComparisonResultDto>> CompareWithExternalAsync();
}
