using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ExchangeRate
    {
        public int Id { get; set; }
        public short SourceCurrencyId { get; set; }
        public short TargetCurrencyId { get; set; }
        public decimal Rate { get; set; }
        public bool IsRateCurrentlyInForce { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
    }
}
