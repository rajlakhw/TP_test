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
    public class TMSClientSearch : ITMSClientSearch
    {
        private readonly IRepository<Employee> empRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<OrgGroup> orgGroupRepository;
        private readonly IRepository<LocalCountryInfo> countryRepository;

        private readonly IRepository<ClientTechnology> technologyRepository;

        public TMSClientSearch(IRepository<Org> repository, IRepository<Contact> crepository, IRepository<OrgGroup> grouprepository, IRepository<LocalCountryInfo> lcirepository, IRepository<ClientTechnology> ctrepository, IRepository<Employee> empRepository)
        {
            this.orgRepository = repository;
            this.contactRepository = crepository;
            this.orgGroupRepository = grouprepository;
            this.countryRepository = lcirepository;
            this.technologyRepository = ctrepository;
            this.empRepository = empRepository;
        }

        public async Task<List<ViewModels.TMSClientSearch.TMSClientTechnology>> GetTechnologies()
        {
            var resultsList = await technologyRepository.All().Where(o => o.Id > 0).Select(x => new ViewModels.TMSClientSearch.TMSClientTechnology() { Id = x.Id, TechnologyName = x.TechnologyName }).OrderBy(x => x.TechnologyName).ToListAsync();
            return resultsList;
        }

        public async Task<List<ViewModels.TMSClientSearch.TMSCountry>> GetCountries()
        {
            var resultsList = await countryRepository.All().Where(o => o.CountryId > 0 && o.LanguageIanacode == "en").Select(x => new ViewModels.TMSClientSearch.TMSCountry() { Id = x.CountryId, CountryName = x.CountryName }).OrderBy(x => x.CountryName).ToListAsync();
            return resultsList;
        }

        public async Task<List<ViewModels.TMSClientSearch.OrgSearchResults>> GetOrgResults(string orgName, string groupName, string EmailAddressVlaue, string PostCodeValue, string HFMCodeISValue, string HFMCodeBSValue, string CountriesValue, string ClientTechnologyValue)
        {

            var res = new List<ViewModels.TMSClientSearch.OrgSearchResults>();



            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = string.Format(@"declare @orgName nvarchar(max)
declare @groupName nvarchar(max)
declare @emailAddress nvarchar(max)
declare @postCode nvarchar(max)
declare @hfmCodeIS nvarchar(max)
declare @hfmCodeBS nvarchar(max)
declare @CountryList nvarchar(max)
declare @TechnologyList nvarchar(max)


set @orgName = '{0}'  
set @groupName = '{1}'  
set @emailAddress = '{2}'  
set @postCode = '{3}'  
set @hfmCodeIS = '{4}'  
set @hfmCodeBS = '{5}'  
set @CountryList = '{6}'  
set @TechnologyList = '{7}'  

if @orgName = '' begin set @orgName = null end 
if @groupName = '' begin set @groupName = null end 
if @emailAddress = '' begin set @emailAddress = null end 
if @postCode = '' begin set @postCode = null end
if @hfmCodeIS = '' begin set @hfmCodeIS = null end 
if @hfmCodeBS = '' begin set @hfmCodeBS = null end
if @CountryList = '' begin set @CountryList = null end 
if @TechnologyList = '' begin set @TechnologyList = null end 


SELECT  distinct dbo.Orgs.ID AS OrgID, dbo.Orgs.OrgName,isnull(dbo.Orgs.Address1,'') as 'Address', dbo.LocalCountryInfo.CountryName, isnull(dbo.OrgGroups.Name,'') AS GroupName,isnull(dbo.OrgGroups.ID ,0) AS GroupID,
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER1 
where ER1.InForceStartDateTime is not null and ER1.InForceEndDateTime is null and ER1.EmployeeOwnershipTypeID = 2 and ER1.DataObjectID = dbo.Orgs.ID and ER1.DataObjectTypeID = 2 order by er1.InForceStartDateTime desc),0) as 'opslead',
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER2
where ER2.InForceStartDateTime is not null and ER2.InForceEndDateTime is null and ER2.EmployeeOwnershipTypeID in (1,4) and ER2.DataObjectID = dbo.Orgs.ID and ER2.DataObjectTypeID = 2 order by er2.InForceStartDateTime desc),0) as 'saleslead',
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER3
where ER3.InForceStartDateTime is not null and ER3.InForceEndDateTime is null and ER3.EmployeeOwnershipTypeID = 5 and ER3.DataObjectID = dbo.Orgs.ID and ER3.DataObjectTypeID = 2 order by er3.InForceStartDateTime desc),0) as 'enqlead',
 isnull(dbo.Orgs.EmailAddress,'') as 'EmailAddress'                 
FROM            dbo.orgs 
INNER JOIN dbo.LocalCountryInfo ON dbo.LocalCountryInfo.CountryID = dbo.Orgs.CountryID and dbo.LocalCountryInfo.LanguageIANAcode = 'en'
LEFT JOIN dbo.OrgGroups ON dbo.Orgs.OrgGroupID = dbo.OrgGroups.ID and OrgGroups.DeletedDate is null
left outer join dbo.OrgTechnologyRelationships on orgs.id = OrgTechnologyRelationships.OrgID
WHERE        (dbo.Orgs.DeletedDate IS NULL) 
and (@orgName  is null or dbo.Orgs.OrgName like '%' + @orgName + '%')
and (@groupName  is null or dbo.Orggroups.Name like '%' + @groupName + '%')
and (@emailAddress  is null or dbo.Orgs.EmailAddress like '%' + @emailAddress + '%')
and (@postCode  is null or dbo.Orgs.PostcodeOrZip like '%' + @postCode + '%')
and (@hfmCodeIS  is null or dbo.Orgs.HFMCodeIS like '%' + @hfmCodeIS + '%')
and (@hfmCodeBS is null or dbo.Orgs.HFMCodeBS like '%' + @hfmCodeBS + '%')
and (@CountryList is null or ','+@CountryList+',' LIKE '%,'+CAST(orgs.CountryID AS varchar)+',%')
and (@TechnologyList is null or ','+@TechnologyList+',' LIKE '%,'+CAST(OrgTechnologyRelationships.OrgTechnologyID AS varchar)+',%')", orgName, groupName, EmailAddressVlaue, PostCodeValue, HFMCodeISValue, HFMCodeBSValue, CountriesValue, ClientTechnologyValue);
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result
                while (await result.ReadAsync())
                {
                    string opsUserName = "";
                    string enqLeadName = "";
                    string salesLeadName = "";

                    if (await result.GetFieldValueAsync<Int16>(6) != 0)
                    {
                        opsUserName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(6));
                    }
                    else
                    {
                        opsUserName = "(none)";
                    }
                    if (await result.GetFieldValueAsync<Int16>(8) != 0)
                    {
                        enqLeadName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(8));
                    }
                    else
                    {
                        enqLeadName = "(none)";
                    }
                    if (await result.GetFieldValueAsync<Int16>(7) != 0)
                    {
                        salesLeadName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(7));
                    }
                    else
                    {
                        salesLeadName = "(none)";
                    }

                    res.Add(new ViewModels.TMSClientSearch.OrgSearchResults()
                    {
                        orgId = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(1),
                        orgAddress = await result.GetFieldValueAsync<string>(2),
                        orgCountry = await result.GetFieldValueAsync<string>(3),
                        orgGroup = await result.GetFieldValueAsync<string>(4),
                        orgGroupID = await result.GetFieldValueAsync<int>(5),
                        orgOpsLeadId = await result.GetFieldValueAsync<Int16>(6),
                        orgEnqLeadId = await result.GetFieldValueAsync<Int16>(8),
                        orgSalesLeadId = await result.GetFieldValueAsync<Int16>(7),
                        orgEmail = await result.GetFieldValueAsync<string>(9),
                        orgOpsLeadName = opsUserName,
                        orgEnqLeadName = enqLeadName,
                        orgSalesLeadName = salesLeadName

                    });
                }
            }
            return res;

        }

        public async Task<string> IdentifyCurrentUserById(Int16 EmployeeID)
        {
            var result = await empRepository.All().Where(x => x.Id == EmployeeID && x.TerminateDate == null).Select(x => x.FirstName + ' ' + x.Surname).FirstOrDefaultAsync();
            return result;
        }


        public async Task<List<ViewModels.TMSClientSearch.OrgSearchResults>> GetOrgResultsByID(string orgID)
        {

            var res = new List<ViewModels.TMSClientSearch.OrgSearchResults>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"SELECT  distinct dbo.Orgs.ID AS OrgID, dbo.Orgs.OrgName,isnull(dbo.Orgs.Address1,'') as 'Address', dbo.LocalCountryInfo.CountryName, isnull(dbo.OrgGroups.Name,'') AS GroupName,isnull(dbo.OrgGroups.ID ,0) AS GroupID,
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER1 
where ER1.InForceStartDateTime is not null and ER1.InForceEndDateTime is null and ER1.EmployeeOwnershipTypeID = 2 and ER1.DataObjectID = dbo.Orgs.ID and ER1.DataObjectTypeID = 2 order by er1.InForceStartDateTime desc),0) as 'opslead',
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER2
where ER2.InForceStartDateTime is not null and ER2.InForceEndDateTime is null and ER2.EmployeeOwnershipTypeID in (1,4) and ER2.DataObjectID = dbo.Orgs.ID and ER2.DataObjectTypeID = 2 order by er2.InForceStartDateTime desc),0) as 'saleslead',
isnull((select top 1 Employeeid from EmployeeOwnershipRelationships as ER3
where ER3.InForceStartDateTime is not null and ER3.InForceEndDateTime is null and ER3.EmployeeOwnershipTypeID = 5 and ER3.DataObjectID = dbo.Orgs.ID and ER3.DataObjectTypeID = 2 order by er3.InForceStartDateTime desc),0) as 'enqlead',
 isnull(dbo.Orgs.EmailAddress,'') as 'EmailAddress'
FROM dbo.orgs 
INNER JOIN dbo.LocalCountryInfo ON dbo.LocalCountryInfo.CountryID = dbo.Orgs.CountryID and dbo.LocalCountryInfo.LanguageIANAcode = 'en'
LEFT JOIN dbo.OrgGroups ON dbo.Orgs.OrgGroupID = dbo.OrgGroups.ID and OrgGroups.DeletedDate is null
WHERE (dbo.Orgs.DeletedDate IS NULL) and Orgs.ID = " + orgID;
                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {
                    string opsUserName = "";
                    string enqLeadName = "";
                    string salesLeadName = "";

                    if (await result.GetFieldValueAsync<Int16>(6) != 0)
                    {
                        opsUserName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(6));
                    }
                    else
                    {
                        opsUserName = "(none)";
                    }
                    if (await result.GetFieldValueAsync<Int16>(8) != 0)
                    {
                        enqLeadName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(8));
                    }
                    else
                    {
                        enqLeadName = "(none)";
                    }
                    if (await result.GetFieldValueAsync<Int16>(7) != 0)
                    {
                        salesLeadName = await IdentifyCurrentUserById(await result.GetFieldValueAsync<Int16>(7));
                    }
                    else
                    {
                        salesLeadName = "(none)";
                    }

                    res.Add(new ViewModels.TMSClientSearch.OrgSearchResults()
                    {
                        orgId = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(1),
                        orgAddress = await result.GetFieldValueAsync<string>(2),
                        orgCountry = await result.GetFieldValueAsync<string>(3),
                        orgGroup = await result.GetFieldValueAsync<string>(4),
                        orgGroupID = await result.GetFieldValueAsync<int>(5),
                        orgOpsLeadId = await result.GetFieldValueAsync<Int16>(6),
                        orgEnqLeadId = await result.GetFieldValueAsync<Int16>(8),
                        orgSalesLeadId = await result.GetFieldValueAsync<Int16>(7),
                        orgEmail = await result.GetFieldValueAsync<string>(9),
                        orgOpsLeadName = opsUserName,
                        orgEnqLeadName = enqLeadName,
                        orgSalesLeadName = salesLeadName


                    });
                }

            }
            return res;

        }

        public async Task<List<ViewModels.TMSClientSearch.OrgGroupResults>> GetOrgGroupResultsByType(string groupType, string groupName)
        {
            var resultsList = new List<ViewModels.TMSClientSearch.OrgGroupResults>();
            if (groupType == "External")
            {
                resultsList = await orgGroupRepository.All()
                        .Join(orgRepository.All(),
                        o => o.Id,
                        t => t.OrgGroupId,
                         (o, t) => new { OrgGroup = o, Org = t })
                        .Where(o => Microsoft.EntityFrameworkCore.EF.Functions.Like(o.OrgGroup.Name, $"%{groupName}%")
                        && o.OrgGroup.DeletedDate == null
                        && (o.OrgGroup.Id != 72112 && o.OrgGroup.Id != 72113 && o.OrgGroup.Id != 72114 && o.OrgGroup.Id != 72115))
                        .GroupBy(s => new { s.OrgGroup.Name, s.OrgGroup.Id })
                        .Select(x => new ViewModels.TMSClientSearch.OrgGroupResults
                        { orgGroup = x.Key.Name, orgGroupID = x.Key.Id, numberOfMembers = x.Count() }).OrderBy(x => x.orgGroup).ToListAsync();

            }
            else if (groupType == "Internal")
            {
                resultsList = await orgGroupRepository.All()
                   .Join(orgRepository.All(),
                   o => o.Id,
                   t => t.OrgGroupId,
                    (o, t) => new { OrgGroup = o, Org = t })
                   .Where(o => Microsoft.EntityFrameworkCore.EF.Functions.Like(o.OrgGroup.Name, $"%{groupName}%")
                   && o.OrgGroup.DeletedDate == null
                   && (o.OrgGroup.Id == 72112 || o.OrgGroup.Id == 72113 || o.OrgGroup.Id == 72114 || o.OrgGroup.Id == 72115))
                   .GroupBy(s => new { s.OrgGroup.Name, s.OrgGroup.Id })
                   .Select(x => new ViewModels.TMSClientSearch.OrgGroupResults
                   { orgGroup = x.Key.Name, orgGroupID = x.Key.Id, numberOfMembers = x.Count() }).OrderBy(x => x.orgGroup).ToListAsync();

            }
            else
            {
                resultsList = await orgGroupRepository.All()
                       .Join(orgRepository.All(),
                       o => o.Id,
                       t => t.OrgGroupId,
                        (o, t) => new { OrgGroup = o, Org = t })
                       .Where(o => Microsoft.EntityFrameworkCore.EF.Functions.Like(o.OrgGroup.Name, $"%{groupName}%")
                       && o.OrgGroup.DeletedDate == null)
                       .GroupBy(s => new { s.OrgGroup.Name, s.OrgGroup.Id })
                       .Select(x => new ViewModels.TMSClientSearch.OrgGroupResults
                       { orgGroup = x.Key.Name, orgGroupID = x.Key.Id, numberOfMembers = x.Count() }).OrderBy(x => x.orgGroup).ToListAsync();

            }
            return resultsList;
        }

        public async Task<List<ViewModels.TMSClientSearch.OrgGroupResults>> GetOrgGroupResultsByID(int groupID)
        {
            var resultsList = await orgGroupRepository.All()
                          .Join(orgRepository.All(),
                          o => o.Id,
                          t => t.OrgGroupId,
                           (o, t) => new { OrgGroup = o, Org = t })
                          .Where(o => o.OrgGroup.Id == groupID
                          && o.OrgGroup.DeletedDate == null && o.Org.DeletedDate == null)
                          .GroupBy(s => new { s.OrgGroup.Name, s.OrgGroup.Id })
                          .Select(x => new ViewModels.TMSClientSearch.OrgGroupResults
                          { orgGroup = x.Key.Name, orgGroupID = x.Key.Id, numberOfMembers = x.Count() }).OrderBy(x => x.orgGroup).ToListAsync();

            return resultsList;
        }
        public async Task<List<ViewModels.TMSClientSearch.ContactResults>> GetContactResultsByID(int contactID)
        {

            var resultsList = await contactRepository.All()
                       .Join(orgRepository.All(), o => o.OrgId, t => t.Id, (o, t) => new { Contact = o, Org = t })
                       .Join(orgGroupRepository.All(), a => a.Org.OrgGroupId, b => b.Id, (a, b) => new { Org = a, OrgGroup = b })
                       .Join(countryRepository.All(), a => a.Org.Org.CountryId, e => e.CountryId, (a, e) => new { Org = a, Country = e })
                       .Where(c => c.Org.Org.Contact.Id == contactID
                       && c.Org.Org.Contact.DeletedDate == null && c.Country.LanguageIanacode == "en")
                       .Select(x => new ViewModels.TMSClientSearch.ContactResults
                       {
                           contactID = x.Org.Org.Contact.Id,
                           contactName = x.Org.Org.Contact.Name,
                           gdprStatusID = x.Org.Org.Contact.Gdprstatus,
                           orgID = x.Org.Org.Org.Id,
                           orgName = x.Org.Org.Org.OrgName,
                           contactCountry = x.Country.CountryName,
                           orgGroup = x.Org.OrgGroup.Name,
                           orgGroupID = x.Org.OrgGroup.Id,
                           contactEmail = x.Org.Org.Contact.EmailAddress,
                           contactLandline = x.Org.Org.Contact.LandlineNumber,
                           contactMobile = x.Org.Org.Contact.MobileNumber,
                           contactTitle = x.Org.Org.Contact.JobTitle
                       }).OrderBy(x => x.contactName).ToListAsync();

            return resultsList;
        }
        //
        public async Task<List<ViewModels.TMSClientSearch.ContactResults>> GetContactResults(string contactName, string contactEmailAddress, string orgName, string groupName, string country)
        {
            if (country != "")
            {
                var resultsList = await contactRepository.All()
                                      .Join(orgRepository.All(), o => o.OrgId, t => t.Id, (o, t) => new { Contact = o, Org = t })
                                      .Join(orgGroupRepository.All(), t => t.Org.OrgGroupId, b => b.Id, (t, b) => new { Org = t, OrgGroup = b })
                                      .Join(countryRepository.All(), t => t.Org.Org.CountryId, e => e.CountryId, (t, e) => new { Org = t, Country = e })
                                      .Where(c => Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Contact.Name, $"%{contactName}%")
                                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Contact.EmailAddress, $"%{contactEmailAddress}%")
                                      && c.Org.Org.Contact.DeletedDate == null
                                      && c.Country.LanguageIanacode == "en"
                                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Org.OrgName, $"%{orgName}%")
                                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.OrgGroup.Name, $"%{groupName}%")
                                      && country.Contains(c.Country.CountryId.ToString()))
                                      .Select(x => new ViewModels.TMSClientSearch.ContactResults
                                      {
                                          contactID = x.Org.Org.Contact.Id,
                                          contactName = x.Org.Org.Contact.Name,
                                          gdprStatusID = x.Org.Org.Contact.Gdprstatus,
                                          orgName = x.Org.Org.Org.OrgName,
                                          orgID = x.Org.Org.Org.Id,
                                          contactCountry = x.Country.CountryName,
                                          orgGroup = x.Org.OrgGroup.Name,
                                          orgGroupID = x.Org.OrgGroup.Id,
                                          contactEmail = x.Org.Org.Contact.EmailAddress,
                                          contactLandline = x.Org.Org.Contact.LandlineNumber,
                                          contactMobile = x.Org.Org.Contact.MobileNumber,
                                          contactTitle = x.Org.Org.Contact.JobTitle
                                      }).OrderBy(x => x.contactName).ToListAsync();

                return resultsList;
            }
            else
            {
                var resultsList = await contactRepository.All()
                      .Join(orgRepository.All(), o => o.OrgId, t => t.Id, (o, t) => new { Contact = o, Org = t })
                      .Join(orgGroupRepository.All(), t => t.Org.OrgGroupId, b => b.Id, (t, b) => new { Org = t, OrgGroup = b })
                      .Join(countryRepository.All(), t => t.Org.Org.CountryId, e => e.CountryId, (t, e) => new { Org = t, Country = e })
                      .Where(c => Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Contact.Name, $"%{contactName}%")
                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Contact.EmailAddress, $"%{contactEmailAddress}%")
                      && c.Org.Org.Contact.DeletedDate == null
                      && c.Country.LanguageIanacode == "en"
                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.Org.Org.OrgName, $"%{orgName}%")
                      && Microsoft.EntityFrameworkCore.EF.Functions.Like(c.Org.OrgGroup.Name, $"%{groupName}%"))
                      .Select(x => new ViewModels.TMSClientSearch.ContactResults
                      {
                          contactID = x.Org.Org.Contact.Id,
                          contactName = x.Org.Org.Contact.Name,
                          gdprStatusID = x.Org.Org.Contact.Gdprstatus,
                          orgName = x.Org.Org.Org.OrgName,
                          orgID = x.Org.Org.Org.Id,
                          orgGroupID = x.Org.OrgGroup.Id,
                          contactCountry = x.Country.CountryName,
                          orgGroup = x.Org.OrgGroup.Name,
                          contactEmail = x.Org.Org.Contact.EmailAddress,
                          contactLandline = x.Org.Org.Contact.LandlineNumber,
                          contactMobile = x.Org.Org.Contact.MobileNumber,
                          contactTitle = x.Org.Org.Contact.JobTitle
                      }).OrderBy(x => x.contactName).ToListAsync();

                return resultsList;
            }

        }
    }
}
