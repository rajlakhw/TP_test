namespace ViewModels.TMS.PmMgmt
{
    public class AllJobOrdersViewModel
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
        public string SubmittedDate { get; set; }
        public string DeliveryDeadline { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
        public decimal Value { get; set; }
        public decimal Margin { get; set; }
    }
}
