using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class ClientPriceList
    {
        public int Id { get; set; }
        public byte DataObjectTypeId { get; set; }
        public int DataObjectId { get; set; }
        public string Name { get; set; }
        public bool ShowInExtranet { get; set; }
        public bool ShowMinimumChargesInExtranet { get; set; }
        public string ExternalNotes { get; set; }
        public string InternalNotes { get; set; }
        public DateTime InForceSinceDateTime { get; set; }
        public DateTime? NoLongerInForceAsOfDateTime { get; set; }
        public short CurrencyId { get; set; }
        public decimal MemoryRateForExactMatches { get; set; }
        public decimal MemoryRateForRepetitions { get; set; }
        public decimal MemoryRateForFuzzyMatches { get; set; }
        public decimal MemoryRateForFuzzyBand2Matches { get; set; }
        public decimal MemoryRateForFuzzyBand3Matches { get; set; }
        public decimal MemoryRateForFuzzyBand4Matches { get; set; }
        public decimal MemoryRateForPerfectMatches { get; set; }
        public decimal? MemoryRateForClientSpecificMatches { get; set; }
        public decimal DefaultTranslationRate { get; set; }
        public bool DefaultTranslationRateIsContractual { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public bool? ApprovedByClient { get; set; }
        public string ApprovedByClientType { get; set; }
        public bool? CheckedForAutomation { get; set; }
    }
}
