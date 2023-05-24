namespace ViewModels.TMS.TeamsMgmt
{
    public class GroupItemsBreakdownViewModel
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public int GroupId { get; set; }
        public string OrgGroupName { get; set; }
        public int ItemsNotSentToSupplier { get; set; }
        public int ItemsInTranslation { get; set; }
        public int ItemsInProofreading { get; set; }
        public int ItemsInClientReview { get; set; }
        public int ItemsInProduction { get; set; }
        public int OverdueAndRejectedItems { get; set; }
    }
}
