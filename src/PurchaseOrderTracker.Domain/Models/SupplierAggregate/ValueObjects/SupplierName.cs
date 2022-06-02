using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects
{
    public class SupplierName : ValueObject
    {
        public SupplierName(string value)
        {
            Value = value;
            ThrowExceptionIfValidationFails();
        }

        private SupplierName()
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
}
