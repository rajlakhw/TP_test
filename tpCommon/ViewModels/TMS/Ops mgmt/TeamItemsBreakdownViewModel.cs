namespace ViewModels.TMS.Ops_mgmt
{
    public class TeamItemsBreakdownViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int ItemsNotSentToSupplier { get; set; }
        public int ItemsInTranslation { get; set; }
        public int ItemsInProofreading { get; set; }
        public int ItemsInClientReview { get; set; }
        public int ItemsInProduction { get; set; }
        public int OverdueAndRejectedItems { get; set; }
        public int DepartmentId { get; set; }
    }
}
