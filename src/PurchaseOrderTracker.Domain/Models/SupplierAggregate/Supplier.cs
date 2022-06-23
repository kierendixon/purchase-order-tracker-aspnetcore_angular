using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate;

/// <summary>
/// Aggregate root
/// </summary>
public class Supplier : Entity
{
    private readonly ICollection<ProductCategory> _productCategories = new List<ProductCategory>();
    private readonly ICollection<Product> _products = new List<Product>();

    // Required for EntityFramework
    private Supplier()
    {
    }

    public Supplier(SupplierName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public SupplierName Name { get; private set; }

    public IEnumerable<ProductCategory> ProductCategories => _productCategories;
    public IEnumerable<Product> Products => _products;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2302:\"nameof\" should be used", Justification = "'product' text is part of the exception message string")]
    public void AddProduct(Product product)
    {
        if (!OwnsCategory(product.Category))
        {
            throw new PurchaseOrderTrackerException($"Category of product does not belong to this supplier: {product}");
        }

        _products.Add(product);
    }

    public void AddProducts(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            AddProduct(product);
        }
    }

    public void RemoveProduct(Product product)
    {
        if (!OwnsProduct(product))
        {
            throw new PurchaseOrderTrackerException($"Product does not belong to this supplier: {product}");
        }

        if (!Products.Any(p => p.Id == product.Id))
        {
            throw new PurchaseOrderTrackerException($"Product already removed from supplier: {product}");
        }

        _products.Remove(product);
    }

    private bool OwnsCategory(ProductCategory category)
    {
        return category.SupplierId == Id;
    }

    private bool OwnsProduct(Product product)
    {
        return product.SupplierId == Id;
    }

    public void AddCategory(ProductCategory category)
    {
        _productCategories.Add(category);
    }

    public void AddCategorys(IEnumerable<ProductCategory> categories)
    {
        foreach (var category in categories)
        {
            _productCategories.Add(category);
        }
    }

    public void RemoveCategory(ProductCategory category)
    {
        _productCategories.Remove(category);
    }
}
