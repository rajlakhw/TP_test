using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using ViewModels.Common;

namespace ViewModels.Quotes
{
    public class QuotesViewModel
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public bool IsCurrentVersion { get; set; }
        public short QuoteCurrencyId { get; set; }
        public string LangIanacode { get; set; }
        public string Title { get; set; }
        public string QuoteFileName { get; set; }
        public string InternalNotes { get; set; }
        public DateTime QuoteDate { get; set; }
        public string QuoteOrgName { get; set; }
        public string QuoteAddress1 { get; set; }
        public string QuoteAddress2 { get; set; }
        public string QuoteAddress3 { get; set; }
        public string QuoteAddress4 { get; set; }
        public string QuoteCountyOrState { get; set; }
        public string QuotePostcodeOrZip { get; set; }
        public short QuoteCountryId { get; set; }
        public string AddresseeSalutationName { get; set; }
        public string OpeningSectionText { get; set; }
        public string ClosingSectionText { get; set; }
        public byte TimelineUnit { get; set; }
        public double TimelineValue { get; set; }
        public byte WordCountPresentationOption { get; set; }
        public bool ShowInterpretingDurationInBreakdown { get; set; }
        public bool ShowWorkDurationInBreakdown { get; set; }
        public bool ShowPagesOrSlidesInBreakdown { get; set; }
        public bool ShowNumberOfCharactersInBreakdown { get; set; }
        public bool ShowNumberOfDocumentsInBreakdown { get; set; }
        public short SalesContactEmployeeId { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public decimal? OverallChargeToClient { get; set; }
        public int? DiscountId { get; set; }
        public int? SurchargeId { get; set; }
        public decimal? SubTotalOverallChargeToClient { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? SurchargeAmount { get; set; }
        public string CustomerSpecificField1Value { get; set; }
        public string CustomerSpecificField2Value { get; set; }
        public string CustomerSpecificField3Value { get; set; }
        public string CustomerSpecificField4Value { get; set; }
        public bool? ShowCustomerSpecificField1Value { get; set; }
        public bool? ShowCustomerSpecificField2Value { get; set; }
        public bool? ShowCustomerSpecificField3Value { get; set; }
        public bool? ShowCustomerSpecificField4Value { get; set; }
        public string ClientPonumber { get; set; }
        public short? AssignedToEmployeeId { get; set; }
        public bool? CreatedAutomatically { get; set; }
        public bool? PrintingProject { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public List<Employee> ListOfEmployees { get; set; }
        public short? Ownership { get; set; }
        public List<LanguageService> LanguageServices { get; set; }
        public List<DropdownOptionViewModel> Languages { get; set; }
        public IEnumerable<CountryViewModel> Countries { get; set; }
        public string SalesContactName { get; set; }
        public string SalesContactEmail { get; set; }
        public string SalesContactNumber { get; set; }
        public List<QuoteItem> QuoteItems { get; set; }
        public IEnumerable<DropdownOptionViewModel> Currencies { get; set; }
        public DateTime? ArchivedToLionBoxDateTime { get; set; }
        public DateTime? ArchivedToAmazonS3dateTime { get; set; }
        public DateTime? ArchivedDateTime { get; set; }
        public string EnquiryFolder { get; set; }
        public string QuotePDFPath { get; set; }
        public ViewModels.QuoteItems.QuoteItemsViewModel QuoteItem { get; set; }
        public bool IsAllowedToEdit { get; set; }
        public bool EditPageEnabled { get; set; }
        public IEnumerable<DropdownOptionViewModel> LanguageServiceCategory { get; set; }
        public IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory> SurchargesCategories { get; set; }
        public IEnumerable<QuoteAndOrderDiscountsAndSurchargesCategory> DiscountCategories { get; set; }
        public SurchargeDetails SurchargeDetails { get; set; }
        public DiscountDetails DiscountDetails { get; set; }
        public int QuoteItemPopupID { get; set; }
    }

    public class QuoteItemsResults
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
    public class SurchargeDetails
    {
        public int SurchargeId { get; set; }
        public byte SurchargeCategory { get; set; }
        public bool PercentageOrValue { get; set; }
        public string Description { get; set; }
        public decimal SurchargeAmount { get; set; }
    }

    public class DiscountDetails
    {
        public int DiscountId { get; set; }
        public byte DiscountCategory { get; set; }
        public bool PercentageOrValue { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
    }

}
