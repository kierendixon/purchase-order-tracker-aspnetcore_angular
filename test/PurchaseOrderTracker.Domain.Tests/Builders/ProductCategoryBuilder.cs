using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class ProductCategoryBuilder
    {
        private string _name = "furniture";
        private int? _supplierId;

        public ProductCategoryBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public ProductCategoryBuilder SupplierId(int id)
        {
            _supplierId = id;
            return this;
        }

        public ProductCategory Build()
        {
            var category = new ProductCategory(_name);
            if (_supplierId != null)
                category.SetPrivatePropertyValue(nameof(category.SupplierId), _supplierId);
            return category;
        }
    }
}