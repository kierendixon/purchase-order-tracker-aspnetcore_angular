using System.ComponentModel.DataAnnotations;
using PurchaseOrderTracker.Domain.Models.ShipmentAggregate.ValueObjects;

namespace PurchaseOrderTracker.WebApi.Features.Shipment.Models;

public class EditStatusCommandDto
{
    [Required]
    public ShipmentStatus.Trigger? UpdatedStatus { get; set; }
}
