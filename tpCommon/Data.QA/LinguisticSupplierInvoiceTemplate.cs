using System;
using System.Collections.Generic;

#nullable disable

namespace Data.QA
{
    public partial class LinguisticSupplierInvoiceTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string LinguisticSupplierName { get; set; }
        public int? LinguisticSupplierId { get; set; }
        public int? PaidMethod { get; set; }
        public double? Vatrate { get; set; }
        public string Vatnumber { get; set; }
        public string InvoiceLangIanacode { get; set; }
        public short? InvoiceCurrencyId { get; set; }
        public string ClientPonumber { get; set; }
        public string NameToShowOnInvoice { get; set; }
        public string Address1ToShowOnInvoice { get; set; }
        public string Address2ToShowOnInvoice { get; set; }
        public string Address3ToShowOnInvoice { get; set; }
        public string Address4ToShowOnInvoice { get; set; }
        public int? CountryToShowOnInvoice { get; set; }
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
        public DateTime? DeletedDateTime { get; set; }
        public string CompanyName { get; set; }
        public string CompanyRegistrationNumber { get; set; }
    }
}
