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
        [StringLength(150, MinimumLength = 3)]
        public string OrderNo { get; set; }

        public int? ShipmentId { get; set; }
    }
}
