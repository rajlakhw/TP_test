using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.TMS.Ops_mgmt;

namespace Services
{
    public class OpsDashboardService : IOpsDashboardService
    {
        public OpsDashboardService(IRepository<EmployeeDepartment> departmentRepository, IRepository<EmployeeTeam> teamRepository)
        {
            this.departmentRepository = departmentRepository;
            this.teamRepository = teamRepository;
        }

        private readonly IRepository<EmployeeTeam> teamRepository;
        private readonly IRepository<EmployeeDepartment> departmentRepository;

        private static readonly DateTime currentYearStartDate = new DateTime(DateTime.Today.Year, 1, 1);
        private static readonly DateTime currentYearEndDate = new DateTime(DateTime.Today.Year, 12, DateTime.DaysInMonth(DateTime.Today.Year, 12));

        private static readonly string JobOrdersDataTableCommandText = @"select distinct EmployeeTeams.ID, EmployeeTeams.Name, JobOrders.ID, JobOrders.JobName, (Employees.FirstName + ' ' + Employees.Surname) as ProjectManager, FORMAT (Joborders.OverallDeliveryDeadline, 'dd MMM yyyy HH:mm'),
                    (Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
                    when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
                    when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'
                    end) as OrderStatus, Orgs.ID, Orgs.OrgName, Orgs.OrgGroupID, OrgGroups.Name, OriginatedFromEnquiryID, 
					ISNULL(OverallChargeToClient, 0), AnticipatedGrossMarginPercentage, ProjectManagerEmployeeID as PmId,
                    (SELECT [Currencies].[Prefix] FROM [dbo].[Currencies] WHERE [Currencies].[ID] = JobOrders.ClientCurrencyID) as Currency

                    from JobOrders

                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
                    inner join OrgGroups on orgs.OrgGroupID = OrgGroups.ID";
        private static readonly string JobOrderDataTableCommandTextForTBAEmployee = @"select distinct ProjectManagerEmployeeID, (Employees.FirstName + ' ' + Employees.Surname) as PMname, JobOrders.ID, JobOrders.JobName, (Employees.FirstName + ' ' + Employees.Surname) as ProjectManager, FORMAT (Joborders.OverallDeliveryDeadline, 'dd MMM yyyy HH:mm'),
                    (Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
                    when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
                    when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'
                    end) as OrderStatus, Orgs.ID, Orgs.OrgName, Orgs.OrgGroupID, OrgGroups.Name, OriginatedFromEnquiryID, 
					ISNULL(OverallChargeToClient, 0), AnticipatedGrossMarginPercentage,
                    (SELECT [Currencies].[Prefix] FROM [dbo].[Currencies] WHERE [Currencies].[ID] = JobOrders.ClientCurrencyID) as Currency
                    
                    from JobOrders
                    
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
					inner join OrgGroups on orgs.OrgGroupID = OrgGroups.ID";
        private static readonly string ItemsCountColumnsQuerySection = @"COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.SupplierSentWorkDateTime is null and 
JobItems.SupplierCompletedItemDateTime is null THEN JobItems.id END) as 'ItemsNotSentToSupplier',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID IN(1,17) and 
JobItems.SupplierAcceptedWorkDateTime is not null and
JobItems.SupplierCompletedItemDateTime is null THEN JobItems.id END) as 'ItemsInTranslation',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID = 7 and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInProofreading',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID = 21 and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInClientReview',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.LanguageServiceID IN(4, 35, 38, 52, 53, 54, 57, 69, 62, 23) and 
JobItems.WeCompletedItemDateTime is null and
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierAcceptedWorkDateTime is not null THEN JobItems.id END) as 'ItemsInProduction',

COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and 
JobItems.WeCompletedItemDateTime is null and 
JobItems.SupplierSentWorkDateTime < dbo.funcGetCurrentUKTime() and
JobItems.SupplierCompletedItemDateTime is null and
JobItems.SupplierCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'Overdue Items'";

        public async Task<IEnumerable<GroupBreakdownViewModel>> GetAllGroupsDataForOpsMgmt(int departmentId)
        {
            var res = new List<GroupBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select OrgGroups.ID, OrgGroups.Name, count(distinct(joborders.id)) as 'OpenJobsCount', count(jobitems.ID) as 'OpenItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'
    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
    inner join contacts on contacts.ID = joborders.ContactID
    inner join Orgs on Contacts.OrgID = Orgs.ID
    inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
    where joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520) and EmployeeTeams.DepartmentID = " + departmentId + @"
    group by OrgGroups.name, OrgGroups.ID
    order by OrgGroups.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new GroupBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<int>(0),
                        OrgGroupName = await result.GetFieldValueAsync<string>(1),
                        OpenJobsCount = await result.GetFieldValueAsync<int>(2),
                        OpenItemsCount = await result.GetFieldValueAsync<int>(3),
                        OverdueJobsCount = await result.GetFieldValueAsync<int>(4),
                        OverdueItemsCount = await result.GetFieldValueAsync<int>(5),
                        JobsDueToday = await result.GetFieldValueAsync<int>(6),
                        ItemsDueToday = await result.GetFieldValueAsync<int>(7)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<GroupItemsBreakdownViewModel>> GetAllItemsByGroupsForOpsMgmt(int departmentId)
        {
            var res = new List<GroupItemsBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select OrgGroups.ID, OrgGroups.Name,
" + ItemsCountColumnsQuerySection + @"
from JobOrders
left outer join JobItems on JobItems.JobOrderID = joborders.ID
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on Contacts.OrgID = Orgs.ID
inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
where joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520) and EmployeeTeams.DepartmentID = " + departmentId + @"
group by OrgGroups.name, OrgGroups.ID
order by OrgGroups.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new GroupItemsBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<int>(0),
                        GroupName = await result.GetFieldValueAsync<string>(1),
                        ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(2),
                        ItemsInTranslation = await result.GetFieldValueAsync<int>(3),
                        ItemsInProofreading = await result.GetFieldValueAsync<int>(4),
                        ItemsInClientReview = await result.GetFieldValueAsync<int>(5),
                        ItemsInProduction = await result.GetFieldValueAsync<int>(6),
                        OverdueItems = await result.GetFieldValueAsync<int>(7)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<TeamItemsBreakdownViewModel>> GetAllItemsByTeamsForOpsMgmt(int departmentId)
        {
            var res = new List<TeamItemsBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select EmployeeTeams.ID, EmployeeTeams.Name,
    " + ItemsCountColumnsQuerySection + @"
    ,employeeteams.departmentid
    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
    inner join contacts on contacts.ID = joborders.ContactID
    inner join Orgs on orgs.ID = Contacts.OrgID

    where EmployeeTeams.DepartmentID in (" + departmentId + @") and EmployeeTeams.ID not in (39) --Maroon
    and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520)
    group by EmployeeTeams.Name, EmployeeTeams.ID, DepartmentID
    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new TeamItemsBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<byte>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(2),
                        ItemsInTranslation = await result.GetFieldValueAsync<int>(3),
                        ItemsInProofreading = await result.GetFieldValueAsync<int>(4),
                        ItemsInClientReview = await result.GetFieldValueAsync<int>(5),
                        ItemsInProduction = await result.GetFieldValueAsync<int>(6),
                        OverdueAndRejectedItems = await result.GetFieldValueAsync<int>(7),
                        DepartmentId = await result.GetFieldValueAsync<byte>(8)
                    });
                }
            }
            res.Insert(0, new TeamItemsBreakdownViewModel()
            {
                Id = departmentId,
                TeamName = departmentId == 9 ? "GLS Department" : "TP Department",
                ItemsNotSentToSupplier = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsNotSentToSupplier),
                ItemsInTranslation = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsInTranslation),
                ItemsInProofreading = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsInProofreading),
                ItemsInClientReview = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsInClientReview),
                ItemsInProduction = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsInProduction),
                OverdueAndRejectedItems = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.OverdueAndRejectedItems),
            });
            return res;
        }
        public async Task<IEnumerable<TeamBreakdownViewModel>> GetAllTeamsDataForOpsMgmt(int departmentId)
        {
            var res = new List<TeamBreakdownViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select employeeteams.ID, employeeteams.name as TeamName, count(distinct(joborders.id)) as 'OpenJobsCount', count(jobitems.ID) as 'OpenItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'
    ,employeeteams.departmentid
    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.ID
    inner join contacts on contacts.ID = joborders.ContactID
    inner join Orgs on orgs.ID = Contacts.OrgID

    where EmployeeTeams.DepartmentID in (" + departmentId + @") and EmployeeTeams.ID not in (39) --Maroon
    and Employees.ID not in (435)
    and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null
    and Orgs.OrgGroupID not in(18520)
    group by employeeteams.name, EmployeeTeams.ID, DepartmentID
    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result
                while (await result.ReadAsync())
                {
                    res.Add(new TeamBreakdownViewModel()
                    {
                        Id = await result.GetFieldValueAsync<byte>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        OpenJobsCount = await result.GetFieldValueAsync<int>(2),
                        OpenItemsCount = await result.GetFieldValueAsync<int>(3),
                        OverdueJobsCount = await result.GetFieldValueAsync<int>(4),
                        OverdueItemsCount = await result.GetFieldValueAsync<int>(5),
                        JobsDueToday = await result.GetFieldValueAsync<int>(6),
                        ItemsDueToday = await result.GetFieldValueAsync<int>(7),
                        DepartmentId = await result.GetFieldValueAsync<byte>(8)
                    });
                }
            }
            res.Insert(0, new TeamBreakdownViewModel()
            {
                Id = departmentId,
                TeamName = departmentId == 9 ? "GLS Department" : "TP Department",
                OpenJobsCount = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.OpenJobsCount),
                OpenItemsCount = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.OpenItemsCount),
                OverdueJobsCount = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.OverdueJobsCount),
                OverdueItemsCount = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.OverdueItemsCount),
                JobsDueToday = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.JobsDueToday),
                ItemsDueToday = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ItemsDueToday),
            });

            return res;
        }
        public async Task<decimal> GetChartInvoicedRevenueForOpsMgmt(int departmentId)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();
            //var res = new RevenueFiguresForChart();
            decimal revenue = 0;

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string query = @"SELECT 
ISNULL((CONVERT(DECIMAL(38,2), SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)))), 0) as TotalGBPInvoiced
  
FROM JobOrders
inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
inner join Contacts on Contacts.ID = JobOrders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where ClientInvoices.InvoiceDate between '2022-02-01' and '2022-03-01'
and JobOrders.DeletedDate is null
and EmployeeTeams.DepartmentID in (9)
and ClientInvoices.DeletedDateTime is null
and Orgs.OrgGroupID not in(18520)
and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
and ClientInvoices.IsFinalised = 1";

                var a = @"-- INVOICED REVENUE MONTHLY
                    Select (SELECT 
                          ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.AnticipatedFinalValueAmount)))), 0) as TotalGBPInvoiced
                        
                      FROM JobOrders
                      inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                      inner join Contacts on Contacts.ID = JobOrders.ContactID
                      inner join Orgs on orgs.ID = Contacts.OrgID
                      inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                      inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                      
                      where ClientInvoices.InvoiceDate between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                      and JobOrders.DeletedDate is null
                      and EmployeeTeams.DepartmentID in (" + departmentId + @")
                      and ClientInvoices.DeletedDateTime is null
                      and Orgs.OrgGroupID not in(18520)
                      and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                      and ClientInvoices.IsFinalised = 1) as InvoicedRevenue,
                    
                      -- PENDING REVENUE MONTHLY
                      (SELECT 
                          ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.OverallChargeToClient)))), 0) as PendingGBPRevenue
                        
                      FROM JobOrders
                      left outer join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                      inner join Contacts on Contacts.ID = JobOrders.ContactID
                      inner join Orgs on orgs.ID = Contacts.OrgID
                      inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                      inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                      
                      where JobOrders.SubmittedDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                      and JobOrders.DeletedDate is null
                      and EmployeeTeams.DepartmentID in (" + departmentId + @")
                      and ClientInvoices.DeletedDateTime is null
                      and Orgs.OrgGroupID not in(18520)
                      and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and JobOrders.OverallChargeToClient is not null
                      and ClientInvoices.ExportedToSageDateTime is null) as Pending,
                    
                      -- Recognised revenue monthly
                      (SELECT 
                          ISNULL((CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(JobOrders.ClientCurrencyID,4,JobOrders.AnticipatedFinalValueAmount)))), 0) as RecognisedRevenue
                        
                      FROM JobOrders
                      inner join ClientInvoices on JobOrders.ClientInvoiceID = ClientInvoices.ID
                      inner join Contacts on Contacts.ID = JobOrders.ContactID
                      inner join Orgs on orgs.ID = Contacts.OrgID
                      inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.ID
                      inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                      
                      where ClientInvoices.ExportedToSageDateTime is not null
                      and ClientInvoices.ExportedToSageDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"'
                      and JobOrders.DeletedDate is null
                      and EmployeeTeams.DepartmentID in (" + departmentId + @")
                      and ClientInvoices.DeletedDateTime is null
                      and Orgs.OrgGroupID not in(18520)
                      and Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                      and Joborders.OverallCompletedDateTime is not null and JobOrders.OverallChargeToClient is not null) as Recognised";

                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    revenue = await result.GetFieldValueAsync<decimal>(0);
                    //res.InvoicedRevenueMonthly = await result.GetFieldValueAsync<decimal>(0);
                    //res.PendingRevenueMonthly = await result.GetFieldValueAsync<decimal>(1);
                    //res.RecognisedRevenue = await result.GetFieldValueAsync<decimal>(2);
                }
            }
            return revenue;
        }
        public async Task<IEnumerable<DisplayRevenueTableViewModel>> GetRevenueForOpsMgmt(int departmentId)
        {
            var res = new List<DisplayRevenueTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- REVENUE TABLE Arian Tweaks
                    select EmployeeTeams.ID, EmployeeTeams.Name,
                    --CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(Joborders.ClientCurrencyID, 4, JobOrders.OverallChargeToClient)), 2) as 'TotalValueOfOrdersInGBP',
                    --(Case when joborders.OverallCompletedDateTime is null and joborders.DeletedDate is null then CONVERT(DECIMAL(38,2), SUM(dbo.funcCurrencyConversion(Joborders.ClientCurrencyID, 4, JobOrders.OverallChargeToClient)), 2) end) as 'TotalValueOfOpenOrdersInGBP',
                    
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is not null and (Month(JobItems.WeCompletedItemDateTime) <= Month((getdate())) AND Month(JobItems.WeCompletedItemDateTime) >= Month((getdate()))) AND (YEAR(JobItems.WeCompletedItemDateTime) <= YEAR((getdate())) AND YEAR(JobItems.WeCompletedItemDateTime) >= YEAR((getdate()))) THEN JobItems.id END) as 'ClosedItemsCurentMonth',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is not null and (YEAR(JobItems.WeCompletedItemDateTime) <= YEAR((getdate())) AND YEAR(JobItems.WeCompletedItemDateTime) >= YEAR((getdate()))) THEN JobItems.id END) as 'ClosedItemsCurentYear',
                    COUNT(DISTINCT CASE WHEN JobOrders.DeletedDate is null and JobOrders.OverallCompletedDateTime is not null and (Month(JobOrders.OverallCompletedDateTime) <= Month((getdate())) AND Month(JobOrders.OverallCompletedDateTime) >= Month((getdate()))) AND (YEAR(JobOrders.OverallCompletedDateTime) <= YEAR((getdate())) AND YEAR(JobOrders.OverallCompletedDateTime) >= YEAR((getdate()))) THEN JobOrders.id END) as 'ClosedOrdersCurentMonth',
                    COUNT(DISTINCT CASE WHEN JobOrders.DeletedDate is null and JobOrders.OverallCompletedDateTime is not null and (YEAR(JobOrders.OverallCompletedDateTime) <= YEAR((getdate())) AND YEAR(JobOrders.OverallCompletedDateTime) >= YEAR((getdate()))) THEN JobOrders.id END) as 'ClosedOrdersCurentYear'
                    , DepartmentID

                    from JobOrders
                    left outer join JobItems on JobItems.JobOrderID = joborders.ID
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
                    
                    where EmployeeTeams.DepartmentID in (" + departmentId + @") and EmployeeTeams.ID not in (39) and --Maroon
                    joborders.DeletedDate is null  and 
                    Orgs.OrgGroupID not in(18520) and 
                    (JobOrders.SubmittedDateTime between '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"') and
                    Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0
                    
                    group by EmployeeTeams.Name, EmployeeTeams.ID, DepartmentID
                    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new DisplayRevenueTableViewModel()
                    {
                        Id = await result.GetFieldValueAsync<byte>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        ClosedItemsCurrentMonth = await result.GetFieldValueAsync<int>(2),
                        ClosedItemsCurrentYear = await result.GetFieldValueAsync<int>(3),
                        ClosedOrdersCurrentMonth = await result.GetFieldValueAsync<int>(4),
                        ClosedOrdersCurrentYear = await result.GetFieldValueAsync<int>(5),
                        DepartmentId = await result.GetFieldValueAsync<byte>(6)
                    });
                }
                res = await GetRecognisedRevenueFYinGBP(res);
                res = await GetMonthlyPendingRevenueinGBP(res);
                res = await TotalValueOfAllOpenOrdersinGBP(res);
                res = await GetRecognisedRevenueCurrentMonthinGBP(res);
                res = await GetMarginPercentageFY(res);

                var totalClientChargeForDepartment = res.Where(x => x.DepartmentId == departmentId).Select(x => x.ClientChargeForMargin).DefaultIfEmpty(0).Sum();
                var supplierPaymentForDepartment = res.Where(x => x.DepartmentId == departmentId).Select(x => x.PaymentToSupplierForMargin).DefaultIfEmpty(0).Sum();

                res.Insert(0, new DisplayRevenueTableViewModel()
                {
                    Id = departmentId,
                    TeamName = departmentId == 9 ? "GLS Department" : "TP Department",
                    TotalValueOfAllOpenJobOrders = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.TotalValueOfAllOpenJobOrders),
                    ClosedItemsCurrentMonth = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ClosedItemsCurrentMonth),
                    ClosedItemsCurrentYear = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ClosedItemsCurrentYear),
                    ClosedOrdersCurrentMonth = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ClosedOrdersCurrentMonth),
                    ClosedOrdersCurrentYear = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.ClosedOrdersCurrentYear),
                    RecognisedRevenueCurrentYearInGBP = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.RecognisedRevenueCurrentYearInGBP),
                    PendingRevenueCurrentMonthInGBP = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.PendingRevenueCurrentMonthInGBP),
                    RecognisedRevenueCurrentMonthInGBP = res.Where(x => x.DepartmentId == departmentId).ToList().Sum(x => x.RecognisedRevenueCurrentMonthInGBP),
                    MarginCurrentYear = (1 - (supplierPaymentForDepartment / totalClientChargeForDepartment)) * 100
                });
            }
            return res;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetRecognisedRevenueFYinGBP(List<DisplayRevenueTableViewModel> list)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                var query = @"-- Recognised revenue FY GBP
select EmployeeTeams.ID, EmployeeTeams.Name,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)),0))) as RecognisedRevenueGBP

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where EmployeeTeams.DepartmentID in (9,10) and 
EmployeeTeams.ID not in (39) and --Maroon
Orgs.OrgGroupID not in(18520) and 
(JobItems.SupplierCompletedItemDateTime between '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"') and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by EmployeeTeams.Name, EmployeeTeams.ID
order by EmployeeTeams.Name";

                command.CommandText = query;

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var teamId = await result.GetFieldValueAsync<byte>(0);

                    if (list.Select(t => t.Id).ToList().Contains(teamId))
                    {
                        list.FirstOrDefault(x => x.Id == teamId).RecognisedRevenueCurrentYearInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetRecognisedRevenueCurrentMonthinGBP(List<DisplayRevenueTableViewModel> list)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                var query = @"-- Recognised revenue FY GBP
select EmployeeTeams.ID, EmployeeTeams.Name,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversionFromRateInForceAtSpecificDate([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges, JobItems.SupplierCompletedItemDateTime)),0))) as RecognisedRevenueGBP

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where EmployeeTeams.DepartmentID in (9,10) and 
EmployeeTeams.ID not in (39) and --Maroon
Orgs.OrgGroupID not in(18520) and 
(JobItems.SupplierCompletedItemDateTime between '" + startMonthDate.ToString("yyyy-MM-dd") + "' and '" + endMonthDate.ToString("yyyy-MM-dd") + @"') and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by EmployeeTeams.Name, EmployeeTeams.ID
order by EmployeeTeams.Name";

                command.CommandText = query;

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var teamId = await result.GetFieldValueAsync<byte>(0);

                    if (list.Select(t => t.Id).ToList().Contains(teamId))
                    {
                        list.FirstOrDefault(x => x.Id == teamId).RecognisedRevenueCurrentMonthInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetMonthlyPendingRevenueinGBP(List<DisplayRevenueTableViewModel> list)
        {
            var (startMonthDate, endMonthDate) = GeneralUtils.GetStartEndDatesOfCurrentMonth();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                
                var query = @"-- Pending Revenue Current Month GBP
select EmployeeTeams.ID, EmployeeTeams.Name,
(CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].funcCurrencyConversion([JobOrders].[ClientCurrencyID], 4, JobItems.ChargeToClientAfterDiscountSurcharges)),0))) as PendingRevenue

from JobOrders
inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join JobItems on JobOrders.ID = JobItems.JobOrderID

where EmployeeTeams.DepartmentID in (9) and 
EmployeeTeams.ID not in (39) and --Maroon
Orgs.OrgGroupID not in(18520) and 
(ISNULL(JobItems.SupplierCompletionDeadline, JobItems.OurCompletionDeadline) >= CONVERT(datetime, '" + startMonthDate.ToString("yyyy-MM-dd") + @"') AND
ISNULL(JobItems.SupplierCompletionDeadline, JobItems.OurCompletionDeadline) < CONVERT(datetime, '" + endMonthDate.ToString("yyyy-MM-dd") + @"') AND	
JobItems.SupplierCompletedItemDateTime IS NULL) and
Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
joborders.DeletedDate is null  and 
JobItems.DeletedDateTime is null

group by EmployeeTeams.Name, EmployeeTeams.ID
order by EmployeeTeams.Name";

                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var teamId = await result.GetFieldValueAsync<byte>(0);

                    if (list.Select(t => t.Id).ToList().Contains(teamId))
                    {
                        list.FirstOrDefault(x => x.Id == teamId).PendingRevenueCurrentMonthInGBP = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> TotalValueOfAllOpenOrdersinGBP(List<DisplayRevenueTableViewModel> list)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Total value of all open orders FY GBP
                    select EmployeeTeams.ID, EmployeeTeams.Name,
                    (CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].[funcCurrencyConversion](JobOrders.ClientCurrencyID,(4),ISNULL(JobOrders.OverallChargeToClient,0))),0))) as OpenOrdersGBPValue
                    
                    from JobOrders
                    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.id
                    inner join contacts on contacts.ID = joborders.ContactID
                    inner join Orgs on orgs.ID = Contacts.OrgID
                    
                    where EmployeeTeams.DepartmentID in (9,10) and EmployeeTeams.ID not in (39) and --Maroon
                    joborders.DeletedDate is null  and 
                    Orgs.OrgGroupID not in(18520) and 
                    Joborders.IsATrialProject = 0 and Joborders.IsActuallyOnlyAQuote = 0 and
                    Joborders.OverallCompletedDateTime is null and JobOrders.OverallChargeToClient is not null
                    
                    group by EmployeeTeams.Name, EmployeeTeams.ID
                    order by EmployeeTeams.Name";

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var teamId = await result.GetFieldValueAsync<byte>(0);

                    if (list.Select(t => t.Id).ToList().Contains(teamId))
                    {
                        list.FirstOrDefault(x => x.Id == teamId).TotalValueOfAllOpenJobOrders = await result.GetFieldValueAsync<decimal>(2);
                    }
                }
            }
            return list;
        }
        private async Task<List<DisplayRevenueTableViewModel>> GetMarginPercentageFY(List<DisplayRevenueTableViewModel> list)
        {
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();

                var query = @"SELECT TeamId, TeamName, CASE WHEN SUM(ClientChargeInGBP)=0 AND SUM(PaymentToSupplierInGBP)=0 THEN 0
		 WHEN SUM(ClientChargeInGBP)=0 AND SUM(PaymentToSupplierInGBP)!=0 THEN -100
		ELSE ISNULL(((SUM(ClientChargeInGBP) - SUM(PaymentToSupplierInGBP)) 
			/ (SUM(ClientChargeInGBP))) * 100, 0) END as GrossMarginPercentage, 
			ISNULL(SUM(ClientChargeInGBP),0) as ClientCharge, ISNULL(SUM(PaymentToSupplierInGBP),0) as SupplierPayment
FROM(
SELECT	DISTINCT JobOrdersTable.ID, EmployeeTeams.ID as TeamId, EmployeeTeams.Name as TeamName,
			[dbo].funcCurrencyConversion([JobOrdersTable].[ClientCurrencyID], 4, JobItemsTable.ChargeToClientAfterDiscountSurcharges)AS ClientChargeInGBP,
			[dbo].funcCurrencyConversion(JobItemsTable.[PaymentToSupplierCurrencyID], 4, ISNULL(JobItemsTable.[PaymentToSupplier],0)) AS PaymentToSupplierInGBP
		
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
LEFT JOIN [dbo].[JobItems] as JobItemsTable2
			ON JobItemsTable2.ID = [JobItemsTable].ID and [LanguageServices].IncludeInMarginCalculations = 1
INNER JOIN Employees 
			ON	Employees.ID = JobOrdersTable.ProjectManagerEmployeeID	 
INNER JOIN EmployeeTeams 
			ON EmployeeTeams.ID = Employees.TeamID
 
WHERE	OrgGroupsTable.ID not in (72112,72113,72114,72115, 18520)

AND		JobItemsTable.SupplierCompletedItemDateTime BETWEEN '" + currentYearStartDate.ToString("yyyy-MM-dd") + "' and '" + currentYearEndDate.ToString("yyyy-MM-dd") + @"'
			
AND		[CountriesView].[LanguageIANAcode] = 'en'

 --		ignore anything deleted
AND		[JobOrdersTable].[DeletedDate] IS NULL
AND		JobItemsTable.DeletedDateTime IS NULL
AND		Employees.TerminateDate IS NULL
AND		EmployeeTeams.DepartmentID IN (9, 10) and EmployeeTeams.ID not in (39) --Maroon

) as table_order_1 GROUP BY TeamID, TeamName";

                command.CommandText = query;
                command.CommandTimeout = 120;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var teamId = await result.GetFieldValueAsync<byte>(0);

                    if (list.Select(t => t.Id).ToList().Contains(teamId))
                    {
                        var row = list.FirstOrDefault(x => x.Id == teamId);
                        row.MarginCurrentYear = await result.GetFieldValueAsync<decimal>(2);
                        row.ClientChargeForMargin = await result.GetFieldValueAsync<decimal>(3);
                        row.PaymentToSupplierForMargin = await result.GetFieldValueAsync<decimal>(4);
                    }
                }
            }
            return list;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetOpenJobOrdersDataForOpsMgmt(int teamId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Open jobs
                    " + JobOrdersDataTableCommandText + @"
                    where Employees.TeamID = " + teamId.ToString() + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetOverdueJobOrdersDataForOpsMgmt(int teamId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Overdue jobs
                    " + JobOrdersDataTableCommandText + @"
                    where Employees.TeamID = " + teamId.ToString() + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime()";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetDueTodayJobOrdersDataForOpsMgmt(int teamId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Due today jobs
                    " + JobOrdersDataTableCommandText + @"
                    where Employees.TeamID = " + teamId.ToString() + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetOpenJobOrdersDataByDepartmentForOpsMgmt(int departmentId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Open jobs by department
                    " + JobOrdersDataTableCommandText + @"
                    where EmployeeTeams.DepartmentID in (" + departmentId + @") and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
                    
                    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetOverdueJobOrdersDataByDepartmentForOpsMgmt(int departmentId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Overdue jobs by department
                    " + JobOrdersDataTableCommandText + @"
                    where EmployeeTeams.DepartmentID in (" + departmentId + @") and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
                    and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime()
                    
                    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetDueTodayJobOrdersDataByDepartmentForOpsMgmt(int departmentId)
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Due today jobs by department
                    " + JobOrdersDataTableCommandText + @"
                    where EmployeeTeams.DepartmentID in (" + departmentId + @") and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
                    and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)

                    order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<EmployeeDepartment> GetDepartmentFromTeamId(int teamId)
        {
            var res = await teamRepository.All().Where(t => t.Id == teamId).Join(departmentRepository.All(), t => t.DepartmentId, d => d.Id, (t, d) => d).FirstOrDefaultAsync();
            return res;
        }
        public async Task<IEnumerable<PeopleHolidayHeadcountViewModel>> GetPeopleDataForTodayByDepartmentForOpsMgmt(int departmentId)
        {
            var res = new List<PeopleHolidayHeadcountViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- General Linguistic Services: [Current date] overview
select EmployeeTeams.ID, EmployeeTeams.Name, 
COUNT(distinct case when Employees.IsTeamManager = 1 then Employees.ID end) as CountTMs,
COUNT(distinct case when Employees.IsTeamManager <> 1 then Employees.ID end) as CountPMs,
COUNT(distinct case when 
Convert(date,EmployeesSickness.SicknessStartDateTime) <= Convert(date, getdate()) and
Convert(date,EmployeesSickness.SicknessEndDateTime) >= Convert(date, getdate()) then EmployeesSickness.EmployeeID end) as SickLeaves,
COUNT(distinct case when 
Convert(date, EmployeeHolidayRequests.HolidayStartDateTime) <= Convert(date, getdate()) and 
Convert(date, EmployeeHolidayRequests.HolidayEndDateTime) >= Convert(date, getdate()) then EmployeeHolidayRequests.EmployeeID end) as Holiday
-- Available = Total - Holidays - Sick leaves
from Employees
left join EmployeeTeams on Employees.TeamID = EmployeeTeams.ID
left join EmployeesSickness on EmployeesSickness.EmployeeID = Employees.ID and EmployeesSickness.DeletedDateTime is null AND Employees.TeamID <> 18
left join EmployeeHolidayRequests on EmployeeHolidayRequests.EmployeeID = Employees.ID and EmployeeHolidayRequests.ApprovedDateTime is not null and EmployeeHolidayRequests.DeletedDateTime is null
Where Employees.TerminateDate is null and EmployeeTeams.DepartmentID in (" + departmentId + @")
group by EmployeeTeams.Name, EmployeeTeams.ID
order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var row = new PeopleHolidayHeadcountViewModel();

                    row.Id = await result.GetFieldValueAsync<byte>(0);
                    row.TeamName = await result.GetFieldValueAsync<string>(1);
                    row.TMsCount = await result.GetFieldValueAsync<int>(2);
                    row.PMsCount = await result.GetFieldValueAsync<int>(3);
                    row.SickLeaves = await result.GetFieldValueAsync<int>(4);
                    row.Holiday = await result.GetFieldValueAsync<int>(5);
                    row.Available = row.TMsCount + row.PMsCount - row.SickLeaves - row.Holiday;

                    res.Add(row);
                }

                res.Insert(0, new PeopleHolidayHeadcountViewModel()
                {
                    Id = departmentId,
                    TeamName = departmentId == 9 ? "GLS" : "TP",
                    Holiday = res.Sum(x => x.Holiday),
                    TMsCount = res.Sum(x => x.TMsCount),
                    PMsCount = res.Sum(x => x.PMsCount),
                    SickLeaves = res.Sum(x => x.SickLeaves),
                    Available = res.Sum(x => x.Available)
                }) ;
            }
            return res;
        }
        public async Task<IEnumerable<PeopleHolidayHeadcountViewModel>> GetPeopleDataForTwoWeeksByDepartmentForOpsMgmt(int departmentId)
        {
            var res = new List<PeopleHolidayHeadcountViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- General Linguistic Services: Upcoming 2 weeks overview
select EmployeeTeams.ID, EmployeeTeams.Name, 
COUNT(distinct case when Employees.IsTeamManager = 1 then Employees.ID end) as CountTMs,
COUNT(distinct case when Employees.IsTeamManager <> 1 then Employees.ID end) as CountPMs,
COUNT(distinct case when 
Convert(date,EmployeesSickness.SicknessStartDateTime) >= Convert(date, getdate()) and
Convert(date,EmployeesSickness.SicknessEndDateTime) <= Convert(date, DATEADD(week, 2, getdate())) then EmployeesSickness.EmployeeID end) as SickLeaves,
COUNT(distinct case when 
Convert(date, EmployeeHolidayRequests.HolidayStartDateTime) >= Convert(date, getdate()) and 
Convert(date, EmployeeHolidayRequests.HolidayEndDateTime) <= Convert(date, DATEADD(week, 2, getdate())) then EmployeeHolidayRequests.EmployeeID end) as Holiday
-- Available = Total - Holidays - Sick leaves
from Employees
left join EmployeeTeams on Employees.TeamID = EmployeeTeams.ID
left join EmployeesSickness on EmployeesSickness.EmployeeID = Employees.ID and EmployeesSickness.DeletedDateTime is null AND Employees.TeamID <> 18
left join EmployeeHolidayRequests on EmployeeHolidayRequests.EmployeeID = Employees.ID and EmployeeHolidayRequests.ApprovedDateTime is not null and EmployeeHolidayRequests.DeletedDateTime is null
Where Employees.TerminateDate is null and EmployeeTeams.DepartmentID in (" + departmentId + @")
group by EmployeeTeams.Name, EmployeeTeams.ID
order by EmployeeTeams.Name";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    var row = new PeopleHolidayHeadcountViewModel();

                    row.Id = await result.GetFieldValueAsync<byte>(0);
                    row.TeamName = await result.GetFieldValueAsync<string>(1);
                    row.TMsCount = await result.GetFieldValueAsync<int>(2);
                    row.PMsCount = await result.GetFieldValueAsync<int>(3);
                    row.SickLeaves = await result.GetFieldValueAsync<int>(4);
                    row.Holiday = await result.GetFieldValueAsync<int>(5);
                    row.Available = row.TMsCount + row.PMsCount - row.SickLeaves - row.Holiday;

                    res.Add(row);
                }

                res.Insert(0, new PeopleHolidayHeadcountViewModel()
                {
                    Id = departmentId,
                    TeamName = departmentId == 9 ? "GLS" : "TP",
                    Holiday = res.Sum(x => x.Holiday),
                    TMsCount = res.Sum(x => x.TMsCount),
                    PMsCount = res.Sum(x => x.PMsCount),
                    SickLeaves = res.Sum(x => x.SickLeaves),
                    Available = res.Sum(x => x.Available)
                });
            }
            return res;
        }
        public async Task<TeamBreakdownViewModel> GetTBAEmployeeJobsForOpsMgmt()
        {
            var res = new TeamBreakdownViewModel();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select employees.ID as EmpId, (Employees.FirstName + ' ' + Employees.Surname) as PMname, count(distinct(joborders.id)) as 'OpenJobsCount', count(jobitems.ID) as 'OpenItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
    COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'

    from JobOrders
    left outer join JobItems on JobItems.JobOrderID = joborders.ID
    inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
    inner join EmployeeTeams on employees.TeamID = EmployeeTeams.ID
    inner join contacts on contacts.ID = joborders.ContactID
    inner join Orgs on orgs.ID = Contacts.OrgID

    where Employees.ID in (435)
    and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null
    and Orgs.OrgGroupID not in(18520)

    group by employees.FirstName, Employees.Surname, Employees.ID
    order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Id = await result.GetFieldValueAsync<short>(0);
                    res.TeamName = await result.GetFieldValueAsync<string>(1);
                    res.OpenJobsCount = await result.GetFieldValueAsync<int>(2);
                    res.OpenItemsCount = await result.GetFieldValueAsync<int>(3);
                    res.OverdueJobsCount = await result.GetFieldValueAsync<int>(4);
                    res.OverdueItemsCount = await result.GetFieldValueAsync<int>(5);
                    res.JobsDueToday = await result.GetFieldValueAsync<int>(6);
                    res.ItemsDueToday = await result.GetFieldValueAsync<int>(7);
                }
            }

            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeOpenJobsForOpsMgmt()
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Open jobs
                    " + JobOrderDataTableCommandTextForTBAEmployee + @"
                    where Employees.ID = 435 and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new JobOrdersDataTableViewModel()
                    {
                        TeamId = await result.GetFieldValueAsync<short>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        JobOrderId = await result.GetFieldValueAsync<int>(2),
                        JobOrderName = await result.GetFieldValueAsync<string>(3),
                        ProjectManagerName = await result.GetFieldValueAsync<string>(4),
                        DeliveryDeadline = await result.GetFieldValueAsync<string>(5),
                        OrderStatus = await result.GetFieldValueAsync<string>(6),
                        OrgId = await result.GetFieldValueAsync<int>(7),
                        OrgName = await result.GetFieldValueAsync<string>(8),
                        OrgGroupId = await result.GetFieldValueAsync<int>(9),
                        OrgGroupName = await result.GetFieldValueAsync<string>(10),
                        OriginatedFromEnquiryId = await result.GetFieldValueAsync<int>(11),
                        OverallChargeToClient = await result.GetFieldValueAsync<decimal>(12),
                        MarginPercentage = await result.GetFieldValueAsync<decimal>(13),
                        Currency = await result.GetFieldValueAsync<string>(14)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeOverdueJobsForOpsMgmt()
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Overdue jobs
                    " + JobOrderDataTableCommandTextForTBAEmployee + @"
                    where Employees.ID = 435 and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520) and
                    JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime()";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new JobOrdersDataTableViewModel()
                    {
                        TeamId = await result.GetFieldValueAsync<short>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        JobOrderId = await result.GetFieldValueAsync<int>(2),
                        JobOrderName = await result.GetFieldValueAsync<string>(3),
                        ProjectManagerName = await result.GetFieldValueAsync<string>(4),
                        DeliveryDeadline = await result.GetFieldValueAsync<string>(5),
                        OrderStatus = await result.GetFieldValueAsync<string>(6),
                        OrgId = await result.GetFieldValueAsync<int>(7),
                        OrgName = await result.GetFieldValueAsync<string>(8),
                        OrgGroupId = await result.GetFieldValueAsync<int>(9),
                        OrgGroupName = await result.GetFieldValueAsync<string>(10),
                        OriginatedFromEnquiryId = await result.GetFieldValueAsync<int>(11),
                        OverallChargeToClient = await result.GetFieldValueAsync<decimal>(12),
                        MarginPercentage = await result.GetFieldValueAsync<decimal>(13),
                        Currency = await result.GetFieldValueAsync<string>(14)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersDataTableViewModel>> GetTBAEmployeeDueTodayJobsForOpsMgmt()
        {
            var res = new List<JobOrdersDataTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"-- Due today jobs
                    " + JobOrderDataTableCommandTextForTBAEmployee + @"
                    where Employees.ID = 435 and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18520) and
                    Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new JobOrdersDataTableViewModel()
                    {
                        TeamId = await result.GetFieldValueAsync<short>(0),
                        TeamName = await result.GetFieldValueAsync<string>(1),
                        JobOrderId = await result.GetFieldValueAsync<int>(2),
                        JobOrderName = await result.GetFieldValueAsync<string>(3),
                        ProjectManagerName = await result.GetFieldValueAsync<string>(4),
                        DeliveryDeadline = await result.GetFieldValueAsync<string>(5),
                        OrderStatus = await result.GetFieldValueAsync<string>(6),
                        OrgId = await result.GetFieldValueAsync<int>(7),
                        OrgName = await result.GetFieldValueAsync<string>(8),
                        OrgGroupId = await result.GetFieldValueAsync<int>(9),
                        OrgGroupName = await result.GetFieldValueAsync<string>(10),
                        OriginatedFromEnquiryId = await result.GetFieldValueAsync<int>(11),
                        OverallChargeToClient = await result.GetFieldValueAsync<decimal>(12),
                        MarginPercentage = await result.GetFieldValueAsync<decimal>(13),
                        Currency = await result.GetFieldValueAsync<string>(14)
                    });
                }
            }
            return res;
        }
        public async Task<TeamItemsBreakdownViewModel> GetTBAEmployeeItemsForOpsMgmt()
        {
            var res = new TeamItemsBreakdownViewModel();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select employees.ID as EmpId, (Employees.FirstName + ' ' + Employees.Surname) as PMname,
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.SupplierSentWorkDateTime is null THEN JobItems.id END) as 'ItemsNotSentToSupplier',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.LanguageServiceID IN(1,17) and JobItems.SupplierAcceptedWorkDateTime is not null and JobItems.SupplierCompletedItemDateTime is null THEN JobItems.id END) as 'ItemsInTranslation',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.LanguageServiceID = 7 and JobItems.WeCompletedItemDateTime is null and JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInProofreading',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.LanguageServiceID = 21 and JobItems.WeCompletedItemDateTime is null and JobItems.SupplierAcceptedWorkDateTime is not null  THEN JobItems.id END) as 'ItemsInClientReview',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.LanguageServiceID IN(4, 35) and JobItems.WeCompletedItemDateTime is null and JobItems.SupplierAcceptedWorkDateTime is not null THEN JobItems.id END) as 'ItemsInProduction',
                    COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.SupplierSentWorkDateTime < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'Overdue'
                    	
                        from JobOrders
                        left outer join JobItems on JobItems.JobOrderID = joborders.ID
                        inner join Employees on JobOrders.ProjectManagerEmployeeID = Employees.id
                        inner join contacts on contacts.ID = joborders.ContactID
                        inner join Orgs on orgs.ID = Contacts.OrgID
                    
                        where Employees.ID in (435)
                        and joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null
                        and Orgs.OrgGroupID not in(18520)
                    
                        group by employees.FirstName, Employees.Surname, Employees.ID
                        order by Employees.FirstName";
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {

                    res.Id = await result.GetFieldValueAsync<short>(0);
                    res.TeamName = await result.GetFieldValueAsync<string>(1);
                    res.ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(2);
                    res.ItemsInTranslation = await result.GetFieldValueAsync<int>(3);
                    res.ItemsInProofreading = await result.GetFieldValueAsync<int>(4);
                    res.ItemsInClientReview = await result.GetFieldValueAsync<int>(5);
                    res.ItemsInProduction = await result.GetFieldValueAsync<int>(6);
                    res.OverdueAndRejectedItems = await result.GetFieldValueAsync<int>(7);
                }
            }

            return res;
        }
        private async Task<List<JobOrdersDataTableViewModel>> BindPropertiesForDataTable(System.Data.Common.DbDataReader result, List<JobOrdersDataTableViewModel> res)
        {
            while (await result.ReadAsync())
            {
                res.Add(new JobOrdersDataTableViewModel()
                {
                    TeamId = await result.GetFieldValueAsync<byte>(0),
                    TeamName = await result.GetFieldValueAsync<string>(1),
                    JobOrderId = await result.GetFieldValueAsync<int>(2),
                    JobOrderName = await result.GetFieldValueAsync<string>(3),
                    ProjectManagerName = await result.GetFieldValueAsync<string>(4),
                    DeliveryDeadline = await result.GetFieldValueAsync<string>(5),
                    OrderStatus = await result.GetFieldValueAsync<string>(6),
                    OrgId = await result.GetFieldValueAsync<int>(7),
                    OrgName = await result.GetFieldValueAsync<string>(8),
                    OrgGroupId = await result.GetFieldValueAsync<int>(9),
                    OrgGroupName = await result.GetFieldValueAsync<string>(10),
                    OriginatedFromEnquiryId = await result.GetFieldValueAsync<int>(11),
                    OverallChargeToClient = await result.GetFieldValueAsync<decimal>(12),
                    MarginPercentage = await result.GetFieldValueAsync<decimal>(13),
                    PmId = await result.GetFieldValueAsync<short>(14),
                    Currency = await result.GetFieldValueAsync<string>(15)
                });
            }

            return res;
        }
    }
}
