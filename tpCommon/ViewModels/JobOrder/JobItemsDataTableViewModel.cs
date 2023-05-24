using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.JobOrder
{
    public class JobItemsDataTableViewModel
    {
        public int Id { get; set; }
        public int JobOrderId { get; set; }
        public string SourceLanguageName { get; set; }
        public string TargetLanguageName { get; set; }
        public string LanguageServiceName { get; set; }
        public DateTime? SupplierCompletionDeadline { get; set; }
        public DateTime? WeCompletedItemDateTime { get; set; }
        public DateTime? SupplierSentWorkDateTime { get; set; }
        public DateTime? SupplierAcceptedWorkDateTime { get; set; }
        public DateTime? SupplierCompletedItemDateTime { get; set; }
        public string SourceLanguageIANACode { get; set; }
        public string TargetLanguageIANACode { get; set; }
        public Boolean? SupplierIsClientReviewer { get; set; }
        public int? LinguisticSupplierOrClientReviewerId { get; set; }
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public string ClientCurrency { get; set; }
        public decimal? ChargeToClient { get; set; }
        public string SupplierCurrency { get; set; }
        public decimal? PaymentToSupplier { get; set; }
        public Boolean? MinimumSupplierChargeApplied { get; set; }
        public decimal? Margin { get; set; }
        public int WorkMinutes { get; set; }
        public int NewClientWordCount { get; set; }
        public int TotalClientWordCount { get; set; }
        public int NewSupplierWordCount { get; set; }
        public int TotalSupplierWordCount { get; set; }
        public string Status { get; set; }
        public string StatusColour { get; set; }
        public string Visible { get; set; }
        public string VisibleStatus { get; set; }
        public string SuppliersInitials { get; set; }
        public string ContactsInitials { get; set; }
        public string StatusBGColour { get; set; }
        public short? ExtranetClientStatusId { get; set; }
        public short? WebServiceClientStatusId { get; set; }
    }
}
