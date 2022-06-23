using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.Supplier.Models;

public class EditCommandDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }
}
