using System;

namespace ViewModels.JobOrder
{
    public class JobOrderDataTableViewModel
    {
        public int JobOrderId { get; set; }
        public string JobOrderName { get; set; }
        public int EnquiryId { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string SourceLang { get; set; }
        public string TargetLang { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime DeliveryDeadline { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Value { get; set; }
        public decimal Margin { get; set; }
        public decimal SupplierCost { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string SourceLangsCombined { get; set; }
        public string TargetLangsCombined { get; set; }
        public string PONumber { get; set; }
        public byte? Priority { get; set; }
        public string Progress { get; set; }
        public string Cost { get; set; }
        public string PriorityName { get; set; }
    }
}
