using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.Supplier.Models
{
    public class CreateProductCommandDto
    {
        [Required]
        public string ProdCode { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        public int? CategoryId { get; set; }

        public double? Price { get; set; }

        // TODO
        // [BindNever]
        public Dictionary<int, string> CategoryOptions { get; }
    }
}
