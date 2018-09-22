using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PurchaseOrderTracker.DAL;
using PurchaseOrderTracker.Domain.Exceptions;
using PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate;
using X.PagedList;

namespace PurchaseOrderTracker.Web.Features.Api.PurchaseOrder
{
    public class EditLineItems
    {
        public class Query : IRequest<Result>
        {
            [Required]
            [FromRoute(Name = "Id")]
            public int PurchaseOrderId { get; set; }
        }

        public class Result
        {
            public Result(int purchaseOrderId, string purchaseOrderOrderNo, 
                List<PurchaseOrderLineViewModel> lineItems, Dictionary<int, string> productOptions)
            {
                PurchaseOrderId = purchaseOrderId;
                PurchaseOrderOrderNo = purchaseOrderOrderNo;
                LineItems = lineItems;
                ProductOptions = productOptions;
            }

            public int PurchaseOrderId { get; }
            public string PurchaseOrderOrderNo { get; }
            public List<PurchaseOrderLineViewModel> LineItems { get; }
            public Dictionary<int, string> ProductOptions { get; set; }

            public class PurchaseOrderLineViewModel
            {
                //TODO: We shouldn't need this
                public PurchaseOrderLineViewModel()
                {
                }

                public PurchaseOrderLineViewModel(int id, int productId, decimal purchasePrice, int purchaseQty)
                {
                    Id = id;
                    ProductId = productId;
                    PurchasePrice = purchasePrice;
                    PurchaseQty = purchaseQty;
                }

                public int Id { get; }
                public int ProductId { get; }
                public decimal PurchasePrice { get; }
                public int PurchaseQty { get; }
            }
        }

        public class Handler : IRequestHandler<Query, Result>
        {
            private readonly PoTrackerDbContext _context;
            private readonly IMapper _mapper;

            public Handler(PoTrackerDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }
            
            public async Task<Result> Handle(Query query, CancellationToken cancellationToken)
            {
                var purchaseOrder = await _context.PurchaseOrder
                    .Include(o => o.Supplier)
                    .Include(o => o.LineItems)
                    .ThenInclude(li => li.Product)
                    .SingleAsync(o => o.Id == query.PurchaseOrderId);
                if (purchaseOrder == null)
                    throw new PurchaseOrderTrackerException($"Cannot find Purchase Order with id '${query.PurchaseOrderId}'");
                var productOptions = await _context.Product.Where(p => p.SupplierId == purchaseOrder.Supplier.Id)
                    .ToListAsync();

                return new Result(query.PurchaseOrderId, purchaseOrder.OrderNo,
                    _mapper.Map<IEnumerable<PurchaseOrderLine>, List<Result.PurchaseOrderLineViewModel>>(purchaseOrder.LineItems),
                    productOptions.ToDictionary(p => p.Id, p => p.Name));

                
            }
        }
    }
}