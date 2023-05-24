using Data;
using System.Collections.Generic;

namespace ViewModels.TMS.Ops_mgmt
{
    public class OpsDashboardViewModel
    {
        public IEnumerable<TeamBreakdownViewModel> teamBreakdowns { get; set; }
        public IEnumerable<GroupBreakdownViewModel> groupBreakdowns { get; set; }
        public IEnumerable<TeamItemsBreakdownViewModel> teamItemsBreakdowns { get; set; }
        public IEnumerable<GroupItemsBreakdownViewModel> groupItemsBreakdowns { get; set; }
        public IEnumerable<PeopleHolidayHeadcountViewModel> peopleHeadcountBreakdownForToday { get; set; }
        public IEnumerable<PeopleHolidayHeadcountViewModel> peopleHeadcountBreakdownForTwoWeeks { get; set; }
        public decimal invoicedRevenue { get; set; }
        public string departmentName { get; set; }
        public int departmentId { get; set; }
        public Employee Employee { get; set; }
    }
}
