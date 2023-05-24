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
using System.IO;
using ViewModels.Common;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;

namespace Services
{
    public class TPEnquiriesService : ITPEnquiriesService
    {
        private readonly IRepository<ExtranetUser> extranetUserRepo;
        private readonly IRepository<Employee> employeRepo;
        private readonly IRepository<ExtranetAccessLevels> extranetAccessLevelsRepo;
        private readonly IRepository<Enquiry> enquiriesRepo;
        private readonly IRepository<Quote> quotesRepo;
        private readonly IRepository<Contact> contactRepo;
        private readonly IRepository<Org> orgRepo;
        private readonly ITPExchangeService exchangeService;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly GlobalVariables globalVariables;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPCurrenciesLogic currencyService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPBrandsService brandService;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly ITPQuotesLogic quoteService;
        private readonly ITPLanguageLogic langService;
        private readonly ITPEmployeeOwnershipsLogic ownershipsLogicService;
        private readonly ITPEmployeesService empService;
        private readonly IEmailUtilsService emailUtils;
        private readonly ITPContactsLogic contactService;
        private readonly IRepository<EnquiryQuoteItem> enquiryquoteitemsRepo;
        private readonly ITPEnquiryQuoteItemsService enquiryquoteitemService;

        public TPEnquiriesService(IRepository<ExtranetUser> repository, IRepository<ExtranetAccessLevels> repository1,
                            IRepository<Enquiry> repository2, IRepository<Quote> repository3, IRepository<Contact> repository4,
                            IRepository<Org> repository5, ITPExchangeService exchangeService1, IRepository<Employee> employeRepo,
                            ITPTimeZonesService tPTimeZonesService, IConfiguration configuration, ITPExtranetUserService tPExtranetUserService,
                            ITPCurrenciesLogic tPCurrenciesLogic, ITPOrgsLogic tPOrgsLogic, ITPBrandsService tPBrandsService,
                            ITPMiscResourceService tPMiscResourceService, ITPQuotesLogic tPQuotesLogic,
                            ITPLanguageLogic tPLanguageLogic, ITPEmployeeOwnershipsLogic tPEmployeeOwnershipsLogic,
                            ITPEmployeesService tPEmployeesService, IEmailUtilsService emailUtilsService,
                            ITPContactsLogic tPContactsLogic,IRepository<EnquiryQuoteItem> enquiryquoteitemsrepository,
                            ITPEnquiryQuoteItemsService enquiryquoteitemService)
        {
            this.extranetUserRepo = repository;
            this.extranetAccessLevelsRepo = repository1;
            this.enquiriesRepo = repository2;
            this.quotesRepo = repository3;
            this.contactRepo = repository4;
            this.orgRepo = repository5;
            this.exchangeService = exchangeService1;
            this.employeRepo = employeRepo;
            this.timeZonesService = tPTimeZonesService;


            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.extranetUserService = tPExtranetUserService;
            this.currencyService = tPCurrenciesLogic;
            this.orgService = tPOrgsLogic;
            this.brandService = tPBrandsService;
            this.miscResourceService = tPMiscResourceService;
            this.quoteService = tPQuotesLogic;
            this.langService = tPLanguageLogic;
            this.ownershipsLogicService = tPEmployeeOwnershipsLogic;
            this.empService = tPEmployeesService;
            this.emailUtils = emailUtilsService;
            this.contactService = tPContactsLogic;
            this.enquiryquoteitemsRepo = enquiryquoteitemsrepository;
            this.enquiryquoteitemService = enquiryquoteitemService;
        }


        public string GetFileExtentions(int orgId, int enquiryID)
        {
            // in case of any file system errors, return an empty string
            // if key network job folders cannot be found
            try
            {
                // find the first matching directory within the org folder
                // which starts with the order ID, regardless of what comes after it
                string OrgDirSearchPattern = orgId.ToString() + "*";
                string matchingEnqName = string.Empty;
                string OrgDirPath = string.Empty;
                var DirInfo = new DirectoryInfo("\\\\GBLONPFSPDM0005\\Quotes");
                // find org folder first (key client info folder should then appear within that)
                var MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingOrgDirs.Count() == 0)
                {
                    string newOrgDirSearchPattern = orgId.ToString();
                    var newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (newMatchingOrgDirs.Count() == 0)
                        return "";
                    else
                    {
                        OrgDirPath = newMatchingOrgDirs[0].FullName;
                        var OrgDirInfo = new DirectoryInfo(newMatchingOrgDirs[0].FullName);
                        string EnqDirSearchPattern = enquiryID.ToString() + "*";

                        var MatchingEnq = OrgDirInfo.GetDirectories(EnqDirSearchPattern, SearchOption.TopDirectoryOnly);
                        if (MatchingEnq.Count() == 0)
                        {
                            string newEnqDirSearch = enquiryID.ToString();
                            var newEnqOrgDirs = OrgDirInfo.GetDirectories(newEnqDirSearch, SearchOption.TopDirectoryOnly);
                            if (newEnqOrgDirs.Count() == 0)
                            {
                                return "";
                            }
                            else
                            {
                                matchingEnqName = MatchingEnq[0].FullName;
                            }
                        }
                        else
                        {
                            matchingEnqName = MatchingEnq[0].FullName;
                        }
                    }

                }
                // no org folder found, so don't bother searching further
                else
                {
                    OrgDirPath = MatchingOrgDirs[0].FullName;
                    var OrgDirInfo = new DirectoryInfo(OrgDirPath);
                    string EnqDirSearchPattern = enquiryID.ToString() + "*";

                    var MatchingEnq = OrgDirInfo.GetDirectories(EnqDirSearchPattern, SearchOption.TopDirectoryOnly);
                    if (MatchingEnq.Count() == 0)
                    {
                        string newEnqDirSearch = enquiryID.ToString();
                        var newEnqOrgDirs = OrgDirInfo.GetDirectories(newEnqDirSearch, SearchOption.TopDirectoryOnly);
                        if (newEnqOrgDirs.Count() == 0)
                        {
                            return "";
                        }
                        else
                        {
                            matchingEnqName = MatchingEnq[0].FullName;
                        }
                    }
                    else
                    {
                        matchingEnqName = MatchingEnq[0].FullName;
                    }
                }



                // now look for the key client info folder within the org folder
                string SourceFilesPath = Path.Combine(matchingEnqName, "Source Files");
                string extentions = string.Empty;
                var sourceFiles = Directory.GetFiles(SourceFilesPath, "*.*", SearchOption.AllDirectories).ToList();
                if (sourceFiles.Count() == 0)
                {
                    return "";
                }
                else
                {
                    foreach (string fileName in sourceFiles)
                    {
                        if (extentions != string.Empty)
                        {
                            extentions = extentions + ", " + Path.GetExtension(fileName);
                        }
                        else
                        {
                            extentions = Path.GetExtension(fileName);
                        }

                    }
                    return extentions;
                }

            }
            catch (System.Exception ex)
            {
                return "";
            }
        }


        public async Task<ViewModels.Enquiries.EnquiriesViewModel> Update(string enqToUpdate, string enqPriorityToUpdate, string enqAssignedToUpdate, string enqNotesToUpdate)
        {
            var enqViewModel = new ViewModels.Enquiries.EnquiriesViewModel();
            if (enqToUpdate != "")
            {

                var enqID = enqToUpdate;
                var enqPriority = enqPriorityToUpdate;
                var enqAssignedTo = enqAssignedToUpdate;
                var enqNotes = enqNotesToUpdate;

                var enqObject = await enquiriesRepo.All().Where(x => x.Id == Int32.Parse(enqID)).FirstOrDefaultAsync();

                if (enqObject == null)
                    return enqViewModel;
                if (enqPriority != "99")
                {
                    enqObject.PriorityID = Int32.Parse(enqPriority);
                }
                else
                {
                    enqObject.PriorityID = null;
                }
                if (enqAssignedTo != "0")
                {
                    if (enqAssignedTo == "delete")
                    {

                        enqObject.AssignedToEmployeeID = null;
                    }
                    else
                    {
                        enqObject.AssignedToEmployeeID = Int32.Parse(enqAssignedTo);
                    }

                }
                if (enqNotes != " (none) ")
                {
                    enqObject.InternalNotes = enqNotes;
                }

                await enquiriesRepo.SaveChangesAsync();


                return enqViewModel;
            }
            return enqViewModel;


        }

        public async Task<List<ViewModels.Enquiries.SentEnquiries>> GetSentEnquiries(string startDate = null, string endDate = null, int EmployeeID = 0)
        {
            var res = new List<ViewModels.Enquiries.SentEnquiries>();
            var startDateConverted = Convert.ToDateTime(startDate);
            var endDateConverted = Convert.ToDateTime(endDate);
            var newStartDate = startDateConverted.ToString("yyyy-MM-dd");
            var newEndDate = endDateConverted.ToString("yyyy-MM-dd");
            newEndDate = newEndDate + " 23:59:59";
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select  Enquiries.id, Enquiries.CreatedDateTime , isnull(Orgs.OrgName,'(none)'),
case when Enquiries.Status = 1 then 'Sent to Client'
when Enquiries.Status = 5 then 'Sent to Sales'
when Enquiries.Status = 6 then 'Sent to Ops'
else '(none)' end as 'Status',cast(isnull(Quotes.OverallChargeToClient,'0') as decimal(10,2)) 'Charge to Client',
cast(isnull ((dbo.funcCurrencyConversion (Quotes.QuoteCurrencyID, 4, Quotes.OverallChargeToClient)), '0') as decimal(10,2)) 'Charge to client GBP'
, isnull(Currencies.Prefix,''), isnull(Enquiries.PriorityID,99), isnull(Enquiries.AssignedToEmployeeID,0)
, isnull(EP.FirstName + ' ' + EP.Surname,'(none)') as 'EmployeeAssignedTo', isnull(Orgs.ID,0),
isnull(EO.FirstName + ' ' + EO.Surname,'(none)') as 'SalesOwner', isnull(cast(EO.ID as int),0),
isnull(Enquiries.InternalNotes,'')
 from Enquiries
left outer join DecisionReasons on DecisionReasons.ID = Enquiries.DecisionReasonID
left outer join Quotes on Quotes.EnquiryID = Enquiries.ID and Quotes.DeletedDateTime is null and quotes.IsCurrentVersion = 1
left outer join Contacts on Contacts.ID = Enquiries.ContactID and contacts.DeletedDate is null
left outer join Currencies on Currencies.ID = Quotes.QuoteCurrencyID
left outer join Employees EP on EP.ID = Enquiries.AssignedToEmployeeID
left outer join Orgs on Orgs.ID = Contacts.OrgID and orgs.DeletedDate is null
left outer JOIN EmployeeOwnershipRelationships  ON  EmployeeOwnershipRelationships.DataObjectID = Orgs.ID 
and EmployeeOwnershipRelationships.DataObjectTypeID = 2 and EmployeeOwnershipTypeID in (1,4) and EmployeeOwnershipRelationships.InForceEndDateTime is null
left outer join Employees EO on EO.ID = EmployeeOwnershipRelationships.EmployeeID
left outer join OrgSalesCategories on OrgSalesCategories.ID = Orgs.Salescategoryid
 where Status in (1,5,6) and Enquiries.DeletedDateTime is null 
 and  Enquiries.CreatedDateTime between '" + newStartDate + "' and '" + newEndDate + "'";

                if (EmployeeID != 0)
                {
                    testcom = testcom + " and Enquiries.AssignedToEmployeeID = '" + EmployeeID + "'";
                }


                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {

                    res.Add(new ViewModels.Enquiries.SentEnquiries()
                    {
                        EnquiryID = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(2),
                        createdDate = await result.GetFieldValueAsync<DateTime>(1),
                        enquiryStatus = await result.GetFieldValueAsync<string>(3),
                        chargetToClient = await result.GetFieldValueAsync<decimal>(4),
                        chargetToClientGBP = await result.GetFieldValueAsync<decimal>(5),
                        enqPrefix = await result.GetFieldValueAsync<string>(6),
                        enqPriorityID = await result.GetFieldValueAsync<int>(7),
                        assignedToID = await result.GetFieldValueAsync<int>(8),
                        assignedToEmployeeName = await result.GetFieldValueAsync<string>(9),
                        orgID = await result.GetFieldValueAsync<int>(10),
                        SalesOwner = await result.GetFieldValueAsync<string>(11),
                        SalesOwnerID = await result.GetFieldValueAsync<int>(12),
                        enquiryNotes = await result.GetFieldValueAsync<string>(13)

                    });
                }

            }
            return res;

        }

        public async Task<List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>> GetPendingEnquiries(int EmployeeID = 0)
        {
            var res = new List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select  Enquiries.id, Enquiries.CreatedDateTime , isnull(Orgs.OrgName,'(none)'), isnull(OrgSalesCategories.Name,'(none)'),
case when Enquiries.Status = 0 then 'Draft'
when Enquiries.Status = 4 then 'Not started'
else '(none)' end as 'Status', 
(select dbo.funcGetSourceLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as SourceLangCombined,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as TargetLangCombined,
isnull(Enquiries.InternalNotes,''),isnull(Orgs.ID,0), isnull(cast(Orgs.SLA as nvarchar),'(none)'), isnull(Enquiries.PriorityID,99),
isnull(Enquiries.AssignedToEmployeeID,0), isnull(Enquiries.EnquiryDeadline,'1989-01-01 00:00:00.000'), isnull(EP.FirstName + ' ' + EP.Surname,'(none)') as 'EmployeeAssignedTo'
, (select dbo.funcGetSourceLangOrLangsFullStringEnquiryID(Enquiries.ID)) as sourceCount,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryID(Enquiries.ID)) as targetCount, isnull(Orgs.ID,0),
isnull(EO.FirstName + ' ' + EO.Surname,'(none)') as 'SalesOwner', isnull(cast(EO.ID as int),0),  isnull(cast(Enquiries.WinChance as int),-1), isnull(Enquiries.ClientDecisionDate,'1989-01-01 00:00:00.000')
from Enquiries
left outer join DecisionReasons on DecisionReasons.ID = Enquiries.DecisionReasonID
left outer join Quotes on Quotes.EnquiryID = Enquiries.ID and Quotes.DeletedDateTime is null and quotes.IsCurrentVersion = 1
left outer join Contacts on Contacts.ID = Enquiries.ContactID and contacts.DeletedDate is null
left outer join Orgs on Orgs.ID = Contacts.OrgID and orgs.DeletedDate is null
left outer join Employees EP on EP.ID = Enquiries.AssignedToEmployeeID
left outer JOIN EmployeeOwnershipRelationships  ON  EmployeeOwnershipRelationships.DataObjectID = Orgs.ID 
and EmployeeOwnershipRelationships.DataObjectTypeID = 2 and EmployeeOwnershipTypeID in (1,4) and EmployeeOwnershipRelationships.InForceEndDateTime is null
left outer join Employees EO on EO.ID = EmployeeOwnershipRelationships.EmployeeID
left outer join OrgSalesCategories on OrgSalesCategories.ID = Orgs.Salescategoryid
 where Status in (0,4) and Enquiries.DeletedDateTime is null";

                if (EmployeeID != 0)
                {
                    testcom = testcom + " and Enquiries.AssignedToEmployeeID = '" + EmployeeID + "'";
                }

                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {


                    res.Add(new ViewModels.Enquiries.PendingOrNotStartedEnquiries()
                    {
                        EnquiryID = await result.GetFieldValueAsync<int>(0),
                        createdDate = await result.GetFieldValueAsync<DateTime>(1),
                        orgName = await result.GetFieldValueAsync<string>(2),
                        orgStatus = await result.GetFieldValueAsync<string>(3),
                        enquiryStatus = await result.GetFieldValueAsync<string>(4),
                        sourceLanguages = await result.GetFieldValueAsync<string>(5),
                        targetLanguages = await result.GetFieldValueAsync<string>(6),
                        enquiryNotes = await result.GetFieldValueAsync<string>(7),
                        //enqfileExtentions = GetFileExtentions(await result.GetFieldValueAsync<int>(8), await result.GetFieldValueAsync<int>(0)),
                        orgSLA = await result.GetFieldValueAsync<string>(9),
                        enqPriorityID = await result.GetFieldValueAsync<int>(10),
                        assignedToEmployeeID = await result.GetFieldValueAsync<int>(11),
                        clientDeadline = await result.GetFieldValueAsync<DateTime?>(12),
                        assignedToEmployeeName = await result.GetFieldValueAsync<string>(13),
                        sourceLanguagesCount = await result.GetFieldValueAsync<string>(14),
                        targetLanguagesCount = await result.GetFieldValueAsync<string>(15),
                        orgID = await result.GetFieldValueAsync<int>(16),
                        SalesOwner = await result.GetFieldValueAsync<string>(17),
                        SalesOwnerID = await result.GetFieldValueAsync<int>(18),
                        WinChance = await result.GetFieldValueAsync<int>(19),
                        ClientDecisionDate = await result.GetFieldValueAsync<DateTime?>(20)
                    });
                }

            }
            return res;

        }

        public async Task<List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>> GetPendingEnquiriesNotPGD(int EmployeeID = 0)
        {
            var res = new List<ViewModels.Enquiries.PendingOrNotStartedEnquiries>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select  Enquiries.id, Enquiries.CreatedDateTime , isnull(Orgs.OrgName,'(none)'), isnull(OrgSalesCategories.Name,'(none)'),
case when Enquiries.Status = 0 then 'Draft'
when Enquiries.Status = 4 then 'Not started'
else '(none)' end as 'Status', 
(select dbo.funcGetSourceLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as SourceLangCombined,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryIDAllLanguages(Enquiries.ID)) as TargetLangCombined,
isnull(Enquiries.InternalNotes,''),isnull(Orgs.ID,0), isnull(cast(Orgs.SLA as nvarchar),'(none)'), isnull(Enquiries.PriorityID,99),
isnull(Enquiries.AssignedToEmployeeID,0), isnull(Enquiries.EnquiryDeadline,'1989-01-01 00:00:00.000'), isnull(EP.FirstName + ' ' + EP.Surname,'(none)') as 'EmployeeAssignedTo'
, (select dbo.funcGetSourceLangOrLangsFullStringEnquiryID(Enquiries.ID)) as sourceCount,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryID(Enquiries.ID)) as targetCount, isnull(Orgs.ID,0),
isnull(EO.FirstName + ' ' + EO.Surname,'(none)') as 'SalesOwner', isnull(cast(EO.ID as int),0),  isnull(cast(Enquiries.WinChance as int),-1), isnull(Enquiries.ClientDecisionDate,'1989-01-01 00:00:00.000')
from Enquiries
left outer join DecisionReasons on DecisionReasons.ID = Enquiries.DecisionReasonID
left outer join Quotes on Quotes.EnquiryID = Enquiries.ID and Quotes.DeletedDateTime is null and quotes.IsCurrentVersion = 1
left outer join Contacts on Contacts.ID = Enquiries.ContactID and contacts.DeletedDate is null
left outer join Orgs on Orgs.ID = Contacts.OrgID and orgs.DeletedDate is null
left outer join Employees EP on EP.ID = Enquiries.AssignedToEmployeeID
left outer JOIN EmployeeOwnershipRelationships  ON  EmployeeOwnershipRelationships.DataObjectID = Orgs.ID 
and EmployeeOwnershipRelationships.DataObjectTypeID = 2 and EmployeeOwnershipTypeID in (1,4) and EmployeeOwnershipRelationships.InForceEndDateTime is null
left outer join Employees EO on EO.ID = EmployeeOwnershipRelationships.EmployeeID
left outer join OrgSalesCategories on OrgSalesCategories.ID = Orgs.Salescategoryid
where Status in (0,4) and Enquiries.DeletedDateTime is null and Orgs.OrgGroupID not in(72112, 72113)";

                if (EmployeeID != 0)
                {
                    testcom = testcom + " and Enquiries.AssignedToEmployeeID = '" + EmployeeID + "'";
                }

                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {


                    res.Add(new ViewModels.Enquiries.PendingOrNotStartedEnquiries()
                    {
                        EnquiryID = await result.GetFieldValueAsync<int>(0),
                        createdDate = await result.GetFieldValueAsync<DateTime>(1),
                        orgName = await result.GetFieldValueAsync<string>(2),
                        orgStatus = await result.GetFieldValueAsync<string>(3),
                        enquiryStatus = await result.GetFieldValueAsync<string>(4),
                        sourceLanguages = await result.GetFieldValueAsync<string>(5),
                        targetLanguages = await result.GetFieldValueAsync<string>(6),
                        enquiryNotes = await result.GetFieldValueAsync<string>(7),
                        //enqfileExtentions = GetFileExtentions(await result.GetFieldValueAsync<int>(8), await result.GetFieldValueAsync<int>(0)),
                        orgSLA = await result.GetFieldValueAsync<string>(9),
                        enqPriorityID = await result.GetFieldValueAsync<int>(10),
                        assignedToEmployeeID = await result.GetFieldValueAsync<int>(11),
                        clientDeadline = await result.GetFieldValueAsync<DateTime?>(12),
                        assignedToEmployeeName = await result.GetFieldValueAsync<string>(13),
                        sourceLanguagesCount = await result.GetFieldValueAsync<string>(14),
                        targetLanguagesCount = await result.GetFieldValueAsync<string>(15),
                        orgID = await result.GetFieldValueAsync<int>(16),
                        SalesOwner = await result.GetFieldValueAsync<string>(17),
                        SalesOwnerID = await result.GetFieldValueAsync<int>(18),
                        WinChance = await result.GetFieldValueAsync<int>(19),
                        ClientDecisionDate = await result.GetFieldValueAsync<DateTime?>(20)

                    });
                }

            }
            return res;

        }


        public async Task<List<ViewModels.Enquiries.GoneOrRejectedEnquiries>> GetApprovedRejectedEnquiries(string startDate, string endDate, int EmployeeID = 0)
        {
            var res = new List<ViewModels.Enquiries.GoneOrRejectedEnquiries>();

            var startDateConverted = Convert.ToDateTime(startDate);
            var endDateConverted = Convert.ToDateTime(endDate);
            var newStartDate = startDateConverted.ToString("yyyy-MM-dd");
            var newEndDate = endDateConverted.ToString("yyyy-MM-dd");
            newEndDate = newEndDate + " 23:59:59";
            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select  Enquiries.id, Enquiries.CreatedDateTime , isnull(Orgs.OrgName,'(none)'),
case when Enquiries.Status = 2 then 'Rejected'
when Enquiries.Status = 3 then 'Gone ahead'
else '(none)' end as 'Status', isnull(DecisionReasons.Reason,'(none)'),
cast(isnull(Quotes.OverallChargeToClient,'0') as decimal(10,2)) 'Charge to Client',
cast(isnull ((dbo.funcCurrencyConversion (Quotes.QuoteCurrencyID, 4, Quotes.OverallChargeToClient)), '0') as decimal(10,2)) 'Charge to client GBP'
, isnull(Currencies.Prefix,''), isnull(Enquiries.WentAheadAsJobOrderID,0),
isnull(EP.FirstName + ' ' + EP.Surname,'(none)') as 'EmployeeAssignedTo', isnull(Orgs.ID,0),
isnull(EO.FirstName + ' ' + EO.Surname,'(none)') as 'SalesOwner', isnull(cast(EO.ID as int),0),
isnull(Enquiries.InternalNotes,'')
 from Enquiries
left outer join DecisionReasons on DecisionReasons.ID = Enquiries.DecisionReasonID
left outer join Quotes on Quotes.EnquiryID = Enquiries.ID and Quotes.DeletedDateTime is null and quotes.IsCurrentVersion = 1
left outer join Contacts on Contacts.ID = Enquiries.ContactID and contacts.DeletedDate is null
left outer join Employees EP on EP.ID = Enquiries.AssignedToEmployeeID
left outer join Currencies on Currencies.ID = Quotes.QuoteCurrencyID
left outer join Orgs on Orgs.ID = Contacts.OrgID and orgs.DeletedDate is null
left outer JOIN EmployeeOwnershipRelationships  ON  EmployeeOwnershipRelationships.DataObjectID = Orgs.ID 
and EmployeeOwnershipRelationships.DataObjectTypeID = 2 and EmployeeOwnershipTypeID in (1,4) and EmployeeOwnershipRelationships.InForceEndDateTime is null
left outer join Employees EO on EO.ID = EmployeeOwnershipRelationships.EmployeeID
left outer join OrgSalesCategories on OrgSalesCategories.ID = Orgs.Salescategoryid
 where Status in (2,3) and Enquiries.DeletedDateTime is null 
  and  Enquiries.DecisionMadeDateTime between '" + newStartDate + "' and '" + newEndDate + "'";

                if (EmployeeID != 0)
                {
                    testcom = testcom + " and Enquiries.AssignedToEmployeeID = '" + EmployeeID + "'";
                }

                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {


                    res.Add(new ViewModels.Enquiries.GoneOrRejectedEnquiries()
                    {
                        EnquiryID = await result.GetFieldValueAsync<int>(0),
                        orgName = await result.GetFieldValueAsync<string>(2),
                        createdDate = await result.GetFieldValueAsync<DateTime>(1),
                        enquiryStatus = await result.GetFieldValueAsync<string>(3),
                        enqReason = await result.GetFieldValueAsync<string>(4),
                        chargetToClient = await result.GetFieldValueAsync<decimal>(5),
                        chargetToClientGBP = await result.GetFieldValueAsync<decimal>(6),
                        enqPrefix = await result.GetFieldValueAsync<string>(7),
                        approvedJobOrderID = await result.GetFieldValueAsync<int>(8),
                        assignedTo = await result.GetFieldValueAsync<string>(9),
                        orgID = await result.GetFieldValueAsync<int>(10),
                        SalesOwner = await result.GetFieldValueAsync<string>(11),
                        SalesOwnerID = await result.GetFieldValueAsync<int>(12),
                        enquiryNotes = await result.GetFieldValueAsync<string>(13)
                    });
                }

            }
            return res;

        }

        public async Task<int> GetNumberOfPendingEnquiriesForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();



            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();

                int result = enquiriesRepo.All().Where(e => e.DeletedDateTime == null && (e.Status == 0 || e.Status == 1))
                                   .Select(e => new { e.Id, e.ContactId })
                                   .Join(contactRepo.All(),
                                    e => e.ContactId,
                                    c => c.Id,
                                    (e, c) => new { EnquiryId = e.Id, orgId = c.OrgId })
                                   .Join(orgRepo.All(),
                                   c => c.orgId,
                                   o => o.Id,
                                   (c, o) => new { EnquiryId = c.EnquiryId, groupId = o.OrgGroupId }).Where(e => e.groupId == groupID)
                                   .Select(e => e.EnquiryId).Distinct().Count();

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                int result = enquiriesRepo.All().Where(e => e.DeletedDateTime == null && (e.Status == 0 || e.Status == 1))
                            .Select(e => new { e.Id, e.ContactId })
                                   .Join(contactRepo.All(),
                                    e => e.ContactId,
                                    c => c.Id,
                                    (e, c) => new { EnquiryId = e.Id, orgId = c.OrgId }).Where(e => e.orgId == orgID)
                                   .Select(e => e.EnquiryId).Distinct().Count();

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                int result = enquiriesRepo.All().Where(e => e.DeletedDateTime == null && e.ContactId == clientContactID && (e.Status == 0 || e.Status == 1))
                                   .Select(e => e.Id).Distinct().Count();

                return result;
            }

        }

        public async Task<decimal> GetValueOfPendingEnquiriesForClient(string extranetUserName)
        {
            int accessLevelID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(e => e.AccessLevelId).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherGroupOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherGroupOrders).FirstOrDefaultAsync();

            bool canViewDetailsOfOtherOrgOrders = await extranetAccessLevelsRepo.All().Where(a => a.Id == accessLevelID).Select(al => al.CanViewDetailsOfOtherOrgOrders).FirstOrDefaultAsync();

            short? defaultCurrency = extranetUserService.GetExtranetUserOrg(extranetUserName).Result.InvoiceCurrencyId;

            if (defaultCurrency == null)
            {
                defaultCurrency = 4;
            }


            if (canViewDetailsOfOtherGroupOrders == true)
            {
                int? groupID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId })
                                    .Join(orgRepo.All(),
                                          c => c.orgId,
                                          o => o.Id,
                                          (c, o) => new { grID = o.OrgGroupId }).Select(g => g.grID).FirstOrDefaultAsync();


                var result = quotesRepo.All().Where(q => q.DeletedDateTime == null && q.IsCurrentVersion == true)
                                       .Select(q => new { q.EnquiryId, q.QuoteCurrencyId, q.OverallChargeToClient })
                                       .Join(enquiriesRepo.All().Where(e => e.DeletedDateTime == null && (e.Status == 0 || e.Status == 1)),
                                       q => q.EnquiryId,
                                       e => e.Id,
                                       (q, e) => new { Quotes = q, contactId = e.ContactId })
                                       .Join(contactRepo.All(),
                                        e => e.contactId,
                                        c => c.Id,
                                        (e, c) => new { Quotes = e.Quotes, orgId = c.OrgId })
                                       .Join(orgRepo.All().Where(e => e.OrgGroupId == groupID),
                                       c => c.orgId,
                                       o => o.Id,
                                       (c, o) => new { Quotes = c.Quotes })
                                       .Select(s => s.Quotes).Distinct()
                                       .Select(q => new { Charge = exchangeService.Convert(q.QuoteCurrencyId, defaultCurrency.Value, q.OverallChargeToClient.Value) }).ToList().Sum(a => a.Charge);

                return result;

            }
            else if (canViewDetailsOfOtherOrgOrders == true)
            {
                int? orgID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId)
                                    .Join(contactRepo.All(),
                                          e => e,
                                          c => c.Id,
                                          (e, c) => new { orgId = c.OrgId }).Select(g => g.orgId).FirstOrDefaultAsync();

                var result = quotesRepo.All().Where(q => q.DeletedDateTime == null && q.IsCurrentVersion == true)
                                       .Select(q => new { q.EnquiryId, q.QuoteCurrencyId, q.OverallChargeToClient })
                                       .Join(enquiriesRepo.All().Where(e => e.DeletedDateTime == null && (e.Status == 0 || e.Status == 1)),
                                       q => q.EnquiryId,
                                       e => e.Id,
                                       (q, e) => new { Quotes = q, contactId = e.ContactId })
                                       .Join(contactRepo.All().Where(e => e.OrgId == orgID),
                                        e => e.contactId,
                                        c => c.Id,
                                        (e, c) => new { Quotes = e.Quotes })
                                       .Select(s => s.Quotes).Distinct()
                                       .Select(q => new { Charge = exchangeService.Convert(q.QuoteCurrencyId, defaultCurrency.Value, q.OverallChargeToClient.Value) }).ToList().Sum(a => a.Charge);

                return result;
            }
            else
            {
                var clientContactID = await extranetUserRepo.All().Where(e => e.UserName == extranetUserName).Select(c => c.DataObjectId).FirstOrDefaultAsync();

                var result = quotesRepo.All().Where(q => q.DeletedDateTime == null && q.IsCurrentVersion == true)
                                       .Select(q => new { q.EnquiryId, q.QuoteCurrencyId, q.OverallChargeToClient })
                                       .Join(enquiriesRepo.All().Where(e => e.DeletedDateTime == null && e.ContactId == clientContactID && (e.Status == 0 || e.Status == 1)),
                                       q => q.EnquiryId,
                                       e => e.Id,
                                       (q, e) => new { Quotes = q })
                                       .Select(s => s.Quotes).Distinct()
                                       .Select(q => new { Charge = exchangeService.Convert(q.QuoteCurrencyId, defaultCurrency.Value, q.OverallChargeToClient.Value) }).ToList().Sum(a => a.Charge);

                return result;

            }
        }

        public async Task<Enquiry> CreateEnquiry(int contactId, byte orderChannelID, string externalNotes, string internalNotes, string jobName,
                                     short createdByEmployeeID, DateTime? deadlineRequestedByClient = null, bool printingProject = false,
                                     bool fromExternalServer = false, DateTime? EnquiryDeadline = null, int? assignedToEmployeeID = null)
        {
            var orderContact = await contactRepo.All().Where(o => o.Id == contactId && o.DeletedDate == null).FirstOrDefaultAsync();

            var org = await orgRepo.All().Where(o => o.Id == orderContact.OrgId).FirstOrDefaultAsync();

            if (orderContact == null)
            {
                throw new Exception("The contact ID, " + contactId.ToString() + ", does not exist in the database. An order must be created against a valid contact.");
            }
            else
            {
                if (externalNotes != null)
                {
                    externalNotes = externalNotes.Trim();

                }
                if (internalNotes != null)
                {
                    internalNotes = internalNotes.Trim();
                }

                if (jobName != null)
                {
                    jobName = jobName.Trim();
                }

                DateTime EnquiryDeadlineTOUse = DateTime.MinValue;
                if (EnquiryDeadline == null)
                {
                    EnquiryDeadline = timeZonesService.GetCurrentGMT().AddDays(2);
                }
                else
                {
                    EnquiryDeadlineTOUse = (DateTime)EnquiryDeadline;
                }

                if (EnquiryDeadlineTOUse.DayOfWeek == DayOfWeek.Saturday || EnquiryDeadlineTOUse.DayOfWeek == DayOfWeek.Sunday || (EnquiryDeadlineTOUse.DayOfWeek == DayOfWeek.Friday && EnquiryDeadlineTOUse.Hour > 17) || (EnquiryDeadlineTOUse.DayOfWeek == DayOfWeek.Monday && EnquiryDeadlineTOUse.Hour < 7))
                {
                    while (EnquiryDeadlineTOUse.DayOfWeek != DayOfWeek.Monday)
                    {
                        EnquiryDeadlineTOUse = EnquiryDeadlineTOUse.AddDays(1);
                    }
                    EnquiryDeadlineTOUse = new DateTime(EnquiryDeadlineTOUse.Year, EnquiryDeadlineTOUse.Month, EnquiryDeadlineTOUse.Day, 10, 0, 0);
                }

                short createdById = globalVariables.iplusEmployeeID;

                if (createdByEmployeeID > 0)
                {
                    createdById = createdByEmployeeID;
                }
                Enquiry newEnquiry;

                newEnquiry = new Enquiry()
                {
                    ContactId = contactId,
                    OrderChannelId = orderChannelID,
                    ExternalNotes = externalNotes,
                    InternalNotes = internalNotes,
                    JobName = jobName,
                    DeadlineRequestedByClient = deadlineRequestedByClient,
                    CreatedDateTime = timeZonesService.GetCurrentGMT(),
                    CreatedByEmployeeId = createdById,
                    PrintingProject = printingProject,
                    EnquiryDeadline = EnquiryDeadline,
                    AssignedToEmployeeID = assignedToEmployeeID
                };

                await enquiriesRepo.AddAsync(newEnquiry);
                await enquiriesRepo.SaveChangesAsync();

                return newEnquiry;

            }
        }

        public async Task<bool> EnquiryExists(int enquiryId)
        {
            var result = await GetEnquiryById(enquiryId);

            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public async Task<Enquiry> GetEnquiryById(int enquiryId)
        {
            var result = await enquiriesRepo.All().Where(e => e.Id == enquiryId).FirstOrDefaultAsync();

            return result;
        }

        public string ExtranetAndWSDirectoryPathForApp(int enquiryId)
        {
            //Unlike our internal folder structures, the Extranet and Web Service
            //will be based (for now) on individual contacts, rather than orgs.

            //Find the first matching job order directory within the contact folder
            //which starts with the order ID, regardless of what comes after it
            var enquiry = enquiriesRepo.All().Where(x => x.Id == enquiryId).FirstOrDefault();

            string ContactDirSearchPattern = enquiry.ContactId.ToString() + "*";

            string EnquiryDirSearchPattern = enquiry.Id.ToString() + "*";

            string ExtranetBaseDir = Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), enquiry.ContactId.ToString());

            if (System.IO.Directory.Exists(ExtranetBaseDir) == true)
            {
                DirectoryInfo DirInfo = new DirectoryInfo(ExtranetBaseDir);

                DirectoryInfo[] MatchingDirs = DirInfo.GetDirectories(EnquiryDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (MatchingDirs.Count() == 0)
                {
                    return "";
                }
                else
                {
                    return MatchingDirs.ElementAt(0).FullName;
                }

            }
            else
            {
                return "";
            }

        }

        public void AnnounceThisEnquiryCreation(int quoteId, string SendingEmailAddress, string SubjectLine, bool ExternalNotification, bool AcknowledgeAsQuote,
                                             string UILangIANACode = "", string CustomerSpecificMessage = "", bool IsFileInDTPFormat = true,
                                             bool IsPrintingProject = false)
        {
            string MessageBodyCore = "";
            string RecipientEmailAddress = "";


            Quote quote = quoteService.GetQuoteById(quoteId).Result;
            Enquiry enquiry = enquiriesRepo.All().Where(o => o.Id == quote.EnquiryId).FirstOrDefault();
            Contact contact = contactService.GetContactDetails(enquiry.ContactId).Result;
            ExtranetUsersTemp extranetUser = extranetUserService.GetExtranetUserByContactId(contact.Id).Result;
            Org org = orgService.GetOrgDetails(contact.OrgId).Result;
            LocalCurrencyInfo clientCurrency = currencyService.GetCurrencyInfo(quote.QuoteCurrencyId, "en").Result;

            if (ExternalNotification == true)
            {
                if (enquiry.OrderChannelId == 7 || enquiry.OrderChannelId == 18)
                {
                    RecipientEmailAddress = extranetUser.WebServiceNotificationEmailAddress;
                }
                else
                {
                    RecipientEmailAddress = contact.EmailAddress;
                }


                Brand currentBrand = brandService.GetBrandForClient(org.OrgGroupId.Value).Result;

                if (AcknowledgeAsQuote == true)
                {
                    var messageBody = miscResourceService.GetMiscResourceByName("QuoteRequestReceivedEmailBody1", "en").Result;
                    MessageBodyCore = "<p>" + String.Format(messageBody.StringContent.Replace("iplus.{tpbrand}.com", currentBrand.DomainName).Replace("i&nbsp;plus", currentBrand.ApplicationName),
                                                            contact.Name, enquiry.ContactId.ToString()) + "</p><br />";
                }
                else
                {
                    var messageBody = miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailBody1", "en").Result;
                    MessageBodyCore = "<p>" + String.Format(messageBody.StringContent.Replace("{tp brand}", currentBrand.CompanyNameToShow).Replace("iplus.{tpbrand}.com", currentBrand.DomainName).Replace("i&nbsp;plus", currentBrand.ApplicationName),
                                                            contact.Name, contact.Id.ToString()) + "</p><br />";
                }

                var messageBody1 = miscResourceService.GetMiscResourceByName("EmailCoreOrderDetails", "en").Result;
                var currencyInfo = currencyService.GetCurrencyInfo(quote.QuoteCurrencyId, "en").Result;
                MessageBodyCore += "<p>" + String.Format(messageBody1.StringContent,
                                                        System.Web.HttpUtility.HtmlEncode(enquiry.JobName),
                                                        enquiry.DeadlineRequestedByClient.Value.ToString("d MMMM yyyy HH:mm"),
                                                        System.Web.HttpUtility.HtmlEncode(quote.ClientPonumber),
                                                        currencyInfo.CurrencyName,
                                                        System.Web.HttpUtility.HtmlEncode(enquiry.ExternalNotes),
                                                        GetTotalSourceLangsCountString(quoteId, "en")) + " <br />";

                List<QuoteItem> allJobItems = new List<QuoteItem>();
                try
                {
                    allJobItems = quoteService.GetAllQuoteItems(quoteId).Result;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10);
                    allJobItems = quoteService.GetAllQuoteItems(quoteId).Result;
                }
                foreach (QuoteItem item in allJobItems)
                {
                    if (item.LanguageServiceId != 21)
                    {
                        MessageBodyCore += String.Format("{0} - Item number" + ": {1}", langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name, item.Id.ToString()) + " <br />";
                    }
                }

                MessageBodyCore += "</p> <br />";

                //Check to see whether to include others in the e-mail
                if (contact.IncludeInNotificationsOn == true && contact.IncludeInNotifications != "")
                {
                    RecipientEmailAddress = RecipientEmailAddress + "," + contact.IncludeInNotifications;
                }
                else
                {
                    if (org.IncludeInNotificationsOn == true && org.IncludeInNotifications != "")
                    {
                        RecipientEmailAddress = RecipientEmailAddress + "," + org.IncludeInNotifications;
                    }
                }


            }
            else
            {
                List<Employee> allSalesOwners = new List<Employee>();

                var salesOnwer = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead).Result;

                if (salesOnwer != null)
                {
                    allSalesOwners.Add(empService.IdentifyCurrentUserById(salesOnwer.EmployeeId).Result);
                }
                salesOnwer = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead).Result;

                if (salesOnwer != null)
                {
                    allSalesOwners.Add(empService.IdentifyCurrentUserById(salesOnwer.EmployeeId).Result);
                }

                if (allSalesOwners.Count == 0)
                {
                    RecipientEmailAddress = "";
                }
                else
                {
                    foreach (Employee emp in allSalesOwners)
                    {
                        if (emp != null)
                        {
                            if (RecipientEmailAddress == "")
                            {
                                RecipientEmailAddress = emp.EmailAddress + ", ";
                            }
                            else
                            {
                                RecipientEmailAddress += emp.EmailAddress + ", ";
                            }

                        }
                    }
                }

                var opsOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.OperationsLead).Result;
                var clientIntroOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.ClientIntroLead).Result;

                if (opsOwner == null)
                {
                    if (clientIntroOwner != null)
                    {
                        var clientIntoEmpObj = empService.IdentifyCurrentUserById(clientIntroOwner.EmployeeId).Result;
                        RecipientEmailAddress += clientIntoEmpObj.EmailAddress;
                    }
                    else
                    {
                        RecipientEmailAddress += globalVariables.InternalNotificationRecipientAddress;
                    }
                }
                else
                {
                    var opsOnwerEmpObj = empService.IdentifyCurrentUserById(opsOwner.EmployeeId).Result;
                    Employee opsOwnersManager = null;

                    if (opsOnwerEmpObj.Manager != null)
                    {
                        opsOwnersManager = empService.GetManagerOfEmployee(opsOnwerEmpObj.Manager.Value).Result;
                    }

                    if (opsOwnersManager == null)
                    {
                        RecipientEmailAddress += opsOnwerEmpObj.EmailAddress;
                    }
                    else
                    {
                        RecipientEmailAddress += opsOnwerEmpObj.EmailAddress + ", " + opsOwnersManager.EmailAddress;
                    }

                    if (clientIntroOwner != null)
                    {
                        var clientIntoEmpObj = empService.IdentifyCurrentUserById(clientIntroOwner.EmployeeId).Result;
                        RecipientEmailAddress += clientIntoEmpObj.EmailAddress;
                    }

                }

                var enqOwner = ownershipsLogicService.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.EnquiriesLead).Result;

                if (enqOwner != null)
                {
                    var enqOwnerEmpObj = empService.IdentifyCurrentUserById(enqOwner.EmployeeId).Result;
                    RecipientEmailAddress += ", " + enqOwnerEmpObj.EmailAddress;
                }

                RecipientEmailAddress += ", Enquiries@translateplus.com";

                try
                {
                    if (enquiry.OrderChannelId != 7 && enquiry.OrderChannelId != 8 && enquiry.OrderChannelId != 12 &&
                        enquiry.OrderChannelId != 18 && enquiry.OrderChannelId != 21 && enquiry.OrderChannelId != 22)
                    {
                        MessageBodyCore = "<p><b>Enquiry number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + enquiry.Id.ToString() + "\">" + enquiry.Id.ToString() + "</a></p><br />" +
                                          "<p>The source files are in the newly created enquiry folder:<br />" +
                                           "<a href=\"file://" + EnquiryDirectoryPathForUser(enquiry.Id) + "\">" + EnquiryDirectoryPathForUser(enquiry.Id) + "</a></p><br />" +
                                           "<p><b>Reference:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.JobName) + " <br />" +
                                           "<b>Deadline (GMT): </b> " + enquiry.DeadlineRequestedByClient.Value.ToString("d MMMM yyyy HH:mm") + " <br />" +
                                           "<b>Purchase order number:</b> " + System.Web.HttpUtility.HtmlEncode(quote.ClientPonumber) + "<br />" +
                                           "<b>Currency:</b> " + clientCurrency.CurrencyName + " <br />" +
                                           "<b>Notes:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.ExternalNotes) + " </p>" +
                                           CustomerSpecificMessage + "<br />" +
                                           "<p><b>Source language:</b> " + GetTotalSourceLangsCountString(quoteId, "en") + " <br />" +
                                           "<b>Target language(s):</b> <br />";
                    }
                    else
                    {
                        MessageBodyCore = "<p><b>Enquiry number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + enquiry.Id.ToString() + "\">" + enquiry.Id.ToString() + "</a></p><br />" +
                                          "<p><b>Reference:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.JobName) + " <br />" +
                                          "<b>Deadline (GMT): </b> " + enquiry.DeadlineRequestedByClient.Value.ToString("d MMMM yyyy HH:mm") + " <br />" +
                                          "<b>Purchase order number:</b> " + System.Web.HttpUtility.HtmlEncode(quote.ClientPonumber) + "<br />" +
                                          "<b>Currency:</b> " + clientCurrency.CurrencyName + " <br />" +
                                          "<b>Notes:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.ExternalNotes) + " </p>" +
                                          CustomerSpecificMessage + "<br />" +
                                          "<p><b>Source language:</b> " + GetTotalSourceLangsCountString(quoteId, "en") + " <br />" +
                                          "<b>Target language(s):</b> <br />";
                    }


                }

                catch (Exception ex)
                {
                    MessageBodyCore = "<p><b>Enquiry number:</b> <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + enquiry.Id.ToString() + "\">" + enquiry.Id.ToString() + "</a></p><br />" +
                                       "<p><b>Reference:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.JobName) + " <br />" +
                                       "<b>Deadline (GMT): </b> " + enquiry.DeadlineRequestedByClient.Value.ToString("d MMMM yyyy HH:mm") + " <br />" +
                                       "<b>Purchase order number:</b> " + System.Web.HttpUtility.HtmlEncode(quote.ClientPonumber) + "<br />" +
                                       "<b>Currency:</b> " + clientCurrency.CurrencyName + " <br />" +
                                       "<b>Notes:</b> " + System.Web.HttpUtility.HtmlEncode(enquiry.ExternalNotes) + " </p>" +
                                       CustomerSpecificMessage + "<br />" +
                                       "<p><b>Source language:</b> " + GetTotalSourceLangsCountString(quoteId, "en") + " <br />" +
                                       "<b>Target language(s):</b> <br />";
                }

                List<QuoteItem> allJobItems = new List<QuoteItem>();
                try
                {
                    allJobItems = quoteService.GetAllQuoteItems(quoteId).Result;
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(10);
                    allJobItems = quoteService.GetAllQuoteItems(quoteId).Result;
                }

                foreach (QuoteItem item in allJobItems)
                {
                    //for internal use, it doesn't matter if the items are "client-visible"
                    //or not, but as of April 2011, do notify about client review job items
                    if (item.LanguageServiceId == 21)
                    {
                        MessageBodyCore += "Client review - " + langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name + " - Quote item number: <a href=\"https://myplusbeta.publicisgroupe.net/Quote?ID=109104&QuoteItemID=" + item.Id.ToString() + "\">" + item.Id.ToString() + "</a><br />";
                    }
                    else
                    {
                        MessageBodyCore += langService.GetLanguageInfo(item.TargetLanguageIanacode, "en").Name + " - Quote item number: <a href=\"https://myplusbeta.publicisgroupe.net/Quote?ID=109104&QuoteItemID=" + item.Id.ToString() + "\">" + item.Id.ToString() + "</a><br />";
                    }
                }

                if (IsPrintingProject == true)
                {
                    MessageBodyCore += "</p> <br />";
                    MessageBodyCore += "<p> This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager.";
                }


                if (IsFileInDTPFormat == false)
                {
                    MessageBodyCore += "<p><b><font color=\"red\"> The client indicated that they wanted translate plus to carry out full DTP. However, the file type(s) submitted do not appear to be recognised DTP formats, so no DTP items have been created in the intranet, as this may have been a default selection on the part of the client. Please check carefully to see if we should in fact add DTP items for this request. </font></b></p> ";
                }

            }
            if (globalVariables.CurrentAppModeString == "PRODUCTION")
            {
                emailUtils.SendMail(SendingEmailAddress,
                                    RecipientEmailAddress,
                                    SubjectLine,
                                    MessageBodyCore, true,
                                    IsExternalNotification: ExternalNotification);
            }
            else
            {
                emailUtils.SendMail(SendingEmailAddress,
                                    "ariand@translateplus.com, kavitaj@translateplus.com",
                                    SubjectLine,
                                    MessageBodyCore, true,
                                    IsExternalNotification: ExternalNotification);
            }


        }

        public string GetTotalSourceLangsCountString(int quoteId, string IANACode)
        {
            var result = quoteService.GetAllQuoteItems(quoteId).Result;

            List<string> allSourceLang = new List<string>();
            if (result.Count > 0)
            {
                foreach (QuoteItem item in result)
                {
                    if (allSourceLang.Contains(item.SourceLanguageIanacode) == false)
                    {
                        allSourceLang.Add(item.SourceLanguageIanacode);
                    }
                }

            }

            if (allSourceLang.Count == 1)
            {
                var langObj = langService.GetLanguageInfo(allSourceLang.ElementAt(0), IANACode);
                return langObj.Name;
            }
            else
            {
                if (IANACode == "en")
                {
                    return allSourceLang.Count().ToString() + " languages";
                }
                else if (IANACode == "da")
                {
                    return allSourceLang.Count().ToString() + " sprog";
                }
                else if (IANACode == "de")
                {
                    return allSourceLang.Count().ToString() + " Sprachen";
                }
                else if (IANACode == "sv")
                {
                    return allSourceLang.Count().ToString() + " språk";
                }
                else
                {
                    return allSourceLang.Count().ToString() + " languages";
                }

            }

        }

        public async Task<string> EnquiryDirectoryPathForUser(int enquiryId)
        {
            Enquiry enquiry = await enquiriesRepo.All().Where(o => o.Id == enquiryId).FirstOrDefaultAsync();
            Contact enqContact = await contactRepo.All().Where(c => c.Id == enquiry.ContactId).FirstOrDefaultAsync();

            string OrgDirSearchPattern = enqContact.OrgId.ToString() + "*";
            string OrgDirPath;

            string OrderDirSearchPattern = enquiryId.ToString() + "*";
            DirectoryInfo DirInfo = new DirectoryInfo(globalVariables.InternalQuoteDriveBaseDirectoryPathForUser);

            // find org folder first (job folders should then appear within that)
            DirectoryInfo[] MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

            if (MatchingOrgDirs.Count() > 0)
            {
                string newOrgDirSearchPattern = enqContact.OrgId.ToString();

                DirectoryInfo[] newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (newMatchingOrgDirs.Count() == 0)
                {
                    return "";
                }
                else
                {
                    OrgDirPath = newMatchingOrgDirs[0].FullName;
                }
            }
            else
            {
                OrgDirPath = MatchingOrgDirs[0].FullName;
            }

            // now look for an existing enquiy folder within the org folder
            DirInfo = new DirectoryInfo(OrgDirPath);
            DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingOrderDirs.Count() != 0)
            {
                return MatchingOrderDirs[0].FullName;
            }
            else
            {
                return "";
            }

        }


        public async Task<string> EnquiryDirectoryPathForApp(int enquiryId)
        {
            Enquiry enquiry = await enquiriesRepo.All().Where(o => o.Id == enquiryId).FirstOrDefaultAsync();
            Contact enqContact = await contactRepo.All().Where(c => c.Id == enquiry.ContactId).FirstOrDefaultAsync();

            string OrgDirSearchPattern = enqContact.OrgId.ToString() + "*";
            string OrgDirPath;

            string OrderDirSearchPattern = enquiryId.ToString() + "*";
            DirectoryInfo DirInfo = new DirectoryInfo(globalVariables.InternalQuoteDriveBaseDirectoryPathForApp);

            // find org folder first (job folders should then appear within that)
            DirectoryInfo[] MatchingOrgDirs = DirInfo.GetDirectories(OrgDirSearchPattern, SearchOption.TopDirectoryOnly);

            if (MatchingOrgDirs.Count() > 0)
            {
                string newOrgDirSearchPattern = enqContact.OrgId.ToString();

                DirectoryInfo[] newMatchingOrgDirs = DirInfo.GetDirectories(newOrgDirSearchPattern, SearchOption.TopDirectoryOnly);

                if (newMatchingOrgDirs.Count() == 0)
                {
                    return "";
                }
                else
                {
                    OrgDirPath = newMatchingOrgDirs[0].FullName;
                }
            }
            else
            {
                OrgDirPath = MatchingOrgDirs[0].FullName;
            }

            // now look for an existing enquiy folder within the org folder
            DirInfo = new DirectoryInfo(OrgDirPath);
            DirectoryInfo[] MatchingOrderDirs = DirInfo.GetDirectories(OrderDirSearchPattern, SearchOption.TopDirectoryOnly);
            if (MatchingOrderDirs.Count() != 0)
            {
                return MatchingOrderDirs[0].FullName;
            }
            else
            {
                return "";
            }

        }
        public async Task<List<Enquiry>> GetEnquiryByContactId(int contactId)
        {
            var result = await enquiriesRepo.All().Where(e => e.ContactId == contactId).ToListAsync();

            return result;
        }
        public async Task<Enquiry> ApproveOrRejectEnquiry(Enquiry model)
        {
            var Details = await enquiriesRepo.All().Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (Details != null)
            {

                Details.DecisionMadeByContactId = model.DecisionMadeByContactId;
                Details.DecisionMadeDateTime = GeneralUtils.GetCurrentUKTime();
                Details.DecisionReasonId = model.DecisionReasonId;
                Details.Status = model.Status;
                Details.LastModifiedByEmployeeId = model.LastModifiedByEmployeeId;
                Details.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
                Details.AdditionalDetails = model.AdditionalDetails;
                Details.WentAheadAsJobOrderId = model.WentAheadAsJobOrderId;
                enquiriesRepo.Update(Details);
                await enquiriesRepo.SaveChangesAsync();
            }

            return Details;
        }
        public string CopyReferenceFilesFromExternalServer(string QuoteRefPath, string ExternalRefPath)
        {
            string FileCopiedMessage = "";
            int GoodRefFilesCopied = 0;
            // first create the reference folders if they dont exist
            if (Directory.Exists(ExternalRefPath) == false)
                Directory.CreateDirectory(ExternalRefPath);

            string[] AllRefFiles = Directory.GetFiles(QuoteRefPath);

            foreach (var File in AllRefFiles)
            {
                try
                {
                    FileSystem.FileCopy(File, ExternalRefPath + @"\" + Path.GetFileName(File));

                    GoodRefFilesCopied += 1;
                }
                catch (Exception ex)
                {
                }
            }

            if (AllRefFiles.Count() > 0 & GoodRefFilesCopied == 0)
                FileCopiedMessage = string.Format("<br /><font color=\"red\">Approved quote has reference file(s) attached to it, which could not be copied over to this job. Please copy these files manually from <a href=\"{0}\">here</a>.</font>", QuoteRefPath);
            else if (AllRefFiles.Count() > GoodRefFilesCopied)
                FileCopiedMessage = string.Format("<br /><font color=\"red\">Approved quote has reference file(s) attached to it, some of which could not be copied over to this job. Please copy these files manually from <a href=\"{0}\">here</a>.</font>", QuoteRefPath);
            else if (AllRefFiles.Count() == 0)
                FileCopiedMessage = "";
            else
                FileCopiedMessage = "<br /><font color=\"green\">Approved quote has reference file(s) attached to it. These files have been copied over to the relevant job folder.</font>";

            return FileCopiedMessage;
        }
        public async Task<EnquiryQuoteItem> CreateEnquiryQuoteItems(int enquiryId, int languageServiceID, string sourceLangaugeIanaCode, string targetLangaugeIanaCode, short createdByEmployeeID, int languageServiceCategoryId = 0)
        {


            if (await EnquiryExists(enquiryId) == false)
            {
                throw new Exception("The enquiry ID, " + enquiryId.ToString() + ", does not exist in the database. An enquiry must be created first.");
            }
            else
            {
                EnquiryQuoteItem newEnquiryQuoteItem;

                newEnquiryQuoteItem = new EnquiryQuoteItem()
                {
                    EnquiryId = enquiryId,
                    LanguageServiceId = languageServiceID,
                    SourceLanguageIanaCode = sourceLangaugeIanaCode,
                    TargetLanguageIanaCode = targetLangaugeIanaCode,
                    CreatedDateTime = timeZonesService.GetCurrentGMT(),
                    CreatedByEmployeeId = createdByEmployeeID,
                    LanguageServiceCategoryId = languageServiceCategoryId
                };

                await enquiryquoteitemsRepo.AddAsync(newEnquiryQuoteItem);
                await enquiryquoteitemsRepo.SaveChangesAsync();

                return newEnquiryQuoteItem;

            }
        }

        public async Task<EnquiryQuoteItem> CreateMultipleEnquiryQuoteItems(int enquiryId, string quoteitems, short createdByEmployeeID)
        {


            if (await EnquiryExists(enquiryId) == false)
            {
                throw new Exception("The enquiry ID, " + enquiryId.ToString() + ", does not exist in the database. An enquiry must be created first.");
            }
            else
            {
                EnquiryQuoteItem newEnquiryQuoteItem = null;

                foreach (string quoteitem in quoteitems.Split("$"))
                {
                    newEnquiryQuoteItem = await CreateEnquiryQuoteItems(enquiryId, Int32.Parse(quoteitem.Split(",")[0]), quoteitem.Split(",")[1], quoteitem.Split(",")[2], createdByEmployeeID);
                }

                return newEnquiryQuoteItem;

            }
        }

        public async Task<EnquiryQuoteItem> RemoveQuoteItem(int enquiryquoteitemid, short deletedByEmployeeID)
        {
            var EnquiryQuoteItem = await enquiryquoteitemService.GetEnquiryQuoteItemById(enquiryquoteitemid);

            EnquiryQuoteItem.DeletedByEmployeeId = deletedByEmployeeID;
            EnquiryQuoteItem.DeletedDateTime = (DateTime)timeZonesService.GetCurrentGMT();

            enquiryquoteitemsRepo.Update(EnquiryQuoteItem);
            await enquiryquoteitemsRepo.SaveChangesAsync();

            return EnquiryQuoteItem;
        }

        public async Task<Enquiry> UpdateKeyInformation(int enquiryID, byte orderChannelID, string jobName,
                                     short lastModifiedByEmployeeID, DateTime? deadlineRequestedByClient = null,
                                    DateTime? EnquiryDeadline = null, int? assignedToEmployeeID = null)
        {
            var enquiry = await GetEnquiryById(enquiryID);

            // maybe add security check later here and autoMapper
            enquiry.OrderChannelId = orderChannelID;
            enquiry.JobName = jobName;
            enquiry.DeadlineRequestedByClient = deadlineRequestedByClient;
            enquiry.LastModifiedByEmployeeId = lastModifiedByEmployeeID;
            enquiry.EnquiryDeadline = EnquiryDeadline;
            //enquiry.AssignedToEmployeeID = assignedToEmployeeID;

            enquiriesRepo.Update(enquiry);
            await enquiriesRepo.SaveChangesAsync();

            return enquiry;
        }


        public async Task<Enquiry> UpdateOptionalInformation(int enquiryID, string clientNotes, string internalNotes, string additionalDetails,
                                     short lastModifiedByEmployeeID, bool printingProject = false)
        {
            var enquiry = await GetEnquiryById(enquiryID);

            // maybe add security check later here and autoMapper
            enquiry.ExternalNotes = clientNotes;
            enquiry.InternalNotes = internalNotes;
            enquiry.AdditionalDetails = additionalDetails;
            enquiry.PrintingProject = printingProject;
            enquiry.LastModifiedByEmployeeId = lastModifiedByEmployeeID;

            enquiriesRepo.Update(enquiry);
            await enquiriesRepo.SaveChangesAsync();

            return enquiry;
        }
        public async Task<ViewModels.Enquiries.EnquiriesViewModel> GetViewModelById(int enquiryId)
        {
            var result = await enquiriesRepo.All().Where(e => e.Id == enquiryId)
                .Select(x => new ViewModels.Enquiries.EnquiriesViewModel()
                {
                    Id = x.Id,
                    ContactId = x.ContactId,
                    OrderChannelId = x.OrderChannelId,
                    Status = x.Status,
                    DecisionReasonId = x.DecisionReasonId,
                    DecisionMadeByContactId = x.DecisionMadeByContactId,
                    DecisionMadeDateTime = x.DecisionMadeDateTime,
                    ExternalNotes = x.ExternalNotes,
                    InternalNotes = x.InternalNotes,
                    DeadlineRequestedByClient = x.DeadlineRequestedByClient,
                    WentAheadAsJobOrderId = x.WentAheadAsJobOrderId,
                    JobName = System.Web.HttpUtility.HtmlDecode(x.JobName),
                    AdditionalDetails = x.AdditionalDetails,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    CreatedDateTime = x.CreatedDateTime,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    LastModifiedDateTime = x.LastModifiedDateTime,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    DeletedDateTime = x.DeletedDateTime,
                    EnqFilesDeletedDateTime = x.EnqFilesDeletedDateTime,
                    PrintingProject = x.PrintingProject,
                    ArchivedToLionBoxDateTime = x.ArchivedToLionBoxDateTime,
                    ArchivedToAmazonS3dateTime = x.ArchivedToAmazonS3dateTime,
                    ArchivedDateTime = x.ArchivedDateTime,
                    AssignedToEmployeeID = x.AssignedToEmployeeID,
                    PriorityID = x.PriorityID,
                    EnquiryDeadline = x.EnquiryDeadline,
                    WinChance = x.WinChance,
                    ClientDecisionDate = x.ClientDecisionDate
                })
                .FirstOrDefaultAsync();


            return result;
        }



        public async Task<List<ViewModels.Enquiries.QuotesResults>> GetQuotes(int EnquiryID)
        {

            var res = new List<ViewModels.Enquiries.QuotesResults>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select distinct isnull(Quotes.CreatedAutomatically, 0) 'CreatedAutomatically', Quotes.IsCurrentVersion, Quotes.ID 'QuoteID', Quotes.QuoteFileName, LocalLanguageInfo.Name 'Language',
Quotes.InternalNotes, Quotes.QuoteDate, isnull(Quotes.LastModifiedDateTime, '1989-01-01 00:00:00.000') as 'LastModifiedDateTime',
Currencies.Prefix, isnull(convert(decimal(29,2), Quotes.OverallChargeToClient), 0.00) 'OverallChargeToClient',
isnull(ES.FirstName + ' ' + Es.Surname,'(none)') as 'SalesContact', isnull(ES.ID, 0) 'SalesContactID',
EC.FirstName + ' ' + EC.Surname as 'CreatedBy', isnull(EC.ID, 0) 'CreatedByID',
Quotes.CreatedDateTime, Quotes.LangIANACode as 'LanguageIANACode',isnull(Quotes.LastModifiedByEmployeeID, 0) 'LastModifiedByEmployeeID'

from Quotes
inner join Enquiries on Enquiries.ID = Quotes.EnquiryID
left outer join contacts on contacts.ID = Enquiries.ContactID
left outer join Orgs on Contacts.OrgID = orgs.ID
left outer join Employees EC on EC.id = Enquiries.CreatedByEmployeeID
left outer join Employees ES on ES.id = quotes.SalesContactEmployeeID
inner join LocalLanguageInfo on LocalLanguageInfo.LanguageIANAcodeBeingDescribed = Quotes.LangIANACode and LocalLanguageInfo.LanguageIANAcode = 'en'
inner join Currencies on Currencies.ID = Quotes.QuoteCurrencyID

where Enquiries.ID = " + EnquiryID + @" and
Quotes.DeletedDateTime is null and Enquiries.DeletedDateTime is null 
order by Quotes.IsCurrentVersion desc, Quotes.CreatedDateTime desc";
                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.Enquiries.QuotesResults()
                    {
                        CreatedAutomatically = await result.GetFieldValueAsync<bool>(0),
                        CurrentVersion = await result.GetFieldValueAsync<bool>(1),
                        QuoteID = await result.GetFieldValueAsync<int>(2),
                        QuoteFileName = await result.GetFieldValueAsync<string>(3),
                        Language = await result.GetFieldValueAsync<string>(4),
                        InternalNotes = await result.GetFieldValueAsync<string>(5),
                        QuoteDate = await result.GetFieldValueAsync<DateTime>(6),
                        LastModifiedDateTime = await result.GetFieldValueAsync<DateTime?>(7),
                        Prefix = await result.GetFieldValueAsync<string>(8),
                        OverallChargeToClient = await result.GetFieldValueAsync<Decimal>(9),
                        SalesContact = await result.GetFieldValueAsync<string>(10),
                        SalesContactID = await result.GetFieldValueAsync<short>(11),
                        CreatedBy = await result.GetFieldValueAsync<string>(12),
                        CreatedByID = await result.GetFieldValueAsync<short>(13),
                        CreatedDateTime = await result.GetFieldValueAsync<DateTime>(14),
                        LanguageIANACode = await result.GetFieldValueAsync<string>(15),
                        LastModifiedEmployeeID = await result.GetFieldValueAsync<short>(16)
                    });
                }
            }
            return res;
        }

        public async Task<Enquiry> UpdateStatus(int enqToUpdate, byte status, int contactID, short lastModifiedByEmployeeID)
        {
            var enquiry = await GetEnquiryById(enqToUpdate);

            // maybe add security check later here and autoMapper
            enquiry.Status = status;
            enquiry.DecisionMadeByContactId = contactID;
            enquiry.DecisionMadeDateTime = GeneralUtils.GetCurrentGMT();
            enquiry.LastModifiedByEmployeeId = lastModifiedByEmployeeID;
            enquiry.LastModifiedDateTime = GeneralUtils.GetCurrentGMT();

            enquiriesRepo.Update(enquiry);
            await enquiriesRepo.SaveChangesAsync();

            return enquiry;
        }
        public async Task<Enquiry> UpdateJobOrder(int enqToUpdate, int joborderID, short lastModifiedByEmployeeID)
        {
            var enquiry = await GetEnquiryById(enqToUpdate);

            // maybe add security check later here and autoMapper
            enquiry.WentAheadAsJobOrderId = joborderID;
            enquiry.LastModifiedByEmployeeId = lastModifiedByEmployeeID;
            enquiry.LastModifiedDateTime = GeneralUtils.GetCurrentGMT();

            enquiriesRepo.Update(enquiry);
            await enquiriesRepo.SaveChangesAsync();

            return enquiry;
        }
        public async Task<Enquiry> UpdateLogicalInformation(int enquiryID, int? winChance, DateTime? clientDecisionDate, short lastModifiedByEmployeeID)

        {
            var enquiry = await GetEnquiryById(enquiryID);

            // maybe add security check later here and autoMapper

            enquiry.WinChance = winChance;
            if (clientDecisionDate != DateTime.MinValue)
            {
                enquiry.ClientDecisionDate = clientDecisionDate;
            }
            enquiry.LastModifiedDateTime = GeneralUtils.GetCurrentGMT();
            enquiry.LastModifiedByEmployeeId = lastModifiedByEmployeeID;

            enquiriesRepo.Update(enquiry);
            await enquiriesRepo.SaveChangesAsync();

            return enquiry;
        }

    }



}

