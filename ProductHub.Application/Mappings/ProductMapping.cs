using ProductHub.Application.DTOs.Products;
using ProductHub.Domain.Entity.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Application.Mappings;

public static class ProductMapping
{
    public static ProductDto ToDto(this Product product)
    {
        if (product is null)
            return null;

        return new ProductDto
        {
            Id = product.Id,
            ExternalProductId = product.ExternalProductId,
            Name = product.Name,
            Category = product.Category,
            Description = product.Description,
            Image = product.Image,
            Price = product.Price,
            IsActive = product.IsActive
        };
    }
}
