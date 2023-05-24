using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class ClientInvoice
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int? SecondContactId { get; set; }
        public short OwnerEmployeeId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public bool IsFinalised { get; set; }
        public DateTime? FinalisedDateTime { get; set; }
        public string InvoiceLangIanacode { get; set; }
        public short InvoiceCurrencyId { get; set; }
        public string ClientPonumber { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDescription { get; set; }
        public decimal Vatrate { get; set; }
        public string InvoiceOrgName { get; set; }
        public string InvoiceAddress1 { get; set; }
        public string InvoiceAddress2 { get; set; }
        public string InvoiceAddress3 { get; set; }
        public string InvoiceAddress4 { get; set; }
        public string InvoiceCountyOrState { get; set; }
        public string InvoicePostcodeOrZip { get; set; }
        public short InvoiceCountryId { get; set; }
        public bool ShowOrderPonumbersInBreakdown { get; set; }
        public bool ShowNotesInBreakdown { get; set; }
        public bool ShowSourceLangsInBreakdown { get; set; }
        public bool ShowTargetLangsInBreakdown { get; set; }
        public bool ShowJobItemsInBreakdown { get; set; }
        public bool ShowContactNamesInBreakdown { get; set; }
        public byte WordCountPresentationOption { get; set; }
        public bool ShowInterpretingDurationInBreakdown { get; set; }
        public bool ShowWorkDurationInBreakdown { get; set; }
        public bool ShowPagesOrSlidesInBreakdown { get; set; }
        public bool ShowCustomerSpecificField1ValueInBreakdown { get; set; }
        public byte PaymentTermDays { get; set; }
        public bool AutoChaseWhenOverdue { get; set; }
        public DateTime? PaidDate { get; set; }
        public byte? PaidMethodId { get; set; }
        public DateTime? ExportedToSageDateTime { get; set; }
        public decimal? OverallChargeToClient { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public short CreatedByEmployeeId { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public short? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public string DeletedFreeTextReason { get; set; }
        public bool EarlyPaymentDiscountUsed { get; set; }
        public DateTime? EarlyPaymentDiscountDate { get; set; }
        public decimal? AnticipatedFinalValueAmountExcludingVat { get; set; }
        public decimal? AnticipatedFinalVatamount { get; set; }
        public decimal? FinalValueAmountExcludingVat { get; set; }
        public decimal? FinalVatamount { get; set; }
        public decimal? FinalValueAmount { get; set; }
        public decimal? FinalValueAmountInGbp { get; set; }
        public bool? CustSpecificPaymentTermDaysEnabled { get; set; }
        public DateTime? EonfilesDeletedDateTime { get; set; }
        public bool ApplyLateFees { get; set; }
        public short? LatePaymentDaysCharged { get; set; }
        public short? ReminderLettersSent { get; set; }
        public decimal? ReminderFeeCharge { get; set; }
    }
}
