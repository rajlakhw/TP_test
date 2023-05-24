using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.TMS.TeamsMgmt;

namespace Services
{
    public class TeamsDashboardService : ITeamsDashboardService
    {
        private readonly IRepository<EmployeeTeam> teamRepository;

        private static readonly DateTime currentYearStartDate = new DateTime(DateTime.Today.Year, 1, 1);
        private static readonly DateTime currentYearEndDate = new DateTime(DateTime.Today.Year, 12, DateTime.DaysInMonth(DateTime.Today.Year, 12));

        private static readonly string JobOrdersDataTableCommand = @"select distinct Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as ProjectManager, JobOrders.ID, JobOrders.JobName,  FORMAT (Joborders.OverallDeliveryDeadline, 'dd MMM yyyy HH:mm') as Delivery,
            (Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
            when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
            when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'
            end) as OrderStatus, Orgs.ID, Orgs.OrgName, Orgs.OrgGroupID, OrgGroups.Name, OriginatedFromEnquiryID, 
            ISNULL(OverallChargeToClient, 0) as ChargeToClient, AnticipatedGrossMarginPercentage,
            (SELECT [Currencies].[Prefix] FROM [dbo].[Currencies] WHERE [Currencies].[ID] = JobOrders.ClientCurrencyID) as Currency
            
            from JobOrders
            
            inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
            inner join contacts on contacts.ID = joborders.ContactID
            inner join Orgs on orgs.ID = Contacts.OrgID
            inner join OrgGroups on orgs.OrgGroupID = OrgGroups.ID";
        private static readonly string ItemsCountColumnsQuerySection = @"COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.SupplierSentWorkDateTime is null and 
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierCompletionDeadline > dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'ItemsNotSentToSupplier',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID IN(1,17) and 
JobItems.SupplierAcceptedWorkDateTime is not null and
JobItems.SupplierCompletionDeadline > dbo.funcGetCurrentUKTime() and
JobItems.SupplierCompletedItemDateTime is null THEN JobItems.id END) as 'ItemsInTranslation',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID = 7 and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierCompletedItemDateTime is not null and
JobItems.SupplierCompletionDeadline > dbo.funcGetCurrentUKTime() and
JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInProofreading',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID = 21 and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierCompletedItemDateTime is not null and
JobItems.SupplierCompletionDeadline > dbo.funcGetCurrentUKTime() and
JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInClientReview',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID IN(4, 35) and 
JobItems.WeCompletedItemDateTime is null and
JobItems.SupplierCompletedItemDateTime is not null and
JobItems.SupplierCompletionDeadline > dbo.funcGetCurrentUKTime() and
JobItems.SupplierAcceptedWorkDateTime is not null THEN JobItems.id END) as 'ItemsInProduction',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierSentWorkDateTime < dbo.funcGetCurrentUKTime() and
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'Overdue Items'";

        public TeamsDashboardService(IRepository<EmployeeTeam> teamRepository)
        {
            this.teamRepository = teamRepository;
        }

        public async Task<IEnumerable<GroupBreakdownViewModel>> GetAllClientsData(int teamId)
        {
            var res = new List<GroupBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Orgs.ID, Orgs.OrgName, OrgGroups.ID, OrgGroups.Name, count(distinct(joborders.id)) as 'OpenJobsCount', count(jobitems.ID) as 'OpenItemsCount',
                    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
                    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'
                    
                    from JobOrders
                    
                    left outer join JobItems on JobItems.JobOrderID = joborders.ID
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on Contacts.OrgID = Orgs.ID
                    inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
                    
                    where Employees.TeamID in (" + teamId + @") and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
                    
                    group by Orgs.ID, Orgs.OrgName, OrgGroups.name, OrgGroups.ID
                    order by Orgs.OrgName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result
                while (await result.ReadAsync())
                {
                    res.Add(new GroupBreakdownViewModel()
                    {
                        OrgId = await result.GetFieldValueAsync<int>(0),
                        OrgName = await result.GetFieldValueAsync<string>(1),
                        GroupId = await result.GetFieldValueAsync<int>(2),
                        OrgGroupName = await result.GetFieldValueAsync<string>(3),
                        OpenJobsCount = await result.GetFieldValueAsync<int>(4),
                        OpenItemsCount = await result.GetFieldValueAsync<int>(5),
                        OverdueJobsCount = await result.GetFieldValueAsync<int>(6),
                        OverdueItemsCount = await result.GetFieldValueAsync<int>(7),
                        JobsDueToday = await result.GetFieldValueAsync<int>(8),
                        ItemsDueToday = await result.GetFieldValueAsync<int>(9)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<GroupItemsBreakdownViewModel>> GetAllItemsByClients(int teamId)
        {
            var res = new List<GroupItemsBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Orgs.ID, Orgs.OrgName, OrgGroups.ID, OrgGroups.Name,
" + ItemsCountColumnsQuerySection + @"
from JobOrders
left outer join JobItems on JobItems.JobOrderID = joborders.ID
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on Contacts.OrgID = Orgs.ID
inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
where Employees.TeamID in (" + teamId + @") and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
group by Orgs.ID, Orgs.OrgName, OrgGroups.name, OrgGroups.ID
order by Orgs.OrgName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new GroupItemsBreakdownViewModel()
                    {
                        OrgId = await result.GetFieldValueAsync<int>(0),
                        OrgName = await result.GetFieldValueAsync<string>(1),
                        GroupId = await result.GetFieldValueAsync<int>(2),
                        OrgGroupName = await result.GetFieldValueAsync<string>(3),
                        ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(4),
                        ItemsInTranslation = await result.GetFieldValueAsync<int>(5),
                        ItemsInProofreading = await result.GetFieldValueAsync<int>(6),
                        ItemsInClientReview = await result.GetFieldValueAsync<int>(7),
                        ItemsInProduction = await result.GetFieldValueAsync<int>(8),
                        OverdueAndRejectedItems = await result.GetFieldValueAsync<int>(9)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<TeamItemsBreakdownViewModel>> GetAllItemsByPMs(int teamId, string teamName)
        {
            var res = new List<TeamItemsBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMName,
    " + ItemsCountColumnsQuerySection + @"
    ,employeeteams.ID
    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.ID
    inner join contacts on contacts.ID = joborders.ContactID
    where EmployeeTeams.ID in (" + teamId + @") and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null
    and Contacts.OrgID <> 83923
    group by Employees.ID, Employees.FirstName, Employees.Surname, EmployeeTeams.ID
    order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new TeamItemsBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<short>(0),
                        PMName = await result.GetFieldValueAsync<string>(1),
                        ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(2),
                        ItemsInTranslation = await result.GetFieldValueAsync<int>(3),
                        ItemsInProofreading = await result.GetFieldValueAsync<int>(4),
                        ItemsInClientReview = await result.GetFieldValueAsync<int>(5),
                        ItemsInProduction = await result.GetFieldValueAsync<int>(6),
                        OverdueAndRejectedItems = await result.GetFieldValueAsync<int>(7),
                        TeamId = await result.GetFieldValueAsync<byte>(8)
                    });
                }
            }
            res.Insert(0, new TeamItemsBreakdownViewModel()
            {
                Id = teamId,
                TeamId = teamId,
                PMName = "Total " + teamName,
                ItemsNotSentToSupplier = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsNotSentToSupplier),
                ItemsInTranslation = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsInTranslation),
                ItemsInProofreading = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsInProofreading),
                ItemsInClientReview = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsInClientReview),
                ItemsInProduction = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsInProduction),
                OverdueAndRejectedItems = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.OverdueAndRejectedItems),
            });

            return res;
        }
        public async Task<decimal> GetInvoicedRevenueForTMs(int teamId)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();
            decimal res = 0;

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"SELECT 
                          ISNULL((CONVERT(DECIMAL(38,2), SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)))), 0) as TotalGBPInvoiced
                        
                      FROM JobOrders
                      inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                      inner join Contacts on Contacts.ID = JobOrders.ContactID
                      inner join Orgs on orgs.ID = Contacts.OrgID
                      inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                      inner join JobItems on JobOrders.ID = JobItems.JobOrderID
                      
                      where ClientInvoices.InvoiceDate between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                      and JobOrders.DeletedDate is null
                      and Employees.TeamID in (" + teamId + @")
                      and ClientInvoices.DeletedDateTime is null
                      and Orgs.OrgGroupID not in(18648,18520)
                      and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                      and ClientInvoices.IsFinalised = 1";

                //var as = @"-- INVOICED REVENUE MONTHLY
                //Select (SELECT 
                //      ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.AnticipatedFinalValueAmount)))), 0) as TotalGBPInvoiced

                //  FROM JobOrders
                //  inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                //  inner join Contacts on Contacts.ID = JobOrders.ContactID
                //  inner join Orgs on orgs.ID = Contacts.OrgID
                //  inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID

                //  where ClientInvoices.InvoiceDate between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                //  and JobOrders.DeletedDate is null
                //  and Employees.TeamID in (" + teamId + @")
                //  and ClientInvoices.DeletedDateTime is null
                //  and Orgs.OrgGroupID not in(18648,18520)
                //  and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                //  and ClientInvoices.IsFinalised = 1) as InvoicedRevenue,

                //  -- PENDING REVENUE MONTHLY
                //  (SELECT 
                //      ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.OverallChargeToClient)))), 0) as PendingGBPRevenue

                //  FROM JobOrders
                //  left outer join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                //  inner join Contacts on Contacts.ID = JobOrders.ContactID
                //  inner join Orgs on orgs.ID = Contacts.OrgID
                //  inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID

                //  where JobOrders.SubmittedDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                //  and JobOrders.DeletedDate is null
                //  and Employees.TeamID in (" + teamId + @")
                //  and ClientInvoices.DeletedDateTime is null
                //  and Orgs.OrgGroupID not in(18648,18520)
                //  and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and JobOrders.OverallChargeToClient is not null
                //  and ClientInvoices.ExportedToSageDateTime is null) as Pending,

                //  -- Recognised revenue monthly
                //  (SELECT 
                //      ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.AnticipatedFinalValueAmount)))), 0) as RecognisedRevenue

                //  FROM JobOrders
                //  inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                //  inner join Contacts on Contacts.ID = JobOrders.ContactID
                //  inner join Orgs on orgs.ID = Contacts.OrgID
                //  inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID

                //  where ClientInvoices.ExportedToSageDateTime is not null
                //  and ClientInvoices.ExportedToSageDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                //  and JobOrders.DeletedDate is null
                //  and Employees.TeamID in (" + teamId + @")
                //  and ClientInvoices.DeletedDateTime is null
                //  and Orgs.OrgGroupID not in(18648,18520)
                //  and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                //  and Joborders.OverallCompletedDateTime is not null and JobOrders.OverallChargeToClient is not null) as Recognised";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res = await result.GetFieldValueAsync<decimal>(0);
                    //res.InvoicedRevenueMonthly = await result.GetFieldValueAsync<decimal>(0);
                    //res.PendingRevenueMonthly = await result.GetFieldValueAsync<decimal>(1);
                    //res.RecognisedRevenue = await result.GetFieldValueAsync<decimal>(2);
                }
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetDueTodayJobOrdersDataForPM(int projectManagerId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Due today jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.ID = " + projectManagerId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetDueTodayJobOrdersDataByTeam(int teamId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Due today jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.TeamID = " + teamId.ToString() + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOpenJobOrdersDataByTeam(int teamId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Open jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.TeamID = " + teamId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOpenJobOrdersDataForPM(int pmId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Open jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.ID = " + pmId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOverdueJobOrdersDataForPM(int projectManagerId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Overdue jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.ID = " + projectManagerId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime()";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableTMViewModel>> GetOverdueJobOrdersDataByTeam(int teamId)
        {
            var res = new List<JobOrdersDataTableTMViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Overdue jobs
                    " + JobOrdersDataTableCommand + @"
                    where Employees.TeamID = " + teamId.ToString() + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime()";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<DisplayRevenueTableViewModel>> GetRevenue(int teamId)
        {
            var res = new List<DisplayRevenueTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is not null and (Month(JobItems.WeCompletedItemDateTime) <= Month((getdate())) AND Month(JobItems.WeCompletedItemDateTime) >= Month((getdate()))) AND (YEAR(JobItems.WeCompletedItemDateTime) <= YEAR((getdate())) AND YEAR(JobItems.WeCompletedItemDateTime) >= YEAR((getdate()))) THEN JobItems.id END) as 'ClosedItemsCurentMonth',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is not null and (YEAR(JobItems.WeCompletedItemDateTime) <= YEAR((getdate())) AND YEAR(JobItems.WeCompletedItemDateTime) >= YEAR((getdate()))) THEN JobItems.id END) as 'ClosedItemsCurentYear',
                    COUNT(DISTINCT CASE WHEN JobOrders.DeletedDate is null and JobOrders.OverallCompletedDateTime is not null and (Month(JobOrders.OverallCompletedDateTime) <= Month((getdate())) AND Month(JobOrders.OverallCompletedDateTime) >= Month((getdate()))) AND (YEAR(JobOrders.OverallCompletedDateTime) <= YEAR((getdate())) AND YEAR(JobOrders.OverallCompletedDateTime) >= YEAR((getdate()))) THEN JobOrders.id END) as 'ClosedOrdersCurentMonth',
                    COUNT(DISTINCT CASE WHEN JobOrders.DeletedDate is null and JobOrders.OverallCompletedDateTime is not null and (YEAR(JobOrders.OverallCompletedDateTime) <= YEAR((getdate())) AND YEAR(JobOrders.OverallCompletedDateTime) >= YEAR((getdate()))) THEN JobOrders.id END) as 'ClosedOrdersCurentYear'
                    
                    from JobOrders
                    left outer join JobItems on JobItems.JobOrderID = joborders.ID
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
                    
                    where Employees.TeamID in (" + teamId + @") and
                    Employees.TerminateDate is null and
                    joborders.DeletedDate is null  and 
                    Orgs.OrgGroupID not in(18648,18520) and 
                    (JobOrders.SubmittedDateTime between '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"') and
                    Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                    
                    group by Employees.FirstName, Employees.Surname, Employees.ID
                    order by Employees.FirstName, Employees.Surname";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new DisplayRevenueTableViewModel()
                    {
                        PMId = await result.GetFieldValueAsync<short>(0),
                        PMName = await result.GetFieldValueAsync<string>(1),
                        ClosedItemsCurrentMonth = await result.GetFieldValueAsync<int>(2),
                        ClosedItemsCurrentYear = await result.GetFieldValueAsync<int>(3),
                        ClosedOrdersCurrentMonth = await result.GetFieldValueAsync<int>(4),
                        ClosedOrdersCurrentYear = await result.GetFieldValueAsync<int>(5)
                    });
                }
                res = await GetRecognisedRevenueFYinGBP(res, teamId);
                res = await GetMonthlyPendingRevenueinGBP(res, teamId);
                res = await TotalValueOfAllOpenOrdersFYinGBP(res, teamId);
                res = await GetRecognisedRevenueCurrentMonthinGBP(res, teamId);
                res = await GetMarginPercentageFY(res, teamId);

                var team = await GetTeam(teamId);
                res.Insert(0, new DisplayRevenueTableViewModel()
                {
                    PMId = teamId,
                    PMName = team.Name,
                    TotalValueOfAllOpenJobOrders = res.Sum(x => x.TotalValueOfAllOpenJobOrders),
                    ClosedItemsCurrentMonth = res.Sum(x => x.ClosedItemsCurrentMonth),
                    ClosedItemsCurrentYear = res.Sum(x => x.ClosedItemsCurrentYear),
                    ClosedOrdersCurrentMonth = res.Sum(x => x.ClosedOrdersCurrentMonth),
                    ClosedOrdersCurrentYear = res.Sum(x => x.ClosedOrdersCurrentYear),
                    RecognisedRevenueCurrentYearInGBP = res.Sum(x => x.RecognisedRevenueCurrentYearInGBP),
                    PendingRevenueCurrentMonthInGBP = res.Sum(x => x.PendingRevenueCurrentMonthInGBP),
                    RecognisedRevenueCurrentMonthInGBP = res.Sum(x => x.RecognisedRevenueCurrentMonthInGBP),
                    MarginCurrentYear = res.Average(x => x.MarginCurrentYear)
                });
            }
            return res;
        }
        public async Task<IEnumerable<PMBreakdownViewModel>> GetStatusPerPM(int teamId, string teamName)
        {
            var res = new List<PMBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMName, count(distinct(joborders.id)) as 'OpenJobsCount', count(jobitems.ID) as 'OpenItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'
    ,employeeteams.ID
    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.ID
    inner join contacts on contacts.ID = joborders.ContactID
	inner join Orgs on orgs.ID = Contacts.OrgID

    where EmployeeTeams.ID in (" + teamId + @") and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null
    and Orgs.OrgGroupID not in(18648,18520)

    group by Employees.ID, Employees.FirstName, Employees.Surname, EmployeeTeams.ID
    order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new PMBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<short>(0),
                        PMName = await result.GetFieldValueAsync<string>(1),
                        OpenJobsCount = await result.GetFieldValueAsync<int>(2),
                        OpenItemsCount = await result.GetFieldValueAsync<int>(3),
                        OverdueJobsCount = await result.GetFieldValueAsync<int>(4),
                        OverdueItemsCount = await result.GetFieldValueAsync<int>(5),
                        JobsDueToday = await result.GetFieldValueAsync<int>(6),
                        ItemsDueToday = await result.GetFieldValueAsync<int>(7),
                        TeamId = await result.GetFieldValueAsync<byte>(8)
                    });
                }
            }
            res.Insert(0, new PMBreakdownViewModel()
            {
                Id = teamId,
                PMName = "Total " + teamName,
                OpenJobsCount = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.OpenJobsCount),
                OpenItemsCount = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.OpenItemsCount),
                OverdueJobsCount = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.OverdueJobsCount),
                OverdueItemsCount = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.OverdueItemsCount),
                JobsDueToday = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.JobsDueToday),
                ItemsDueToday = res.Where(x => x.TeamId == teamId).ToList().Sum(x => x.ItemsDueToday),
            });

            return res;
        }
        public async Task<EmployeeTeam> GetTeam(int teamId)
        {
            var res = await teamRepository.All().FirstOrDefaultAsync(x => x.Id == teamId);
            return res;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetMarginPercentageFY(List<DisplayRevenueTableViewModel> list, int teamId)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();

                var query = @"
SELECT EmpID, PMname, CASE WHEN SUM(ISNULL(NetRevenue,0))=0 AND SUM(ISNULL(NetPaymentToSupplier,0))=0 THEN 0 
			     WHEN (SUM(NetRevenue) - SUM(ISNULL(NetPaymentToSupplier,0))) < 0 AND SUM(NetRevenue) < 0 THEN - (SUM(NetRevenue) - SUM(ISNULL(NetPaymentToSupplier,0)))/ NULLIF((SUM(NetRevenue)),0) * 100
				 ELSE ISNULL(((SUM(NetRevenue) - SUM(ISNULL(NetPaymentToSupplier,0))))/ NULLIF((SUM(NetRevenue)),0), -1) * 100 END as NetMarginPercentage
FROM(
SELECT	DISTINCT (Employees.FirstName +' '+ Employees.Surname) as PMname, Employees.ID as EmpID,
			[dbo].funcCurrencyConversion([JobOrdersTable].[ClientCurrencyID], 4, JobItemsTable2.ChargeToClientAfterDiscountSurcharges) AS NetRevenue,
			[dbo].funcCurrencyConversion(JobItemsTable2.[PaymentToSupplierCurrencyID], 4, ISNULL(JobItemsTable2.[PaymentToSupplier],0))  AS NetPaymentToSupplier
		
FROM		[dbo].[JobOrders] as JobOrdersTable 
INNER JOIN	[dbo].[Contacts] as ContactsTable
			ON [JobOrdersTable].[ContactID] = [ContactsTable].[ID]
INNER JOIN	[dbo].[Orgs] as OrgsTable
			ON [ContactsTable].[OrgID] = [OrgsTable].[ID]
INNER JOIN [dbo].[viewCountriesMultilingualInfo] as CountriesView
			ON [OrgsTable].[CountryID] = [CountriesView].[CountryID]
INNER JOIN [dbo].[OrgGroups] AS OrgGroupsTable
			ON [OrgGroupsTable].ID = [OrgsTable].OrgGroupID
INNER JOIN [dbo].[JobItems] as JobItemsTable
			ON JobOrdersTable.ID = JobItemsTable.JobOrderID 
INNER JOIN [dbo].[LanguageServices]
			ON [LanguageServices].[ID] = JobItemsTable.LanguageServiceID
INNER JOIN [dbo].[JobItems] as JobItemsTable2
			ON JobItemsTable2.ID = [JobItemsTable].ID and [LanguageServices].IncludeInMarginCalculations = 1
INNER JOIN Employees 
			ON	Employees.ID = JobOrdersTable.ProjectManagerEmployeeID	 
INNER JOIN EmployeeTeams 
			ON EmployeeTeams.ID = Employees.TeamID
  
 WHERE		OrgGroupsTable.ID not in (72112,72113,72114,72115, 18648,18520)

AND		--(SubmittedDateTime BETWEEN '2022-01-01' and '2022-12-31')
				--JobItemsTable.SupplierCompletedItemDateTime BETWEEN '2022-01-01' and '2022-12-31'
				ISNULL(JobItemsTable.SupplierCompletionDeadline, JobItemsTable.OurCompletionDeadline) BETWEEN '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"'
			
AND		[CountriesView].[LanguageIANAcode] = 'en'	
			
  --		ignore anything deleted
AND		[JobOrdersTable].[DeletedDate] IS NULL
AND		Employees.TerminateDate IS NULL
AND		Employees.TeamID IN (" + teamId + @")
 
 AND		JobItemsTable.DeletedDateTime IS NULL
 ) as table_order_1 GROUP BY PMname, EmpID";

                command.CommandText = query;
                command.CommandTimeout = 120;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var PMId = await result.GetFieldValueAsync<short>(0);

                    if (list.Select(t => t.PMId).ToList().Contains(PMId))
                    {
                        list.FirstOrDefault(x => x.PMId == PMId).MarginCurrentYear = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> TotalValueOfAllOpenOrdersFYinGBP(List<DisplayRevenueTableViewModel> list, int teamId)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Total value of all open orders FY GBP
                    select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
                    (CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].[funcCurrencyConversion](JobOrders.ClientCurrencyID,(4),ISNULL(JobOrders.OverallChargeToClient,0))),0))) as OpenOrdersGBPValue
                    
                    from JobOrders
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
                    
                    where Employees.TeamID in (" + teamId + @") and 
                    joborders.DeletedDate is null  and 
                    Orgs.OrgGroupID not in(18648,18520) and 
                    Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
                    Joborders.OverallCompletedDateTime is null and JobOrders.OverallChargeToClient is not null
                    
                    group by Employees.FirstName, Employees.Surname, Employees.ID
                    order by Employees.FirstName";

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var PMId = await result.GetFieldValueAsync<short>(0);

                    if (list.Select(t => t.PMId).ToList().Contains(PMId))
                    {
                        list.FirstOrDefault(x => x.PMId == PMId).TotalValueOfAllOpenJobOrders = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetRecognisedRevenueCurrentMonthinGBP(List<DisplayRevenueTableViewModel> list, int teamId)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                var query = @"-- Recognised revenue Monthly GBP
select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)),0))) as RecognisedRevenueGBP

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where Employees.TeamID in (" + teamId + @") and
Employees.TeamID not in (39) and --Maroon
Orgs.OrgGroupID not in(18648,18520) and 
(JobItems.SupplierCompletedItemDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"') and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by Employees.FirstName, Employees.Surname, Employees.ID
order by Employees.FirstName";

                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var PMId = await result.GetFieldValueAsync<short>(0);

                    if (list.Select(t => t.PMId).ToList().Contains(PMId))
                    {
                        list.FirstOrDefault(x => x.PMId == PMId).RecognisedRevenueCurrentMonthInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetRecognisedRevenueFYinGBP(List<DisplayRevenueTableViewModel> list, int teamId)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                var query = @"-- Recognised revenue FY GBP
select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)),0))) as RecognisedRevenueGBP

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where Employees.TeamID in (" + teamId + @") and
Orgs.OrgGroupID not in(18648,18520) and 
(JobItems.SupplierCompletedItemDateTime between '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"') and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by Employees.FirstName, Employees.Surname, Employees.ID
order by Employees.FirstName";

                command.CommandText = query;

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var PMId = await result.GetFieldValueAsync<short>(0);

                    if (list.Select(t => t.PMId).ToList().Contains(PMId))
                    {
                        list.FirstOrDefault(x => x.PMId == PMId).RecognisedRevenueCurrentYearInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetMonthlyPendingRevenueinGBP(List<DisplayRevenueTableViewModel> list, int teamId)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();

                var query = @"-- Pending Revenue Current Month GBP
select Employees.ID, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversion([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges)),0))) as PendingRevenue

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where Employees.TeamID in (" + teamId + @") and
Employees.TerminateDate is null and
Orgs.OrgGroupID not in(18648,18520) and 
(ISNULL(JobItems.SupplierCompletionDeadline, JobItems.OurCompletionDeadline) >= CONVERT(datetime, '" + startMonthDate.ToString("yyyy-MM-dd") + @"') AND
ISNULL(JobItems.SupplierCompletionDeadline, JobItems.OurCompletionDeadline) < CONVERT(datetime, '" + endMonthDate.ToString("yyyy-MM-dd") + @"') AND	
JobItems.SupplierCompletedItemDateTime IS NULL) and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by Employees.FirstName, Employees.Surname, Employees.ID
order by Employees.FirstName";

                command.CommandText = query;

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var PMId = await result.GetFieldValueAsync<short>(0);

                    if (list.Select(t => t.PMId).ToList().Contains(PMId))
                    {
                        list.FirstOrDefault(x => x.PMId == PMId).PendingRevenueCurrentMonthInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        public async Task<IEnumerable<HolidaysBreakdowForTeamViewModel>> HolidaysBreakdowForTeam(int teamId)
        {
            var res = new List<HolidaysBreakdowForTeamViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- General Linguistic Services: [Current date] overview
select Employees.ID, (Employees.FirstName+' '+Employees.Surname) as EmpName,
ISNULL(CONVERT(int, SUM(distinct case when 
Convert(date,EmployeesSickness.SicknessStartDateTime) <= Convert(date, getdate()) and
Convert(date,EmployeesSickness.SicknessEndDateTime) >= Convert(date, getdate()) then EmployeesSickness.TotalDays end)), 0) as SickLeaves,
ISNULL(CONVERT(int, SUM(distinct case when
Convert(date, EmployeeHolidayRequests.HolidayStartDateTime) <= Convert(date, getdate()) and 
Convert(date, EmployeeHolidayRequests.HolidayEndDateTime) >= Convert(date, getdate()) then EmployeeHolidayRequests.TotalDays end)), 0) as Holiday
from Employees
left join EmployeeTeams on Employees.TeamID = EmployeeTeams.ID
left join EmployeesSickness on EmployeesSickness.EmployeeID = Employees.ID and EmployeesSickness.DeletedDateTime is null AND Employees.TeamID <> 18
left join EmployeeHolidayRequests on EmployeeHolidayRequests.EmployeeID = Employees.ID and EmployeeHolidayRequests.ApprovedDateTime is not null and EmployeeHolidayRequests.DeletedDateTime is null
Where Employees.TerminateDate is null and EmployeeTeams.ID in (" + teamId + @")
group by Employees.FirstName, Employees.Surname, Employees.ID
order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new HolidaysBreakdowForTeamViewModel()
                    {
                        EmpId = await result.GetFieldValueAsync<short>(0),
                        EmployeeName = await result.GetFieldValueAsync<string>(1),
                        SickLeaves = await result.GetFieldValueAsync<int>(2),
                        Holidays = await result.GetFieldValueAsync<int>(3)

                    });
                }

                res.Insert(0, new HolidaysBreakdowForTeamViewModel()
                {
                    EmpId = teamId,
                    EmployeeName = "Total",
                    Holidays = res.Sum(x => x.Holidays),
                    SickLeaves = res.Sum(x => x.SickLeaves)
                });
            }
            return res;
        }
        public async Task<IEnumerable<HolidaysBreakdowForTeamViewModel>> HolidaysBreakdowNextTwoWeeksForTeam(int teamId)
        {
            var res = new List<HolidaysBreakdowForTeamViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- General Linguistic Services: next two weeks overview
select Employees.ID, (Employees.FirstName+' '+Employees.Surname) as EmpName,
ISNULL(CONVERT(int, SUM(distinct case when 
Convert(date,EmployeesSickness.SicknessStartDateTime) >= Convert(date, getdate()) and
Convert(date,EmployeesSickness.SicknessEndDateTime) <= Convert(date, DATEADD(week, 2, getdate())) then EmployeesSickness.TotalDays end)), 0) as SickLeaves,
ISNULL(CONVERT(int, SUM(distinct case when
Convert(date, EmployeeHolidayRequests.HolidayStartDateTime) >= Convert(date, getdate()) and 
Convert(date, EmployeeHolidayRequests.HolidayEndDateTime) <= Convert(date, DATEADD(week, 2, getdate())) then EmployeeHolidayRequests.TotalDays end)), 0) as Holiday
from Employees
left join EmployeeTeams on Employees.TeamID = EmployeeTeams.ID
left join EmployeesSickness on EmployeesSickness.EmployeeID = Employees.ID and EmployeesSickness.DeletedDateTime is null AND Employees.TeamID <> 18
left join EmployeeHolidayRequests on EmployeeHolidayRequests.EmployeeID = Employees.ID and EmployeeHolidayRequests.ApprovedDateTime is not null and EmployeeHolidayRequests.DeletedDateTime is null
Where Employees.TerminateDate is null and EmployeeTeams.ID in (" + teamId + @")
group by Employees.FirstName, Employees.Surname, Employees.ID
order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new HolidaysBreakdowForTeamViewModel()
                    {
                        EmpId = await result.GetFieldValueAsync<short>(0),
                        EmployeeName = await result.GetFieldValueAsync<string>(1),
                        SickLeaves = await result.GetFieldValueAsync<int>(2),
                        Holidays = await result.GetFieldValueAsync<int>(3)

                    });
                }

                res.Insert(0, new HolidaysBreakdowForTeamViewModel()
                {
                    EmpId = teamId,
                    EmployeeName = "Total",
                    Holidays = res.Sum(x => x.Holidays),
                    SickLeaves = res.Sum(x => x.SickLeaves)
                });
            }
            return res;
        }
        private async Task<List<JobOrdersDataTableTMViewModel>> BindPropertiesForDataTable(System.Data.Common.DbDataReader result, List<JobOrdersDataTableTMViewModel> res)
        {
            while (await result.ReadAsync())
            {
                res.Add(new JobOrdersDataTableTMViewModel()
                {
                    PMId = await result.GetFieldValueAsync<short>(0),
                    PMName = await result.GetFieldValueAsync<string>(1),
                    JobOrderId = await result.GetFieldValueAsync<int>(2),
                    JobOrderName = await result.GetFieldValueAsync<string>(3),
                    DeliveryDeadline = await result.GetFieldValueAsync<string>(4),
                    OrderStatus = await result.GetFieldValueAsync<string>(5),
                    OrgId = await result.GetFieldValueAsync<int>(6),
                    OrgName = await result.GetFieldValueAsync<string>(7),
                    OrgGroupId = await result.GetFieldValueAsync<int>(8),
                    OrgGroupName = await result.GetFieldValueAsync<string>(9),
                    OriginatedFromEnquiryId = await result.GetFieldValueAsync<int>(10),
                    OverallChargeToClient = await result.GetFieldValueAsync<decimal>(11),
                    MarginPercentage = await result.GetFieldValueAsync<decimal>(12),
                    Currency = await result.GetFieldValueAsync<string>(13)
                });
            }
            return res;
        }
    }
}
