using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using ViewModels.translateOnline;
using ViewModels.flowPlusExternal.ReviewPlus;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Web;
using System.Xml;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TranslateOnlineService : ITranslateOnlineService
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
        private readonly ITPEmployeesService employeesService;
        private readonly IEmailUtilsService emailService;
        public TranslateOnlineService(IRepository<Contact> _contactRepository,
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
                                     IConfiguration configuration,
                                     ITPEmployeesService _employeesService,
                                     IEmailUtilsService _emailService)
        {
            contactRepository = _contactRepository;
            orgRepository = _orgRepository;
            extranetUserRepository = _extranetUserRepository;
            oldExtranetUserRepository = _oldExtranetUserRepository;
            extranetUserReviewLangRepository = _extranetUserReviewLangRepository;
            timeZonesService = tPTimeZonesService;
            orgRepo = repository4;
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
            this.employeesService = _employeesService;
            this.emailService = _emailService;
        }
        public async Task<List<translateOnlineStatusModel>> GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus reviewStatus, string extranetUserName, bool loadAssigned,
           int pageNumber = -1, int pageSize = -1, string searchTerm = "", int columnToOrderBy = -1, string orderDirection = "")
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var userAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var extranetUserContact = await contactRepository.All().Where(x => x.Id == extranetUser.DataObjectId && x.DeletedDate == null).FirstOrDefaultAsync();
            bool ViewItemsAssignedToCurrentUser = false;
            List<translateOnlineStatusModel> translationDocuments = new List<translateOnlineStatusModel>();
            if (extranetUser.DataObjectTypeId != Convert.ToByte(Enumerations.DataObjectTypes.Contact))
            {
                // Return empty table
                return null;
            }
            if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherOrgJobsInAnyLanguageCombo == false
                && userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo == false)
            {
                ViewItemsAssignedToCurrentUser = true;
            }

            // Get pending or inprogress translation items assigned to logged in user.
            if (ViewItemsAssignedToCurrentUser || loadAssigned == true)
            {
                translationDocuments = await (from order in orderRepository.All()
                                              join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                              join contactTranslator in contactRepository.All().Where(c => c.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                              join contactOrder in contactRepository.All().Where(c => c.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                              join org in orgRepo.All().Where(c => c.DeletedDate == null) on contactTranslator.OrgId equals org.Id
                                              select new
                                              {
                                                  jItem,
                                                  order,
                                                  contactTranslator,
                                                  contactOrder,
                                                  org
                                              })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.order.JobOrderChannelId != 21
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactTranslator.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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
                    translationDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                            join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                            join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                            join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                            join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                            select new
                                            {
                                                jItem,
                                                order,
                                                contactTranslator,
                                                contactOrder,
                                                org
                                            })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactTranslator.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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

                    translationDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == contactOrg.OrgGroupId)
                                            join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                            join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                            join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                            join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                            select new
                                            {
                                                jItem,
                                                order,
                                                contactTranslator,
                                                contactOrder,
                                                org
                                            })
                               .Where(x => x.jItem.LanguageServiceId == 1
                               && x.jItem.SupplierIsClientReviewer == true
                               && x.jItem.SupplierSentWorkDateTime != null
                               && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                               && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                               && x.jItem.SupplierCompletedItemDateTime == null
                               && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactTranslator.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                               .Select(x => new translateOnlineStatusModel
                               {
                                   RequestedBy = x.contactOrder.Name,
                                   Contact = x.contactTranslator.Name,
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

                translationDocuments = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                        join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                        join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                        join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                        join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                        select new
                                        {
                                            jItem,
                                            order,
                                            contactTranslator,
                                            contactOrder,
                                            org
                                        })
                              .Where(x => x.jItem.LanguageServiceId == 1
                              && x.jItem.SupplierIsClientReviewer == true
                              && x.jItem.SupplierSentWorkDateTime != null
                              && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                              && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                              && x.jItem.SupplierCompletedItemDateTime == null
                               && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactTranslator.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                              .Select(x => new translateOnlineStatusModel
                              {
                                  RequestedBy = x.contactOrder.Name,
                                  Contact = x.contactTranslator.Name,
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
                translationDocuments = (from order in orderRepository.All()
                                        join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                        join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                        join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                        join org in orgRepo.All() on contactTranslator.OrgId equals org.Id
                                        select new
                                        {
                                            jItem,
                                            order,
                                            contactTranslator,
                                            contactOrder,
                                            org
                                        })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && (reviewStatus == Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance ? x.jItem.SupplierAcceptedWorkDateTime == null : x.jItem.SupplierAcceptedWorkDateTime != null)
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null
                                 && (searchTerm == "" || (x.contactOrder.Name.Contains(searchTerm) || x.contactTranslator.Name.Contains(searchTerm) ||
                                                          x.jItem.Id.ToString().Contains(searchTerm) || x.order.JobName.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == x.jItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name.Contains(searchTerm) ||
                                                          x.jItem.SupplierSentWorkDateTime.ToString().Contains(searchTerm))))
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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
                foreach (translateOnlineStatusModel translationDoc in translationDocuments)
                {
                    translationDoc.TranslationProgress = GetTranslationProgress(translationDoc.JobItemId);
                }
            }

            if (pageNumber > -1 && pageSize > -1)
            {
                translationDocuments = translationDocuments.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            //if (searchTerm != "")
            //{
            //    //translationDocuments = translationDocuments.Where(r => r.RequestedBy.Contains(searchTerm) || r.Reviewer.Contains(searchTerm) || r.JobItemId.ToString().Contains(searchTerm) ||
            //    //                      r.JobName.Contains(searchTerm) || r.SourceLanguage.Contains(searchTerm) || r.TargetLanguage.Contains(searchTerm) ||
            //    //                      r.DueDate.ToString().Contains(searchTerm) || r.SupplierWordCountTotal.ToString().Contains(searchTerm) ||
            //    //                      r.SupplierSentWorkDateTime.ToString().Contains(searchTerm)).ToList();
            //    translationDocuments = translationDocuments.Where(r => r.RequestedBy.Contains(searchTerm)).ToList();
            //}

            if (orderDirection == "desc")
            {
                if (columnToOrderBy == 0)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.RequestedBy).ToList();
                }
                else if (columnToOrderBy == 1)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.Contact).ToList();
                }
                else if (columnToOrderBy == 2)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.JobItemId).ToList();
                }
                else if (columnToOrderBy == 3)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.JobName).ToList();
                }
                else if (columnToOrderBy == 4)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.SourceLanguage).ToList();
                }
                else if (columnToOrderBy == 5)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.TargetLanguage).ToList();
                }
                else if (columnToOrderBy == 7)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.DueDate).ToList();
                }
                else if (columnToOrderBy == 8)
                {
                    translationDocuments = translationDocuments.OrderByDescending(x => x.SupplierWordCountTotal).ToList();
                }
            }
            else if (orderDirection == "asc")
            {
                if (columnToOrderBy == 0)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.RequestedBy).ToList();
                }
                else if (columnToOrderBy == 1)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.Contact).ToList();
                }
                else if (columnToOrderBy == 2)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.JobItemId).ToList();
                }
                else if (columnToOrderBy == 3)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.JobName).ToList();
                }
                else if (columnToOrderBy == 4)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.SourceLanguage).ToList();
                }
                else if (columnToOrderBy == 5)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.TargetLanguage).ToList();
                }
                else if (columnToOrderBy == 7)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.DueDate).ToList();
                }
                else if (columnToOrderBy == 8)
                {
                    translationDocuments = translationDocuments.OrderBy(x => x.SupplierWordCountTotal).ToList();
                }
            }

            // Generating data for view
            return translationDocuments;
        }
        public async Task<PendingAndInProgressTranslationModel> GetPendingAndInprogressTranslationJobItems(string extranetUserName)
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var userAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var extranetUserContact = await contactRepository.All().Where(x => x.Id == extranetUser.DataObjectId && x.DeletedDate == null).FirstOrDefaultAsync();
            bool ViewItemsAssignedToCurrentUser = false;
            List<translateOnlineStatusModel> inProgressTranslationDocuments = new List<translateOnlineStatusModel>();
            List<translateOnlineStatusModel> pendingTranslationDocuments = new List<translateOnlineStatusModel>();
            //List<ReviewStatusModel> inProgressTranslationDocuments = new List<ReviewStatusModel>();
            if (extranetUser.DataObjectTypeId != Convert.ToByte(Enumerations.DataObjectTypes.Contact))
            {
                // Return empty table
            }
            if (userAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherOrgJobsInAnyLanguageCombo == false
                && userAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos == false
                && userAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo == false)
            {
                ViewItemsAssignedToCurrentUser = true;
            }

            var translationData = new List<translateOnlineStatusModel>();

            // Get pending or inprogress translation items assigned to logged in user.
            if (ViewItemsAssignedToCurrentUser)
            {
                translationData = (from order in orderRepository.All().Where(x => x.DeletedDate == null)
                                   join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                   join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                   join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                   join org in orgRepo.All().Where(x => x.DeletedDate == null) on contactTranslator.OrgId equals org.Id
                                   select new
                                   {
                                       jItem,
                                       order,
                                       contactTranslator,
                                       contactOrder,
                                       org
                                   })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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
                    translationData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                       join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                       join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                       join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                       join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                       select new
                                       {
                                           jItem,
                                           order,
                                           contactTranslator,
                                           contactOrder,
                                           org
                                       })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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
                    translationData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.OrgGroupId == contactOrg.OrgGroupId)
                                       join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                       join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                       join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                       join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                       select new
                                       {
                                           jItem,
                                           order,
                                           contactTranslator,
                                           contactOrder,
                                           org
                                       })
                               .Where(x => x.jItem.LanguageServiceId == 1
                               && x.jItem.SupplierIsClientReviewer == true
                               && x.jItem.SupplierSentWorkDateTime != null
                               && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                               && x.jItem.SupplierCompletedItemDateTime == null)
                               .Select(x => new translateOnlineStatusModel
                               {
                                   RequestedBy = x.contactOrder.Name,
                                   Contact = x.contactTranslator.Name,
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

                translationData = (from org in orgRepo.All().Where(o => o.DeletedDate == null && o.Id == contactOrg.Id)
                                   join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on org.Id equals contactOrder.OrgId
                                   join order in orderRepository.All().Where(x => x.DeletedDate == null) on contactOrder.Id equals order.ContactId
                                   join jItem in jobitemsRepo.All().Where(x => x.DeletedDateTime == null) on order.Id equals jItem.JobOrderId
                                   join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                   select new
                                   {
                                       jItem,
                                       order,
                                       contactTranslator,
                                       contactOrder,
                                       org
                                   })
                              .Where(x => x.jItem.LanguageServiceId == 1
                              && x.jItem.SupplierIsClientReviewer == true
                              && x.jItem.SupplierSentWorkDateTime != null
                              && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                              && x.jItem.SupplierCompletedItemDateTime == null)
                              .Select(x => new translateOnlineStatusModel
                              {
                                  RequestedBy = x.contactOrder.Name,
                                  Contact = x.contactTranslator.Name,
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
                translationData = (from order in orderRepository.All().Where(x => x.DeletedDate == null)
                                   join jItem in jobitemsRepo.All().Where(i => i.DeletedDateTime == null && i.LinguisticSupplierOrClientReviewerId == extranetUserContact.Id && i.SupplierIsClientReviewer == true) on order.Id equals jItem.JobOrderId
                                   join contactTranslator in contactRepository.All().Where(x => x.DeletedDate == null) on jItem.LinguisticSupplierOrClientReviewerId equals contactTranslator.Id
                                   join contactOrder in contactRepository.All().Where(x => x.DeletedDate == null) on order.ContactId equals contactOrder.Id
                                   join org in orgRepo.All().Where(x => x.DeletedDate == null) on contactTranslator.OrgId equals org.Id
                                   select new
                                   {
                                       jItem,
                                       order,
                                       contactTranslator,
                                       contactOrder,
                                       org
                                   })
                                 .Where(x => x.jItem.LanguageServiceId == 1
                                 && x.jItem.SupplierIsClientReviewer == true
                                 && x.jItem.SupplierSentWorkDateTime != null
                                 && x.jItem.DeletedDateTime == null && x.order.DeletedDate == null
                                 && x.jItem.SupplierCompletedItemDateTime == null)
                                 .Select(x => new translateOnlineStatusModel
                                 {
                                     RequestedBy = x.contactOrder.Name,
                                     Contact = x.contactTranslator.Name,
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

            pendingTranslationDocuments = translationData.Where(x => x.SupplierAcceptedWorkDateTime == null).ToList();
            inProgressTranslationDocuments = translationData.Where(x => x.SupplierAcceptedWorkDateTime != null).ToList();

            // Generating data for view
            PendingAndInProgressTranslationModel model = new();
            model.PendingTranslationDocuments = pendingTranslationDocuments;
            model.InProgressTranslationDocuments = inProgressTranslationDocuments;
            return model;
        }

        public string GetTranslationProgress(int jobItemId)
        {
            decimal translationProgress = 0;
            var jobItem = jobItemService.GetById(jobItemId).Result;
            var thisJobOrder = orderService.GetById(jobItem.JobOrderId).Result;
            var thisContact = contactService.GetById(thisJobOrder.ContactId).Result;
            if (jobItem != null)
            {
                if (jobItem.WeCompletedItemDateTime != null)
                {
                    translationProgress = 100;
                }
                else
                {
                    if (jobItem.SupplierCompletedItemDateTime != null)
                    {
                        translationProgress = 100;
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
                        SqlDataAdapter TranslateOnlineDataAdapter = new SqlDataAdapter("procGetPercentageReviewingProgress", SQLConn);

                        TranslateOnlineDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                        TranslateOnlineDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@P_JOB_ITEM_ID", SqlDbType.Int)).Value = jobItemId;
                        TranslateOnlineDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@P_FILE_NAME", SqlDbType.NVarChar)).Value = BilingualDocPath;

                        SqlParameter OutputParameter = TranslateOnlineDataAdapter.SelectCommand.Parameters.Add(new SqlParameter("@PROGRESS", SqlDbType.Decimal));
                        OutputParameter.Precision = 4;
                        OutputParameter.Scale = 1;
                        OutputParameter.Direction = ParameterDirection.Output;


                        try
                        {
                            TranslateOnlineDataAdapter.SelectCommand.Connection.Open();
                            TranslateOnlineDataAdapter.SelectCommand.ExecuteScalar();
                            translationProgress = (decimal)OutputParameter.Value;
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error running stored procedure to get review progress.");
                        }
                        finally
                        {
                            try
                            {
                                TranslateOnlineDataAdapter.SelectCommand.Connection.Close();
                                TranslateOnlineDataAdapter.Dispose();
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
                translationProgress = (decimal)translationProgress / 2;
            }



            return translationProgress.ToString("0.0");
        }

        public async System.Threading.Tasks.Task ApproveTranslation(string extranetUserName, int jobItemId, string comments)
        {
            var extranetUser = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var userContact = await extranetUserService.GetExtranetUserContact(extranetUserName);

            var jobItem = await jobItemService.GetById(jobItemId);
            var thisOrder = await orderService.GetById(jobItem.JobOrderId);
            var thisContact = await contactService.GetById(thisOrder.ContactId);
            var thisOrg = await orgService.GetOrgDetails(thisContact.OrgId);


            // update status (CompleteAndSignedOffByReviewer)
            // update supplier status (CompleteAndSignedOffBySupplier)
            jobItem.SupplierCompletedItemDateTime = GeneralUtils.GetCurrentGMT();
            if (comments != null)
            {
                jobItem.ExtranetSignoffComment = comments;
            }
            if (jobItem.SupplierAcceptedWorkDateTime == null)
            {
                jobItem.SupplierAcceptedWorkDateTime = GeneralUtils.GetCurrentGMT();
            }
            await jobitemsRepo.SaveChangesAsync();

            var projectManager = await employeesService.IdentifyCurrentUserById(thisOrder.ProjectManagerEmployeeId);
            // sending internal notification
            string NotificationRecipients = projectManager.EmailAddress;


            string JobInfo = string.Format(@"<table><tr><td><p>Organisation:  </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/Organisation?id={0}""> {0} - {1}</a></td></tr><tr><td><p>Contact: </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/Contact?contactid={2}"">{2} - {3}</a></td></tr><tr><td><p>Job order: </td><td><p><a href=""https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid={4}"">{4} - {5}</a></td></tr></table>",
                                    thisOrg.Id, thisOrg.OrgName, thisContact.Id, thisContact.Name, thisOrder.Id, System.Web.HttpUtility.HtmlEncode(thisOrder.JobName));
            string InfoAboutComments = "";
            if (comments == null || comments == "")
            {
                InfoAboutComments = "No comments were submitted with the approval.";
            }
            else
            {
                if (jobItem.LanguageServiceId == 1 && ((bool)jobItem.SupplierIsClientReviewer || thisOrg.Id == 79702))
                {
                    InfoAboutComments = string.Format(@"The following comments were submitted with the completion on translation, and have been added to the job item's ""Client contact signoff comments"": <br /><br /><font color=""green"">" + HttpUtility.HtmlEncode(comments) + "</font>");


                }
                else if (jobItem.LanguageServiceId == 1 && jobItem.SupplierIsClientReviewer == false)
                {
                    InfoAboutComments = string.Format(@"The following comments were submitted with the completion on translation, and have been added to the job item's ""Linguist signoff comments"": <br /><br /><font color=""green"">" + HttpUtility.HtmlEncode(comments) + "</font>");

                }

            }

            if (jobItem.SupplierIsClientReviewer == true)
            {
                var translator = await contactService.GetById(jobItem.LinguisticSupplierOrClientReviewerId.Value);
                emailService.SendMail("flow plus <flowplus@translateplus.com>", NotificationRecipients, "Online translation job item " + jobItemId.ToString() + " for " +
                               thisOrg.OrgName + " has been signed off",
                               "<p>Job item ID <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" + jobItemId.ToString() + "\">" + jobItemId.ToString() +
                               "</a> (assigned to client contact <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + translator.Id.ToString() + "\">" + translator.Name +
                               "</a>) has been marked as completed in translate online by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + userContact.Id.ToString() + "\">" + userContact.Name +
                               "</a>. <br /><br />" + JobInfo + "<br />" + InfoAboutComments +
                               " <br /><br />We should proceed to any internal checking stages prior to final delivery to the client; or if this has been carried out by an internal translator at the client then the file will be automatically delivered back to the requesting contact.</p>",
                               IsExternalNotification: true);
            }
            else
            {
                //var linguiticSupplier = await li.GetById(jobItem.LinguisticSupplierOrClientReviewerId.Value);
                //emailService.SendMail("i plus <iplus@translateplus.com>", NotificationRecipients, "Online translation job item " + jobItemId.ToString() + " for " +
                //              thisOrg.OrgName + " has been signed off",
                //              "<p>Job item ID <a href=\"http://myplus/JobItem.aspx?JobItemID=" + jobItemId.ToString() + "\">" + jobItemId.ToString() +
                //              "</a> (assigned to linguistic supplier <a href=\"http://myplus/Linguist.aspx?LinguistID=" + jobItem.LinguisticSupplierOrClientReviewerId.ToString() + """>" + JobItemBeingSignedOff.LinguisticSupplier.DisplayName +
                //              "</a>) has been marked as completed in translate online by <a href=""http://myplus/Linguist.aspx?LinguistID=" + LoggedInExtranetUser.LinguisticSupplierObject.ID.ToString() + """>" + LoggedInExtranetUser.LinguisticSupplierObject.DisplayName +
                //              "</a>. <br /><br />" + JobInfo + "<br />" + InfoAboutComments + 
                //              " <br /><br />We should proceed to any internal checking stages prior to final delivery to the client; or if this has been carried out by an internal translator at the client then the file will be automatically delivered back to the requesting contact.</p>")
            }



            XmlDocument BatchDoc = new XmlDocument();
            BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                             "<!-- translate plus process automation batch file -->" + Environment.NewLine +
                             "<translateplusBatch />");

            XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

            XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
            BatchDocAttr.Value = "1.0";
            RootNode.Attributes.Append(BatchDocAttr);

            BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
            BatchDocAttr.Value = "iplus";
            RootNode.Attributes.Append(BatchDocAttr);

            //write e-mail notification address(es) info
            BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
            BatchDocAttr.Value = projectManager.EmailAddress;
            RootNode.Attributes.Append(BatchDocAttr);


            XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");


            //write task type info
            BatchDocAttr = BatchDoc.CreateAttribute("Type");
            BatchDocAttr.Value = "ReceiveJobItemFromTranslateOnlineViaExtranet";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            //write task number info 
            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
            BatchDocAttr.Value = "1";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            BatchDocAttr = BatchDoc.CreateAttribute("JobItemID");
            BatchDocAttr.Value = jobItemId.ToString();
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            //now append the node to the doc
            RootNode.AppendChild(IndividualTaskNode);

            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            BatchDoc.Save(BatchFilePath);


        }


        public void RevertAllTranslations(string extranetUserName, int jobItemId, string FileName)
        {

            SqlConnection SQLConn = new SqlConnection(globalVariables.ConnectionString);
            SqlDataAdapter TranslationDataAdapter = new SqlDataAdapter();

            TranslationDataAdapter.UpdateCommand = new SqlCommand("procRevertReviewDocumentTranslation", SQLConn);

            TranslationDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;

            TranslationDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_JOBITEM_ID", SqlDbType.Int)).Value = jobItemId;
            TranslationDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_FILE_NAME", SqlDbType.NVarChar)).Value = FileName;
            TranslationDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_LAST_MODIFIED_USERNAME", SqlDbType.NVarChar)).Value = extranetUserName;

            var ReturnValParam = TranslationDataAdapter.UpdateCommand.Parameters.Add("@ROWS_AFFECTED", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                TranslationDataAdapter.UpdateCommand.Connection.Open();
                TranslationDataAdapter.UpdateCommand.ExecuteScalar();

                if (int.Parse(TranslationDataAdapter.UpdateCommand.Parameters["@ROWS_AFFECTED"].Value.ToString()) == 0)
                {
                    throw new Exception("Revert was not successful.");
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
                    TranslationDataAdapter.UpdateCommand.Connection.Close();
                    TranslationDataAdapter.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw new Exception("SQL exception: " + se.Message);
                }
            }

        }

    }
}
