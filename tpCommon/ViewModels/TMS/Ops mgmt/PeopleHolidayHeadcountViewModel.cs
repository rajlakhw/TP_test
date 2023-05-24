namespace ViewModels.TMS.Ops_mgmt
{
    public class PeopleHolidayHeadcountViewModel
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int TMsCount { get; set; }
        public int PMsCount { get; set; }
        public int SickLeaves { get; set; }
        public int Available { get; set; }
        public int Holiday { get; set; }
    }
}
