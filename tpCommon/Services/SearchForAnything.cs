using System;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using System.Text;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.SearchForAnything;
using System.Threading.Tasks;

namespace Services
{
    public class SearchForAnything : ISearchForAnything
    {
        public async Task<SearchForAnythingResults> GetAllResults(string searchedFor)
        {
            var searchResults = new SearchForAnythingResults();
            searchResults.searchResults = new List<ViewModels.SearchForAnything.SearchForAnything>();

            string query = String.Empty;
            searchedFor = searchedFor.Replace(" ", "%");

            bool IsDigitsOnly(string str)
            {
                foreach (char c in str)
                {
                    if (c < '0' || c > '9')
                        return false;
                }

                return true;
            }

            if (IsDigitsOnly(searchedFor) == true)
            {
                query = @"
if object_id('tempdb..#tempSearch','U') is not null drop table #tempSearch

create table #tempSearch
(
ID int IDENTITY(1,1) primary key,
Type nvarchar(max),
ObjectID int,
Description nvarchar(max)
)

SET IDENTITY_INSERT #tempSearch OFF;

insert into #tempSearch (Type,ObjectID,Description)
select 'Job order' as 'Type', joborders.id, ' - ' + JobOrders.JobName + '' as 'Description' 
from JobOrders where id like '%" + searchedFor + @"%' and DeletedDate is null


insert into #tempSearch (Type,ObjectID,Description)
select 'Job item' as 'Type', Jobitems.id, ' - ' +  ISNULL(joborders.JobName,'No job order name')  + ' - ' + LanguageServices.Name + ' - ' + LC1.Name + ' into ' + LC2.Name + ''   as 'Description'
from JobItems
inner join joborders on joborders.id = JobItems.JobOrderID
left outer join LanguageServices on LanguageServices.ID = JobItems.LanguageServiceID
left outer join LocalLanguageInfo LC1 on LC1.LanguageIANAcodeBeingDescribed = JobItems.SourceLanguageIANAcode and LC1.LanguageIANAcode = 'en'
left outer join LocalLanguageInfo LC2 on LC2.LanguageIANAcodeBeingDescribed = JobItems.TargetLanguageIANAcode and LC2.LanguageIANAcode = 'en'
where JobItems.id like '%" + searchedFor + @"%'  and JobItems.DeletedDateTime is null order by JobItems.ID

insert into #tempSearch (Type,ObjectID,Description)
select  'Contact' as 'Type', Contacts.ID, ' - '  +  contacts.Name + ' - ' +  ISNULL(orgs.OrgName,'No org name')  + ''  as 'Description' from Contacts
left outer join Orgs on orgs.id = contacts.orgid
where Contacts.id like '%" + searchedFor + @"%'  and Contacts.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Organisation' as 'Type', Orgs.id, ' - '  +  Orgs.OrgName + '' as 'Description' from Orgs 
where Orgs.id like '%" + searchedFor + @"%'  and Orgs.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Organisation Group' as 'Type', OrgGroups.ID, ' - '  +  OrgGroups.Name + '' as 'Description' from OrgGroups 
where OrgGroups.id like '%" + searchedFor + @"%'  and OrgGroups.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Client invoice' as 'Type', ClientInvoices.id, ' for '  +  Contacts.Name + '' as 'Description' from ClientInvoices 
left outer join Contacts on Contacts.ID = ClientInvoices.ContactID
where ClientInvoices.id like '%" + searchedFor + @"%'  and ClientInvoices.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Linguistic supplier' as 'Type',LinguisticSuppliers.id, ' - '  + 
case 
when linguisticsuppliers.AgencyOrTeamName <> '' then linguisticsuppliers.AgencyOrTeamName
else linguisticsuppliers.MainContactFirstName + ' ' + linguisticsuppliers.MainContactSurname  + ''
end as 'Description' from LinguisticSuppliers 
where LinguisticSuppliers.id like '%" + searchedFor + @"%'  and LinguisticSuppliers.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Supplier invoice' as 'Type',LinguisticSupplierInvoices.id, ' for '  +  LinguisticSupplierInvoices.LinguisticSupplierName + '' as 'Description' from LinguisticSupplierInvoices 
where LinguisticSupplierInvoices.id like '%" + searchedFor + @"%'  and LinguisticSupplierInvoices.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Quote' as 'Type',Quotes.id, ' for '  +  Enquiries.JobName + '' as 'Description' from Quotes 
left outer join Enquiries on Enquiries.id = quotes.EnquiryID 
where Quotes.id like '%" + searchedFor + @"%'  and Quotes.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Enquiry' as 'Type',Enquiries.id, ' - '  +  Enquiries.JobName + '' as 'Description' from Enquiries 
where Enquiries.id like '%" + searchedFor + @"%'  and Enquiries.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'share plus' as 'Type',SharePlusArticles.id, ' - '  +  SharePlusArticles.Title + '' as 'Description' from SharePlusArticles 
where SharePlusArticles.id like '%" + searchedFor + @"%'  and SharePlusArticles.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Employee' as 'Type',Employees.ID, ' - '  +  Employees.FirstName + ' ' + Employees.Surname + '' as 'Description' from Employees 
where Employees.ID like '%" + searchedFor + @"%'  and Employees.TerminateDate is null 

select * from #tempSearch order by Type ";
            }
            else
            {
                query = @"

if object_id('tempdb..#tempSearch','U') is not null drop table #tempSearch

create table #tempSearch
(
ID int IDENTITY(1,1) primary key,
Type nvarchar(max),
ObjectID int,
Description nvarchar(max)
)

SET IDENTITY_INSERT #tempSearch OFF;

insert into #tempSearch (Type,ObjectID,Description)
select 'Job order' as 'Type', joborders.id, ' - ' + JobOrders.JobName + '' as 'Description' 
from JobOrders where JobOrders.JobName like '%" + searchedFor + @"%' and DeletedDate is null


insert into #tempSearch (Type,ObjectID,Description)
select 'Job item' as 'Type', Jobitems.id, ' - ' +  ISNULL(joborders.JobName,'No job order name')  + ' - ' + LanguageServices.Name + ' - ' + LC1.Name + ' into ' + LC2.Name + ''   as 'Description'
from JobItems
inner join joborders on joborders.id = JobItems.JobOrderID
left outer join LanguageServices on LanguageServices.ID = JobItems.LanguageServiceID
left outer join LocalLanguageInfo LC1 on LC1.LanguageIANAcodeBeingDescribed = JobItems.SourceLanguageIANAcode and LC1.LanguageIANAcode = 'en'
left outer join LocalLanguageInfo LC2 on LC2.LanguageIANAcodeBeingDescribed = JobItems.TargetLanguageIANAcode and LC2.LanguageIANAcode = 'en'
where Joborders.JobName like '%" + searchedFor + @"%' and JobItems.DeletedDateTime is null order by JobItems.ID

insert into #tempSearch (Type,ObjectID,Description)
select  'Contact' as 'Type', Contacts.ID, ' - '  +  contacts.Name + ' - ' +  ISNULL(orgs.OrgName,'No org name')  + ''  as 'Description' from Contacts
left outer join Orgs on orgs.id = contacts.orgid
where Contacts.Name  like '%" + searchedFor + @"%' and Contacts.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Organisation' as 'Type', Orgs.id, ' - '  +  Orgs.OrgName + '' as 'Description' from Orgs 
where Orgs.OrgName  like '%" + searchedFor + @"%' and Orgs.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Organisation Group' as 'Type', OrgGroups.ID, ' - '  +  OrgGroups.Name + '' as 'Description' from OrgGroups 
where OrgGroups.Name like '%" + searchedFor + @"%' and OrgGroups.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select 'Linguistic supplier' as 'Type',LinguisticSuppliers.id, ' - '  + 
case 
when linguisticsuppliers.AgencyOrTeamName <> '' then linguisticsuppliers.AgencyOrTeamName
else linguisticsuppliers.MainContactFirstName + ' ' + linguisticsuppliers.MainContactSurname  + ''
end as 'Description' from LinguisticSuppliers 
where (LinguisticSuppliers.AgencyOrTeamName like '%" + searchedFor + @"%' or LinguisticSuppliers.MainContactFirstName like '%" + searchedFor + @"%' or LinguisticSuppliers.MainContactSurname like '%" + searchedFor + @"%' or LinguisticSuppliers.MainContactFirstName +  LinguisticSuppliers.MainContactSurname like '%" + searchedFor + @"%')  and LinguisticSuppliers.DeletedDate is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Quote' as 'Type',Quotes.id, ' for '  +  Enquiries.JobName + '' as 'Description' from Quotes 
left outer join Enquiries on Enquiries.id = quotes.EnquiryID 
where Enquiries.JobName like '%" + searchedFor + @"%'  and Quotes.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Enquiry' as 'Type',Enquiries.id, ' - '  +  Enquiries.JobName + '' as 'Description' from Enquiries 
where Enquiries.JobName like '%" + searchedFor + @"%'  and Enquiries.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'share plus' as 'Type',SharePlusArticles.id, ' - '  +  SharePlusArticles.Title + '' as 'Description' from SharePlusArticles 
where SharePlusArticles.Title like '%" + searchedFor + @"%'  and SharePlusArticles.DeletedDateTime is null 

insert into #tempSearch (Type,ObjectID,Description)
select  'Employee' as 'Type',Employees.ID, ' - '  +  Employees.FirstName + ' ' + Employees.Surname + '' as 'Description' from Employees 
where (Employees.FirstName like '%" + searchedFor + @"%' or Employees.Surname like '%" + searchedFor + @"%'
or Employees.FirstName +  Employees.Surname like '%" + searchedFor + @"%') and Employees.TerminateDate is null 


select * from #tempSearch order by Type";

            }

            using (var context = new Data.TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
   
                while (await result.ReadAsync())
                {
                    
                    searchResults.searchResults.Add(new ViewModels.SearchForAnything.SearchForAnything()
                    {
                        resultType = await result.GetFieldValueAsync<string>(1),
                        resultId = await result.GetFieldValueAsync<int>(2),
                        resultDescription = await result.GetFieldValueAsync<string>(3)
                       
                    });
 
                }
                searchResults.resultCount = result.RecordsAffected;

            }
            return searchResults;
        }
    }
}
