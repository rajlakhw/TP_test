using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class LinguisticSupplierInvoice
    {
        public int Id { get; set; }
        public string LinguisticSupplierName { get; set; }
        public int? LinguisticSupplierId { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? PaidMethod { get; set; }
        public double? Vatrate { get; set; }
        public string InvoiceLangIanacode { get; set; }
        public short? InvoiceCurrencyId { get; set; }
        public decimal? SubTotalValue { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? Vatvalue { get; set; }
        public string Vatnumber { get; set; }
        public decimal? TotalValue { get; set; }
        public decimal? Amount { get; set; }
        public string LinguisticSupplierInvoiceNumber { get; set; }
        public string NameToShowOnInvoice { get; set; }
        public string Address1ToShowOnInvoice { get; set; }
        public string Address2ToShowOnInvoice { get; set; }
        public string Address3ToShowOnInvoice { get; set; }
        public string Address4ToShowOnInvoice { get; set; }
        public string CountryToShowOnInvoice { get; set; }
        public short? EarlyPaymentOption { get; set; }
        public int? AgreedPaymentMethod { get; set; }
        public string BankBranchName { get; set; }
        public string BankBranchAddress { get; set; }
        public string BankBranchPostCode { get; set; }
        public string BankBranchCountry { get; set; }
        public string BankBranchCity { get; set; }
        public string BankAccountName { get; set; }
        public string BankAccountSortCode { get; set; }
        public string BankAccountRtnnumber { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankAccountSwiftorBic { get; set; }
        public string BankAccountIban { get; set; }
        public string IntermediaryBankDetails { get; set; }
        public string AdditionalInformationForBank { get; set; }
        public string PayPalId { get; set; }
        public string SkrillId { get; set; }
        public string OptionalText1 { get; set; }
        public string OptionalText2 { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public int? CreatedByEmployee { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public int? LastModifiedByEmployeeId { get; set; }
        public DateTime? DeletedDateTime { get; set; }
        public short? DeletedByEmployeeId { get; set; }
        public DateTime? SupplierInvoiceHoldDateTime { get; set; }
        public int? SupplierInvoiceHoldByEmployee { get; set; }
        public DateTime? SupplierInvoiceOffHoldDateTime { get; set; }
        public int? SupplierInvoiceOffHoldByEmployee { get; set; }
        public bool SupplierInvoiceOnHold { get; set; }
        public DateTime? ExportedToSageDateTime { get; set; }
    }
}
