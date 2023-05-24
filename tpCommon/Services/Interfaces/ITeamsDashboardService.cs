using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.TMS.TeamsMgmt;

namespace Services.Interfaces
{
    public interface ITeamsDashboardService : IService
    {
        Task<IEnumerable<PMBreakdownViewModel>> GetStatusPerPM(int teamId, string teamName);
        Task<IEnumerable<GroupBreakdownViewModel>> GetAllClientsData(int teamId);
        Task<IEnumerable<GroupItemsBreakdownViewModel>> GetAllItemsByClients(int teamId);
        Task<IEnumerable<TeamItemsBreakdownViewModel>> GetAllItemsByPMs(int teamId, string teamName);
        Task<IEnumerable<DisplayRevenueTableViewModel>> GetRevenue(int teamId);
        Task<decimal> GetInvoicedRevenueForTMs(int teamId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOpenJobOrdersDataByTeam(int teamId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOverdueJobOrdersDataForPM(int projectManagerId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetDueTodayJobOrdersDataForPM(int projectManagerId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOpenJobOrdersDataForPM(int pmId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOverdueJobOrdersDataByTeam(int teamId);
        Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetDueTodayJobOrdersDataByTeam(int teamId);
        Task<IEnumerable<HolidaysBreakdowForTeamViewModel>> HolidaysBreakdowForTeam(int teamId);
        Task<IEnumerable<HolidaysBreakdowForTeamViewModel>> HolidaysBreakdowNextTwoWeeksForTeam(int teamId);
        Task<EmployeeTeam> GetTeam(int teamId);
    }
}
