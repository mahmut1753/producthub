using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductHub.API.Common;
using ProductHub.Application.Abstractions.Services;
using ProductHub.Application.DTOs.Products;
using ProductHub.Application.DTOs.Users;
using ProductHub.Application.Services;

namespace ProductHub.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<BaseResponse<IEnumerable<ProductDto>>> GetAll()
    {
        var result = await _productService.GetAllAsync();

        return result.IsSuccess
            ? BaseResponse<IEnumerable<ProductDto>>.Ok(result.Value!)
            : BaseResponse<IEnumerable<ProductDto>>.Fail(result.Error!);
    }

    [HttpGet("{id:int}")]
    public async Task<BaseResponse<ProductDto>> GetById(int id)
    {
        var result = await _productService.GetByIdAsync(id);

        return result.IsSuccess
            ? BaseResponse<ProductDto>.Ok(result.Value!)
            : BaseResponse<ProductDto>.Fail(result.Error!);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse<int>> Create(CreateProductRequest request)
    {
        var result = await _productService.CreateAsync(request);

        return result.IsSuccess
            ? BaseResponse<int>.Ok(result.Value!)
            : BaseResponse<int>.Fail(result.Error!);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse<bool>> Update(int id, [FromBody] UpdateProductRequest request)
    {
        var result = await _productService.UpdateAsync(id, request);

        return result.IsSuccess
            ? BaseResponse<bool>.Ok(true)
            : BaseResponse<bool>.Fail(result.Error!);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<BaseResponse<bool>> Delete(int id)
    {
        var result = await _productService.DeleteAsync(id);

        return result.IsSuccess
            ? BaseResponse<bool>.Ok(true)
            : BaseResponse<bool>.Fail(result.Error!);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("{id:int}/match-external")]
    public async Task<BaseResponse<bool>> MatchExternal(int id, [FromBody] MatchExternalProductRequest request)
    {
        var result = await _productService.MatchExternalAsync(id, request.ExternalProductId);

        return result.IsSuccess
            ? BaseResponse<bool>.Ok(true)
            : BaseResponse<bool>.Fail(result.Error!);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}/match-external")]
    public async Task<BaseResponse<bool>> UnmatchExternal(int id)
    {
        var result = await _productService.UnmatchExternalAsync(id);

        return result.IsSuccess
            ? BaseResponse<bool>.Ok(true)
            : BaseResponse<bool>.Fail(result.Error!);
    }

    [HttpGet("compare-external")]
    public async Task<BaseResponse<ProductComparisonResultDto>> CompareWithExternal()
    {
        var result = await _productService.CompareWithExternalAsync();

        return result.IsSuccess
            ? BaseResponse<ProductComparisonResultDto>.Ok(result.Value!)
            : BaseResponse<ProductComparisonResultDto>.Fail(result.Error!);
    }
}
