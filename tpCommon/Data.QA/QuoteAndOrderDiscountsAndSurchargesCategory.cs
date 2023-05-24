using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class QuoteAndOrderDiscountsAndSurchargesCategory
    {
        public int Id { get; set; }
        public string DiscountOrSurchargeCategory { get; set; }
        public bool AppliesToDiscountOrSurcharge { get; set; }
    }
}
