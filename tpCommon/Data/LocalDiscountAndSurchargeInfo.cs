using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LocalDiscountAndSurchargeInfo
    {
        public int DiscountOrSurchargeId { get; set; }
        public string LanguageIanacode { get; set; }
        public string DiscountOrSurchargeName { get; set; }
    }
}
