using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class JobItem
    {
        public int Id { get; set; }
        public int JobOrderId { get; set; }
        public bool IsVisibleToClient { get; set; }
        public byte LanguageServiceId { get; set; }
        public string SourceLanguageIanacode { get; set; }
        public string TargetLanguageIanacode { get; set; }
        public string CustomerSpecificField { get; set; }
        public int? WordCountNew { get; set; }
        public int? WordCountFuzzyBand1 { get; set; }
        public int? WordCountFuzzyBand2 { get; set; }
        public int? WordCountFuzzyBand3 { get; set; }
        public int? WordCountFuzzyBand4 { get; set; }
        public int? WordCountExact { get; set; }
        public int? WordCountRepetitions { get; set; }
        public int? WordCountPerfectMatches { get; set; }
        public int? WordCountClientSpecific { get; set; }
        public byte TranslationMemoryRequired { get; set; }
        public int? Pages { get; set; }
        public int? Characters { get; set; }
        public int? Documents { get; set; }
        public int? InterpretingExpectedDurationMinutes { get; set; }
        public int? InterpretingActualDurationMinutes { get; set; }
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
        public DateTime? SupplierSentWorkDateTime { get; set; }
        public DateTime? SupplierAcceptedWorkDateTime { get; set; }
        public DateTime? SupplierCompletionDeadline { get; set; }
        public DateTime? SupplierCompletedItemDateTime { get; set; }
        public DateTime? OurCompletionDeadline { get; set; }
        public DateTime? WeCompletedItemDateTime { get; set; }
        public string DescriptionForSupplierOnly { get; set; }
        public string FileName { get; set; }
        public bool? SupplierIsClientReviewer { get; set; }
        public int? LinguisticSupplierOrClientReviewerId { get; set; }
        public string ExtranetSignoffComment { get; set; }
        public decimal? ChargeToClient { get; set; }
        public decimal? PaymentToSupplier { get; set; }
        public short? PaymentToSupplierCurrencyId { get; set; }
        public DateTime? SupplierInvoicePaidDate { get; set; }
        public byte? SupplierInvoicePaidMethodId { get; set; }
        public short? ExtranetClientStatusId { get; set; }
        public short? WebServiceClientStatusId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedFreeTextReason { get; set; }
        public decimal? ChargeToClientAfterDiscountSurcharges { get; set; }
        public int? TotalNumberOfReviewSegments { get; set; }
        public int? TotalNumberOfChangedReviewSegments { get; set; }
        public decimal? PercentageOfChangedReviewSegments { get; set; }
        public int? DownloadedByContactId { get; set; }
        public decimal? AnticipatedFinalValueAmount { get; set; }
        public int? SupplierWordCountNew { get; set; }
        public int? SupplierWordCountFuzzyBand1 { get; set; }
        public int? SupplierWordCountFuzzyBand2 { get; set; }
        public int? SupplierWordCountFuzzyBand3 { get; set; }
        public int? SupplierWordCountFuzzyBand4 { get; set; }
        public int? SupplierWordCountExact { get; set; }
        public int? SupplierWordCountRepetitions { get; set; }
        public int? SupplierWordCountPerfectMatches { get; set; }
        public bool? SupplierWordCountsTakenFromClient { get; set; }
        public string ContextFieldsList { get; set; }
        public int? SupplierWordCountClientSpecific { get; set; }
        public byte? SupplierResponsivenessRating { get; set; }
        public byte? SupplierFollowingTheInstructionsRating { get; set; }
        public byte? SupplierAttitudeRating { get; set; }
        public byte? SupplierQualityOfWorkRating { get; set; }
        public string QualityRatingNotGivenReason { get; set; }
        public DateTime? SupplierRatingsGivenDateTime { get; set; }
        public int? SupplierRatingsGivenBy { get; set; }
        public bool? QualityRatingReminderDisabled { get; set; }
        public bool? NotifiedLinguistForApproachingDeadline { get; set; }
        public decimal? AnticipatedGrossMarginPercentage { get; set; }
        public decimal? MarginAfterDiscountSurcharges { get; set; }
        public int? ClientCostCalculatedById { get; set; }
        public DateTime? ClientCostCalculatedByDateTime { get; set; }
        public bool? ClientCostCalculatedByPriceList { get; set; }
        public int? SupplierCostCalculatedById { get; set; }
        public DateTime? SupplierCostCalculatedByDateTime { get; set; }
        public bool? SupplierCostCalculatedByPriceList { get; set; }
        public bool? MinimumSupplierChargeApplied { get; set; }
        public decimal? ChargeToClientAfterDiscountSurcharges1 { get; set; }
        public byte? LanguageServiceCategoryId { get; set; }
    }
}
