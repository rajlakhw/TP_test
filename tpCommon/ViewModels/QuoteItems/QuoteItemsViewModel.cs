using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using ViewModels.Common;

namespace ViewModels.QuoteItems
{
    public class QuoteItemsViewModel
    {
        public int Id { get; set; }
        public int QuoteId { get; set; }
        public byte LanguageServiceId { get; set; }
        public string SourceLanguageIanacode { get; set; }
        public string TargetLanguageIanacode { get; set; }
        public int? WordCountNew { get; set; }
        public int? WordCountFuzzyBand1 { get; set; }
        public int? WordCountFuzzyBand2 { get; set; }
        public int? WordCountFuzzyBand3 { get; set; }
        public int? WordCountFuzzyBand4 { get; set; }
        public int? WordCountExact { get; set; }
        public int? WordCountRepetitions { get; set; }
        public int? WordCountPerfectMatches { get; set; }
        public int? WordCountClientSpecific { get; set; }
        public int? Pages { get; set; }
        public int? Characters { get; set; }
        public int? Documents { get; set; }
        public int? InterpretingExpectedDurationMinutes { get; set; }
        public string InterpretingLocationOrgName { get; set; }
        public string InterpretingLocationAddress1 { get; set; }
        public string InterpretingLocationAddress2 { get; set; }
        public string InterpretingLocationAddress3 { get; set; }
        public string InterpretingLocationAddress4 { get; set; }
        public string InterpretingLocationCountyOrState { get; set; }
        public string InterpretingLocationPostcodeOrZip { get; set; }
        public short? InterpretingLocationCountryId { get; set; }
        public int? AudioMinutes { get; set; }
        public int? WorkMinutes { get; set; }
        public string ExternalNotes { get; set; }
        public decimal? ChargeToClient { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public int? SupplierWordCountNew { get; set; }
        public int? SupplierWordCountFuzzyBand1 { get; set; }
        public int? SupplierWordCountFuzzyBand2 { get; set; }
        public int? SupplierWordCountFuzzyBand3 { get; set; }
        public int? SupplierWordCountFuzzyBand4 { get; set; }
        public int? SupplierWordCountExact { get; set; }
        public int? SupplierWordCountRepetitions { get; set; }
        public int? SupplierWordCountPerfectMatches { get; set; }
        public int? SupplierWordCountClientSpecific { get; set; }
    }
}
