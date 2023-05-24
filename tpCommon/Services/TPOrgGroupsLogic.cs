using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using ViewModels.OrgGroup;
using Microsoft.EntityFrameworkCore;
using Global_Settings;
using Services.Interfaces;

namespace Services
{
    public class TPOrgGroupsLogic : ITPOrgGroupsLogic
    {
        private readonly IRepository<OrgGroup> orgGroupRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<LocalCurrencyInfo> currencyRepository;
        private readonly IRepository<ClientPriceList> priceListsRepository;
        private readonly IRepository<Brand> brandRepository;
        private readonly IRepository<LocalCountryInfo> LIcountryRepository;
        private readonly ITPVolumeDiscountsService volumeDiscountsService;
        private readonly ITPflowplusLicencingLogic flowplusLicencingLogic;
        private readonly IRepository<flowPlusLicenceMapping> flowPlusLicenceMappingRepo;
        public TPOrgGroupsLogic(IRepository<OrgGroup> repository, IRepository<ClientPriceList> priceListsRepository,
            IRepository<Org> orgRepository, IRepository<Brand> brandRepository, IRepository<LocalCurrencyInfo> currencyRepository,
            IRepository<LocalCountryInfo> LIcountryRepository, ITPVolumeDiscountsService tPVolumeDiscountsService,
            ITPflowplusLicencingLogic flowplusLicencingLogic, IRepository<flowPlusLicenceMapping> flowPlusLicenceMappingRepo)
        {
            this.orgGroupRepository = repository;
            this.currencyRepository = currencyRepository;
            this.brandRepository = brandRepository;
            this.LIcountryRepository = LIcountryRepository;
            this.orgRepository = orgRepository;
            this.priceListsRepository = priceListsRepository;
            this.volumeDiscountsService = tPVolumeDiscountsService;
            this.flowplusLicencingLogic = flowplusLicencingLogic;
            this.flowPlusLicenceMappingRepo = flowPlusLicenceMappingRepo;
        }
        public async Task<OrgGroup> GetOrgGroupDetails(int ID)
        {
            var result = await orgGroupRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();
            return result;
        }


        public async Task<List<ViewModels.OrgGroup.Brands>> GetBrandsList()
        {
            var result = await brandRepository.All().Where(a => a.DeletedDateTime == null).Select(x => new ViewModels.OrgGroup.Brands
            { Id = x.Id, BrandName = x.Name }).ToListAsync();
            return result;
        }

        //public async Task<List<ViewModels.Contact.ContactCountry>> GetContactCountries()
        //{
        //    var resultsList = await countryRepository.All()
        //           .Join(LIcountryRepository.All(),
        //           o => o.Id,
        //           t => t.CountryId,
        //            (o, t) => new { Country = o, LocalCountryInfo = t })
        //           .Where(o => o.LocalCountryInfo.LanguageIanacode == "en")
        //           .Select(x => new ViewModels.Contact.ContactCountry
        //           { Id = x.Country.Id, IsoCode = x.Country.Isocode, Prefix = x.Country.DiallingPrefix, CountryName = x.LocalCountryInfo.CountryName + "   +" + x.Country.DiallingPrefix }).ToListAsync();
        //    return resultsList;
        //}

        public async Task<(int, int)> GetAllEnquiriesCountForDataObjectAndType(int dataObjectId, int dataTypeId, string searchTerm)
        {
            int totalRecords = 0;
            int filteredRecords = 0;

            string query = String.Empty;
            string selectSection = String.Empty;
            string whereSection = String.Empty;

            selectSection = @"select count(Enquiries.ID) 
from Enquiries 
inner join Contacts on Contacts.ID = Enquiries.ContactID
inner join Orgs on Orgs.ID = Contacts.OrgID";

            if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
            {
                whereSection = " where Orgs.ID = " + dataObjectId + @" and Enquiries.DeletedDateTime is null ";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
            {
                whereSection = " where Contacts.ID = " + dataObjectId + @" and Enquiries.DeletedDateTime is null";
            }
            else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
            {
                whereSection = " where Orgs.OrgGroupID = " + dataObjectId + @" and Enquiries.DeletedDateTime is null";
            }

            var newQuery = @"select 
	(select count(Enquiries.ID) 
		from Enquiries 
		inner join Contacts on Contacts.ID = Enquiries.ContactID
		inner join Orgs on Orgs.ID = Contacts.OrgID
		" + whereSection + @") as totalRecords,

(select COUNT(enqCount) from 
	(select count(Enquiries.ID) as enqCount , JobName, 
		CASE
			WHEN Enquiries.Status = 0 THEN 'Draft'
			WHEN Enquiries.Status = 1 THEN 'Pending'
			WHEN Enquiries.Status = 2 THEN 'Rejected'
			WHEN Enquiries.Status = 3 THEN 'Gone ahead'
		END AS 'Status',
		(select dbo.funcGetSourceLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as SourceLangCombined,
		(select dbo.funcGetTargetLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as TargetLangCombined
		
		from Enquiries 
		inner join Contacts on Contacts.ID = Enquiries.ContactID
		inner join Orgs on Orgs.ID = Contacts.OrgID
		
		" + whereSection + @"
		GROUP BY Enquiries.ID, JobName, [Status]) table1

		where (SourceLangCombined + TargetLangCombined + JobName + [Status]) LIKE N'%" + searchTerm + @"%'
) as filteredRecords";

            query = selectSection + whereSection;

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = newQuery;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    totalRecords = await result.GetFieldValueAsync<int>(0);
                    filteredRecords = await result.GetFieldValueAsync<int>(1);
                }
            }
            return (totalRecords, filteredRecords);
        }

        public async Task<List<ViewModels.OrgGroup.JobGroupResults>> GetJobOrders(string GroupID)
        {

            var res = new List<ViewModels.OrgGroup.JobGroupResults>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select distinct JobOrders.ID, JobOrders.JobName,
CONVERT(datetime, FORMAT(OverallDeliveryDeadline, 'dd MMM yyyy HH:mm')) as DeliveryDeadline,
(Case when JobOrders.OverallCompletedDateTime is not null then 'Completed'
when JobOrders.OverallDeliveryDeadline < dbo.funcGetCurrentUKTime() then 'Overdue'
when JobOrders.OverallDeliveryDeadline > dbo.funcGetCurrentUKTime() then 'In progress'end) as OrderStatus,
cast(ISNULL(OverallChargeToClient, 0) as decimal(10,2)) as Value, cast(ISNULL(JobOrders.AnticipatedGrossMarginPercentage, 0) as decimal(10,2)) as AnticipatedGrossMarginPercentage,
isnull((select dbo.funcGetSourceLangOrLangsFullStringJobOrderID(JobOrders.ID)),'(none)') as SourceLang,
isnull((select dbo.funcGetTargetLangOrLangsFullStringJobOrderID(JobOrders.ID)),'(none)') as TargetLang,
(select LocalCurrencyInfo.CurrencyName from LocalCurrencyInfo where CurrencyID = JobOrders.ClientCurrencyID) as Currency,
JobOrders.SubmittedDateTime

from JobOrders

inner join contacts on contacts.ID = joborders.ContactID
inner join Orgs on orgs.ID = Contacts.OrgID
inner join OrgGroups on orgs.OrgGroupID = OrgGroups.ID

where Orgs.OrgGroupID = " + GroupID + @" and
joborders.DeletedDate is null
order by JobOrders.SubmittedDateTime desc";
                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.OrgGroup.JobGroupResults()
                    {
                        JobId = await result.GetFieldValueAsync<int>(0),
                        JobName = await result.GetFieldValueAsync<string>(1),
                        Deadline = await result.GetFieldValueAsync<DateTime>(2),
                        JobStatus = await result.GetFieldValueAsync<string>(3),
                        JobValue = await result.GetFieldValueAsync<decimal?>(4),
                        JobMargin = await result.GetFieldValueAsync<decimal?>(5),
                        SourceLanguage = await result.GetFieldValueAsync<string>(6),
                        TargetLanguage = await result.GetFieldValueAsync<string>(7),
                        JobCurrencyName = await result.GetFieldValueAsync<string>(8)
                    });
                }
            }
            return res;
        }

        public async Task<IEnumerable<EnquiriesGroupResults>> GetEnquiriesbOrdersForDataObjectAndType(int dataObjectId, int dataTypeId, int pageNumber, int pageSize, string searchTerm, int columnToOrderBy, string orderDirection)
        {

            var res = new List<EnquiriesGroupResults>();
            string query = String.Empty;
            string selectSection = String.Empty;
            string whereSection = String.Empty;
            string secondWhereSection = String.Empty;
            string pagingSection = String.Empty;

            var columns = new Dictionary<int, string>();
            columns.Add(0, "ID");
            columns.Add(1, "JobName");
            columns.Add(2, "Status");
            columns.Add(3, "SourceLang");
            columns.Add(4, "TargetLang");
            columns.Add(5, "Reason");
            columns.Add(6, "Currency");
            columns.Add(7, "DeadlineRequestedByClient");
            columns.Add(8, "ENQCreatedDate");
            columns.Add(9, "LastModifiedDateTime");
            columns.Add(10, "[Created by]");
            columns.Add(11, "[Sales contact]");
            columns.Add(12, "SourceLangCombined");
            columns.Add(13, "TargetLangCombined");

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                selectSection = @"SELECT * FROM (select distinct Enquiries.ID, Enquiries.JobName,
CASE
    WHEN Enquiries.Status = 0 THEN 'Draft'
    WHEN Enquiries.Status = 1 THEN 'Pending'
	WHEN Enquiries.Status = 2 THEN 'Rejected'
	WHEN Enquiries.Status = 3 THEN 'Gone ahead'
WHEN Enquiries.Status = 4 THEN 'Not started'
WHEN Enquiries.Status = 5 THEN 'Sent to sales'
WHEN Enquiries.Status = 6 THEN 'Sent to Ops'
WHEN Enquiries.Status = 7 THEN 'Awaiting Feedback'
WHEN Enquiries.Status = 8 THEN 'On Hold'
WHEN Enquiries.Status = 9 THEN 'Delivered'
WHEN Enquiries.Status = 10 THEN 'In progress'
END AS 'Status',
Isnull(DecisionReasons.Reason,'(none)') as 'Reason',cast(ISNULL(Quotes.OverallChargeToClient, 0) as decimal(10,2)) as Value, 
(select dbo.funcGetSourceLangOrLangsFullStringEnquiryID(Enquiries.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryID(Enquiries.ID)) as TargetLang,
isnull((select Currencies.Prefix from Currencies where ID = Quotes.QuoteCurrencyID),'') as Currency,
isnull(Enquiries.DeadlineRequestedByClient, '1989-01-01 00:00:00.000') as 'DeadlineRequestedByClient', isnull(Enquiries.LastModifiedDateTime, '1989-01-01 00:00:00.000') as 'LastModifiedDateTime',
isnull(EL.FirstName + ' ' + EL.Surname, '(none)') as 'Last modified by', isnull(EL.ID,0) as ELID,
EC.FirstName + ' ' + EC.Surname as 'Created by', EC.ID as ECID,Enquiries.CreatedDateTime as ENQCreatedDate,
isnull(ES.FirstName + ' ' + Es.Surname,'(none)') as 'Sales contact', ISNULL(ES.ID, 0) as ESID,
(select dbo.funcGetSourceLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as SourceLangCombined,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as TargetLangCombined

from Enquiries
left outer join DecisionReasons on DecisionReasons.id = Enquiries.DecisionReasonID
left outer join contacts on contacts.ID = Enquiries.ContactID
left outer join Quotes on  quotes.EnquiryID = Enquiries.ID and Quotes.IsCurrentVersion = 1
left outer join QuoteItems on QuoteItems.QuoteID= Quotes.ID
left outer join Orgs on Contacts.OrgID = orgs.ID
left outer join Employees EC on EC.id = Enquiries.CreatedByEmployeeID
left outer join Employees EL on EL.id = Enquiries.LastModifiedByEmployeeID
left outer join Employees ES on ES.id = quotes.SalesContactEmployeeID
 ";
                if (dataTypeId == ((int)Enumerations.DataObjectTypes.Org))
                {
                    whereSection = " where Orgs.Id = " + dataObjectId + @" and
Enquiries.DeletedDateTime is null";
                }
                else if (dataTypeId == ((int)Enumerations.DataObjectTypes.Contact))
                {
                    whereSection = " where Contacts.ID = " + dataObjectId + @" and
Enquiries.DeletedDateTime is null";
                }
                else if (dataTypeId == ((int)Enumerations.DataObjectTypes.OrgGroup))
                {
                    whereSection = " where Orgs.OrgGroupID = " + dataObjectId + @" and
Enquiries.DeletedDateTime is null";
                }

                secondWhereSection = @") table1

where (SourceLangCombined + TargetLangCombined + JobName + [Status]) LIKE N'%" + searchTerm + "%'";

                pagingSection = @"
            ORDER BY table1." + columns.FirstOrDefault(x => x.Key == columnToOrderBy).Value + " " + orderDirection + @"

            OFFSET " + pageNumber + @" ROWS
            FETCH NEXT " + pageSize + " ROWS ONLY";

                query = selectSection + whereSection + secondWhereSection + pagingSection;
                command.CommandText = query;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();

                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.OrgGroup.EnquiriesGroupResults()
                    {

                        Id = await result.GetFieldValueAsync<int>(0),
                        EnqName = await result.GetFieldValueAsync<string>(1),
                        EnqStatus = await result.GetFieldValueAsync<string>(2),
                        EnqReason = await result.GetFieldValueAsync<string>(3),
                        EnqValue = await result.GetFieldValueAsync<decimal?>(4),
                        SourceLanguage = await result.GetFieldValueAsync<string>(5),
                        TargetLanguage = await result.GetFieldValueAsync<string>(6),
                        EnqCurrencyName = await result.GetFieldValueAsync<string>(7),
                        Deadline = await result.GetFieldValueAsync<DateTime>(8),
                        EnqModified = await result.GetFieldValueAsync<DateTime?>(9),
                        EnqModifiedBy = await result.GetFieldValueAsync<string>(10),
                        EnqModifiedByID = await result.GetFieldValueAsync<short?>(11),
                        SubmittedBy = await result.GetFieldValueAsync<string>(12),
                        SubmittedByID = await result.GetFieldValueAsync<short>(13),
                        SubmittedDateTime = await result.GetFieldValueAsync<DateTime>(14),
                        EnqSales = await result.GetFieldValueAsync<string>(15),
                        EnqSalesID = await result.GetFieldValueAsync<short>(16),
                        SourceLanguagesCombined = await result.GetFieldValueAsync<string>(17),
                        TargetLanguagesCombined = await result.GetFieldValueAsync<string>(18)
                    });
                }
            }
            return res;
        }

        public async Task<List<ViewModels.OrgGroup.OrgResults>> GetOrgs(string GroupID)
        {

            var result = await orgRepository.All()
                .Join(LIcountryRepository.All(),
                o => o.CountryId,
                s => s.CountryId, (o, s) => new { Org = o, LocalCountryInfo = s })
                .Where(a => a.Org.DeletedDate == null && a.LocalCountryInfo.LanguageIanacode == "en" && a.Org.OrgGroupId == Int32.Parse(GroupID)).Select(x => new ViewModels.OrgGroup.OrgResults
                { OrgID = x.Org.Id, OrgName = x.Org.OrgName, Address = x.Org.Address1, Country = x.LocalCountryInfo.CountryName }).Distinct().ToListAsync();
            return result;
        }

        public async Task<List<ViewModels.OrgGroup.OrgPriceListsResults>> GetPriceLists(string GroupID)
        {

            var result = await priceListsRepository.All()
                .Join(currencyRepository.All(),
                o => o.CurrencyId,
                s => s.CurrencyId, (o, s) => new { ClientPriceList = o, LocalCurrencyInfo = s })
                .Where(a => a.ClientPriceList.DeletedDateTime == null && a.ClientPriceList.DataObjectId == Int32.Parse(GroupID) && a.ClientPriceList.DataObjectTypeId == 3
                && a.ClientPriceList.NoLongerInForceAsOfDateTime == null).Select(x => new ViewModels.OrgGroup.OrgPriceListsResults
                { PriceListID = x.ClientPriceList.Id, PriceListName = x.ClientPriceList.Name, InForceSince = x.ClientPriceList.InForceSinceDateTime, Currency = x.LocalCurrencyInfo.CurrencyName }).ToListAsync();
            return result;
        }


        public async Task<OrgGroupViewModel> GroupUpdate(OrgGroupViewModel group, bool HQUpdate)
        {
            if (HQUpdate != true)
            {
                var dbOrgGroup = await orgGroupRepository.All().Where(x => x.Id == group.GroupID).FirstOrDefaultAsync();

                if (dbOrgGroup == null)
                    return group;


                dbOrgGroup.Name = group.GroupName;
                dbOrgGroup.Description = group.Description;
                dbOrgGroup.BrandId = (byte)group.BrandID;
                dbOrgGroup.OnlyAllowEncryptedSuppliers = group.EncryptedSuppliers;
                //dbOrgGroup.ShowProofreadingOptionForClients = group.ShowProofreadingOptionToClient;
                if (group.JobFilesCountryID != null)
                { dbOrgGroup.JobFilesToBeSavedWithinRegion = (byte)group.JobFilesCountryID; }

                dbOrgGroup.LastModifiedByEmployeeId = (short?)group.EmployeCurrentlyLoggedInID;
                dbOrgGroup.LastModifiedDate = GeneralUtils.GetCurrentUKTime();

                await orgGroupRepository.SaveChangesAsync();

                return group;
            }
            else
            {
                var dbOrgGroup = await orgGroupRepository.All().Where(x => x.Id == group.GroupID).FirstOrDefaultAsync();

                if (dbOrgGroup == null)
                    return group;



                dbOrgGroup.HqorgId = group.HQOrgID;

                dbOrgGroup.LastModifiedByEmployeeId = (short?)group.EmployeCurrentlyLoggedInID;
                dbOrgGroup.LastModifiedDate = GeneralUtils.GetCurrentUKTime();

                await orgGroupRepository.SaveChangesAsync();

                return group;
            }

        }

        public bool HasAnyVolumeDiscountSetUp(int GroupID)
        {
            var volumeDiscounts = volumeDiscountsService.GetAllVolumeDiscountsForDataObject(GroupID, 3).Result.Count;

            if (volumeDiscounts > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Boolean> CheckOrgLevelLicencesForGroup(int groupId)
        {
            var allMappings = await orgRepository.All().Where(o => o.DeletedDate == null && o.OrgGroupId == groupId)
                               .Join(flowPlusLicenceMappingRepo.All().Where(f => f.AccessForDataObjectTypeID == 2),
                                     o => o.Id,
                                     f => f.AccessForDataObjectID,
                                     (o, f) => new { licence = f }).Select(f => f.licence).ToListAsync();

            bool orgLevelLicenceExists = false;
            if (allMappings.Count > 0)
            {
                foreach (flowPlusLicenceMapping licenceMapping in allMappings)
                {
                    if (licenceMapping.flowplusLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.flowplusLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }

                    if (licenceMapping.reviewPlusLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.reviewPlusLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }

                    if (licenceMapping.translateOnlineLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.translateOnlineLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }

                    if (licenceMapping.designPlusLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.designPlusLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }

                    if (licenceMapping.AIOrMTLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.AIOrMTLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }

                    if (licenceMapping.CMSLicenceID > 0)
                    {
                        var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.CMSLicenceID.Value);
                        if (licence.IsEnabled == true)
                        {
                            orgLevelLicenceExists = true;
                            break;
                        }
                    }
                }
            }
            return orgLevelLicenceExists;
        }

        public async Task<Boolean> CheckIfGroupLevelLicenceExists(int groupId)
        {
            var licenceMapping = flowPlusLicenceMappingRepo.All().Where(f => f.AccessForDataObjectTypeID == 3 && f.AccessForDataObjectID == groupId).FirstOrDefault();

            bool groupLevelLicenceExists = false;

            if (licenceMapping != null)
            {
                if (licenceMapping.flowplusLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.flowplusLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }

                if (licenceMapping.reviewPlusLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.reviewPlusLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }

                if (licenceMapping.translateOnlineLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.translateOnlineLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }

                if (licenceMapping.designPlusLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.designPlusLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }

                if (licenceMapping.AIOrMTLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.AIOrMTLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }

                if (licenceMapping.CMSLicenceID > 0)
                {
                    var licence = await flowplusLicencingLogic.GetflowPlusLicenceObj(licenceMapping.CMSLicenceID.Value);
                    if (licence.IsEnabled == true)
                    {
                        groupLevelLicenceExists = true;
                        return groupLevelLicenceExists;
                    }
                }
            }

            return groupLevelLicenceExists;
        }

        public async Task<OrgGroup> UpdateShowProofreadingOptionSetting(int groupId, bool ShowProofreadingOptionToClient)
        {
            var group = await GetOrgGroupDetails(groupId);

            group.ShowProofreadingOptionForClients = ShowProofreadingOptionToClient;

            await orgGroupRepository.SaveChangesAsync();

            return group;

        }
        public async Task<OrgGroupViewModel> GroupContractUpdate(OrgGroupViewModel group)
        {
            
                var dbOrgGroup = await orgGroupRepository.All().Where(x => x.Id == group.GroupID).FirstOrDefaultAsync();

                if (dbOrgGroup == null)
                    return group;


                dbOrgGroup.ContractExpiryDate = group.ContractExpiryDate;
                
                dbOrgGroup.LastModifiedByEmployeeId = (short?)group.EmployeCurrentlyLoggedInID;
                dbOrgGroup.LastModifiedDate = GeneralUtils.GetCurrentUKTime();

                await orgGroupRepository.SaveChangesAsync();

                return group;
            

        }

    }
}
