using System.Collections.Generic;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models
{
    public class EditLineItemsQueryResultDto
    {
        public EditLineItemsQueryResultDto(int purchaseOrderId, string purchaseOrderOrderNo,
            List<PurchaseOrderLineDto> lineItems, Dictionary<int, string> productOptions)
        {
            PurchaseOrderId = purchaseOrderId;
            PurchaseOrderOrderNo = purchaseOrderOrderNo;
            LineItems = lineItems;
            ProductOptions = productOptions;
        }

        public int PurchaseOrderId { get; }
        public string PurchaseOrderOrderNo { get; }
        public List<PurchaseOrderLineDto> LineItems { get; }
        public Dictionary<int, string> ProductOptions { get; set; }

        public class PurchaseOrderLineDto
        {
            public PurchaseOrderLineDto(int id, int productId, decimal purchasePrice, int purchaseQty)
            {
                Id = id;
                ProductId = productId;
                PurchasePrice = purchasePrice;
                PurchaseQty = purchaseQty;
            }

            public int Id { get; }
            public int ProductId { get; }
            public decimal PurchasePrice { get; }
            public int PurchaseQty { get; }
        }
    }
}
