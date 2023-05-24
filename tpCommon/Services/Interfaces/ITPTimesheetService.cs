using System;
using System.Collections.Generic;
using System.Collections;
using Data;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPTimesheetService : IService
    {
        Task<List<Data.Timesheet>> GetAllTimesheetLogs<Timesheet>(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<Hashtable> GetAllTandPRowHeadersForAWeek(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<List<Data.Org>> GetAllRowHeaderOrgCategoriesForAWeek<Org>(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<Data.Timesheet> GetTimesheetLogForAGivenDate<Timesheet>(int? OrgID, int EmployeeID, DateTime LogDate, int? EndClientID = null, int? BrandID = null, int? CategoryID = null, int? CampaignID = null);
        Task<Dictionary<int, IEnumerable<Data.TimesheetLogBreakdown>>> GetAllTimesheetLogBreakdown<TimesheetLogBreakdown>(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<Dictionary<int, Data.TimesheetTaskCategory>> GetAllTimesheetCategories<TimesheetTaskCategory>(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<IEnumerable<Data.TimesheetTaskCategory>> GetAllTimesheetForTeam<TimesheetTaskCategory>(EmployeeTeam team = null, byte CategoryTypeID = 0);
        Task<Data.TimesheetTaskCategory> GetTimesheetCategory<TimesheetTaskCategory>(int CategoryId);
        Task<TimesheetLogBreakdown> GetBreakdownById(int breakdownId);
        Task<Timesheet> GetTimesheetLogById(int logId);
        Task<int> UpdateBreakdown<TimesheetBreakdownModel>(int Id, int CategoryID, short TaskHours, short TaskMinutes, short LastModifiedByEmpId);
        Task<int> CreateNewBreakdown<TimesheetBreakdownModel>(int TimesheetLogId, int CategoryID, short TaskHours, short TaskMinutes, short CreatedByEmployeeId);
        Task<int> CreateNewTimesheetLog<TimesheetLogModel>(int EmployeeID, int? OrgID, DateTime TimeLogDate, int? EndClientID = null, int? BrandID = null, int? CategoryID = null, int? CampaignID = null);
        Task<int> UpdateTimesheetLog<Timesheet>(int Id, int EmployeeID, int? OrgID, DateTime TimeLogDate, int? EndClientID = null, int? BrandID = null, int? CategoryID = null, int? CampaignID = null);
        System.Threading.Tasks.Task Delete(int Id, short DeletedByEmplId);
        Task<string> GetWeeklyTotalForOrg(int EmployeeID, DateTime StartDate, DateTime EndDate, int? OrgID);
        Task<string> GetWeeklyTotalForTandPOrg(int EmployeeID, DateTime StartDate, DateTime EndDate, int? OrgID, int? EndClientID, int? BrandID, int? CategoryID, int? CampaignID);
        Task<List<string>> GetAllDailyTotals(int EmployeeID, DateTime StartDate, DateTime EndDate);
        Task<int> ApproveTimesheetForWeek<TimesheetsApproval>(int EmployeeIdToApprove, DateTime StartDate, DateTime EndDate, int ApprovedByEmpId);
        Task<int> UnlockTimesheetForEditing(int EmployeeIdToUnlock, DateTime StartDate, DateTime EndDate, int UnlockedByEmpId);
        Task<TimesheetsApproval> GetApprovalDetailsForTimesheets(int employeeId, DateTime weekStarting);
        Task<Dictionary<int, string>> GetAllOrgGroupsIdAndName();

        Task<Dictionary<int, string>> GetAllOrgsIdAndName();

        Task<List<TimesheetTaskCategory>> GetAllActivitiesForGivenTypeIDString(string allTypeIDsString);
        System.Threading.Tasks.Task SubmitTimesheet(int EmployeeIdToApprove, DateTime StartDate, DateTime EndDate);

        //Task<IEnumerable<Org>> GetAllRowHeaderCategoriesForAWeek<T>(short id, DateTime startDate, DateTime endDate);

        //Task<Data.Timesheet> GetTimesheetLogDetails<Timesheet>(int entryID);
    }
}
