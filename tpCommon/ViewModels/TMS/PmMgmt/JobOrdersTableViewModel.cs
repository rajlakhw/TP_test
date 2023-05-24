namespace ViewModels.TMS.PmMgmt
{
    public class JobOrdersTableViewModel
    {
        public int JobOrderId { get; set; }
        public string JobOrderName { get; set; }
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int ConctactId { get; set; }
        public string ContactName { get; set; }
        public string DeliveryDeadline { get; set; }
        public decimal Margin { get; set; }
        public decimal OverallChargeToClient { get; set; }
        public string Currency { get; set; }
    }
}
