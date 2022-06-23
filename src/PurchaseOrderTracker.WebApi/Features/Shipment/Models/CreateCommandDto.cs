using System;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.WebApi.Features.Shipment.Models;

public class CreateCommandDto
{
    [Required]
    public string TrackingId { get; set; }

    [Required]
    public string Company { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? EstimatedArrivalDate { get; set; }

    public string Comments { get; set; }

    public decimal? ShippingCost { get; set; }

    public string DestinationAddress { get; set; }
}
