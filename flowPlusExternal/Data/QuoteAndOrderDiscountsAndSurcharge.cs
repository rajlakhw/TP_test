using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class QuoteAndOrderDiscountsAndSurcharge
    {
        public int Id { get; set; }
        public bool DiscountOrSurcharge { get; set; }
        public byte DiscountOrSurchargeCategory { get; set; }
        public string Description { get; set; }
        public bool PercentageOrValue { get; set; }
        public decimal DiscountOrSurchargeAmount { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
