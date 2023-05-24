using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class GroupeMasterRate
    {
        public int Id { get; set; }
        public decimal? FixedPcrate { get; set; }
        public decimal? Ohrate { get; set; }
        public decimal? PgdcentralCostsRate { get; set; }
        public decimal? MarkUp { get; set; }
        public decimal? PmhourlyRate { get; set; }
        public decimal? SupportContribution { get; set; }
        public decimal? Fpc { get; set; }
        public int? CurrencyId { get; set; }
        public DateTime InForcedStartDate { get; set; }
        public DateTime? InForcedEndDate { get; set; }
    }
}
