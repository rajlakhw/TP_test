using System.Threading.Tasks;
using System.Collections.Generic;
using Services.Common;
using ViewModels.TMS.PmMgmt;

namespace Services.Interfaces
{
    public interface IPMDashboardService : IService
    {
        Task<IEnumerable<JobOrdersTableViewModel>> GetTodaysDeliveriesJobs(int pmId);
        Task<IEnumerable<JobOrdersTableViewModel>> GetApproachingDeadlinesInNextThreeDaysJobs(int pmId);
        Task<IEnumerable<JobOrdersTableViewModel>> GetOverdueJobs(int pmId);
        Task<IEnumerable<JobOrdersTableViewModel>> GetClientReviewJobs(int pmId);
        Task<IEnumerable<AllJobOrdersViewModel>> GetAllJobOrdersForPm(int pmId);
        Task<IEnumerable<PMOrdersByClientViewModel>> GetJobOrdersByClients(int pmId);
        Task<IEnumerable<PMItemsByClientViewModel>> GetJobItemsByClients(int pmId);
    }
}
