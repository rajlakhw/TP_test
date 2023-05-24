namespace ViewModels.TMS.TeamsMgmt
{
    public class TeamItemsBreakdownViewModel
    {
        public int Id { get; set; }
        public string PMName { get; set; }
        public int ItemsNotSentToSupplier { get; set; }
        public int ItemsInTranslation { get; set; }
        public int ItemsInProofreading { get; set; }
        public int ItemsInClientReview { get; set; }
        public int ItemsInProduction { get; set; }
        public int OverdueAndRejectedItems { get; set; }
        public int TeamId { get; set; }
    }
}
