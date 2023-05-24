using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Services.Interfaces;

namespace Services
{
    public class LinguistProfile : ILinguistProfile
    {
        private readonly IRepository<Employee> empRepository;


        public LinguistProfile(IRepository<Employee> empRepository)
        {
            this.empRepository = empRepository;
        }

        public async Task<string> IdentifyCurrentUserById(Int16 EmployeeID)
        {
            var result = await empRepository.All().Where(x => x.Id == EmployeeID && x.TerminateDate == null).Select(x => x.FirstName + ' ' + x.Surname).FirstOrDefaultAsync();
            return result;
        }
        
        public async Task<List<ViewModels.LinguistSearch.LinguistSearchResults>> GetLinguistResultsById(string NumberValue)
        {

            var res = new List<ViewModels.LinguistSearch.LinguistSearchResults>();
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();


                command.CommandText = string.Format(@"select LinguisticSuppliers.Id,
case when LinguisticSuppliers.SupplierTypeID in(2, 3, 10) then LinguisticSuppliers.AgencyOrTeamName
else LinguisticSuppliers.MainContactFirstName + ' ' + LinguisticSuppliers.MainContactSurname end as 'Name',
LinguisticSupplierTypes.Name 'Type', LinguisticSuppliers.EmailAddress, count(jobitems.id) 'Number of items',
isnull(LinguisticSuppliers.SupplierStatusID,0) 
--LanguageRateUnits.Name
from LinguisticSuppliers
left outer join LinguisticSupplierTypes on LinguisticSuppliers.SupplierTypeID = LinguisticSupplierTypes.ID
left outer join JobItems on JobItems.LinguisticSupplierOrClientReviewerID = LinguisticSuppliers.ID
and JobItems.SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null and Jobitems.CreatedDateTime >= DATEADD(DAY, -91, dbo.funcGetCurrentUKTime())
where LinguisticSuppliers.deleteddate is null
and dbo.LinguisticSuppliers.Id  = " + NumberValue + @" 
group by LinguisticSuppliers.Id, LinguisticSuppliers.SupplierTypeID, LinguisticSuppliers.MainContactFirstName,
LinguisticSuppliers.MainContactSurname,LinguisticSuppliers.AgencyOrTeamName, LinguisticSupplierTypes.Name, LinguisticSuppliers.EmailAddress,
LinguisticSuppliers.SupplierStatusID
order by LinguisticSuppliers.ID");

                context.Database.OpenConnection();
                using var result = command.ExecuteReader();


                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.LinguistSearch.LinguistSearchResults()
                    {
                        linguistID = await result.GetFieldValueAsync<int>(0),
                        linguistName = await result.GetFieldValueAsync<string>(1),
                        numberOfJobs = await result.GetFieldValueAsync<int>(4),
                        linguistType = await result.GetFieldValueAsync<string>(2),
                        lingStatusID = await result.GetFieldValueAsync<byte>(5)
                    });
                }
            }
            return res;
        }

        public async Task<List<ViewModels.LinguistSearch.LinguistSearchResults>> GetLinguistResults(string NumberValue, string linguistName,
            string EmailValue, string PostcodeValue, string KeyWords, string MasterFieldsValues, string LanguageServicesValues
            , string SubFieldsValues, string MediaTypesValues, string myRangeValue
            , string SourceLanguagesValues, string TargetLanguageValues
            , string SpecialRatesValues, string supplierStatusValues
            , string CountriesNationalyValues, string CountryOfResidenceValues
            , string SoftwaresValues, string gdprStatusListValues
            , string SupplierAvailabilityValues, string RateUnitsListValues, string AppliesToValues, string SupplierNDAValue
            , string SupplierMemoryMatchValue, string SupplierEncryptedValue, string rbPreferred, string rbPreferredObject,
            string PreferredObjectValue, string rbJobsObject, string JobsDoneForClient)
        {

            var res = new List<ViewModels.LinguistSearch.LinguistSearchResults>();

            string JobsDone = "and ((@JobsType is null) or (JobOrders.ContactID = case when @JobsType = 1 then @JobsID end) or (Orgs.ID = case when @JobsType = 2 then @JobsID end) or (OrgGroups.ID = case when @JobsType = 3 then @JobsID end))";
            string PrefLinuist = "and ((@PrefSuppliersID is null or @PrefSuppliersID = CAST(ApprovedOrBlockedLinguisticSuppliers.AppliesToDataObjectID AS varchar)) and(@PrefSuppliersType is null or @PrefSuppliersType = CAST(ApprovedOrBlockedLinguisticSuppliers.AppliesToDataObjectTypeID AS varchar)) and(@PrefApprovedType is null or @PrefApprovedType = Cast(ApprovedOrBlockedLinguisticSuppliers.Status as varchar)))";
            if (SupplierNDAValue == "true") { SupplierNDAValue = "and LinguisticSuppliers.NDAUploadedDateTime is not null"; } else { SupplierNDAValue = ""; }
            if (SupplierEncryptedValue == "true") { SupplierEncryptedValue = "and linguisticsuppliers.HasEncryptedComputer = 1"; } else { SupplierEncryptedValue = ""; }
            if (JobsDoneForClient == null) { JobsDone = ""; };
            if (PreferredObjectValue == null) { PrefLinuist = ""; PreferredObjectValue = ""; };
            using (var context = new TPCoreProductionContext())
            {

                using var command = context.Database.GetDbConnection().CreateCommand();
                if (LanguageServicesValues == null && SourceLanguagesValues == null && TargetLanguageValues == null
                      && (SpecialRatesValues == null || SpecialRatesValues == "5") && RateUnitsListValues == null && AppliesToValues == null)
                {
                    command.CommandText = string.Format(@"declare @lingName nvarchar(max)
declare @emailAddress nvarchar(max)
declare @keyWords nvarchar(max)
declare @masterFields nvarchar(max)
declare @SubFieldsValues nvarchar(max)
declare @MediaTypesValues nvarchar(max)
declare @supplierStatusValues nvarchar(max)
declare @CountriesNationalyValues nvarchar(max)
declare @CountryOfResidenceValues nvarchar(max)
declare @SoftwaresValues nvarchar(max)
declare @gdprStatusListValues nvarchar(max)
declare @SupplierAvailabilityValues nvarchar(max)
declare @PrefSuppliersID nvarchar(max)
declare @PrefSuppliersType nvarchar(max)
declare @PrefApprovedType nvarchar(max)
declare @JobsType nvarchar(max)
declare @JobsID nvarchar(max)
declare @PostCode nvarchar(max)


set @lingName = '{0}'
set @emailAddress = '{1}'
set @keyWords = '{2}'
set @masterFields = '{3}'
set @SubFieldsValues = '{4}'
set @MediaTypesValues = '{5}'
set @supplierStatusValues = '{6}'
set @CountriesNationalyValues = '{7}'
set @CountryOfResidenceValues = '{8}'
set @SoftwaresValues = '{9}'
set @gdprStatusListValues = '{10}'
set @SupplierAvailabilityValues = '{11}'
set @PrefSuppliersID = '{12}'
set @PrefSuppliersType = '{13}'
set @PrefApprovedType = '{14}'
set @JobsType = '{15}'
set @JobsID = '{16}'
set @PostCode = '{17}'

if @lingName = '' begin set @lingName = null end 
if @emailAddress = '' begin set @emailAddress = null end 
if @keyWords = '' begin set @keyWords = null end
if @masterFields = '' begin set @masterFields = null end 
if @SubFieldsValues = '' begin set @SubFieldsValues = null end 
if @MediaTypesValues = '' begin set @MediaTypesValues = null end 
if @supplierStatusValues = '' begin set @supplierStatusValues = null end 
if @CountriesNationalyValues = '' begin set @CountriesNationalyValues = null end 
if @CountryOfResidenceValues = '' begin set @CountryOfResidenceValues = null end 
if @SoftwaresValues = '' begin set @SoftwaresValues = null end 
if @gdprStatusListValues = '' begin set @gdprStatusListValues = null end 
if @SupplierAvailabilityValues = '' begin set @SupplierAvailabilityValues = null end
if @PrefSuppliersID = '' begin set @PrefSuppliersID = null end 
if @PrefSuppliersType = '' begin set @PrefSuppliersType = null end 
if @PrefApprovedType = '' begin set @PrefApprovedType = null end
if @JobsType = '' begin set @JobsType = null end 
if @JobsID = '' begin set @JobsID = null end 
if @PostCode = '' begin set @PostCode = null end 


if @SupplierAvailabilityValues is null
begin

select LinguisticSuppliers.Id,
case when LinguisticSuppliers.SupplierTypeID in(2,3,10) then LinguisticSuppliers.AgencyOrTeamName 
else LinguisticSuppliers.MainContactFirstName + ' ' + LinguisticSuppliers.MainContactSurname end as 'Name', LinguisticSupplierTypes.Name 'Type',
LinguisticSuppliers.EmailAddress, count(distinct jobitems.id) 'Number of items', isnull(LinguisticSuppliers.SupplierStatusID,0) 
from LinguisticSuppliers
left outer join LinguisticSupplierTypes on LinguisticSuppliers.SupplierTypeID = LinguisticSupplierTypes.ID
--left outer join ExtranetUsers on ExtranetUsers.DataObjectID = LinguisticSuppliers.Id and ExtranetUsers.DataObjectTypeID = 4
--left outer join PlanningCalendarAppointments on PlanningCalendarAppointments.ExtranetUserName = ExtranetUsers.UserName
left outer join JobItems on JobItems.LinguisticSupplierOrClientReviewerID = LinguisticSuppliers.ID
and JobItems.SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null and Jobitems.CreatedDateTime >= DATEADD(DAY, -91, dbo.funcGetCurrentUKTime()) 
left outer join ApprovedOrBlockedLinguisticSuppliers on ApprovedOrBlockedLinguisticSuppliers.LinguisticSupplierID = LinguisticSuppliers.ID
left outer join JobOrders on JobOrders.ID = JobItems.Joborderid
left outer join Contacts on Contacts.ID = Joborders.ContactID
left outer join Orgs on Orgs.ID = Contacts.OrgID
left outer join OrgGroups on OrgGroups.ID = Orgs.OrgGroupID
left outer join LinguisticSupplierSoftwareApplications on LinguisticSupplierSoftwareApplications.LinguisticSupplierID = LinguisticSuppliers.ID
where LinguisticSuppliers.deleteddate is null 
and (@lingName  is null or LinguisticSuppliers.MainContactFirstName like  '%' + @lingName + '%'
or LinguisticSuppliers.MainContactSurname like '%' + @lingName + '%'  or LinguisticSuppliers.AgencyOrTeamName like '%' + @lingName + '%')
and (@PostCode  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @PostCode + '%')
and (@emailAddress  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @emailAddress + '%')
and (@keyWords  is null or dbo.LinguisticSuppliers.SubjectAreaSpecialismsAsDescribedBySupplier like '%' + @keyWords + '%')
--and (@masterFields  is null or dbo.LinguisticSuppliers.MasterFieldID like '%' + @masterFields + '%')
--and (@SubFieldsValues  is null or dbo.LinguisticSuppliers.SubFieldID like '%' + @SubFieldsValues + '%')
--and (@MediaTypesValues is null or dbo.LinguisticSuppliers.MediaTypeID like '%' + @MediaTypesValues + '%')
and (@supplierStatusValues is null or ','+@supplierStatusValues+',' LIKE '%,'+CAST(LinguisticSuppliers.SupplierStatusID AS varchar)+',%')
and (@CountryOfResidenceValues is null or ','+@CountryOfResidenceValues+',' LIKE '%,'+CAST(LinguisticSuppliers.CountryID AS varchar)+',%')
and (@CountriesNationalyValues is null or ','+@CountriesNationalyValues+',' LIKE '%,'+CAST(LinguisticSuppliers.MainContactNationalityCountryID AS varchar)+',%')
and (@SoftwaresValues is null or ','+@SoftwaresValues+',' LIKE '%,'+CAST(LinguisticSupplierSoftwareApplications.SoftwareApplicationID AS varchar)+',%')
and (@gdprStatusListValues is null or ','+@gdprStatusListValues+',' LIKE '%,'+CAST(LinguisticSuppliers.GDPRStatus AS varchar)+',%')
--and ((@SupplierAvailabilityValues is null or ((','+@SupplierAvailabilityValues+',' LIKE '%,'+CAST(PlanningCalendarAppointments.Category AS varchar)+',%') 
--and (PlanningCalendarAppointments.EndDateTime > dbo.funcGetCurrentUKTime()) and (PlanningCalendarAppointments.StartDateTime <= dbo.funcGetCurrentUKTime()))))
{21}
{20}
{18}  {19}
group by LinguisticSuppliers.Id, LinguisticSuppliers.SupplierTypeID, LinguisticSuppliers.MainContactFirstName,
LinguisticSuppliers.MainContactSurname,LinguisticSuppliers.AgencyOrTeamName, LinguisticSupplierTypes.Name, LinguisticSuppliers.EmailAddress,
LinguisticSuppliers.SupplierStatusID
end
else
begin

select LinguisticSuppliers.Id,
case when LinguisticSuppliers.SupplierTypeID in(2,3,10) then LinguisticSuppliers.AgencyOrTeamName 
else LinguisticSuppliers.MainContactFirstName + ' ' + LinguisticSuppliers.MainContactSurname end as 'Name',
LinguisticSupplierTypes.Name 'Type', LinguisticSuppliers.EmailAddress, count(distinct jobitems.id) 'Number of items',
isnull(LinguisticSuppliers.SupplierStatusID,0) 
from LinguisticSuppliers
left outer join LinguisticSupplierTypes on LinguisticSuppliers.SupplierTypeID = LinguisticSupplierTypes.ID
left outer join ExtranetUsers on ExtranetUsers.DataObjectID = LinguisticSuppliers.Id and ExtranetUsers.DataObjectTypeID = 4
left outer join PlanningCalendarAppointments on PlanningCalendarAppointments.ExtranetUserName = ExtranetUsers.UserName
left outer join JobItems on JobItems.LinguisticSupplierOrClientReviewerID = LinguisticSuppliers.ID
and JobItems.SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null and Jobitems.CreatedDateTime >= DATEADD(DAY, -91, dbo.funcGetCurrentUKTime()) 
left outer join ApprovedOrBlockedLinguisticSuppliers on ApprovedOrBlockedLinguisticSuppliers.LinguisticSupplierID = LinguisticSuppliers.ID
left outer join JobOrders on JobOrders.ID = JobItems.Joborderid
left outer join Contacts on Contacts.ID = Joborders.ContactID
left outer join Orgs on Orgs.ID = Contacts.OrgID
left outer join OrgGroups on OrgGroups.ID = Orgs.OrgGroupID
left outer join LinguisticSupplierSoftwareApplications on LinguisticSupplierSoftwareApplications.LinguisticSupplierID = LinguisticSuppliers.ID
where LinguisticSuppliers.deleteddate is null 
and (@lingName  is null or LinguisticSuppliers.MainContactFirstName like  '%' + @lingName + '%'
or LinguisticSuppliers.MainContactSurname like '%' + @lingName + '%'  or LinguisticSuppliers.AgencyOrTeamName like '%' + @lingName + '%')
and (@PostCode  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @PostCode + '%')
and (@emailAddress  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @emailAddress + '%')
and (@keyWords  is null or dbo.LinguisticSuppliers.SubjectAreaSpecialismsAsDescribedBySupplier like '%' + @keyWords + '%')
--and (@masterFields  is null or dbo.LinguisticSuppliers.MasterFieldID like '%' + @masterFields + '%')
--and (@SubFieldsValues  is null or dbo.LinguisticSuppliers.SubFieldID like '%' + @SubFieldsValues + '%')
--and (@MediaTypesValues is null or dbo.LinguisticSuppliers.MediaTypeID like '%' + @MediaTypesValues + '%')
and (@supplierStatusValues is null or ','+@supplierStatusValues+',' LIKE '%,'+CAST(LinguisticSuppliers.SupplierStatusID AS varchar)+',%')
and (@CountryOfResidenceValues is null or ','+@CountryOfResidenceValues+',' LIKE '%,'+CAST(LinguisticSuppliers.CountryID AS varchar)+',%')
and (@CountriesNationalyValues is null or ','+@CountriesNationalyValues+',' LIKE '%,'+CAST(LinguisticSuppliers.MainContactNationalityCountryID AS varchar)+',%')
and (@SoftwaresValues is null or ','+@SoftwaresValues+',' LIKE '%,'+CAST(LinguisticSupplierSoftwareApplications.SoftwareApplicationID AS varchar)+',%')
and (@gdprStatusListValues is null or ','+@gdprStatusListValues+',' LIKE '%,'+CAST(LinguisticSuppliers.GDPRStatus AS varchar)+',%')
and ((@SupplierAvailabilityValues is null or ((','+@SupplierAvailabilityValues+',' LIKE '%,'+CAST(PlanningCalendarAppointments.Category AS varchar)+',%') 
and (PlanningCalendarAppointments.EndDateTime > dbo.funcGetCurrentUKTime()) and (PlanningCalendarAppointments.StartDateTime <= dbo.funcGetCurrentUKTime()))))
{21}
{20} 
{18}  {19}
group by LinguisticSuppliers.Id, LinguisticSuppliers.SupplierTypeID, LinguisticSuppliers.MainContactFirstName,
LinguisticSuppliers.MainContactSurname,LinguisticSuppliers.AgencyOrTeamName, LinguisticSupplierTypes.Name, LinguisticSuppliers.EmailAddress,
LinguisticSuppliers.SupplierStatusID
end", linguistName, EmailValue, KeyWords, MasterFieldsValues, SubFieldsValues, MediaTypesValues,
 supplierStatusValues, CountriesNationalyValues, CountryOfResidenceValues,
SoftwaresValues, gdprStatusListValues, SupplierAvailabilityValues, PreferredObjectValue, rbPreferredObject,
rbPreferred, rbJobsObject, JobsDoneForClient, PostcodeValue, SupplierNDAValue, SupplierEncryptedValue, JobsDone, PrefLinuist);

                    context.Database.OpenConnection();
                    using var result = command.ExecuteReader();


                    while (await result.ReadAsync())
                    {
                        res.Add(new ViewModels.LinguistSearch.LinguistSearchResults()
                        {
                            linguistID = await result.GetFieldValueAsync<int>(0),
                            linguistName = await result.GetFieldValueAsync<string>(1),
                            numberOfJobs = await result.GetFieldValueAsync<int>(4),
                            linguistType = await result.GetFieldValueAsync<string>(2),
                            lingStatusID = await result.GetFieldValueAsync<byte>(5)
                        });
                    }
                }
                else
                {
                    command.CommandText = string.Format(@"declare @lingName nvarchar(max)
declare @emailAddress nvarchar(max)
declare @keyWords nvarchar(max)
declare @masterFields nvarchar(max)
declare @SubFieldsValues nvarchar(max)
declare @MediaTypesValues nvarchar(max)
declare @supplierStatusValues nvarchar(max)
declare @CountriesNationalyValues nvarchar(max)
declare @CountryOfResidenceValues nvarchar(max)
declare @SoftwaresValues nvarchar(max)
declare @gdprStatusListValues nvarchar(max)
declare @SupplierAvailabilityValues nvarchar(max)
declare @PrefSuppliersID nvarchar(max)
declare @PrefSuppliersType nvarchar(max)
declare @PrefApprovedType nvarchar(max)
declare @JobsType nvarchar(max)
declare @JobsID nvarchar(max)
declare @PostCode nvarchar(max)
declare @LanguageServices nvarchar(max)
declare @SourceLanguagesValues nvarchar(max)
declare @TargetLanguageValues nvarchar(max)
declare @SpecialRatesValues nvarchar(max)
declare @RateUnitsListValues nvarchar(max)
declare @AppliesToValues nvarchar(max)

set @lingName = '{0}'
set @emailAddress = '{1}'
set @keyWords = '{2}'
set @masterFields = '{3}'
set @SubFieldsValues = '{4}'
set @MediaTypesValues = '{5}'
set @supplierStatusValues = '{6}'
set @CountriesNationalyValues = '{7}'
set @CountryOfResidenceValues = '{8}'
set @SoftwaresValues = '{9}'
set @gdprStatusListValues = '{10}'
set @SupplierAvailabilityValues = '{11}'
set @PrefSuppliersID = '{12}'
set @PrefSuppliersType = '{13}'
set @PrefApprovedType = '{14}'
set @JobsType = '{15}'
set @JobsID = '{16}'
set @PostCode = '{17}'
set @LanguageServices = '{22}'
set @SourceLanguagesValues = '{23}'
set @TargetLanguageValues = '{24}'
set @SpecialRatesValues = '{25}'
set @RateUnitsListValues = '{26}'
set @AppliesToValues = '{27}'


if @lingName = '' begin set @lingName = null end 
if @emailAddress = '' begin set @emailAddress = null end 
if @keyWords = '' begin set @keyWords = null end
if @masterFields = '' begin set @masterFields = null end 
if @SubFieldsValues = '' begin set @SubFieldsValues = null end 
if @MediaTypesValues = '' begin set @MediaTypesValues = null end 
if @supplierStatusValues = '' begin set @supplierStatusValues = null end 
if @CountriesNationalyValues = '' begin set @CountriesNationalyValues = null end 
if @CountryOfResidenceValues = '' begin set @CountryOfResidenceValues = null end 
if @SoftwaresValues = '' begin set @SoftwaresValues = null end 
if @gdprStatusListValues = '' begin set @gdprStatusListValues = null end 
if @SupplierAvailabilityValues = '' begin set @SupplierAvailabilityValues = null end
if @PrefSuppliersID = '' begin set @PrefSuppliersID = null end 
if @PrefSuppliersType = '' begin set @PrefSuppliersType = null end 
if @PrefApprovedType = '' begin set @PrefApprovedType = null end
if @JobsType = '' begin set @JobsType = null end 
if @JobsID = '' begin set @JobsID = null end 
if @PostCode = '' begin set @PostCode = null end 
if @LanguageServices = '' begin set @LanguageServices = null end 
if @SourceLanguagesValues = '' begin set @SourceLanguagesValues = null end 
if @TargetLanguageValues = '' begin set @TargetLanguageValues = null end 
if @SpecialRatesValues = '' begin set @SpecialRatesValues = null end
if @RateUnitsListValues = '' begin set @RateUnitsListValues = null end 
if @AppliesToValues = '' begin set @AppliesToValues = null end 

if @SupplierAvailabilityValues is null
begin

select LinguisticSuppliers.Id,
case when LinguisticSuppliers.SupplierTypeID in(2,3,10) then LinguisticSuppliers.AgencyOrTeamName 
else LinguisticSuppliers.MainContactFirstName + ' ' + LinguisticSuppliers.MainContactSurname end as 'Name', LinguisticSupplierTypes.Name 'Type', 
LinguisticSuppliers.EmailAddress, count(distinct jobitems.id) 'Number of items', Currencies.Prefix + cast(LinguisticSupplierRates.StandardRate as nvarchar),
'£'+cast(cast(LinguisticSupplierRates.StandardRateSterlingEquivalent as decimal(10,2)) as nvarchar),
LanguageRateUnits.Name, cast(LinguisticSupplierRates.MinimumCharge as nvarchar), LanguageSubjectAreas.Name, 
case when LinguisticSupplierRates.AppliesToDataObjectTypeID = 1 then 'only the individual contact'
 when LinguisticSupplierRates.AppliesToDataObjectTypeID = 2 then 'any contacts at the organisation'
 when LinguisticSupplierRates.AppliesToDataObjectTypeID = 3 then 'any contacts at any organisation within the group'
 else 'any translate plus client ' end, isnull(LinguisticSupplierRates.AppliesToDataObjectID,0), isnull(LinguisticSuppliers.SupplierStatusID,0) 
from LinguisticSuppliers
left outer join LinguisticSupplierTypes on LinguisticSuppliers.SupplierTypeID = LinguisticSupplierTypes.ID
--left outer join ExtranetUsers on ExtranetUsers.DataObjectID = LinguisticSuppliers.Id and ExtranetUsers.DataObjectTypeID = 4
--left outer join PlanningCalendarAppointments on PlanningCalendarAppointments.ExtranetUserName = ExtranetUsers.UserName
left outer join JobItems on JobItems.LinguisticSupplierOrClientReviewerID = LinguisticSuppliers.ID
and JobItems.SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null and Jobitems.CreatedDateTime >= DATEADD(DAY, -91, dbo.funcGetCurrentUKTime()) 
left outer join ApprovedOrBlockedLinguisticSuppliers on ApprovedOrBlockedLinguisticSuppliers.LinguisticSupplierID = LinguisticSuppliers.ID
left outer join JobOrders on JobOrders.ID = JobItems.Joborderid
left outer join LinguisticSupplierRates on LinguisticSuppliers.Id = LinguisticSupplierRates.SupplierID and LinguisticSupplierRates.DeletedDate is null 
left outer join LanguageRateUnits on LanguageRateUnits.ID = LinguisticSupplierRates.RateUnitID
left outer join LanguageSubjectAreas on LanguageSubjectAreas.ID = LinguisticSupplierRates.SubjectAreaID
left outer join Contacts on Contacts.ID = Joborders.ContactID
left outer join Currencies on Currencies.ID = LinguisticSupplierRates.CurrencyID
left outer join Orgs on Orgs.ID = Contacts.OrgID
left outer join OrgGroups on OrgGroups.ID = Orgs.OrgGroupID
left outer join LinguisticSupplierSoftwareApplications on LinguisticSupplierSoftwareApplications.LinguisticSupplierID = LinguisticSuppliers.ID
where LinguisticSuppliers.deleteddate is null 
and (@lingName  is null or LinguisticSuppliers.MainContactFirstName like  '%' + @lingName + '%'
or LinguisticSuppliers.MainContactSurname like '%' + @lingName + '%'  or LinguisticSuppliers.AgencyOrTeamName like '%' + @lingName + '%')
and (@PostCode  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @PostCode + '%')
and (@emailAddress  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @emailAddress + '%')
and (@keyWords  is null or dbo.LinguisticSuppliers.SubjectAreaSpecialismsAsDescribedBySupplier like '%' + @keyWords + '%')
--and (@masterFields  is null or dbo.LinguisticSuppliers.MasterFieldID like '%' + @masterFields + '%')
--and (@SubFieldsValues  is null or dbo.LinguisticSuppliers.SubFieldID like '%' + @SubFieldsValues + '%')
--and (@MediaTypesValues is null or dbo.LinguisticSuppliers.MediaTypeID like '%' + @MediaTypesValues + '%')
and (@LanguageServices is null or ','+@LanguageServices+',' LIKE '%,'+CAST(LinguisticSupplierRates.LanguageServiceID AS varchar)+',%')
and (@SourceLanguagesValues is null or ','+@SourceLanguagesValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.SourceLangIANAcode AS varchar)+',%')
and (@TargetLanguageValues is null or ','+@TargetLanguageValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.TargetLangIANAcode AS varchar)+',%')
and (@SpecialRatesValues is null or ','+@SpecialRatesValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.SubjectAreaID AS varchar)+',%')
and (@RateUnitsListValues is null or ','+@RateUnitsListValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.RateUnitID AS varchar)+',%')
and (@AppliesToValues is null or ','+@AppliesToValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.AppliesToDataObjectTypeID AS varchar)+',%')
and (@supplierStatusValues is null or ','+@supplierStatusValues+',' LIKE '%,'+CAST(LinguisticSuppliers.SupplierStatusID AS varchar)+',%')
and (@CountryOfResidenceValues is null or ','+@CountryOfResidenceValues+',' LIKE '%,'+CAST(LinguisticSuppliers.CountryID AS varchar)+',%')
and (@CountriesNationalyValues is null or ','+@CountriesNationalyValues+',' LIKE '%,'+CAST(LinguisticSuppliers.MainContactNationalityCountryID AS varchar)+',%')
and (@SoftwaresValues is null or ','+@SoftwaresValues+',' LIKE '%,'+CAST(LinguisticSupplierSoftwareApplications.SoftwareApplicationID AS varchar)+',%')
and (@gdprStatusListValues is null or ','+@gdprStatusListValues+',' LIKE '%,'+CAST(LinguisticSuppliers.GDPRStatus AS varchar)+',%')
--and ((@SupplierAvailabilityValues is null or ((','+@SupplierAvailabilityValues+',' LIKE '%,'+CAST(PlanningCalendarAppointments.Category AS varchar)+',%') 
--and (PlanningCalendarAppointments.EndDateTime > dbo.funcGetCurrentUKTime()) and (PlanningCalendarAppointments.StartDateTime <= dbo.funcGetCurrentUKTime()))))
{21}
{20}
{18}  {19}
group by LinguisticSuppliers.Id, LinguisticSuppliers.SupplierTypeID, LinguisticSuppliers.MainContactFirstName,
LinguisticSuppliers.MainContactSurname,LinguisticSuppliers.AgencyOrTeamName, LinguisticSupplierTypes.Name, LinguisticSuppliers.EmailAddress,
LinguisticSupplierRates.StandardRate, LinguisticSupplierRates.StandardRateSterlingEquivalent,
LanguageRateUnits.Name, LinguisticSupplierRates.MinimumCharge, LanguageSubjectAreas.Name, 
LinguisticSupplierRates.AppliesToDataObjectTypeID, LinguisticSupplierRates.AppliesToDataObjectID,Prefix,
LinguisticSuppliers.SupplierStatusID
end
else
begin

select LinguisticSuppliers.Id,
case when LinguisticSuppliers.SupplierTypeID in(2,3,10) then LinguisticSuppliers.AgencyOrTeamName 
else LinguisticSuppliers.MainContactFirstName + ' ' + LinguisticSuppliers.MainContactSurname end as 'Name', LinguisticSupplierTypes.Name 'Type', 
LinguisticSuppliers.EmailAddress, count(distinct jobitems.id) 'Number of items',
Currencies.Prefix + cast(LinguisticSupplierRates.StandardRate as nvarchar),
'£'+cast(cast(LinguisticSupplierRates.StandardRateSterlingEquivalent as decimal(10,2)) as nvarchar),
LanguageRateUnits.Name, cast(LinguisticSupplierRates.MinimumCharge as nvarchar), LanguageSubjectAreas.Name, 
case when LinguisticSupplierRates.AppliesToDataObjectTypeID = 1 then 'only the individual contact'
 when LinguisticSupplierRates.AppliesToDataObjectTypeID = 2 then 'any contacts at the organisation'
 when LinguisticSupplierRates.AppliesToDataObjectTypeID = 3 then 'any contacts at any organisation within the group'
 else 'any translate plus client ' end, isnull(LinguisticSupplierRates.AppliesToDataObjectID,0),
isnull(LinguisticSuppliers.SupplierStatusID,0) 
from LinguisticSuppliers
left outer join LinguisticSupplierTypes on LinguisticSuppliers.SupplierTypeID = LinguisticSupplierTypes.ID
left outer join ExtranetUsers on ExtranetUsers.DataObjectID = LinguisticSuppliers.Id and ExtranetUsers.DataObjectTypeID = 4
left outer join PlanningCalendarAppointments on PlanningCalendarAppointments.ExtranetUserName = ExtranetUsers.UserName
left outer join JobItems on JobItems.LinguisticSupplierOrClientReviewerID = LinguisticSuppliers.ID
and JobItems.SupplierIsClientReviewer = 0 and JobItems.DeletedDateTime is null and Jobitems.CreatedDateTime >= DATEADD(DAY, -91, dbo.funcGetCurrentUKTime()) 
left outer join ApprovedOrBlockedLinguisticSuppliers on ApprovedOrBlockedLinguisticSuppliers.LinguisticSupplierID = LinguisticSuppliers.ID
left outer join JobOrders on JobOrders.ID = JobItems.Joborderid
left outer join LinguisticSupplierRates on LinguisticSuppliers.Id = LinguisticSupplierRates.SupplierID and LinguisticSupplierRates.DeletedDate is null 
left outer join LanguageRateUnits on LanguageRateUnits.ID = LinguisticSupplierRates.RateUnitID
left outer join LanguageSubjectAreas on LanguageSubjectAreas.ID = LinguisticSupplierRates.SubjectAreaID
left outer join Contacts on Contacts.ID = Joborders.ContactID
left outer join Currencies on Currencies.ID = LinguisticSupplierRates.CurrencyID
left outer join Orgs on Orgs.ID = Contacts.OrgID
left outer join OrgGroups on OrgGroups.ID = Orgs.OrgGroupID
left outer join LinguisticSupplierSoftwareApplications on LinguisticSupplierSoftwareApplications.LinguisticSupplierID = LinguisticSuppliers.ID
where LinguisticSuppliers.deleteddate is null 
and (@lingName  is null or LinguisticSuppliers.MainContactFirstName like  '%' + @lingName + '%'
or LinguisticSuppliers.MainContactSurname like '%' + @lingName + '%'  or LinguisticSuppliers.AgencyOrTeamName like '%' + @lingName + '%')
and (@PostCode  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @PostCode + '%')
and (@emailAddress  is null or dbo.LinguisticSuppliers.EmailAddress like '%' + @emailAddress + '%')
and (@keyWords  is null or dbo.LinguisticSuppliers.SubjectAreaSpecialismsAsDescribedBySupplier like '%' + @keyWords + '%')
--and (@masterFields  is null or dbo.LinguisticSuppliers.MasterFieldID like '%' + @masterFields + '%')
--and (@SubFieldsValues  is null or dbo.LinguisticSuppliers.SubFieldID like '%' + @SubFieldsValues + '%')
--and (@MediaTypesValues is null or dbo.LinguisticSuppliers.MediaTypeID like '%' + @MediaTypesValues + '%')
and (@LanguageServices is null or ','+@LanguageServices+',' LIKE '%,'+CAST(LinguisticSupplierRates.LanguageServiceID AS varchar)+',%')
and (@SourceLanguagesValues is null or ','+@SourceLanguagesValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.SourceLangIANAcode AS varchar)+',%')
and (@TargetLanguageValues is null or ','+@TargetLanguageValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.TargetLangIANAcode AS varchar)+',%')
and (@SpecialRatesValues is null or ','+@SpecialRatesValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.SubjectAreaID AS varchar)+',%')
and (@RateUnitsListValues is null or ','+@RateUnitsListValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.RateUnitID AS varchar)+',%')
and (@AppliesToValues is null or ','+@AppliesToValues+',' LIKE '%,'+CAST(LinguisticSupplierRates.AppliesToDataObjectTypeID AS varchar)+',%')
and (@supplierStatusValues is null or ','+@supplierStatusValues+',' LIKE '%,'+CAST(LinguisticSuppliers.SupplierStatusID AS varchar)+',%')
and (@CountryOfResidenceValues is null or ','+@CountryOfResidenceValues+',' LIKE '%,'+CAST(LinguisticSuppliers.CountryID AS varchar)+',%')
and (@CountriesNationalyValues is null or ','+@CountriesNationalyValues+',' LIKE '%,'+CAST(LinguisticSuppliers.MainContactNationalityCountryID AS varchar)+',%')
and (@SoftwaresValues is null or ','+@SoftwaresValues+',' LIKE '%,'+CAST(LinguisticSupplierSoftwareApplications.SoftwareApplicationID AS varchar)+',%')
and (@gdprStatusListValues is null or ','+@gdprStatusListValues+',' LIKE '%,'+CAST(LinguisticSuppliers.GDPRStatus AS varchar)+',%')
and ((@SupplierAvailabilityValues is null or ((','+@SupplierAvailabilityValues+',' LIKE '%,'+CAST(PlanningCalendarAppointments.Category AS varchar)+',%') 
and (PlanningCalendarAppointments.EndDateTime > dbo.funcGetCurrentUKTime()) and (PlanningCalendarAppointments.StartDateTime <= dbo.funcGetCurrentUKTime()))))
{21}
{20} 
{18}  {19}
group by LinguisticSuppliers.Id, LinguisticSuppliers.SupplierTypeID, LinguisticSuppliers.MainContactFirstName,
LinguisticSuppliers.MainContactSurname,LinguisticSuppliers.AgencyOrTeamName, LinguisticSupplierTypes.Name, LinguisticSuppliers.EmailAddress,
LinguisticSupplierRates.StandardRate, LinguisticSupplierRates.StandardRateSterlingEquivalent,
LanguageRateUnits.Name, LinguisticSupplierRates.MinimumCharge, LanguageSubjectAreas.Name, 
LinguisticSupplierRates.AppliesToDataObjectTypeID, LinguisticSupplierRates.AppliesToDataObjectID,Prefix,
LinguisticSuppliers.SupplierStatusID
end", linguistName, EmailValue, KeyWords, MasterFieldsValues, SubFieldsValues, MediaTypesValues,
 supplierStatusValues, CountriesNationalyValues, CountryOfResidenceValues,
SoftwaresValues, gdprStatusListValues, SupplierAvailabilityValues, PreferredObjectValue, rbPreferredObject,
rbPreferred, rbJobsObject, JobsDoneForClient, PostcodeValue, SupplierNDAValue, SupplierEncryptedValue, JobsDone, PrefLinuist, LanguageServicesValues,
SourceLanguagesValues, TargetLanguageValues, SpecialRatesValues, RateUnitsListValues, AppliesToValues);


                    context.Database.OpenConnection();
                    using var result = command.ExecuteReader();
                    // do something with result

                    while (await result.ReadAsync())
                    {
                        res.Add(new ViewModels.LinguistSearch.LinguistSearchResults()
                        {
                            linguistID = await result.GetFieldValueAsync<int>(0),
                            linguistName = await result.GetFieldValueAsync<string>(1),
                            numberOfJobs = await result.GetFieldValueAsync<int>(4),
                            linguistType = await result.GetFieldValueAsync<string>(2),
                            linguistRate = await result.GetFieldValueAsync<string>(5),
                            linguistRateGBP = await result.GetFieldValueAsync<string>(6),
                            linguistRateRateUnit = await result.GetFieldValueAsync<string>(7),
                            minimumCharge = await result.GetFieldValueAsync<string>(8),
                            specialisedRate = await result.GetFieldValueAsync<string>(9),
                            appliesTo = await result.GetFieldValueAsync<string>(10),
                            appliesToId = await result.GetFieldValueAsync<int>(11),
                            lingStatusID = await result.GetFieldValueAsync<byte>(12)
                        });
                    }
                }
                return res;
            }


        }
    }
}
