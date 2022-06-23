using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.Supplier.Models;

// TODO: convert DTO's to C# 9 RecordTypes ?
// https://docs.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-5.0#constructor-binding-and-record-types
public class CreateCommandDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Name { get; set; }
}
