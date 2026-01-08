using ProductHub.Domain.Entity.Common;
using ProductHub.Domain.Entity.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductHub.Domain.Entity.Product;

public class Product : ActivatableEntity
{
    protected Product() { }

    public Product(string name, string description, string imagePath, string category, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name cannot be empty.");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("Product category cannot be empty.");

        if (string.IsNullOrWhiteSpace(imagePath))
            throw new DomainException("Product image cannot be empty.");

        if (price <= 0)
            throw new DomainException("Price must be greater than zero.");

        Name = name;
        Description = description;
        Category = category;
        Image = imagePath;
        Price = price;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public int? ExternalProductId { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string Image { get; private set; }
    public string Category { get; private set; }
    public decimal Price { get; private set; }

    public void MatchWithExternalProduct(int externalProductId)
    {
        if (externalProductId <= 0)
            throw new DomainException("Invalid external product id.");

        if (ExternalProductId.HasValue)
            throw new DomainException("Product is already matched with an external product.");

        ExternalProductId = externalProductId;
    }

    public void UnmatchExternalProduct()
    {
        if (!ExternalProductId.HasValue)
            throw new DomainException("Product is not matched with any external product.");

        ExternalProductId = null;
    }

    public void ChangeName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Product name cannot be empty.");

        Name = newName;
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainException("Price must be greater than zero.");

        Price = newPrice;
    }

    public void ChangeCategory(string newCategory)
    {
        if (string.IsNullOrWhiteSpace(newCategory))
            throw new DomainException("Product category cannot be empty.");

        Category = newCategory;
    }

    public void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Product description cannot be empty.");

        Description = description;
    }

    public void ChangeImage(string imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
            throw new DomainException("Product image cannot be empty.");

        Image = imagePath;
    }
}
