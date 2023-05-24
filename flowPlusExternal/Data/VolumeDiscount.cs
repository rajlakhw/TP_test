using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class VolumeDiscount
    {
        public int Id { get; set; }
        public int DataObjectId { get; set; }
        public byte DataObjectTypeId { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int? IsCurrentDiscount { get; set; }
        public bool IsNextDiscount { get; set; }
        public decimal RevenueLowerLimit { get; set; }
        public decimal? RevenueUpperLimit { get; set; }
        public short RevenueCurrency { get; set; }
        public DateTime? InForceStartDate { get; set; }
        public DateTime? InForceEndDate { get; set; }
        public bool NotifiedOpsAboutThisDiscount { get; set; }
    }
}
