namespace ViewModels.TMS.Ops_mgmt
{
    public class GroupItemsBreakdownViewModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public int ItemsNotSentToSupplier { get; set; }
        public int ItemsInTranslation { get; set; }
        public int ItemsInProofreading { get; set; }
        public int ItemsInClientReview { get; set; }
        public int ItemsInProduction { get; set; }
        public int OverdueItems { get; set; }
    }
}
