using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class ProductBuilder
    {
        private ProductCategory _category = new ProductCategoryBuilder().Build();
        private string _name = "productName";
        private decimal _price = 20;
        private string _prodCode = "productCode";
        private int? _supplierId;

        public ProductBuilder ProdCode(string prodCode)
        {
            _prodCode = prodCode;
            return this;
        }

        public ProductBuilder Name(string name)
        {
            _name = name;
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
                product.SetPrivatePropertyValue(nameof(product.SupplierId), _supplierId);
            return product;
        }
    }
}