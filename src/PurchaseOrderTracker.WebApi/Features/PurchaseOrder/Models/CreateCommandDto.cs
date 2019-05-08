using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models
{
    public class CreateCommandDto
    {
        [Required]
        public int? SupplierId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string OrderNo { get; set; }
    }
}
