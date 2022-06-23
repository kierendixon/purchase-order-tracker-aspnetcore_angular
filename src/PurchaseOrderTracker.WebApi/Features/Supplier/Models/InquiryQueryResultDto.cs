using PurchaseOrderTracker.Application.PagedList;

namespace PurchaseOrderTracker.WebApi.Features.Supplier.Models;

public class InquiryQueryResultDto
{
    public InquiryQueryResultDto(PagedListMinimal<SupplierDto> pagedList)
    {
        PagedList = pagedList;
    }

    public PagedListMinimal<SupplierDto> PagedList { get; }

    public class SupplierDto
    {
        public SupplierDto(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
