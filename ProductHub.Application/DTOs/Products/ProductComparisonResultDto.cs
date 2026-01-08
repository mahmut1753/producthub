using ProductHub.Application.DTOs.ExternalProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.DTOs.Products;

public class ProductComparisonResultDto
{
    public int TotalLocalCount { get; set; }
    public int TotalExternalCount { get; set; }
    public List<ProductMatchDto> MatchedProducts { get; set; } = new();
    public List<ExternalProductDto> OnlyInExternal { get; set; } = new();
    public List<ProductDto> OnlyInLocal { get; set; } = new();
}
