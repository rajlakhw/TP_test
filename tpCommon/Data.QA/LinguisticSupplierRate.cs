using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LinguisticSupplierRate
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public byte LanguageServiceId { get; set; }
        public string SourceLangIanacode { get; set; }
        public string TargetLangIanacode { get; set; }
        public int SubjectAreaId { get; set; }
        public short RateUnitId { get; set; }
        public short CurrencyId { get; set; }
        public decimal StandardRate { get; set; }
        public decimal? StandardRateSterlingEquivalent { get; set; }
        public decimal MinimumCharge { get; set; }
        public byte? AppliesToDataObjectTypeId { get; set; }
        public int? AppliesToDataObjectId { get; set; }
        public string RatesNotes { get; set; }
        public decimal? MemoryRateForPerfectMatches { get; set; }
        public decimal? MemoryRateForExactMatches { get; set; }
        public decimal? MemoryRateForRepetitions { get; set; }
        public decimal? MemoryRateFor95To99Percent { get; set; }
        public decimal? MemoryRateFor85To94Percent { get; set; }
        public decimal? MemoryRateFor75To84Percent { get; set; }
        public decimal? MemoryRateFor50To74Percent { get; set; }
        public decimal? MemoryRateForClientSpecificPercent { get; set; }
        public DateTime CreatedDate { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDate { get; set; }
        public short? DeletedByEmployeeId { get; set; }
    }
}
