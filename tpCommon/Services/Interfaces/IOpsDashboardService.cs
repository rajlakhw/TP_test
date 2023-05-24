using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;
using ViewModels.TMS.Ops_mgmt;

namespace Services.Interfaces
{
    public interface IOpsDashboardService : IService
    {
        Task<IEnumerable<TeamBreakdownViewModel>> GetAllTeamsDataForOpsMgmt(int departmentId);
        Task<IEnumerable<GroupBreakdownViewModel>> GetAllGroupsDataForOpsMgmt(int departmentId);
        Task<IEnumerable<GroupItemsBreakdownViewModel>> GetAllItemsByGroupsForOpsMgmt(int departmentId);
        Task<IEnumerable<TeamItemsBreakdownViewModel>> GetAllItemsByTeamsForOpsMgmt(int departmentId);
        Task<IEnumerable<DisplayRevenueTableViewModel>> GetRevenueForOpsMgmt(int departmentId);
        Task<decimal> GetChartInvoicedRevenueForOpsMgmt(int departmentId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetOpenJobOrdersDataForOpsMgmt(int teamId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetOverdueJobOrdersDataForOpsMgmt(int teamId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetDueTodayJobOrdersDataForOpsMgmt(int teamId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetOpenJobOrdersDataByDepartmentForOpsMgmt(int departmentId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetOverdueJobOrdersDataByDepartmentForOpsMgmt(int departmentId);
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetDueTodayJobOrdersDataByDepartmentForOpsMgmt(int departmentId);
        Task<IEnumerable<PeopleHolidayHeadcountViewModel>> GetPeopleDataForTodayByDepartmentForOpsMgmt(int departmentId);
        Task<IEnumerable<PeopleHolidayHeadcountViewModel>> GetPeopleDataForTwoWeeksByDepartmentForOpsMgmt(int departmentId);
        Task<TeamBreakdownViewModel> GetTBAEmployeeJobsForOpsMgmt();
        Task<TeamItemsBreakdownViewModel> GetTBAEmployeeItemsForOpsMgmt();
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeOpenJobsForOpsMgmt();
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeOverdueJobsForOpsMgmt();
        Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeDueTodayJobsForOpsMgmt();
        Task<EmployeeDepartment> GetDepartmentFromTeamId(int teamId);
    }
}
