using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Security.Claims;
using Services;
using ViewModels.translateOnline;
using ViewModels.flowPlusExternal.ReviewPlus;
using Global_Settings;
using System.IO;
using Data;
using Data.Repositories;
using Newtonsoft.Json;

namespace flowPlusExternal.Controllers
{
    public class translateOnlineController : Controller
    {
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPflowplusLicencingLogic flowplusLicencingService;
        private readonly ITranslateOnlineService translateOnlineService;
        private readonly ITPJobItemService jobItemService;
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPContactsLogic contactService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPClientReviewService reviewService;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly IRepository<LocalLanguageInfo> localLangRepo;
        private readonly ITPBrandsService brandsService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPTimeZonesService timeZonesService;

        public translateOnlineController(ITPExtranetUserService _extranetUserService,
                                         ITPflowplusLicencingLogic _flowplusLicencingService,
                                         ITranslateOnlineService _translateOnlineService,
                                         ITPJobItemService _jobItemService,
                                         ITPJobOrderService _jobOrderService,
                                         ITPContactsLogic _contactService,
                                         ITPOrgsLogic _orgService, ITPClientReviewService tPClientReview,
                                         IRepository<LocalLanguageInfo> _localLangInfo,
                                         ITPMiscResourceService _miscResourceService,
                                         ITPBrandsService _brandsService,
                                         IEmailUtilsService _emailUtils,
                                         ITPEmployeesService _employeesService,
                                         ITPTimeZonesService _timeZonesService)
        {
            this.extranetUserService = _extranetUserService;
            this.flowplusLicencingService = _flowplusLicencingService;
            this.translateOnlineService = _translateOnlineService;
            this.jobItemService = _jobItemService;
            this.jobOrderService = _jobOrderService;
            this.contactService = _contactService;
            this.orgService = _orgService;
            this.reviewService = tPClientReview;
            this.localLangRepo = _localLangInfo;
            this.miscResourceService = _miscResourceService;
            this.brandsService = _brandsService;
            this.emailService = _emailUtils;
            this.employeesService = _employeesService;
            this.timeZonesService = _timeZonesService;
        }
        public async Task<IActionResult> translateOnlineStatus()
        {
            var extranetUserDetails = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserDetails.UserName;

            ViewBag.DataObjectID = extranetUserDetails.DataObjectId;
            ViewBag.DataObjectTypeId = extranetUserDetails.DataObjectTypeId;

            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var FreeTrailCutOffDate = new DateTime(2023, 04, 01);
            if (extranetUserDetails.DataObjectTypeId == 1 && GeneralUtils.GetCurrentUKTime() >= FreeTrailCutOffDate)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserName);
                var translateOnlineLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var translateOnlineLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (translateOnlineLicencingGroupLevel != null)
                {
                    var translateOnlineLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingGroupLevel.translateOnlineLicenceID.Value);
                    if (translateOnlineLicence != null)
                    {
                        AccessPermitted = translateOnlineLicence.IsEnabled;
                    }
                }

                if (translateOnlineLicencingOrgLevel != null)
                {
                    var translateOnlineLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingOrgLevel.translateOnlineLicenceID.Value);
                    if (translateOnlineLicence != null)
                    {
                        AccessPermitted = translateOnlineLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/translate%20online");
                }
            }

            bool allowToChangeReviewer = false;

            if (accessLevel.CanRerouteReviewJobsToOtherOrgExtranetUsers == true || accessLevel.CanRerouteReviewJobsToOtherGroupExtranetUsers == true)
            {
                allowToChangeReviewer = true;
            }

            var showOwnAndOtherToggleButtons = false;
            if (accessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo || accessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos ||
                accessLevel.CanReviewOtherOrgJobsInAnyLanguageCombo || accessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos)
            {
                showOwnAndOtherToggleButtons = true;
            }

            ViewBag.toShowToggleButtons = showOwnAndOtherToggleButtons;

            ViewBag.isAllowedToChangeTranslator = allowToChangeReviewer;

            ViewBag.extranetUserTimeZone = extranetUserDetails.DefaultTimeZone;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> GetPendingDocumentsToBeTranslated([FromBody] DataTablesTranslateOnline dataParams)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var filteredResult = await translateOnlineService.GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);
            //var filteredResult = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, 2, "desc");
            var result = await translateOnlineService.GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned);
            var data = filteredResult.Skip(0).ToList();
            return Ok(new { data, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        }

        [HttpPost]
        public async Task<IActionResult> GetDocumentsInProgress([FromBody] DataTablesTranslateOnline dataParams)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var filteredResult = await translateOnlineService.GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus.ReviewInProgress, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);

            var result = await translateOnlineService.GetPendingAndInprogressTranslationDocuments(Enumerations.ReviewStatus.ReviewInProgress, extranetUserName, (bool)dataParams.loadAssigned);
            var data = filteredResult.Skip(0).ToList();
            return Ok(new { data, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        }

        [HttpGet]
        [Route("[controller]/[action]/{jobItemId}")]
        public async Task<IActionResult> SignOff(int jobItemId)
        {
            var extranetUserObj = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserObj.UserName;

            var jobItem = await jobItemService.GetById(jobItemId);
            var joborder = await jobOrderService.GetById(jobItem.JobOrderId);
            var contact = await contactService.GetById(joborder.ContactId);
            var orderOrg = await orgService.GetOrgDetails(contact.OrgId);

            var loggedInUserOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);
            var extranetUserAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var permittedToViewThisOrder = false;


            if (extranetUserObj.DataObjectTypeId == 1)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserObj.UserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserObj.UserName);
                var translateOnlineLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var translateOnlineLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (translateOnlineLicencingGroupLevel != null)
                {
                    var translateOnlinLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingGroupLevel.translateOnlineLicenceID.Value);
                    if (translateOnlinLicence != null)
                    {
                        AccessPermitted = translateOnlinLicence.IsEnabled;
                    }
                }

                if (translateOnlineLicencingOrgLevel != null)
                {
                    var translateOnlinLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingOrgLevel.translateOnlineLicenceID.Value);
                    if (translateOnlinLicence != null)
                    {
                        AccessPermitted = translateOnlinLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/translate%20online");
                }
            }

            if (extranetUserObj.DataObjectTypeId == 1)
            {
                if (joborder.ContactId == extranetUserObj.DataObjectId)
                {
                    permittedToViewThisOrder = true;
                }
                else if (loggedInUserOrg.Id == contact.OrgId && extranetUserAccessLevel.CanViewDetailsOfOtherOrgOrders == true)
                {
                    permittedToViewThisOrder = true;
                }
                else if (loggedInUserOrg.OrgGroupId != null && orderOrg.OrgGroupId != null)
                {
                    if (loggedInUserOrg.OrgGroupId == orderOrg.OrgGroupId && extranetUserAccessLevel.CanViewDetailsOfOtherGroupOrders == true)
                    {
                        permittedToViewThisOrder = true;
                    }
                }
            }

            if (permittedToViewThisOrder == true && jobItem.SupplierCompletedItemDateTime == null)
            {
                var extranetUserDetails = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
                ViewBag.DataObjectID = extranetUserDetails.DataObjectId;
                ViewBag.DataObjectTypeId = extranetUserDetails.DataObjectTypeId;
                ViewBag.SupplierAccepted = jobItem.SupplierAcceptedWorkDateTime == null ? false : true;

                //update the number of segments updated in the database, in case the job item is from old review plus
                await reviewService.UpdateJobItemReviewSegments(jobItemId);

                //ReviewSignOffModel model = await jobOrderService.GetSignOffData(jobItemId);
                //return View(model);
                return null;
            }
            else
            {
                return Redirect("/Page/Locked");
            }

        }

        [HttpPost]
        public async Task<IActionResult> GetAllContacts()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jobItemId = int.Parse(content.Result);

            var thisItem = await jobItemService.GetById(jobItemId);

            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var thisOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            List<Contact> allContacts = new List<Contact>();

            allContacts = await contactService.GetContactsFromOrgWhoIsAllowedToUseChargeableSoftware(thisOrg.Id, 0);



            return Ok(allContacts);

        }

        [HttpGet]
        [Route("[controller]/[action]/{jobItemId}")]
        public async Task<IActionResult> translateOnlineEdit(int jobItemId)
        {
            var extranetUserObj = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserObj.UserName;

            var jobItem = await jobItemService.GetById(jobItemId);
            var joborder = await jobOrderService.GetById(jobItem.JobOrderId);
            var contact = await contactService.GetById(joborder.ContactId);
            var orderOrg = await orgService.GetOrgDetails(contact.OrgId);

            var loggedInUserOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);
            var extranetUserAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var permittedToViewThisOrder = false;

            if (extranetUserObj.DataObjectTypeId == 1)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserObj.UserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserObj.UserName);
                var translateOnlineLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var translateOnlineLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (translateOnlineLicencingGroupLevel != null)
                {
                    var translateOnlineLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingGroupLevel.translateOnlineLicenceID.Value);
                    if (translateOnlineLicence != null)
                    {
                        AccessPermitted = translateOnlineLicence.IsEnabled;
                    }
                }

                if (translateOnlineLicencingOrgLevel != null)
                {
                    var translateOnlineLicence = await flowplusLicencingService.GetflowPlusLicenceObj(translateOnlineLicencingOrgLevel.translateOnlineLicenceID.Value);
                    if (translateOnlineLicence != null)
                    {
                        AccessPermitted = translateOnlineLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/translate%20online");
                }

                if (joborder.ContactId == extranetUserObj.DataObjectId)
                {
                    permittedToViewThisOrder = true;
                }
                else if (loggedInUserOrg.Id == contact.OrgId && extranetUserAccessLevel.CanViewDetailsOfOtherOrgOrders == true)
                {
                    permittedToViewThisOrder = true;
                }
                else if (loggedInUserOrg.OrgGroupId != null && orderOrg.OrgGroupId != null)
                {
                    if (loggedInUserOrg.OrgGroupId == orderOrg.OrgGroupId && extranetUserAccessLevel.CanViewDetailsOfOtherGroupOrders == true)
                    {
                        permittedToViewThisOrder = true;
                    }
                }

            }
            else
            {
                return Redirect("/Page/AccessDisabled/translate%20online");
            }



            if (permittedToViewThisOrder == true && jobItem.SupplierCompletedItemDateTime == null)
            {
                ViewBag.DataObjectID = extranetUserObj.DataObjectId;
                ViewBag.DataObjectTypeId = extranetUserObj.DataObjectTypeId;
                ViewBag.extranetUserTimeZone = extranetUserObj.DefaultTimeZone;

                ReviewEditModel model = new ReviewEditModel();
                model.JobItemId = jobItemId;

                model.SourceLang = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.SourceLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
                model.TargetLang = localLangRepo.All().Where(r => r.LanguageIanacodeBeingDescribed == jobItem.TargetLanguageIanacode && r.LanguageIanacode == "en").FirstOrDefault().Name;
                var anyReviewTranslation = await reviewService.GetAnyReviewTranslationSegment(jobItemId);
                model.FileName = anyReviewTranslation.FileName;
                ViewBag.TargetIANACode = jobItem.TargetLanguageIanacode;
                var RvTranslation = await reviewService.GetLastModifiedReviewSegment(jobItemId);
                ViewBag.OrgId = 0;
                ViewBag.LastModifiedRvTranslationId = -1;
                ViewBag.LastModifiedSegId = -1;


                //update the jobitem status to in progress, if it has not been accepted by reviewer before
                if (jobItem.SupplierAcceptedWorkDateTime == null && jobItem.SupplierCompletedItemDateTime == null)
                {
                    await jobItemService.UpdateClientReviewStatus(jobItemId, Enumerations.ReviewStatus.ReviewInProgress);
                }


                if (RvTranslation != null)
                {
                    ViewBag.LastModifiedRvTranslationId = RvTranslation.Id;
                    ViewBag.LastModifiedSegId = RvTranslation.Segment;
                }

                if (contact != null) { ViewBag.OrgId = contact.OrgId; }

                ViewBag.NewTextColorCoding = miscResourceService.GetMiscResourceByName("ColourCodingNewText", "en").Result.StringContent;
                ViewBag.NewTextColorCodingInfo = miscResourceService.GetMiscResourceByName("ColourCodingNewTextInfo1", "en").Result.StringContent;

                ViewBag.FuzzyColorCoding = miscResourceService.GetMiscResourceByName("ColourCodingFuzzyText", "en").Result.StringContent;
                ViewBag.FuzzyColorCodingInfo = miscResourceService.GetMiscResourceByName("ColourCodingFuzzyTextInfo1", "en").Result.StringContent;

                ViewBag.ExactMatchColorCoding = miscResourceService.GetMiscResourceByName("ColourCodingExactText", "en").Result.StringContent;
                ViewBag.ExactMatchColorCodingInfo = miscResourceService.GetMiscResourceByName("ColourCodingExactTextInfo1", "en").Result.StringContent;

                return View(model);
            }
            else
            {
                return Redirect("/Page/Locked");
            }

        }

        [HttpPost]
        public async Task<IActionResult> ChangeTranslator()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var returnedString = content.Result;

            var jobItemId = int.Parse(returnedString.Split("$")[0]);
            var reviewerId = int.Parse(returnedString.Split("$")[1]);

            var thisItem = await jobItemService.GetById(jobItemId);

            var previousReviewer = await contactService.GetById(thisItem.LinguisticSupplierOrClientReviewerId.Value);

            //update the reviewer of the jobitem
            await jobItemService.UpdateReviewerOfJobItem(jobItemId, reviewerId);

            var textToReturn = "pending";


            var thisOrder = await jobOrderService.GetById(thisItem.JobOrderId);
            var thisContact = await contactService.GetById(thisOrder.ContactId);
            var thisOrg = await orgService.GetOrgDetails(thisContact.OrgId);

            var newReviewer = await contactService.GetById(reviewerId);
            var newReviewerExtranetUser = await extranetUserService.GetExtranetUserByContactId(newReviewer.Id);

            var newReviewerDeadline = timeZonesService.ConvertGMTToRequestedTimeZone(thisItem.SupplierCompletionDeadline.Value, newReviewerExtranetUser.DefaultTimeZone);

            var extranetUserDetails = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserDetails.UserName;

            var loggedInContact = await extranetUserService.GetExtranetUserContact(extranetUserName);

            try
            {
                var currentBrand = await brandsService.GetBrandById(1);

                if (thisOrg.OrgGroupId != null)
                {
                    currentBrand = await brandsService.GetBrandForClient(thisOrg.OrgGroupId.Value);
                }




                string SpecificInstructionsText = "";
                if (thisItem.DescriptionForSupplierOnly != "")
                {
                    SpecificInstructionsText = "Please note the following specific instructions about this item: <br /><br />" +
                                               "<span style=\"Color: Green;\">" + thisItem.DescriptionForSupplierOnly.Replace(Environment.NewLine, "<br />") +
                                               "</span> <br /><br />";
                }

                string inProgressString = "";
                if (thisItem.SupplierAcceptedWorkDateTime != null)
                {
                    inProgressString = miscResourceService.GetMiscResourceByName("WarningReroutingTranslationInProgress", "en").Result.StringContent.Replace("<br /><br />", "");
                    textToReturn = "inProgress";
                }

                string subjectLine = String.Format(miscResourceService.GetMiscResourceByName("translateOnlineReRouteEmailSubj", "en").Result.StringContent,
                                                   jobItemId, loggedInContact.Name);
                string emailBody = String.Format("<p>Dear {0}, <br /><br />" +
                                            "Job item number <b>{1}</b> (forming part of order number {2}, \"{3}\") has been reassigned to you by {11}. <br /><br />" +
                                            "We have provided a deadline of <b>{4}({5}) on {6}</b>" +
                                            " for you to complete the translation; if you may not be able to complete the translation by that time, we would appreciate it if you could notify us as soon as possible," +
                                            " so that we can discuss timings with {7} at {8}, who requested this translation order. <br /><br />" +
                                            "{12}{9}Please log in via <a href=\"{10}\">{10}</a> and access the \"translate online\" page to accept this translation request, and to start translating. <br /><br />" +
                                            "Yours sincerely, <br /><br />" +
                                            "The <b>flow plus</b> team"
                                            , newReviewer.Name, jobItemId, thisOrder.Id,
                                            System.Web.HttpUtility.HtmlEncode(thisOrder.JobName),
                                            newReviewerDeadline.ToString("HH:mm"),
                                            newReviewerExtranetUser.DefaultTimeZone,
                                            newReviewerDeadline.ToString("dddd d MMMM yyyy"),
                                            thisContact.Name,
                                            thisOrg.OrgName,
                                            SpecificInstructionsText,
                                            "https://flowplus.translateplus.com/",
                                            loggedInContact.Name,
                                            inProgressString);
                //string emailBody = String.Format(miscResourceService.GetMiscResourceByName("ReviewNotificationEmailBody", "en").Result.StringContent.Replace("{tp brand}", currentBrand.CompanyNameToShow).Replace("i plus", currentBrand.ApplicationName).Replace("iplus.{tpbrand}.com", currentBrand.DomainName),
                //                                 newReviewer.Name, jobItemId.ToString(), thisOrder.Id.ToString(), thisOrder.JobName, String.Format("{0:HH:mm}", thisItem.SupplierCompletionDeadline),
                //                                 String.Format("{0:dddd d MMMM yyyy}", thisItem.SupplierCompletionDeadline), thisContact.Name, thisOrg.OrgName,
                //                                 miscResourceService.GetMiscResourceByName("WarningReroutingReviewInProgress", "en").Result.StringContent.Replace("<br /><br />", "") + "<br /><br />");

                var projectManager = await employeesService.IdentifyCurrentUserById(thisOrder.ProjectManagerEmployeeId);

                emailService.SendMail("flow plus <flowplus@translateplus.com>",
                                      contactService.EmailAddressesForNotification(newReviewer.Id).Result,
                                      subjectLine, emailBody, MsgIsHTML: true,
                                      SuppressSignatureForMarketingReasons: false,
                                      CCRecipients: loggedInContact.EmailAddress + ", " + projectManager.EmailAddress + ", " +
                                      previousReviewer.EmailAddress,
                                      IsExternalNotification: true);

            }
            catch
            {
                //do not error out if couldn't send e - mail
            }


            return Ok(textToReturn);

        }


        [HttpPost]
        public async Task<IActionResult> RevertTranslation()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jSonObj = JsonConvert.DeserializeObject<ReviewSegmentModel>(content.Result);

            var thisRvTranslation = await reviewService.GetReviewTranslation(jSonObj.JobItemId, jSonObj.Segment);

            await reviewService.RevertReviewDocumentTranslationUnit(jSonObj.JobItemId, thisRvTranslation.FileName, jSonObj.Segment,
                                                                    extranetUserName);

            var thisRvComment = await reviewService.GetReviewTranslationCommentDetails(jSonObj.JobItemId, jSonObj.Segment);

            var thisJobItem = await jobItemService.GetById(jSonObj.JobItemId);

            var thisRvTranslationModel = new ReviewSegmentModel
            {
                ReviewTranslationId = thisRvTranslation.Id,
                JobItemId = thisRvTranslation.JobItemId,
                TargetText = jSonObj.ShowCollapsedTag == true ? thisRvTranslation.TranslationBeforeReviewCollapsedTags : thisRvTranslation.TranslationBeforeReview,
                CommentText = thisRvComment == null ? "" : (thisJobItem.LanguageServiceId == 1 ? thisRvComment.TranslateOnlineComment : thisRvComment.Comment),
                LastModifiedBy = thisRvTranslation.LastModifiedUserName == null ? "" : extranetUserService.GetExtranetUserContact(thisRvTranslation.LastModifiedUserName).Result.Name,
                LastModifiedDateTime = thisRvTranslation.LastModifiedDateTime == null ? null : thisRvTranslation.LastModifiedDateTime
            };

            return Ok(thisRvTranslationModel);
        }


        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> GetSignOffItemDetails()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jobItemId = Int32.Parse(content.Result);
            var thisItem = await jobItemService.GetById(jobItemId);

            await reviewService.UpdateJobItemReviewSegments(jobItemId);

            ReviewSignOffModel results = await jobOrderService.GetSignOffData(jobItemId);

            results.SupplierAcceptedJobItem = thisItem.SupplierAcceptedWorkDateTime == null ? false : true;

            //TranslateOnlineSignOffModel results = new TranslateOnlineSignOffModel()
            //{
            //    NextAvailableHolidayDate = NextWorkingDay.ToString("dd/MM/yyyy"),
            //    HolidaysRemaining = thisHoliday.HolidaysRemaing,
            //    TotalAnnualHolidays = thisHoliday.TotalAnnualHolidays,
            //    Year = thisHoliday.Year,
            //    TotalHolidaysForCurrentRequest = holidayService.GetTotalNumberOfDaysForHolidayRequest(LoggedInEmployee.Id, startDate, endDate, startDateFullOrAMOrPM, endDateFullOrAMOrPM, lastDayAmPMRadioVisible),
            //    NextWorkingDayAfterHolidayString = holidayService.GetNextWorkingDayAfterHolidayString(LoggedInEmployee.Id, lastDayAmPMRadioVisible, startDateFullOrAMOrPM, endDateFullOrAMOrPM, startDate, endDate)
            //    //LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase()
            //};
            return Ok(results);
        }


        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> ApproveTranslationAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            int jobItemId = Convert.ToInt32(allFields[0]);
            string comments = allFields[1];
            await translateOnlineService.ApproveTranslation(extranetUserName, jobItemId, comments);

            return Ok();
        }

        public async Task<IActionResult> RevertAllTranslations()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jobItemId = Int32.Parse(content.Result);

            var translationSegment = await reviewService.GetAnyReviewTranslationSegment(jobItemId);
            translateOnlineService.RevertAllTranslations(extranetUserName, jobItemId, translationSegment.FileName);

            return Ok();
        }
    }
}
