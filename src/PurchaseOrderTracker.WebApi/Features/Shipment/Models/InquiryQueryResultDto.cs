using System;
using PurchaseOrderTracker.Application.PagedList;

namespace PurchaseOrderTracker.WebApi.Features.Shipment.Models;

public class InquiryQueryResultDto
{
    public InquiryQueryResultDto(PagedListMinimal<ShipmentDto> pagedList)
    {
        PagedList = pagedList;
    }

    public PagedListMinimal<ShipmentDto> PagedList { get; }

    public class ShipmentDto
    {
        public ShipmentDto(int id, string trackingId, string company, DateTime estimatedArrivalDate,
            string comments, decimal shippingCost, string status, string destinationAddress, bool isDelayed,
            bool isDelayedMoreThan7Days, bool isScheduledForDeliveryToday)
        {
            Id = id;
            TrackingId = trackingId;
            Company = company;
            EstimatedArrivalDate = estimatedArrivalDate;
            Comments = comments;
            ShippingCost = shippingCost;
            Status = status;
            DestinationAddress = destinationAddress;
            IsDelayed = isDelayed;
            IsDelayedMoreThan7Days = isDelayedMoreThan7Days;
            IsScheduledForDeliveryToday = isScheduledForDeliveryToday;
        }

        public int Id { get; }
        public string TrackingId { get; }
        public string Company { get; }
        public DateTime EstimatedArrivalDate { get; }
        public string Comments { get; }
        public decimal ShippingCost { get; }
        public string Status { get; }
        public string DestinationAddress { get; }

        public bool IsDelayed { get; }
        public bool IsDelayedMoreThan7Days { get; }
        public bool IsScheduledForDeliveryToday { get; }
    }
}
