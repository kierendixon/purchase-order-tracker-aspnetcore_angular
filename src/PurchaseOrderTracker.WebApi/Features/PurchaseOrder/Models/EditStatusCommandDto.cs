using System.ComponentModel.DataAnnotations;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models
{
    public class EditStatusCommandDto
    {
        [Required]
        public PurchaseOrderStatus.Trigger? UpdatedStatus { get; set; }
    }
}
