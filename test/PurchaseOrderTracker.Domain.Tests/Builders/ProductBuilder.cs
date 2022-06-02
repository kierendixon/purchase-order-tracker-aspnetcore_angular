using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class ProductBuilder
    {
        private ProductCategory _category = new ProductCategoryBuilder().Build();
        private ProductName _name = new("productName");
        private decimal _price = 20;
        private ProductCode _prodCode = new("productCode");
        private int? _supplierId;

        public ProductBuilder ProdCode(string prodCode)
        {
            _prodCode = new ProductCode(prodCode);
            return this;
        }

        public ProductBuilder Name(string name)
        {
            _name = new ProductName(name);
            return this;
        }

        public ProductBuilder Category(ProductCategory category)
        {
            _category = category;
            return this;
        }

        public ProductBuilder Price(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductBuilder SupplierId(int supplierId)
        {
            _supplierId = supplierId;
            return this;
        }

        public Product Build()
        {
            var product = new Product(_prodCode, _name, _category, _price);
            if (_supplierId != null)
            {
                product.SetPrivatePropertyValue(nameof(product.SupplierId), _supplierId);
            }

            return product;
        }
    }
}
