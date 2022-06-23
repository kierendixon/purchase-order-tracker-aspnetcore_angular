using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models;

public class EditLineItemCommandDto
{
    [Required]
    public int? ProductId { get; set; }

    [Required]
    public int? PurchaseQty { get; set; }

    [Required]
    public decimal? PurchasePrice { get; set; }
}
