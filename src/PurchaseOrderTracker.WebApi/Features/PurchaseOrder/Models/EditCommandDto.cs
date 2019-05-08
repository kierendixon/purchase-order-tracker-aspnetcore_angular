using System.ComponentModel.DataAnnotations;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;
using PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models
{
    public class EditCommandDto
    {
        [Required]
        public int? SupplierId { get; set; }

        [Required]
        public OrderNo OrderNo { get; set; }

        public int? ShipmentId { get; set; }
    }
}
