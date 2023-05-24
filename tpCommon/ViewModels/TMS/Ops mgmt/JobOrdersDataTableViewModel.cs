using System;namespace ViewModels.TMS.Ops_mgmt
{
    public class JobOrdersDataTableViewModel
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; }
        public int JobOrderId { get; set; }
        public string JobOrderName { get; set; }
        public int PmId { get; set; }
        public string ProjectManagerName { get; set; }
        public string DeliveryDeadline { get; set; }
        public string OrderStatus { get; set; }
        public string OrgName { get; set; }
        public int OrgId { get; set; }
        public string OrgGroupName { get; set; }
        public int OrgGroupId { get; set; }
        public int OriginatedFromEnquiryId { get; set; }
        public decimal MarginPercentage { get; set; }
        public decimal OverallChargeToClient { get; set; }
        public string Currency { get; set; }
    }
}
