using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.TMS.PmMgmt;

namespace Services
{
    public class PMDashboardService : IPMDashboardService
    {
        private static readonly int timeout = 120;
        private static readonly string smallTablesSelectQuery = @"select distinct JobOrders.ID, JobOrders.JobName, FORMAT (Joborders.OverallDeliveryDeadline, 'dd MMM yyyy HH:mm'),
Orgs.ID, Orgs.OrgName, Contacts.ID, Contacts.Name,
ISNULL(OverallChargeToClient, 0), JobOrders.AnticipatedGrossMarginPercentage,
(SELECT [Currencies].[Prefix] FROM [dbo].[Currencies] WHERE [Currencies].[ID] = JobOrders.ClientCurrencyID) as Currency

from JobOrders

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

        public async Task<IEnumerable<AllJobOrdersViewModel>> GetAllJobOrdersForPm(int pmId)
        {
            var res = new List<AllJobOrdersViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"--PM select query for the main table
        select distinct JobOrders.ID, OriginatedFromEnquiryID, JobOrders.JobName, 
        FORMAT(OverallDeliveryDeadline, 'dd MMM yyyy HH:mm') as DeliveryDeadline, FORMAT(SubmittedDateTime, 'dd MMM yyyy HH:mm') as Submitted,
        (Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
        when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
        when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
        Orgs.ID, Orgs.OrgName, OrgGroupID, OrgGroups.Name, Contacts.ID, Contacts.Name,
        ISNULL(OverallChargeToClient, 0) as Value, JobOrders.AnticipatedGrossMarginPercentage,
        (select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)) as SourceLang,
        (select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)) as TargetLang,
        (SELECT [Currencies].[Prefix] FROM [dbo].[Currencies] WHERE [Currencies].[ID] = JobOrders.ClientCurrencyID) as Currency
        
        --(select distinct (case when CONVERT(nvarchar, COUNT(JobItems.ID) OVER(PARTITION BY JobOrderID)) = 1 then LocalLanguageInfo.Name
        --else CONCAT(CONVERT(nvarchar, COUNT(JobItems.ID) OVER(PARTITION BY JobOrderID)), ' languages') end) 
        --from JobItems
        --inner join LocalLanguageInfo on LocalLanguageInfo.LanguageIANAcodeBeingDescribed = JobItems.SourceLanguageIANAcode
        --where JobItems.DeletedDateTime is null and JobItems.JobOrderID = JobOrders.ID and LocalLanguageInfo.LanguageIANAcode = 'en') as SourceLang
        --,
        --(select distinct (case when CONVERT(nvarchar, COUNT(JobItems.ID) OVER(PARTITION BY JobOrderID)) = 1 then LocalLanguageInfo.Name
        --else CONCAT(CONVERT(nvarchar, COUNT(JobItems.ID) OVER(PARTITION BY JobOrderID)), ' languages') end) 
        --from JobItems
        --inner join LocalLanguageInfo on LocalLanguageInfo.LanguageIANAcodeBeingDescribed = JobItems.TargetLanguageIANAcode
        --where JobItems.DeletedDateTime is null and JobItems.JobOrderID = JobOrders.ID and LocalLanguageInfo.LanguageIANAcode = 'en') as TargetLang
        
        from JobOrders
        
        inner join contacts on contacts.ID = joborders.ContactID
        inner join Orgs on orgs.ID = Contacts.OrgID
        inner join OrgGroups on orgs.OrgGroupID = OrgGroups.ID
        
        where ProjectManagerEmployeeID = " + pmId + @" and
        joborders.OverallCompletedDateTime IS NULL and
        joborders.DeletedDate is null and Orgs.OrgGroupID not in(18648,18520)";

                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new AllJobOrdersViewModel()
                    {
                        JobOrderId = await result.GetFieldValueAsync<int>(0),
                        EnquiryId = await result.GetFieldValueAsync<int>(1),
                        JobOrderName = await result.GetFieldValueAsync<string>(2),
                        DeliveryDeadline = await result.GetFieldValueAsync<string>(3),
                        SubmittedDate = await result.GetFieldValueAsync<string>(4),
                        Status = await result.GetFieldValueAsync<string>(5),
                        OrgId = await result.GetFieldValueAsync<int>(6),
                        OrgName = await result.GetFieldValueAsync<string>(7),
                        OrgGroupId = await result.GetFieldValueAsync<int>(8),
                        OrgGroupName = await result.GetFieldValueAsync<string>(9),
                        ContactId = await result.GetFieldValueAsync<int>(10),
                        ContactName = await result.GetFieldValueAsync<string>(11),
                        Value = await result.GetFieldValueAsync<decimal>(12),
                        Margin = await result.GetFieldValueAsync<decimal>(13),
                        SourceLang = await result.GetFieldValueAsync<string>(14),
                        TargetLang = await result.GetFieldValueAsync<string>(15),
                        Currency = await result.GetFieldValueAsync<string>(16)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersTableViewModel>> GetApproachingDeadlinesInNextThreeDaysJobs(int pmId)
        {
            var res = new List<JobOrdersTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"--Approaching jobs for PM
                    " + smallTablesSelectQuery + @"
                    left join JobItems on JobItems.JobOrderID = JobOrders.ID

                    where ProjectManagerEmployeeID = " + pmId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    JobOrders.OverallDeliveryDeadline >= getdate() and JobOrders.OverallDeliveryDeadline <= DATEADD(day, 3, getdate())";

                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersTableViewModel>> GetClientReviewJobs(int pmId)
        {
            var res = new List<JobOrdersTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"--CR jobs for PM
                    " + smallTablesSelectQuery + @"
                    left join JobItems on JobItems.JobOrderID = JobOrders.ID

                    where ProjectManagerEmployeeID = " + pmId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    IsActuallyOnlyAQuote = 0 and
                    JobItems.LanguageServiceID = 21 and 
                    JobItems.SupplierSentWorkDateTime is not null and 
                    
                    JobItems.SupplierCompletedItemDateTime is null";//JobItems.SupplierAcceptedWorkDateTime is not null and 

                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<PMItemsByClientViewModel>> GetJobItemsByClients(int pmId)
        {
            var res = new List<PMItemsByClientViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Orgs.ID, Orgs.OrgName, OrgGroups.ID, OrgGroups.Name,
" + ItemsCountColumnsQuerySection + @"
from JobOrders
left outer join JobItems on JobItems.JobOrderID = joborders.ID
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on Contacts.OrgID = Orgs.ID
inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
where ProjectManagerEmployeeID = " + pmId + @" and 
joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)
group by Orgs.ID, Orgs.OrgName,OrgGroups.name, OrgGroups.ID
order by OrgGroups.Name";

                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new PMItemsByClientViewModel()
                    {
                        OrgId = await result.GetFieldValueAsync<int>(0),
                        OrgName = await result.GetFieldValueAsync<string>(1),
                        OrgGroupId = await result.GetFieldValueAsync<int>(2),
                        OrgGroupName = await result.GetFieldValueAsync<string>(3),
                        ItemsNotSentToSupplier = await result.GetFieldValueAsync<int>(4),
                        ItemsInTranslation = await result.GetFieldValueAsync<int>(5),
                        ItemsInProofreading = await result.GetFieldValueAsync<int>(6),
                        ItemsInClientReview = await result.GetFieldValueAsync<int>(7),
                        ItemsInProduction = await result.GetFieldValueAsync<int>(8),
                        OverdueItems = await result.GetFieldValueAsync<int>(9)
                    });
                }
            }
            return res;
        }
        public async Task<IEnumerable<PMOrdersByClientViewModel>> GetJobOrdersByClients(int pmId)
        {
            var res = new List<PMOrdersByClientViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"select Orgs.ID, Orgs.OrgName, OrgGroups.ID, OrgGroups.Name,
OrgSalesCategories.Name as SpendCategory,
CONVERT(DECIMAL(38,2),ISNULL(SUM([dbo].[funcCurrencyConversion](JobOrders.ClientCurrencyID,(4),ISNULL(JobOrders.OverallChargeToClient,0))),0)) as OpenOrdersGBPValue,
COUNT(distinct(joborders.id)) as 'OpenJobsCount', COUNT(jobitems.ID) as 'OpenItemsCount',
COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() THEN joborders.id END) as 'OverdueJobsCount',
COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and JobItems.OurCompletionDeadline < dbo.funcGetCurrentUKTime() THEN JobItems.id END) as 'OverdueItemsCount',
COUNT(DISTINCT CASE WHEN joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN joborders.id END) as 'JobsDueToday',
COUNT(DISTINCT CASE WHEN JobItems.DeletedDateTime is null and JobItems.WeCompletedItemDateTime is null and Convert(varchar(10),JobItems.OurCompletionDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120) THEN JobItems.id END) as 'ItemsDueToday'

from JobOrders

left outer join JobItems on JobItems.JobOrderID = joborders.ID
inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on Contacts.OrgID = Orgs.ID
inner join OrgGroups on orgs.OrgGroupID = Orggroups.ID
inner join OrgSalesCategories on Orgsalescategories.ID = SalesCategoryID

where ProjectManagerEmployeeID = "+ pmId +@" and 
joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520)

group by Orgs.ID, Orgs.OrgName, OrgGroups.name, OrgGroups.ID, OrgSalesCategories.Name
order by Orgs.OrgName";

                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new PMOrdersByClientViewModel()
                    {
                        OrgId = await result.GetFieldValueAsync<int>(0),
                        OrgName = await result.GetFieldValueAsync<string>(1),
                        OrgGroupId = await result.GetFieldValueAsync<int>(2),
                        OrgGroupName = await result.GetFieldValueAsync<string>(3),
                        SpendCategory = await result.GetFieldValueAsync<string>(4),
                        TotalOpenOrdersGBPValue = await result.GetFieldValueAsync<decimal>(5),
                        OpenJobsCount = await result.GetFieldValueAsync<int>(6),
                        OpenItemsCount = await result.GetFieldValueAsync<int>(7),
                        OverdueJobsCount = await result.GetFieldValueAsync<int>(8),
                        OverdueItemsCount = await result.GetFieldValueAsync<int>(9),
                        JobsDueToday = await result.GetFieldValueAsync<int>(10),
                        ItemsDueToday = await result.GetFieldValueAsync<int>(11)
                    });
                }
            }
            return res;
        }

        //If an overdue reason of Client review is selected, this job to be moved to the jobs in CR section
        public async Task<IEnumerable<JobOrdersTableViewModel>> GetOverdueJobs(int pmId)
        {
            var res = new List<JobOrdersTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"--Overdue jobs for PM
                    " + smallTablesSelectQuery + @"
                    left join JobItems on JobItems.JobOrderID = JobOrders.ID

                    where ProjectManagerEmployeeID = " + pmId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() and
                    (OverdueReasonId is null and
                    JobItems.LanguageServiceID <> 21)";
                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        public async Task<IEnumerable<JobOrdersTableViewModel>> GetTodaysDeliveriesJobs(int pmId)
        {
            var res = new List<JobOrdersTableViewModel>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = @"--Due Today jobs for PM
                    " + smallTablesSelectQuery + @"
                    where ProjectManagerEmployeeID = " + pmId + @" and
                    joborders.DeletedDate is null and joborders.OverallCompletedDateTime is null and Orgs.OrgGroupID not in(18648,18520) and
                    Convert(varchar(10),JobOrders.OverallDeliveryDeadline,120) = Convert(varchar(10),dbo.funcGetCurrentUKTime(),120)";
                command.CommandTimeout = timeout;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                res = await this.BindPropertiesForDataTable(result, res);
            }
            return res;
        }
        private async Task<List<JobOrdersTableViewModel>> BindPropertiesForDataTable(System.Data.Common.DbDataReader result, List<JobOrdersTableViewModel> res)
        {
            while (await result.ReadAsync())
            {
                res.Add(new JobOrdersTableViewModel()
                {
                    JobOrderId = await result.GetFieldValueAsync<int>(0),
                    JobOrderName = await result.GetFieldValueAsync<string>(1),
                    DeliveryDeadline = await result.GetFieldValueAsync<string>(2),
                    OrgId = await result.GetFieldValueAsync<int>(3),
                    OrgName = await result.GetFieldValueAsync<string>(4),
                    ConctactId = await result.GetFieldValueAsync<int>(5),
                    ContactName = await result.GetFieldValueAsync<string>(6),
                    OverallChargeToClient = await result.GetFieldValueAsync<decimal>(7),
                    Margin = await result.GetFieldValueAsync<decimal>(8),
                    Currency = await result.GetFieldValueAsync<string>(9),
                });
            }
            return res;
        }
    }
}
