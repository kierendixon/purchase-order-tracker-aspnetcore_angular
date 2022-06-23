using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.Supplier.Models;

public class EditProductCommandDto
{
    [Required]
    [StringLength(20, MinimumLength = 3)]
    public string ProdCode { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }

    public int? CategoryId { get; set; }

    [Required]
    public decimal? Price { get; set; }
}
