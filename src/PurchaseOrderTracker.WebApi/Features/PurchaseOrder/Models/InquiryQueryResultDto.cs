using System;
using PurchaseOrderTracker.Application.PagedList;

namespace PurchaseOrderTracker.WebApi.Features.PurchaseOrder.Models
{
    public class InquiryQueryResultDto
    {
        public InquiryQueryResultDto(PagedListMinimal<PurchaseOrderDto> pagedList)
        {
            PagedList = pagedList;
        }

        public PagedListMinimal<PurchaseOrderDto> PagedList { get; }

        public class PurchaseOrderDto
        {
            public PurchaseOrderDto(int id, string orderNo, DateTime createdDate, string supplierName,
                string status)
            {
                Id = id;
                OrderNo = orderNo;
                CreatedDate = createdDate;
                SupplierName = supplierName;
                Status = status;
            }

            public int Id { get; }
            public string OrderNo { get; }
            public DateTime CreatedDate { get; }
            public string SupplierName { get; }
            public string Status { get; }
        }
    }
}
