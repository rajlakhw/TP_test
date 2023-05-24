namespace ViewModels.TMS.PmMgmt
{
    public class PMOrdersByClientViewModel
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public string SpendCategory { get; set; }
        public decimal TotalOpenOrdersGBPValue { get; set; }
        public int OpenJobsCount { get; set; }
        public int OpenItemsCount { get; set; }
        public int OverdueJobsCount { get; set; }
        public int OverdueItemsCount { get; set; }
        public int JobsDueToday { get; set; }
        public int ItemsDueToday { get; set; }
    }
}
