using Data;
using System.Collections.Generic;
using ViewModels.EmployeeModels;

namespace ViewModels.TMS.PmMgmt
{
    public class PMDashboard
    {
        public int PmId { get; set; }
        public string PmName { get; set; }
        public IEnumerable<AllJobOrdersViewModel> AllJobs { get; set; }
        public IEnumerable<JobOrdersTableViewModel> TodayDeliveries { get; set; }
        public IEnumerable<JobOrdersTableViewModel> OverdueJobs { get; set; }
        public IEnumerable<JobOrdersTableViewModel> ApproachingDeadlines { get; set; }
        public IEnumerable<JobOrdersTableViewModel> ClientReviewJobs { get; set; }
        public IEnumerable<PMOrdersByClientViewModel> JobOrdersClientBreakdown { get; set; }
        public IEnumerable<PMItemsByClientViewModel> JobItemsClientBreakdown { get; set; }
        public EmployeeViewModel LoggedInEmployee { get; set; }
        public string TeamName { get; set; }
        public int DeptID { get; set; }
    }
}
