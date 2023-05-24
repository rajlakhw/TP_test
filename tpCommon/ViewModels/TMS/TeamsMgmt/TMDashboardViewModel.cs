using System.Collections.Generic;
using ViewModels.EmployeeModels;

namespace ViewModels.TMS.TeamsMgmt
{
    public class TMDashboardViewModel
    {
        public IEnumerable<PMBreakdownViewModel> PMBreakdowns { get; set; }
        public IEnumerable<GroupBreakdownViewModel> groupBreakdowns { get; set; }
        public IEnumerable<TeamItemsBreakdownViewModel> teamItemsBreakdowns { get; set; }
        public IEnumerable<GroupItemsBreakdownViewModel> groupItemsBreakdowns { get; set; }
        public IEnumerable<HolidaysBreakdowForTeamViewModel> holidaysBreakdownForToday { get; set; }
        public IEnumerable<HolidaysBreakdowForTeamViewModel> holidaysBreakdownForTwoWeeks { get; set; }
        public string TeamName { get; set; }
        public decimal invoicedRevenue { get; set; }
        public EmployeeViewModel LoggedInEmployee { get; set; }
        public int TeamID { get; set; }
        public int DeptID { get; set; }
    }
}
