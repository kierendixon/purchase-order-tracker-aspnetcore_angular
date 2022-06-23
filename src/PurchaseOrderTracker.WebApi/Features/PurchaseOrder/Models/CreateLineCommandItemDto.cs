using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models;

public class CreateLineCommandItemDto
{
    [Required]
    public int? ProductId { get; set; }

    [Required]
    public int? PurchaseQty { get; set; }

    public decimal? PurchasePrice { get; set; }
}
