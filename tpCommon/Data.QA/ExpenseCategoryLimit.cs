using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ExpenseCategoryLimit
    {
        public int Id { get; set; }
        public int ExpenseCategoryId { get; set; }
        public decimal LimitValue { get; set; }
        public int CountryId { get; set; }
        public short CurrencyId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
