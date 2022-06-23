using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.Domain.Models.PurchaseOrderAggregate.ValueObjects;

public class OrderNo : ValueObject
{
    public OrderNo(string value)
    {
        Value = value;
        ThrowExceptionIfValidationFails();
    }

    private OrderNo()
    {
    }

    [StringLength(150, MinimumLength = 3)]
    [Required]
    // TODO: private set required by EF
    public string Value { get; private set; }

    public override string ToString()
    {
        return Value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
