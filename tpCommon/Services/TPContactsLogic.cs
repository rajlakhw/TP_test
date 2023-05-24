using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Repositories;
using Data;
using ViewModels.Contact;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using System;
using ViewModels.ExtranetUsers;

namespace Services
{
    public class TPContactsLogic : ITPContactsLogic
    {
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<Country> countryRepository;
        private readonly IRepository<ExtranetUser> exUserRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<OrgIntroductionSource> orgISRepository;
        private readonly IRepository<LocalCountryInfo> LIcountryRepository;
        private readonly ITPOrgsLogic orgService;
        public TPContactsLogic(IRepository<Contact> repository, IRepository<ExtranetUser> exUserRepository,
                               IRepository<OrgIntroductionSource> orgISRepository, IRepository<Org> orgRepository,
                               IRepository<Country> countryRepository, IRepository<LocalCountryInfo> LIcountryRepository,
                               ITPOrgsLogic _orgService)
        {
            this.contactRepository = repository;
            this.exUserRepository = exUserRepository;
            this.countryRepository = countryRepository;
            this.LIcountryRepository = LIcountryRepository;
            this.orgRepository = orgRepository;
            this.orgISRepository = orgISRepository;
            this.orgService = _orgService;
        }

        public async Task<IEnumerable<ContactTableViewModel>> GetAllContactsForOrg(int orgId)
            => await contactRepository.All()
            .Where(x => x.OrgId == orgId && x.DeletedDate == null)
            .OrderBy(x => x.Name)
            .Select(x => new ContactTableViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                JobTitle = x.JobTitle,
                EmailAddress = x.EmailAddress,
                Department = x.Department,
                LandlineCountryId = x.LandlineCountryId,
                LandlineNumber = x.LandlineNumber,
                MobileCountryId = x.MobileCountryId,
                MobileNumber = x.MobileNumber
            })
            .ToListAsync();

        public async Task<List<string>> GetAllContactIdAndNameStringForOrg(int orgId)
        {
            var result = await contactRepository.All().Where(c => c.OrgId == orgId && c.DeletedDate == null)
                               .Select(c => new { customName = c.Id + " - " + c.Name, contactName = c.Name })
                               .OrderBy(o => o.contactName).Select(c => c.customName).ToListAsync();
            return result;
        }

        public async Task<List<string>> GetAllContactIdAndNameStringForGroup(int groupId)
        {
            var result = await orgRepository.All().Where(o => o.OrgGroupId == groupId && o.DeletedDate == null)
                               .Join(contactRepository.All(),
                                     o => o.Id,
                                     c => c.OrgId,
                                     (o, c) => new { contact = c })
                               .Select(c => c.contact)
                               .Where(c => c.DeletedDate == null)
                               .Select(c => new { customName = c.Id + " - " + c.Name, contactName = c.Name })
                               .OrderBy(o => o.contactName).Select(c => c.customName).ToListAsync();
            return result;
        }
        public async Task<Contact> GetContactDetails(int ID)
        {
            var result = await contactRepository.All().Where(a => a.Id == ID).FirstOrDefaultAsync();
            return result;
        }

        public string GetExtranetUserName(int contactID)
        {
            var result = exUserRepository.All().Where(a => a.DataObjectId == contactID && a.DataObjectTypeId == 1 && a.LockedOutDateTime == null).OrderByDescending(o => o.UserSetupDateTime).Select(x => new { x.UserName }).FirstOrDefault();
            if (result == null)
            {
                return "";
            }
            else
            {
                return result.UserName;
            }
        }

        public int? GetContactOrgGroup(int contactID)
        {
            var result = contactRepository.All()
                .Join(orgRepository.All(),
                o => o.OrgId,
                t => t.Id,
                (o, t) => new { Contact = o, Org = t })
                .Where(a => a.Contact.Id == contactID).Select(x => new { x.Org.OrgGroupId }).SingleOrDefault();
            return result.OrgGroupId;
        }
        public async Task<List<ViewModels.Contact.ContactCountry>> GetContactCountries()
        {
            var resultsList = await countryRepository.All()
                   .Join(LIcountryRepository.All(),
                   o => o.Id,
                   t => t.CountryId,
                    (o, t) => new { Country = o, LocalCountryInfo = t })
                   .Where(o => o.LocalCountryInfo.LanguageIanacode == "en")
                   .Select(x => new ViewModels.Contact.ContactCountry
                   { Id = x.Country.Id, IsoCode = x.Country.Isocode, Prefix = x.Country.DiallingPrefix, CountryName = x.LocalCountryInfo.CountryName + "   +" + x.Country.DiallingPrefix }).ToListAsync();
            resultsList.Add(new ContactCountry() { CountryName = "(None)", Id = 0, IsoCode = "0", Prefix = "0" });
            return resultsList;
        }


        public async Task<List<ViewModels.Contact.JobContactResults>> GetJobOrders(string ContactID)
        {

            var res = new List<ViewModels.Contact.JobContactResults>();

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

where Contacts.ID = " + ContactID + @" and
joborders.DeletedDate is null
order by JobOrders.SubmittedDateTime desc";
                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.Contact.JobContactResults()
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

        public async Task<List<ViewModels.Contact.EnquiriesContactResults>> GetEnquiries(string ContactID)
        {

            var res = new List<ViewModels.Contact.EnquiriesContactResults>();

            using (var context = new TPCoreProductionContext())
            {
                using var command = context.Database.GetDbConnection().CreateCommand();
                string testcom = @"select distinct Enquiries.ID, Enquiries.JobName,
CASE
    WHEN Enquiries.Status = 0 THEN 'Draft'
    WHEN Enquiries.Status = 1 THEN 'Pending'
	WHEN Enquiries.Status = 2 THEN 'Rejected'
	WHEN Enquiries.Status = 3 THEN 'Gone ahead'
END AS 'Status',
Isnull(DecisionReasons.Reason,'(none)') as 'Reason',cast(ISNULL(Quotes.SubTotalOverallChargeToClient, 0) as decimal(10,2)) as Value, 
(select dbo.funcGetSourceLangOrLangsFullStringEnquiryID(Enquiries.ID)) as SourceLang,
(select dbo.funcGetTargetLangOrLangsFullStringEnquiryID(Enquiries.ID)) as TargetLang,
isnull((select LocalCurrencyInfo.CurrencyName from LocalCurrencyInfo where CurrencyID = Quotes.QuoteCurrencyID),'(none)') as Currency,
convert(varchar, Enquiries.DeadlineRequestedByClient, 103) as 'DeadlineRequestedByClient', isnull(Enquiries.LastModifiedDateTime, '1989-01-01 00:00:00.000') as 'LastModifiedDateTime',
isnull(EL.FirstName + ' ' + EL.Surname, '(none)') as 'Last modified by', isnull(EL.ID,0) as ELID,
EC.FirstName + ' ' + EC.Surname as 'Created by', EC.ID,Enquiries.CreatedDateTime,
isnull(ES.FirstName + ' ' + Es.Surname,'(none)') as 'Sales contact'

from Enquiries
left outer join DecisionReasons on DecisionReasons.id = Enquiries.DecisionReasonID
left outer join contacts on contacts.ID = Enquiries.ContactID
left outer join Quotes on  quotes.EnquiryID = Enquiries.ID and Quotes.IsCurrentVersion = 1
left outer join QuoteItems on QuoteItems.QuoteID= Quotes.ID
left outer join Orgs on Contacts.OrgID = orgs.ID
left outer join Employees EC on EC.id = Enquiries.CreatedByEmployeeID
left outer join Employees EL on EL.id = Enquiries.LastModifiedByEmployeeID
left outer join Employees ES on ES.id = quotes.SalesContactEmployeeID

where Contacts.ID = " + ContactID + @" and
Enquiries.DeletedDateTime is null 
order by Enquiries.CreatedDateTime desc";
                command.CommandText = testcom;
                context.Database.OpenConnection();
                using var result = command.ExecuteReader();
                // do something with result

                while (await result.ReadAsync())
                {
                    res.Add(new ViewModels.Contact.EnquiriesContactResults()
                    {

                        Id = await result.GetFieldValueAsync<int>(0),
                        EnqName = await result.GetFieldValueAsync<string>(1),
                        EnqStatus = await result.GetFieldValueAsync<string>(2),
                        EnqReason = await result.GetFieldValueAsync<string>(3),
                        EnqValue = await result.GetFieldValueAsync<decimal?>(4),
                        Deadline = await result.GetFieldValueAsync<string>(8),
                        SourceLanguage = await result.GetFieldValueAsync<string>(5),
                        TargetLanguage = await result.GetFieldValueAsync<string>(6),
                        EnqCurrencyName = await result.GetFieldValueAsync<string>(7),
                        EnqModified = await result.GetFieldValueAsync<DateTime?>(9),
                        EnqModifiedBy = await result.GetFieldValueAsync<string>(10),
                        EnqModifiedByID = await result.GetFieldValueAsync<short?>(11),
                        SubmittedBy = await result.GetFieldValueAsync<string>(12),
                        SubmittedByID = await result.GetFieldValueAsync<short>(13),
                        SubmittedDateTime = await result.GetFieldValueAsync<DateTime>(14),
                        EnqSales = await result.GetFieldValueAsync<string>(15)
                    });
                }
            }
            return res;
        }

        public async Task<List<ViewModels.Contact.ContactSource>> GetIntroductionSource()
        {

            var result = await orgISRepository.All().Select(x => new ViewModels.Contact.ContactSource
            { Id = x.Id, SourceName = x.SourceName }).ToListAsync();
            return result;
        }
        public async Task<ContactViewModel> Update(ContactViewModel contact)
        {
            var dbContact = await contactRepository.All().Where(x => x.Id == contact.Id).FirstOrDefaultAsync();

            if (dbContact == null)
                return contact;

            if (contact.SectionToUpdate == contact.keyInfoSection)
            {
                dbContact.Name = contact.Name;
                dbContact.JobTitle = contact.JobTitle;
                dbContact.Department = contact.Department;
                dbContact.EmailAddress = contact.EmailAddress;
                if (contact.LandlineCountryID != 0)
                {
                    dbContact.LandlineCountryId = contact.LandlineCountryID;
                    dbContact.LandlineNumber = contact.LandlineNumber;
                }

                if (contact.FaxCountryID != 0)
                {
                    dbContact.FaxCountryId = contact.FaxCountryID;
                    dbContact.FaxNumber = contact.FaxNumber;
                }

                if (contact.MobileCountryID != 0)
                {
                    dbContact.MobileCountryId = contact.MobileCountryID;
                    dbContact.MobileNumber = contact.MobileNumber;
                }

                dbContact.TpintroductionSource = contact.TPIntroductionSource;
                dbContact.FaxNumber = contact.FaxNumber;
                dbContact.LastModifiedByEmployeeId = contact.EmployeCurrentlyLoggedInID;
                dbContact.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            }
            else if (contact.SectionToUpdate == contact.notesFormSection)
            {
                dbContact.Notes = contact.notes;
                dbContact.LastModifiedByEmployeeId = contact.EmployeCurrentlyLoggedInID;
                dbContact.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            }
            else if (contact.SectionToUpdate == contact.contactSettingsSection)
            {
                dbContact.HighLowMarginApprovalNeeded = contact.ApprovedHighLowMargin;
                dbContact.ExcludeZeroMarginJobsFromApproval = contact.ExcludeZeroMargin;
                dbContact.ExtranetOnlyNotifyOnDeliveryOfLastJobItem = contact.NotifyOnlyWhenLastJob;
                dbContact.OptedInForMarketingCampaign = contact.OptedInForMarketingCampaign;
                dbContact.IncludeInNotificationsOn = contact.Notifications;
                dbContact.IncludeInNotifications = contact.NotificationEmails;
                dbContact.SpendFrequencyAlertDays = contact.ContacRelatedAlerts;
                dbContact.LastModifiedByEmployeeId = contact.EmployeCurrentlyLoggedInID;
                dbContact.LastModifiedDate = GeneralUtils.GetCurrentUKTime();
            }

            await contactRepository.SaveChangesAsync();

            return contact;
        }

        public async Task<ContactModel> GetById(int Id)
        {
            var contact = await contactRepository.All().Where(x => x.Id == Id && x.DeletedDate == null).Select(x => new ContactModel()
            {
                Id = x.Id,
                OrgId = x.OrgId,
                Name = x.Name,
                LandlineCountryId = x.LandlineCountryId,
                LandlineNumber = x.LandlineNumber,
                MobileCountryId = x.MobileCountryId,
                MobileNumber = x.MobileNumber,
                FaxCountryId = x.FaxCountryId,
                FaxNumber = x.FaxNumber,
                EmailAddress = x.EmailAddress,
                SkypeId = x.SkypeId,
                JobTitle = x.JobTitle,
                Department = x.Department,
                Notes = x.Notes,
                ExtranetOnlyNotifyOnDeliveryOfLastJobItem = x.ExtranetOnlyNotifyOnDeliveryOfLastJobItem,
                CreatedDate = x.CreatedDate,
                CreatedByEmployeeId = x.CreatedByEmployeeId,
                LastModifiedDate = x.LastModifiedDate,
                LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                DeletedDate = x.DeletedDate,
                DeletedByEmployeeId = x.DeletedByEmployeeId,
                HighLowMarginApprovalNeeded = x.HighLowMarginApprovalNeeded,
                ExcludeZeroMarginJobsFromApproval = x.ExcludeZeroMarginJobsFromApproval,
                SpendFrequencyAlertDays = x.SpendFrequencyAlertDays,
                SpendFrequencyAlertLastIssued = x.SpendFrequencyAlertLastIssued,
                IncludeInNotificationsOn = x.IncludeInNotificationsOn,
                IncludeInNotifications = x.IncludeInNotifications,
                InvoiceOrgName = x.InvoiceOrgName,
                InvoiceAddress1 = x.InvoiceAddress1,
                InvoiceAddress2 = x.InvoiceAddress2,
                InvoiceAddress3 = x.InvoiceAddress3,
                InvoiceAddress4 = x.InvoiceAddress4,
                InvoiceCountyOrState = x.InvoiceCountyOrState,
                InvoicePostcodeOrZip = x.InvoicePostcodeOrZip,
                InvoiceCountryId = x.InvoiceCountryId,
                EmailUnsubscribedDateTime = x.EmailUnsubscribedDateTime,
                EmailUnsubscribedReason = x.EmailUnsubscribedReason,
                GdpracceptedDateTime = x.GdpracceptedDateTime,
                OptedInForMarketingCampaign = x.OptedInForMarketingCampaign,
                LastRespondedToOptInPopup = x.LastRespondedToOptInPopup,
                GdpracceptedViaIplus = x.GdpracceptedViaIplus,
                GdprrejectedDateTime = x.GdprrejectedDateTime,
                GdprverballyAccepted = x.GdprverballyAccepted,
                Gdprstatus = x.Gdprstatus,
                TpintroductionSource = x.TpintroductionSource
            }).FirstOrDefaultAsync();

            return contact;
        }

        public async Task<ExtranetUserModel> GetExtranetUser(int contactId)
        {
            return await exUserRepository.All().Where(x => x.DataObjectTypeId == 1 && x.DataObjectId == contactId)
                .Select(x => new ExtranetUserModel()
                {
                    UserName = x.UserName,
                    HashedPassword = x.HashedPassword,
                    Salt = x.Salt,
                    PasswordLastSetDateTime = x.PasswordLastSetDateTime,
                    MustResetPasswordOnNextLogin = x.MustResetPasswordOnNextLogin,
                    DataObjectTypeId = x.DataObjectTypeId,
                    DataObjectId = x.DataObjectId,
                    AccessLevelId = x.AccessLevelId,
                    PreferredExtranetUilangIanacode = x.PreferredExtranetUilangIanacode,
                    WebServiceGuid = x.WebServiceGuid,
                    WebServiceNotificationEmailAddress = x.WebServiceNotificationEmailAddress,
                    WebServiceNotifyOnOrderSubmission = x.WebServiceNotifyOnOrderSubmission,
                    WebServiceNotifyOnFileCollection = x.WebServiceNotifyOnFileCollection,
                    FirstEverLoginDateTime = x.FirstEverLoginDateTime,
                    LastLoginDateTime = x.LastLoginDateTime,
                    NumberOfFailedLoginAttempts = x.NumberOfFailedLoginAttempts,
                    PreviousLoginDateTime = x.PreviousLoginDateTime,
                    UserSetupByEmployeeId = x.UserSetupByEmployeeId,
                    UserSetupDateTime = x.UserSetupDateTime,
                    LockedOutDateTime = x.LockedOutDateTime,
                    DefaultTimeZone = x.DefaultTimeZone,
                    TranslateonlineAllowed = x.TranslateonlineAllowed,
                    DesignplusEnabled = x.DesignplusEnabled,
                    UserProfileImagePath = x.UserProfileImagePath,
                    IsCustomizedHomePageSet = x.IsCustomizedHomePageSet,
                    CustomizedHomePageLayout = x.CustomizedHomePageLayout,
                    CustomizedHomePageVisited = x.CustomizedHomePageVisited,
                    ShowDesignPlusInfoBox = x.ShowDesignPlusInfoBox,
                    ShowNewInfoPopUp = x.ShowNewInfoPopUp,
                    SecretQuestionId = x.SecretQuestionId,
                    HashedSecretQuestionAnswer = x.HashedSecretQuestionAnswer,
                    SecretQuestionAnswerSalt = x.SecretQuestionAnswerSalt,
                    NumberOfFailedAnswerAttempts = x.NumberOfFailedAnswerAttempts
                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ContactModel>> SearchByNameOrId(string searchTerm)
        {
            int id;
            var isId = int.TryParse(searchTerm, out id);

            var contactList = await contactRepository.All()
                .Where(x => (x.Id == id ||
                x.Name.Contains(searchTerm)) &&
                x.DeletedDate == null)
                .Select(x => new ContactModel()
                {
                    Id = x.Id,
                    OrgId = x.OrgId,
                    Name = x.Name,
                    LandlineCountryId = x.LandlineCountryId,
                    LandlineNumber = x.LandlineNumber,
                    MobileCountryId = x.MobileCountryId,
                    MobileNumber = x.MobileNumber,
                    FaxCountryId = x.FaxCountryId,
                    FaxNumber = x.FaxNumber,
                    EmailAddress = x.EmailAddress,
                    SkypeId = x.SkypeId,
                    JobTitle = x.JobTitle,
                    Department = x.Department,
                    Notes = x.Notes,
                    ExtranetOnlyNotifyOnDeliveryOfLastJobItem = x.ExtranetOnlyNotifyOnDeliveryOfLastJobItem,
                    CreatedDate = x.CreatedDate,
                    CreatedByEmployeeId = x.CreatedByEmployeeId,
                    LastModifiedDate = x.LastModifiedDate,
                    LastModifiedByEmployeeId = x.LastModifiedByEmployeeId,
                    DeletedDate = x.DeletedDate,
                    DeletedByEmployeeId = x.DeletedByEmployeeId,
                    HighLowMarginApprovalNeeded = x.HighLowMarginApprovalNeeded,
                    ExcludeZeroMarginJobsFromApproval = x.ExcludeZeroMarginJobsFromApproval,
                    SpendFrequencyAlertDays = x.SpendFrequencyAlertDays,
                    SpendFrequencyAlertLastIssued = x.SpendFrequencyAlertLastIssued,
                    IncludeInNotificationsOn = x.IncludeInNotificationsOn,
                    IncludeInNotifications = x.IncludeInNotifications,
                    InvoiceOrgName = x.InvoiceOrgName,
                    InvoiceAddress1 = x.InvoiceAddress1,
                    InvoiceAddress2 = x.InvoiceAddress2,
                    InvoiceAddress3 = x.InvoiceAddress3,
                    InvoiceAddress4 = x.InvoiceAddress4,
                    InvoiceCountyOrState = x.InvoiceCountyOrState,
                    InvoicePostcodeOrZip = x.InvoicePostcodeOrZip,
                    InvoiceCountryId = x.InvoiceCountryId,
                    EmailUnsubscribedDateTime = x.EmailUnsubscribedDateTime,
                    EmailUnsubscribedReason = x.EmailUnsubscribedReason,
                    GdpracceptedDateTime = x.GdpracceptedDateTime,
                    OptedInForMarketingCampaign = x.OptedInForMarketingCampaign,
                    LastRespondedToOptInPopup = x.LastRespondedToOptInPopup,
                    GdpracceptedViaIplus = x.GdpracceptedViaIplus,
                    GdprrejectedDateTime = x.GdprrejectedDateTime,
                    GdprverballyAccepted = x.GdprverballyAccepted,
                    Gdprstatus = x.Gdprstatus,
                    TpintroductionSource = x.TpintroductionSource
                })
                .OrderBy(x => x.Name)
                .ToListAsync();

            return contactList;
        }

        public async Task<string> EmailAddressesForNotification(int contactId)
        {
            var thisContact = await GetById(contactId);

            if (thisContact.IncludeInNotificationsOn == true && thisContact.IncludeInNotifications != "")
            {
                return thisContact.EmailAddress + ", " + thisContact.IncludeInNotifications;
            }
            else
            {
                var thisOrg = await orgService.GetOrgDetails(thisContact.OrgId);

                if (thisOrg.IncludeInNotificationsOn == true && thisOrg.IncludeInNotifications != "")
                {
                    return thisContact.EmailAddress + ", " + thisOrg.IncludeInNotifications;
                }
                else
                {
                    return thisContact.EmailAddress;
                }
            }

        }

        public async Task<List<Contact>> GetAllContacts()
        {
            var result = await contactRepository.All().ToListAsync();
            return result;
        }

        public async Task<List<Contact>> GetContactsFromOrgWhoIsAllowedToUseChargeableSoftware(int orgId, int ChargeableSoftware)
        {
            var orgGroupID = await orgRepository.All().Where(o => o.Id == orgId).Select(o => o.OrgGroupId).FirstOrDefaultAsync();
            var result = await contactRepository.All().Where(c => c.OrgId == orgId && c.DeletedDate == null)
                         .Join(exUserRepository.All().Where(e => e.DataObjectTypeId == 1 &&
                                                            ((ChargeableSoftware == 0 && e.TranslateonlineAllowed == true) || (ChargeableSoftware == 1 && e.DesignplusEnabled == true))).Select(e => e.DataObjectId),
                               c => c.Id,
                               e => e,
                               (c, e) => new { Contact = c }).Select(c => c.Contact)
                         .Where(c => orgGroupID == 18520 || !(c.EmailAddress.Contains("@translateplus.com")))
                         .OrderBy(o => o.Name).ToListAsync();

            return result;

        }
    }
}
