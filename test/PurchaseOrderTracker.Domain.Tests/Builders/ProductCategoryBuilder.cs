using PurchaseOrderTracker.Domain.Models.SupplierAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class ProductCategoryBuilder
    {
        private ProductCategoryName _name = new ProductCategoryName("furniture");
        private int? _supplierId;

        public ProductCategoryBuilder Name(string name)
        {
            _name = new ProductCategoryName(name);
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
