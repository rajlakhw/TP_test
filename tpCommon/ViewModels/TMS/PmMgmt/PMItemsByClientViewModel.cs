namespace ViewModels.TMS.PmMgmt
{
    public class PMItemsByClientViewModel
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int OrgGroupId { get; set; }
        public string OrgGroupName { get; set; }
        public int ItemsNotSentToSupplier { get; set; }
        public int ItemsInTranslation { get; set; }
        public int ItemsInProofreading { get; set; }
        public int ItemsInClientReview { get; set; }
        public int ItemsInProduction { get; set; }
        public int OverdueItems { get; set; }
    }
}
