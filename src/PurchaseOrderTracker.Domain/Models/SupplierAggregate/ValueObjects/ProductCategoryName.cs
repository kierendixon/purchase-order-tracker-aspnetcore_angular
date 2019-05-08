﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PurchaseOrderTracker.Domain.Models.SupplierAggregate.ValueObjects
{
    public class ProductCategoryName : ValueObject
    {
        public ProductCategoryName(string value)
        {
            Value = value;
            ThrowExceptionIfValidationFails();
        }

        private ProductCategoryName()
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

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
