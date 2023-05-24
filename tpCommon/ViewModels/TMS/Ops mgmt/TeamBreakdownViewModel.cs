namespace ViewModels.TMS.Ops_mgmt
{
    public class TeamBreakdownViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int OpenJobsCount { get; set; }
        public int OpenItemsCount { get; set; }
        public int OverdueJobsCount { get; set; }
        public int OverdueItemsCount { get; set; }
        public int JobsDueToday { get; set; }
        public int ItemsDueToday { get; set; }
        public int DepartmentId { get; set; }
    }
}
