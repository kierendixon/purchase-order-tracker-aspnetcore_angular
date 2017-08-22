using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate;

namespace PurchaseOrderTracker.Domain.Tests.Builders
{
    public class PurchaseOrderLineBuilder
    {
        private Product _product = new ProductBuilder().Build();
        private decimal _purchasePrice = 100;
        private int _purchaseQty = 10;

        public PurchaseOrderLineBuilder Product(Product product)
        {
            _product = product;
            return this;
        }

        public PurchaseOrderLineBuilder PurchasePrice(decimal purchasePrice)
        {
            _purchasePrice = purchasePrice;
            return this;
        }

        public PurchaseOrderLineBuilder PurchaseQty(int purchaseQty)
        {
            _purchaseQty = purchaseQty;
            return this;
        }

        public PurchaseOrderLine Build()
        {
            return new PurchaseOrderLine(_product, _purchasePrice, _purchaseQty);
        }
    }
}