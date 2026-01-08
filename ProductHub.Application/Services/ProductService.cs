using ProductHub.Application.Abstractions.Integrations;
using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.Common;
using ProductHub.Application.DTOs.ExternalProduct;
using ProductHub.Application.DTOs.Products;
using ProductHub.Application.Mappings;
using ProductHub.Domain.Entity.Product;
using ProductHub.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IExternalProductProvider _externalProductProvider;

    public ProductService(IProductRepository productRepository, IExternalProductProvider externalProductProvider)
    {
        _productRepository = productRepository;
        _externalProductProvider = externalProductProvider;
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return Result<IEnumerable<ProductDto>>.Success(products.Select(p => p.ToDto()));
    }

    public async Task<Result<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return Result<ProductDto>.Failure("Product not found.");

        return Result<ProductDto>.Success(product.ToDto());
    }

    public async Task<Result<int>> CreateAsync(CreateProductRequest request)
    {
        var product = new Product(
            request.Name,
            request.Description,
            request.Image,
            request.Category,
            request.Price);

        if (request.ExternalProductId.HasValue)
            product.MatchWithExternalProduct(request.ExternalProductId.Value);

        var id = await _productRepository.InsertAsync(product);

        return Result<int>.Success(id);
    }

    public async Task<Result> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return Result.Failure("Product not found.");

        product.ChangeName(request.Name);
        product.ChangeDescription(request.Description);
        product.ChangeCategory(request.Category);
        product.ChangeImage(request.Image);
        product.UpdatePrice(request.Price);

        await _productRepository.UpdateAsync(product);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product is null)
            return Result.Failure("Product not found.");

        product.DeActivate();

        await _productRepository.UpdateAsync(product);

        return Result.Success();
    }

    public async Task<Result> MatchExternalAsync(int productId, int externalProductId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            return Result.Failure("Product not found.");

        product.MatchWithExternalProduct(externalProductId);

        await _productRepository.MatchExternalAsync(productId, externalProductId);

        return Result.Success();
    }

    public async Task<Result> UnmatchExternalAsync(int productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);

        if (product is null)
            return Result.Failure("Product not found.");

        product.UnmatchExternalProduct();

        await _productRepository.UnMatchExternalAsync(productId);

        return Result.Success();
    }

    public async Task<Result<ProductComparisonResultDto>> CompareWithExternalAsync()
    {
        var localProducts = (await _productRepository.GetAllAsync()).ToList();
        var externalProducts = (await _externalProductProvider.GetProductsAsync()).ToList();

        var localDict = localProducts
            .Where(x => x.ExternalProductId.HasValue)
            .ToDictionary(x => x.ExternalProductId!.Value);

        var externalDict = externalProducts
            .ToDictionary(x => x.Id);

        var matched = new List<ProductMatchDto>();
        var onlyInExternal = new List<ExternalProductDto>();
        var onlyInLocal = new List<ProductDto>();

        foreach (var external in externalProducts)
        {
            if (localDict.TryGetValue(external.Id, out var local))
            {
                matched.Add(new ProductMatchDto
                {
                    LocalProductId = local.Id,
                    ExternalProductId = external.Id,
                    LocalName = local.Name,
                    ExternalName = external.Title,
                    LocalPrice = local.Price,
                    ExternalPrice = external.Price
                });
            }
            else
            {
                onlyInExternal.Add(external);
            }
        }

        foreach (var local in localProducts)
        {
            if (!local.ExternalProductId.HasValue)
            {
                onlyInLocal.Add(local.ToDto());
                continue;
            }

            if (!externalDict.ContainsKey(local.ExternalProductId.Value))
            {
                onlyInLocal.Add(local.ToDto());
            }
        }

        var result = new ProductComparisonResultDto
        {
            TotalLocalCount = localProducts.Count,
            TotalExternalCount = externalProducts.Count,
            MatchedProducts = matched,
            OnlyInExternal = onlyInExternal,
            OnlyInLocal = onlyInLocal
        };

        return Result<ProductComparisonResultDto>.Success(result);
    }
}
