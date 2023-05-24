using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class Quote
    {
        public int Id { get; set; }
        public int EnquiryId { get; set; }
        public bool IsCurrentVersion { get; set; }
        public short QuoteCurrencyId { get; set; }
        public string LangIanacode { get; set; }
        public string Title { get; set; }
        public string QuoteFileName { get; set; }
        public string InternalNotes { get; set; }
        public DateTime? QuoteDate { get; set; }
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
    }
}
