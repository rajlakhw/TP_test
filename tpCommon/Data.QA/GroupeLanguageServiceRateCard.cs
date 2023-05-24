using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class GroupeLanguageServiceRateCard
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public int Unit { get; set; }
        public string SourceIanacode { get; set; }
        public string TargetIanacode { get; set; }
        public int CurrencyId { get; set; }
        public decimal Cost { get; set; }
        public decimal WorstCasePercentage { get; set; }
        public decimal? WorstCaseCost { get; set; }
        public int RateCardId { get; set; }
        public DateTime InForcedSinceDate { get; set; }
        public DateTime? InForceEndDate { get; set; }
        public int CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public int? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
    }
}
