namespace ViewModels.TMS.TeamsMgmt
{
    public class PMBreakdownViewModel
    {
        public int Id { get; set; }
        public string PMName { get; set; }
        public int OpenJobsCount { get; set; }
        public int OpenItemsCount { get; set; }
        public int OverdueJobsCount { get; set; }
        public int OverdueItemsCount { get; set; }
        public int JobsDueToday { get; set; }
        public int ItemsDueToday { get; set; }
        public int TeamId { get; set; }
    }
}
