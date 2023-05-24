using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;
using ViewModels.flowPlusExternal.ReviewPlus;
using Global_Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Services
{
    public class TPClientReviewService : ITPClientReviewService
    {
        private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<Org> orgRepository;
        private readonly IRepository<ExtranetUsersTemp> extranetUserRepository;
        private readonly IRepository<ExtranetUser> oldExtranetUserRepository;
        private readonly IRepository<ExtranetUsersReviewLanguage> extranetUserReviewLangRepository;
        private readonly ITPTimeZonesService timeZonesService;
        //private readonly IRepository<Contact> contactRepository;
        private readonly IRepository<Org> orgRepo;
        private readonly IRepository<JobOrder> orderRepository;
        private readonly IRepository<LocalLanguageInfo> localLangRepo;
        private readonly IRepository<JobItem> jobitemsRepo;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPJobItemService jobItemService;
        private readonly ITPJobOrderService orderService;
        private readonly ITPContactsLogic contactService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPOrgGroupsLogic groupService;
        private readonly IRepository<ReviewTranslation> reviewTranslationRepo;
        private readonly IRepository<ReviewTranslationComment> reviewTranslationCommentsRepo;
        private readonly ITPReviewPlusTagService reviewPlusTagService;
        private readonly GlobalVariables globalVariables;
        private readonly IConfiguration configuration;
        public TPClientReviewService(IRepository<Contact> _contactRepository,
                                     IRepository<Org> _orgRepository,
                                     IRepository<ExtranetUsersTemp> _extranetUserRepository,
                                     IRepository<ExtranetUser> _oldExtranetUserRepository,
                                     IRepository<ExtranetUsersReviewLanguage> _extranetUserReviewLangRepository,
                                     ITPTimeZonesService tPTimeZonesService,
                                     IRepository<Org> repository4,
                                     IRepository<JobOrder> _orderRepository,
                                     IRepository<LocalLanguageInfo> _localLangRepo,
                                     IRepository<JobItem> _jobitemsRepo,
                                     ITPExtranetUserService _extranetUserService,
                                     ITPJobItemService _jobItemService,
                                     ITPJobOrderService _orderService,
                                     ITPContactsLogic _contactService,
                                     ITPOrgsLogic _orgService,
                                     ITPOrgGroupsLogic _groupService,
                                     IRepository<ReviewTranslation> reviewTranslationRepo,
                                     IRepository<ReviewTranslationComment> reviewTranslationCommentsRepo,
                                     ITPReviewPlusTagService _reviewPlusTagService,
                                     IConfiguration configuration)
        {
            contactRepository = _contactRepository;
            orgRepository = _orgRepository;
            extranetUserRepository = _extranetUserRepository;
            oldExtranetUserRepository = _oldExtranetUserRepository;
            extranetUserReviewLangRepository = _extranetUserReviewLangRepository;
            timeZonesService = tPTimeZonesService;
            orgRepo = repository4;
            //contactRepository = repository3;
            orderRepository = _orderRepository;
            localLangRepo = _localLangRepo;
            jobitemsRepo = _jobitemsRepo;
            extranetUserService = _extranetUserService;
            jobItemService = _jobItemService;
            orderService = _orderService;
            contactService = _contactService;
            orgService = _orgService;
            groupService = _groupService;
            this.reviewTranslationRepo = reviewTranslationRepo;
            this.reviewTranslationCommentsRepo = reviewTranslationCommentsRepo;
            this.reviewPlusTagService = _reviewPlusTagService;
            this.configuration = configuration;
            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
        }

        public async Task<List<Contact>> GetAllReviewersForTargetLangForOrg(int OrgId, string TargetIANACode)
        {
            var result = await contactRepository.All().Where(c => c.OrgId == OrgId && c.DeletedDate == null)
                               .Join(oldExtranetUserRepository.All().Where(e => e.DataObjectTypeId == 1),
                                     c => c.Id,
                                     e => e.DataObjectId,
                                     (c, e) => new { contact = c, extranetUser = e })
                               .Join(extranetUserReviewLangRepository.All().Where(r => r.TargetLangIanacode == TargetIANACode),
                                     e => e.extranetUser.UserName,
                                     r => r.ExtranetUserName,
                                     (e, r) => new { contact = e.contact, leadReviewer = r.IsLeadReviewer })
                               .OrderByDescending(o => o.leadReviewer).OrderBy(o => o.contact.Name).Select(c => c.contact).ToListAsync();
            return result;
        }

        public async Task<List<Contact>> GetAllReviewersForTargetLangForGroup(int GroupId, string TargetIANACode)
        {
            var result = await orgRepository.All().Where(o => o.OrgGroupId == GroupId && o.DeletedDate == null)
                               .Join(contactRepository.All().Where(c => c.DeletedDate == null),
                                     o => o.Id,
                                     c => c.OrgId,
                                     (o, c) => new { contact = c })
                               .Join(oldExtranetUserRepository.All().Where(e => e.DataObjectTypeId == 1),
                                     c => c.contact.Id,
                                     e => e.DataObjectId,
                                     (c, e) => new { contact = c.contact, extranetUser = e })
                               .Join(extranetUserReviewLangRepository.All().Where(r => r.TargetLangIanacode == TargetIANACode),
                                     e => e.extranetUser.UserName,
                                     r => r.ExtranetUserName,
                                     (e, r) => new { contact = e.contact, leadReviewer = r.IsLeadReviewer })
                               .OrderByDescending(o => o.leadReviewer).OrderBy(o => o.contact.Name).Select(c => c.contact).ToListAsync();
            return result;
        }

        public async Task<List<ReviewStatusModel>> GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus reviewStatus, string extranetUserName, bool loadAssigned,
            int pageNumber = -1, int pageSize = -1, string searchTerm = "", int columnToOrderBy = -1, string orderDirection = "")
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var userAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var extranetUserContact = await contactRepository.All().Where(x => x.Id == extranetUser.DataObjectId && x.DeletedDate == null).FirstOrDefaultAsync();
            bool ViewItemsAssignedToCurrentUser = false;
            List<ReviewStatusModel> reviewDocuments = new List<ReviewStatusModel>();
            if (extranetUser.DataObjectTypeId != Convert.ToByte(Enumerations.DataObjectTypes.Contact))
            {
                // Return empty table
            }
            if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo == false)
            {
                ViewItemsAssignedToCurrentUser = true;
            }

            // Get pending or inprogress review items assigned to logged in user.
            if (ViewItemsAssignedToCurrentUser || loadAssigned == true)
            {
                reviewDocuments = await (from order in orderRepository.All()
                                         join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                         join contactReviewer in contactRepository.All().Where(c => c.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                         join contactOrder in contactRepository.All().Where(c => c.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                         join org in orgRepo.All().Where(c => c.DeletedDate == null) on contactReviewer.OrgId equals org.Id
                                         select new
                                         {
                                             jItem,
                                             order,
                                             contactReviewer,
                                             contactOrder,
                                             org
                                         })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactReviewer.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToListAsync();
            }
            else if (userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos || userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo)
            {
                var contactOrg = orgRepo.All().Where(x => x.Id == extranetUserContact.OrgId && x.DeletedDate == null).FirstOrDefault();
                if (contactOrg.OrgGroupId == null)
                {
                    reviewDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                       join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                       join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                       join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                       join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                       select new
                                       {
                                           jItem,
                                           order,
                                           contactReviewer,
                                           contactOrder,
                                           org
                                       })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactReviewer.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();

                }
                else
                {

                    reviewDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == contactOrg.OrgGroupId)
                                       join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                       join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                       join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                       join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                       select new
                                       {
                                           jItem,
                                           order,
                                           contactReviewer,
                                           contactOrder,
                                           org
                                       })
                               .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                               && x.order.JobOrderChannelId != 21
                               && x.jItem.SupplierSentWorkDateTime != null
                               && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                               && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                               && x.jItem.SupplierCompletedItemDateTime == null
                               && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactReviewer.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                               .Select(x => new ReviewStatusModel
                               {
                                   RequestedBy = x.contactOrder.Name,
                                   Reviewer = x.contactReviewer.Name,
                                   JobItemId = x.jItem.Id,
                                   JobName = x.order.JobName,
                                   SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                   TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                   Priority = x.order.Priority,
                                   DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                   SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                     + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                     + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                   SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                               }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
                }
            }
            else if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos)
            {
                var contactOrg = orgRepo.All().Where(x => x.Id == extranetUserContact.OrgId && x.DeletedDate == null).FirstOrDefault();

                reviewDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                   join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                   join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                   join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                   join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                   select new
                                   {
                                       jItem,
                                       order,
                                       contactReviewer,
                                       contactOrder,
                                       org
                                   })
                              .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                              && x.order.JobOrderChannelId != 21
                              && x.jItem.SupplierSentWorkDateTime != null
                              && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                              && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                              && x.jItem.SupplierCompletedItemDateTime == null
                               && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactReviewer.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                              .Select(x => new ReviewStatusModel
                              {
                                  RequestedBy = x.contactOrder.Name,
                                  Reviewer = x.contactReviewer.Name,
                                  JobItemId = x.jItem.Id,
                                  JobName = x.order.JobName,
                                  SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                  TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                  Priority = x.order.Priority,
                                  DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                  SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                    + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                    + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                  SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                              }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
            }
            else
            {
                reviewDocuments = (from order in orderRepository.All()
                                   join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                   join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                   join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                   join org in orgRepo.All() on contactReviewer.OrgId equals org.Id
                                   select new
                                   {
                                       jItem,
                                       order,
                                       contactReviewer,
                                       contactOrder,
                                       org
                                   })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactReviewer.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime

                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
            }

            if (reviewStatus == Enumerations.ReviewStatus.ReviewInProgress)
            {
                foreach (ReviewStatusModel reviewDoc in reviewDocuments)
                {
                    reviewDoc.ReviewProgress = GetReviewProgress(reviewDoc.JobItemId);
                }
            }

            if (pageNumber > -1 && pageSize > -1)
            {
                reviewDocuments = reviewDocuments.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            //if (searchTerm != "")
            //{
            //    //reviewDocuments = reviewDocuments.Where(r => r.RequestedBy.Contains(searchTerm) || r.Reviewer.Contains(searchTerm) || r.JobItemId.ToString().Contains(searchTerm) ||
            //    //                      r.JobName.Contains(searchTerm) || r.SourceLanguage.Contains(searchTerm) || r.TargetLanguage.Contains(searchTerm) ||
            //    //                      r.DueDate.ToString().Contains(searchTerm) || r.SupplierWordCountTotal.ToString().Contains(searchTerm) ||
            //    //                      r.SupplierSentWorkDateTime.ToString().Contains(searchTerm)).ToList();
            //    reviewDocuments = reviewDocuments.Where(r => r.RequestedBy.Contains(searchTerm)).ToList();
            //}

            if (orderDirection == "desc")
            {
                if (columnToOrderBy == 0)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.RequestedBy).ToList();
                }
                else if (columnToOrderBy == 1)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.Reviewer).ToList();
                }
                else if (columnToOrderBy == 2)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.JobItemId).ToList();
                }
                else if (columnToOrderBy == 3)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.JobName).ToList();
                }
                else if (columnToOrderBy == 4)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.SourceLanguage).ToList();
                }
                else if (columnToOrderBy == 5)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.TargetLanguage).ToList();
                }
                else if (columnToOrderBy == 7)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.DueDate).ToList();
                }
                else if (columnToOrderBy == 8)
                {
                    reviewDocuments = reviewDocuments.OrderByDescending(x => x.SupplierWordCountTotal).ToList();
                }
            }
            else if (orderDirection == "asc")
            {
                if (columnToOrderBy == 0)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.RequestedBy).ToList();
                }
                else if (columnToOrderBy == 1)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.Reviewer).ToList();
                }
                else if (columnToOrderBy == 2)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.JobItemId).ToList();
                }
                else if (columnToOrderBy == 3)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.JobName).ToList();
                }
                else if (columnToOrderBy == 4)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.SourceLanguage).ToList();
                }
                else if (columnToOrderBy == 5)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.TargetLanguage).ToList();
                }
                else if (columnToOrderBy == 7)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.DueDate).ToList();
                }
                else if (columnToOrderBy == 8)
                {
                    reviewDocuments = reviewDocuments.OrderBy(x => x.SupplierWordCountTotal).ToList();
                }
            }

            // Generating data for view
            return reviewDocuments;
        }
        public async Task<PendingAndInProgressReviewModel> GetPendingAndInprogressReviewJobItems(string extranetUserName)
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var userAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var extranetUserContact = await contactRepository.All().Where(x => x.Id == extranetUser.DataObjectId && x.DeletedDate == null).FirstOrDefaultAsync();
            bool ViewItemsAssignedToCurrentUser = false;
            List<ReviewStatusModel> inProgressReviewDocuments = new List<ReviewStatusModel>();
            List<ReviewStatusModel> pendingReviewDocuments = new List<ReviewStatusModel>();
            //List<ReviewStatusModel> inProgressReviewDocuments = new List<ReviewStatusModel>();
            if (extranetUser.DataObjectTypeId != Convert.ToByte(Enumerations.DataObjectTypes.Contact))
            {
                // Return empty table
            }
            if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo == false)
            {
                ViewItemsAssignedToCurrentUser = true;
            }

            var reviewData = new List<ReviewStatusModel>();

            // Get pending or inprogress review items assigned to logged in user.
            if (ViewItemsAssignedToCurrentUser)
            {
                reviewData = (from order in orderRepository.All().Where(x => x.DeletedDate == null)
                              join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                              join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                              join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                              join org in orgRepo.All().Where(x => x.DeletedDate == null) on contactReviewer.OrgId equals org.Id
                              select new
                              {
                                  jItem,
                                  order,
                                  contactReviewer,
                                  contactOrder,
                                  org
                              })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = (DateTime)x.jItem.SupplierCompletionDeadline,
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     //ReviewProgress = GetReviewProgress(x.jItem.Id),
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
            }
            else if (userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos || userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo)
            {
                var contactOrg = orgRepo.All().Where(x => x.Id == extranetUserContact.OrgId && x.DeletedDate == null).FirstOrDefault();
                if (contactOrg.OrgGroupId == null)
                {
                    reviewData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                  join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                  join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                  join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                  join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                  select new
                                  {
                                      jItem,
                                      order,
                                      contactReviewer,
                                      contactOrder,
                                      org
                                  })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = (DateTime)x.jItem.SupplierCompletionDeadline,
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     //ReviewProgress = GetReviewProgress(x.jItem.Id),
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();

                }
                else
                {
                    reviewData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == contactOrg.OrgGroupId)
                                  join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                  join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                  join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                  join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                                  select new
                                  {
                                      jItem,
                                      order,
                                      contactReviewer,
                                      contactOrder,
                                      org
                                  })
                               .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                               && x.order.JobOrderChannelId != 21
                               && x.jItem.SupplierIsClientReviewer == true
                               && x.jItem.SupplierSentWorkDateTime != null
                               && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                               && x.jItem.SupplierCompletedItemDateTime == null)
                               .Select(x => new ReviewStatusModel
                               {
                                   RequestedBy = x.contactOrder.Name,
                                   Reviewer = x.contactReviewer.Name,
                                   JobItemId = x.jItem.Id,
                                   JobName = x.order.JobName,
                                   SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                   TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                   Priority = x.order.Priority,
                                   DueDate = (DateTime)x.jItem.SupplierCompletionDeadline,
                                   SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                     + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                     + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                   //ReviewProgress = GetReviewProgress(x.jItem.Id),
                                   SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                               }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
                }
            }
            else if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos)
            {
                var contactOrg = orgRepo.All().Where(x => x.Id == extranetUserContact.OrgId && x.DeletedDate == null).FirstOrDefault();

                reviewData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                              join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                              join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                              join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                              join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                              select new
                              {
                                  jItem,
                                  order,
                                  contactReviewer,
                                  contactOrder,
                                  org
                              })
                              .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                              && x.order.JobOrderChannelId != 21
                              && x.jItem.SupplierIsClientReviewer == true
                              && x.jItem.SupplierSentWorkDateTime != null
                              && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                              && x.jItem.SupplierCompletedItemDateTime == null)
                              .Select(x => new ReviewStatusModel
                              {
                                  RequestedBy = x.contactOrder.Name,
                                  Reviewer = x.contactReviewer.Name,
                                  JobItemId = x.jItem.Id,
                                  JobName = x.order.JobName,
                                  SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                  TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                  Priority = x.order.Priority,
                                  DueDate = (DateTime)x.jItem.SupplierCompletionDeadline,
                                  SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                    + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                    + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                  //ReviewProgress = GetReviewProgress(x.jItem.Id),
                                  SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                              }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
            }
            else
            {
                reviewData = (from order in orderRepository.All().Where(x => x.DeletedDate == null)
                              join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                              join contactReviewer in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactReviewer.Id
                              join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                              join org in orgRepo.All().Where(x => x.DeletedDate == null) on contactReviewer.OrgId equals org.Id
                              select new
                              {
                                  jItem,
                                  order,
                                  contactReviewer,
                                  contactOrder,
                                  org
                              })
                                 .Where(x => (x.jItem.LanguageServiceId == 21 || x.jItem.LanguageServiceId == 67)
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new ReviewStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Reviewer = x.contactReviewer.Name,
                                     JobItemId = x.jItem.Id,
                                     JobName = x.order.JobName,
                                     SourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     TargetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name,
                                     Priority = x.order.Priority,
                                     DueDate = timeZonesService.ConvertGMTToRequestedTimeZone((DateTime)x.jItem.SupplierCompletionDeadline, extranetUser.DefaultTimeZone),
                                     SupplierWordCountTotal = x.jItem.SupplierWordCountNew ?? 0 + x.jItem.SupplierWordCountExact ?? 0 + x.jItem.SupplierWordCountRepetitions ?? 0
                                                       + x.jItem.SupplierWordCountPerfectMatches ?? 0 + x.jItem.SupplierWordCountFuzzyBand1 ?? 0 + x.jItem.SupplierWordCountFuzzyBand2 ?? 0
                                                       + x.jItem.SupplierWordCountFuzzyBand3 ?? 0 + x.jItem.SupplierWordCountFuzzyBand4 ?? 0,
                                     //ReviewProgress = GetReviewProgress(x.jItem.Id),
                                     SupplierSentWorkDateTime = x.jItem.SupplierSentWorkDateTime
                                 }).OrderBy(x => x.SupplierSentWorkDateTime).ToList();
            }

            pendingReviewDocuments = reviewData.Where(x => x.SupplierAcceptedWorkDateTime == null).ToList();
            inProgressReviewDocuments = reviewData.Where(x => x.SupplierAcceptedWorkDateTime != null).ToList();

            // Generating data for view
            PendingAndInProgressReviewModel model = new();
            model.PendingReviewDocuments = pendingReviewDocuments;
            model.InProgressReviewDocuments = inProgressReviewDocuments;
            return model;
        }

        public string GetReviewProgress(int jobItemId)
        {
            decimal reviewProgress = 0;
            var jobItem = jobItemService.GetById(jobItemId).Result;
            var thisJobOrder = orderService.GetById(jobItem.JobOrderId).Result;
            var thisContact = contactService.GetById(thisJobOrder.ContactId).Result;
            if (jobItem != null)
            {
                if (jobItem.WeCompletedItemDateTime != null)
                {
                    reviewProgress = 100;
                }
                else
                {
                    if (jobItem.SupplierCompletedItemDateTime != null)
                    {
                        reviewProgress = 100;
                    }
                    else
                    {
                        string BilingualDocFolderPath = "";

                        if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == true)
                        {
                            BilingualDocFolderPath = jobItemService.ExtranetTranslateOnlineFromTranslationDirectoryPathForApp(jobItemId);
                        }
                        else
                        {
                            BilingualDocFolderPath = jobItemService.ExtranetReviewPlusFromReviewDirectoryPathForApp(jobItemId);
                        }



                        System.IO.FileInfo BilingualDocFile = null;
                        string BilingualDocPath = "";

                        try
                        {
                            BilingualDocFile = new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.sdlxliff", SearchOption.AllDirectories)[0];
                        }
                        catch
                        {
                            try
                            {
                                BilingualDocFile = new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.ttx", SearchOption.AllDirectories)[0];
                            }
                            catch
                            {
                                return "0.0";
                            }
                        }


                        BilingualDocPath = BilingualDocFile.FullName.Replace(BilingualDocFolderPath + "\\", "");

                        SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
                        SqlDataAdapter ReviewerDataAdapter = new SqlDataAdapter("procGetPercentageReviewingProgress", SQLConn);

                        ReviewerDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        ReviewerDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@P_JOB_ITEM_ID", SqlDbType.Int)).Value = jobItemId;
                        ReviewerDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@P_FILE_NAME", SqlDbType.NVarChar)).Value = BilingualDocPath;

                        SqlParameter OutputParameter = ReviewerDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@PROGRESS", SqlDbType.Decimal));
                        OutputParameter.Precision = 4;
                        OutputParameter.Scale = 1;
                        OutputParameter.Direction = ParameterDirection.Output;


                        try
                        {
                            ReviewerDataAdapter.SelectCommand.Connection.Open();
                            ReviewerDataAdapter.SelectCommand.ExecuteScalar();
                            reviewProgress = (decimal)OutputParameter.Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error running stored procedure to get review progress.");
                        }
                        finally
                        {
                            try
                            {
                                ReviewerDataAdapter.SelectCommand.Connection.Close();
                                ReviewerDataAdapter.Dispose();
                            }
                            catch (SqlException se)
                            {
                                // Log an event in the Application Event Log.
                                throw;
                            }
                        }
                    }
                }
            }

            if (thisContact.OrgId == 68268)
            {
                reviewProgress = (decimal)reviewProgress / 2;
            }



            return reviewProgress.ToString("0.0");
        }

        public async Task<List<ReviewEditModel>> GetReviewTranslationSegments(int jobItemId, string extranetUserName,
                                                                              string showTags, string searchAll = "",
                                                                              string searchSource = "", string searchTarget = "",
                                                                              int pageNumber = -1, int pageLength = -1)
        {
            try
            {
                var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
                var jobItem = jobitemsRepo.All().Where(x => x.Id == jobItemId && x.DeletedDateTime == null).FirstOrDefault();
                string sourceLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
                string targetLanguage = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
                string BilingualDocFolderPath = "";
                string BilingualDocPath;
                if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == false)
                {
                    BilingualDocFolderPath = jobItemService.ExtranetFolderForSupplierPath(jobItemId);
                    if (!Directory.Exists(BilingualDocFolderPath))
                    {
                        Directory.CreateDirectory(BilingualDocFolderPath);
                    }
                }
                else if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == true)
                {
                    BilingualDocFolderPath = jobItemService.ExtranetTranslateOnlineFromTranslationDirectoryPathForApp(jobItemId);
                    if (!Directory.Exists(BilingualDocFolderPath))
                    {
                        Directory.CreateDirectory(BilingualDocFolderPath);
                    }
                }
                else
                {
                    BilingualDocFolderPath = jobItemService.ExtranetReviewPlusFromReviewDirectoryPathForApp(jobItemId);
                    if (!Directory.Exists(BilingualDocFolderPath))
                    {
                        Directory.CreateDirectory(BilingualDocFolderPath);
                    }
                }
                FileInfo BilingualDocFile = null;
                if (new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.sdlxliff", SearchOption.TopDirectoryOnly).Count() > 0)
                {
                    BilingualDocFile = new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.sdlxliff", SearchOption.TopDirectoryOnly)[0];
                }
                else
                {
                    if (new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.ttx", SearchOption.TopDirectoryOnly).Count() > 0)
                    {
                        BilingualDocFile = new DirectoryInfo(BilingualDocFolderPath).GetFiles("*.ttx", SearchOption.TopDirectoryOnly)[0];
                    }
                }
                if (BilingualDocFile == null)
                {
                    throw new Exception("While trying to display the contents of the bilingual file for job item " + jobItemId.ToString() + ", it was not possible to locate the .sdlxliff or .ttx file to be loaded. This probably means the file was not copied over correctly when it was originally sent to review/online translation.");
                }

                BilingualDocPath = BilingualDocFile.FullName.Replace(BilingualDocFolderPath + "\\", "");
                string FileName = BilingualDocPath.Replace(".sdlxliff", "").Replace(".ttx", "");

                // getting the data for table

                var translationData = (from rt in reviewTranslationRepo.All()
                                       .Where(x => x.JobItemId == jobItemId && x.FileName == BilingualDocPath &&
                                              (searchAll == "" || (x.TranslationBeforeReviewCollapsedTags.Contains(searchAll) || x.TranslationDuringReviewCollapsedTags.Contains(searchAll) || x.Segment.ToString().Contains(searchAll) ||
                                                                   reviewTranslationCommentsRepo.All().Where(cm => cm.JobItemId == x.JobItemId && cm.Segment == x.Segment && cm.DeletedDateTime == null).FirstOrDefault().Comment.Contains(searchAll) ||
                                                                   (x.LastModifiedUserName != null && (extranetUserRepository.All().Where(e => e.UserName == x.LastModifiedUserName && e.DataObjectTypeId == 1).Join(contactRepository.All(), e => e.DataObjectId, c => c.Id, (e, c) => new { contact = c }).FirstOrDefault().contact.Name.Contains(searchAll))))) &&
                                              (searchSource == "" || x.TranslationBeforeReviewCollapsedTags.Contains(searchSource)) &&
                                              (searchTarget == "" || x.TranslationDuringReviewCollapsedTags.Contains(searchTarget)))

                                       select new ReviewEditModel
                                       {
                                           ReviewTranslationId = rt.Id,
                                           JobItemId = rt.JobItemId,
                                           Segment = rt.Segment,
                                           SourceText = rt.SourceText,
                                           Comments = (jobItem.LanguageServiceId == 1 ? reviewTranslationCommentsRepo.All().Where(x => x.JobItemId == rt.JobItemId && x.Segment == rt.Segment && x.DeletedDateTime == null).FirstOrDefault().TranslateOnlineComment : reviewTranslationCommentsRepo.All().Where(x => x.JobItemId == rt.JobItemId && x.Segment == rt.Segment && x.DeletedDateTime == null).FirstOrDefault().Comment) ?? "",
                                           TranslationBeforeReview = showTags == "expanded" ? rt.TranslationBeforeReview : rt.TranslationBeforeReviewCollapsedTags,
                                           TranslationDuringReview = showTags == "expanded" ? rt.TranslationDuringReview : rt.TranslationDuringReviewCollapsedTags,
                                           TranslationBeforeReviewCollapsedTags = rt.TranslationBeforeReviewCollapsedTags,
                                           //TranslationDuringReviewCollapsedTags = rt.TranslationDuringReviewCollapsedTags,
                                           SourceLang = sourceLanguage,
                                           TargetLang = targetLanguage,
                                           ContextFieldId = rt.ContextFieldIds ?? "",
                                           Locked = rt.Locked ?? false,
                                           LastModifiedDateTime = rt.LastModifiedDateTime == null ? null : timeZonesService.ConvertGMTToRequestedTimeZone(rt.LastModifiedDateTime.Value, extranetUser.DefaultTimeZone),
                                           ContentType = rt.ContentType,
                                           ContextInfo = rt.ContextInfo,
                                           KeyContextInfo = rt.KeyContextInfo,
                                           MTTranslationReview = rt.MttranslationReview ?? "",
                                           MTTranslationReviewCollapsedTags = rt.MttranslationReviewCollapsedTags ?? "",
                                           FileName = rt.FileName,
                                           OriginalMatchPercentCSSClass = rt.OriginalMatchPercentCssclass,
                                           LastModifiedBy = rt.LastModifiedUserName == null ? "" : extranetUserRepository.All().Where(e => e.UserName == rt.LastModifiedUserName && e.DataObjectTypeId == 1).Join(contactRepository.All(), e => e.DataObjectId, c => c.Id, (e, c) => new { contact = c }).Select(c => c.contact.Name).First(),
                                       }).OrderBy(x => x.ReviewTranslationId).ToList();

                if (pageLength > -1 && pageNumber > -1)
                {
                    translationData = translationData
                                       .Skip((pageNumber - 1) * pageLength)
                                       .Take(pageLength).ToList();
                }

                return translationData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<ReviewTranslation> GetLastModifiedReviewSegment(int jobItemId)
        {
            var result = await reviewTranslationRepo.All().Where(x => x.JobItemId == jobItemId && x.LastModifiedDateTime != null).OrderByDescending(o => o.LastModifiedDateTime).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ReviewTranslation> GetAnyReviewTranslationSegment(int jobItemId)
        {
            var result = await reviewTranslationRepo.All().Where(x => x.JobItemId == jobItemId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ReviewTranslation> GetReviewTranslation(int jobItemId, int segment)
        {
            var result = await reviewTranslationRepo.All().Where(x => x.JobItemId == jobItemId && x.Segment == segment).FirstOrDefaultAsync();

            return result;
        }

        public async Task<ReviewTranslation> GetReviewTranslation(int RvTranslationId)
        {
            var result = await reviewTranslationRepo.All().Where(x => x.Id == RvTranslationId).FirstOrDefaultAsync();

            return result;
        }

        public async Task<bool> UpdateReviewDocumentTranslationUnit(int JobItemId, string FileName, int Segment, string? TranslationDuringReview,
                                                          string? TranslationDuringReviewCollapsedTags, string SourceText, bool AutoPropagate,
                                                          string UpdatedByUserName, int ReviewDocumentTUid = 0, string Comments = "")
        {
            bool updateSuccessful = false;
            if (TranslationDuringReview != null)
            {

                bool UseAlternativeTags = reviewPlusTagService.UseAlternativeTags(GetReviewDocumentTranslationUnits(JobItemId, FileName).Result);

                TranslationDuringReviewCollapsedTags = reviewPlusTagService.GetStringWithCollapsedTagsFromDB(TranslationDuringReview, JobItemId, FileName, Segment, UseAlternativeTags);
                TranslationDuringReview = reviewPlusTagService.GetStringWithExpandedTags(TranslationDuringReviewCollapsedTags, JobItemId, FileName, Segment);
                TranslationDuringReviewCollapsedTags = UnConvertAmpersandsAndAngledBracketsForXML(TranslationDuringReviewCollapsedTags);

            }

            if (TranslationDuringReviewCollapsedTags != null)
            {
                TranslationDuringReview = reviewPlusTagService.GetStringWithExpandedTags(ConvertAmpersandsAndAngledBracketsForXML(TranslationDuringReviewCollapsedTags), JobItemId, FileName, Segment);
            }

            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter ReviewerDataAdapter = new SqlDataAdapter();

            ReviewerDataAdapter.UpdateCommand = new SqlCommand("procUpdateReviewDocumentTranslationUnit", SQLConn);

            ReviewerDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_JOBITEM_ID", SqlDbType.Int)).Value = JobItemId;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_FILE_NAME", SqlDbType.NVarChar)).Value = FileName;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_SEGMENT", SqlDbType.Int)).Value = Segment;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_TRANSLATION_DURING_REVIEW", SqlDbType.NVarChar)).Value = TranslationDuringReview;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_TRANSLATION_DURING_REVIEW_COLLAPSED_TAGS", SqlDbType.NVarChar)).Value = TranslationDuringReviewCollapsedTags;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_SOURCE_TEXT", SqlDbType.NVarChar)).Value = UnConvertAmpersandsAndAngledBracketsForXML(SourceText);
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_AUTO_PROPAGATE", SqlDbType.Bit)).Value = AutoPropagate;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_LAST_MODIFIED_USERNAME", SqlDbType.NVarChar)).Value = UpdatedByUserName;

            var ReturnValParam = ReviewerDataAdapter.UpdateCommand.Parameters.Add("@P_ROW_COUNT", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                ReviewerDataAdapter.UpdateCommand.Connection.Open();
                ReviewerDataAdapter.UpdateCommand.ExecuteScalar();

                if (int.Parse(ReviewerDataAdapter.UpdateCommand.Parameters["@P_ROW_COUNT"].Value.ToString()) == 0)
                {
                    throw new Exception("Update was not successful.");
                }
                else
                {
                    updateSuccessful = true;
                    JobItem thisJobItem = await jobItemService.GetById(JobItemId);
                    if (thisJobItem.SupplierSentWorkDateTime != DateTime.MinValue && thisJobItem.SupplierAcceptedWorkDateTime == DateTime.MinValue)
                    {
                        await jobItemService.UpdateClientReviewStatus(JobItemId, Enumerations.ReviewStatus.ReviewInProgress);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    ReviewerDataAdapter.UpdateCommand.Connection.Close();
                    ReviewerDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw new Exception("SQL exception: " + se.Message);
                }
            }


            try
            {
                if (ReviewDocumentTUid != 0)
                {
                    var commentDetails = GetReviewTranslationCommentDetails(JobItemId, Segment).Result;

                    if (commentDetails != null)
                    {
                        if (commentDetails.Id != 0 || Comments != "")
                        {
                            if (commentDetails.Id != 0 && Comments != "")
                            {
                                await UpdateReviewDocumentTranslationUnitComment(JobItemId, commentDetails.Id, Comments, UpdatedByUserName);
                            }
                            else if (commentDetails.Id == 0 && Comments != "")
                            {
                                await InsertReviewDocumentTranslationUnitComment(JobItemId, FileName, Segment, Comments, UpdatedByUserName);
                            }
                        }
                    }
                    else
                    {
                        await InsertReviewDocumentTranslationUnitComment(JobItemId, FileName, Segment, Comments, UpdatedByUserName);
                    }


                }
            }
            catch
            {
                //skip and dont do anything
            }

            return updateSuccessful;
        }


        public async Task<bool> RevertReviewDocumentTranslationUnit(int JobItemId, string FileName, int Segment, string UpdatedByUserName)
        {
            bool revertSuccessful = false;

            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter ReviewerDataAdapter = new SqlDataAdapter();

            ReviewerDataAdapter.UpdateCommand = new SqlCommand("procRevertReviewDocumentTranslationUnit", SQLConn);

            ReviewerDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_JOBITEM_ID", SqlDbType.Int)).Value = JobItemId;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_FILE_NAME", SqlDbType.NVarChar)).Value = FileName;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_SEGMENT", SqlDbType.Int)).Value = Segment;
            ReviewerDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_LAST_MODIFIED_USERNAME", SqlDbType.NVarChar)).Value = UpdatedByUserName;

            var ReturnValParam = ReviewerDataAdapter.UpdateCommand.Parameters.Add("@P_ROW_COUNT", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                ReviewerDataAdapter.UpdateCommand.Connection.Open();
                ReviewerDataAdapter.UpdateCommand.ExecuteScalar();

                if (int.Parse(ReviewerDataAdapter.UpdateCommand.Parameters["@P_ROW_COUNT"].Value.ToString()) == 0)
                {
                    throw new Exception("Update was not successful.");
                }
                else
                {
                    revertSuccessful = true;
                    JobItem thisJobItem = await jobItemService.GetById(JobItemId);
                    if (thisJobItem.SupplierSentWorkDateTime != DateTime.MinValue && thisJobItem.SupplierAcceptedWorkDateTime == DateTime.MinValue)
                    {
                        await jobItemService.UpdateClientReviewStatus(JobItemId, Enumerations.ReviewStatus.ReviewInProgress);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                try
                {
                    ReviewerDataAdapter.UpdateCommand.Connection.Close();
                    ReviewerDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw new Exception("SQL exception: " + se.Message);
                }
            }

            return revertSuccessful;
        }

        public string UnConvertAmpersandsAndAngledBracketsForXML(string TextToProcess)
        {
            return TextToProcess.Replace("&gt;", ">").Replace("&lt;", "<").Replace("&amp;", "&");
        }

        public string ConvertAmpersandsAndAngledBracketsForXML(string TextToProcess)
        {
            return TextToProcess.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        public async Task<List<ReviewTranslation>> GetReviewDocumentTranslationUnits(int JobItemId, string FileName)
        {
            var result = await reviewTranslationRepo.All().Where(i => i.JobItemId == JobItemId && i.FileName == FileName).OrderBy(o => o.Id).ToListAsync();
            return result;
        }

        public async Task<ReviewTranslationComment> GetReviewTranslationCommentDetails(int jobItemId, int segment)
        {
            var result = await reviewTranslationCommentsRepo.All().Where(r => r.JobItemId == jobItemId && r.Segment == segment && r.DeletedDateTime == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> InsertReviewDocumentTranslationUnitComment(int JobItemID, string FileName, int Segment, string Comments,
                                                                          string extranetUserName)
        {

            var thisJobItem = await jobItemService.GetById(JobItemID);
            int NewReviewTuCommentId;
            ReviewTranslationComment NewReviewTuComment;
            string RvComment = null;
            string trOnlineComment = null;
            if (thisJobItem.LanguageServiceId == 21 || thisJobItem.LanguageServiceId == 67)
            {
                RvComment = Comments;
            }
            else if (thisJobItem.LanguageServiceId == 1)
            {
                trOnlineComment = Comments;
            }
            NewReviewTuComment = new ReviewTranslationComment()
            {
                JobItemId = JobItemID,
                FileName = FileName,
                Segment = Segment,
                Comment = RvComment,
                TranslateOnlineComment = trOnlineComment,
                CreatedUserName = extranetUserName,
                CreatedDateTime = GeneralUtils.GetCurrentUKTime()
            };
            await reviewTranslationCommentsRepo.AddAsync(NewReviewTuComment);
            await reviewTranslationCommentsRepo.SaveChangesAsync();
            NewReviewTuCommentId = NewReviewTuComment.Id;

            return NewReviewTuCommentId;
        }

        public async Task<ReviewTranslationComment> UpdateReviewDocumentTranslationUnitComment(int JobItemID, int CommentID, string Comments, string extranetUserName)
        {
            var thisJobItem = await jobItemService.GetById(JobItemID);

            ReviewTranslationComment commentToUpdate = await reviewTranslationCommentsRepo.All().Where(r => r.Id == CommentID).FirstOrDefaultAsync();

            if (thisJobItem.LanguageServiceId == 21 || thisJobItem.LanguageServiceId == 67)
            {
                commentToUpdate.Comment = Comments;
            }
            else if (thisJobItem.LanguageServiceId == 1)
            {
                commentToUpdate.TranslateOnlineComment = Comments;
            }

            commentToUpdate.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            commentToUpdate.LastModifiedUserName = extranetUserName;

            reviewTranslationCommentsRepo.Update(commentToUpdate);
            await reviewTranslationCommentsRepo.SaveChangesAsync();

            return commentToUpdate;

        }

        public async Task<bool> ValidateTargetText(bool isCollapsedTag, string textToValidate, int reviewTuId)
        {
            bool isTargetTextValid = false;
            //try
            //{
            List<TPTagModel> BeforeChangesTargetTextTags = null;
            List<TPTagModel> DuringReviewTargetTextTags = null;

            List<TPTagModel> BeforeChangesTargetTextSingleTags = null;
            List<TPTagModel> DuringReviewTargetTextSingleTags = null;

            List<String> SourceTextCollapsedTags = null;

            List<String> DuringReviewTargetTextCollapsedTags = null;
            List<String> BeforeChangesTargetTextCollapsedTags = null;

            var AuxTranslationUnit = await GetReviewTranslation(reviewTuId);

            var JobItemBeingReviewedOrTranslated = await jobItemService.GetById(AuxTranslationUnit.JobItemId);

            //Depending on the file type we have to "count" the tags that were existing in the translation before review and after review.
            //the number of tags HAVE TO MATCH. If not we don't let save the translation since the document could get corrupted

            if (AuxTranslationUnit.FileName.EndsWith(".sdlxliff") == true)
            {
                if (isCollapsedTag == true)
                {
                    DuringReviewTargetTextCollapsedTags = reviewPlusTagService.GetNumberInBrackesTagsList(textToValidate);
                    BeforeChangesTargetTextCollapsedTags = reviewPlusTagService.GetNumberInBrackesTagsList(AuxTranslationUnit.TranslationDuringReviewCollapsedTags);
                    isTargetTextValid = BeforeChangesTargetTextCollapsedTags.SequenceEqual(DuringReviewTargetTextCollapsedTags);
                }
                else
                {
                    BeforeChangesTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(AuxTranslationUnit.TranslationBeforeReview);
                    DuringReviewTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(textToValidate);
                    isTargetTextValid = BeforeChangesTargetTextTags.SequenceEqual(DuringReviewTargetTextTags, new TPTagsComparer());
                }

                var jobOrder = await orderService.GetById(JobItemBeingReviewedOrTranslated.JobOrderId);
                var contact = await contactService.GetById(jobOrder.ContactId);
                var org = await orgService.GetOrgDetails(contact.OrgId);

                OrgGroup group = null;
                if (org.OrgGroupId != null)
                {
                    group = await groupService.GetOrgGroupDetails(org.OrgGroupId.Value);

                }
                if ((org.Id == 68268 || JobItemBeingReviewedOrTranslated.Id == 545525 ||
                       (org.LinguisticDatabaseId != null || (group != null && group.LinguisticDatabaseId != null))) && isTargetTextValid == true)
                {
                    if (isCollapsedTag == true)
                    {
                        BeforeChangesTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(AuxTranslationUnit.TranslationDuringReviewCollapsedTags);
                        DuringReviewTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(textToValidate);
                        isTargetTextValid = BeforeChangesTargetTextTags.SequenceEqual(DuringReviewTargetTextTags, new TPTagsComparer());
                    }
                    else
                    {
                        BeforeChangesTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(System.Web.HttpUtility.HtmlDecode(AuxTranslationUnit.TranslationBeforeReview));
                        DuringReviewTargetTextTags = reviewPlusTagService.GetTagsListSDLXLIFF(textToValidate);
                        isTargetTextValid = BeforeChangesTargetTextTags.SequenceEqual(DuringReviewTargetTextTags, new TPTagsComparer());
                    }
                }
            }
            else
            {
                if (isCollapsedTag == true)
                {
                    DuringReviewTargetTextCollapsedTags = reviewPlusTagService.GetNumberInBrackesTagsList(textToValidate);
                    BeforeChangesTargetTextCollapsedTags = reviewPlusTagService.GetNumberInBrackesTagsList(AuxTranslationUnit.TranslationDuringReviewCollapsedTags);
                    isTargetTextValid = BeforeChangesTargetTextCollapsedTags.SequenceEqual(DuringReviewTargetTextCollapsedTags);
                }
                else
                {
                    BeforeChangesTargetTextTags = reviewPlusTagService.GetTagsList(AuxTranslationUnit.TranslationBeforeReview);
                    DuringReviewTargetTextTags = reviewPlusTagService.GetTagsList(textToValidate);
                    BeforeChangesTargetTextSingleTags = reviewPlusTagService.GetSingleTagsList(AuxTranslationUnit.TranslationBeforeReview);
                    DuringReviewTargetTextSingleTags = reviewPlusTagService.GetSingleTagsList(textToValidate);
                    isTargetTextValid = BeforeChangesTargetTextTags.SequenceEqual(DuringReviewTargetTextTags, new TPTagsComparer()) && BeforeChangesTargetTextSingleTags.SequenceEqual(DuringReviewTargetTextSingleTags, new TPTagsComparer());
                }

            }


            //}
            //catch
            //{
            //    //skip
            //}

            return isTargetTextValid;
        }


        public async Task<JobItem> UpdateJobItemReviewSegments(int jobitemid)
        {
            var jobitem = await jobItemService.GetById(jobitemid);

            jobitem.TotalNumberOfChangedReviewSegments = GetTotalNumberOfUpdatedSegments(jobitemid);

            jobitemsRepo.Update(jobitem);
            await jobitemsRepo.SaveChangesAsync();

            return jobitem;
        }

        public int GetTotalNumberOfUpdatedSegments(int jobitemId)
        {
            var result = reviewTranslationRepo.All().Where(rt => rt.ReviewStatus == false && rt.JobItemId == jobitemId && rt.TranslationBeforeReview != rt.TranslationDuringReview).Count();

            return result;
        }

        public async Task<List<int>> GetAllSegmentsWithSameText(int jobItem, int segmentId, string fileName)
        {
            var thisSegment = await reviewTranslationRepo.All().Where(r => r.JobItemId == jobItem && r.FileName == fileName && r.Segment == segmentId).FirstOrDefaultAsync();
            var result = await reviewTranslationRepo.All().
                         Where(r => r.JobItemId == jobItem && r.FileName == fileName && r.TranslationBeforeReviewCollapsedTags == thisSegment.SourceText.TrimStart().TrimEnd().Replace("&#166;", "¦")).
                         Select(x => x.Segment).ToListAsync();

            return result;

        }


    }

}

