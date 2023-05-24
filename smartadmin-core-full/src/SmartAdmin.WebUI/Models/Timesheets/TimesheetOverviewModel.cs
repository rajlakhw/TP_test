using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;

namespace SmartAdmin.WebUI.Models.Timesheet
{
    public class TimesheetOverviewModel
    {
        //public IEnumerable<Data.Timesheet> AllTimesheetLogsForCurrentView { get; set; }

        public List<Data.Org> AllRowHeaderForAWeek { get; set; }

        public Hashtable AllTandPRowHeaderForAWeek { get; set; }

        public List<Data.EndClient> AllEndClients { get; set; }

        public List<Data.EndClientData> AllGroupeCategories { get; set; }

        public List<Data.EndClientData> AllGroupeBrands { get; set; }

        public List<Data.EndClientData> AllGroupeCampaigns { get; set; }

        public IEnumerable<Data.Timesheet> AllMondayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllTuesdayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllWednesdayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllThursdayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllFridayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllSaturdayTimeLogs { get; set; }

        public IEnumerable<Data.Timesheet> AllSundayTimeLogs { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Dictionary<int, IEnumerable<Data.TimesheetLogBreakdown>> AllTimesheetLogsBreakdown { get; set; }

        public Dictionary<int, Data.TimesheetTaskCategory> AllTimesheetCategories { get; set; }

        public IEnumerable<Data.TimesheetTaskCategory> AllNonChargeableTasksForEmployee { get; set; }

        public IEnumerable<Data.TimesheetTaskCategory> AllChargeableTasksForEmployee { get; set; }

        public IEnumerable<Data.Timesheet> AllNonClientTasksForEmployee { get; set; }

        public IEnumerable<Data.TimesheetTaskCategory> AllNonClientActivitiesForEmployee { get; set; }

        public IEnumerable<String> WeeklyTotalString { get; set; }

        public IEnumerable<String> DailyTotalString { get; set; }

        public IEnumerable<Employee> AllEmployeesForApproval { get; set; }

        public TimesheetsApproval ApprovalDetails { get; set; }

        public List<EndClient> EndClientList { get; set; }

        public List<EndClient> BrandList { get; set; }

        public List<EndClient> CategoryList { get; set; }

        public List<EndClient> CampaignList { get; set; }




        //public IEnumerable<SharePlusArticle> Pinned { get; set; }
    }
}
