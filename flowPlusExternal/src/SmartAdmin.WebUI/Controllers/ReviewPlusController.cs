using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Global_Settings;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.flowPlusExternal.ReviewPlus;
//using ViewModels.Organisation;
using System.Collections.Generic;
using Newtonsoft.Json;
using Services;

namespace flowPlusExternal.Controllers
{
    public class ReviewPlusController : Controller
    {
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPJobItemService jobItemService;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly IRepository<LocalLanguageInfo> localLangRepo;
        private readonly ITPClientReviewService reviewService;
        private readonly ITPContactsLogic contactService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPBrandsService brandsService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPflowplusLicencingLogic flowplusLicencingService;

        public ReviewPlusController(ITPJobOrderService jobOrderService, ITPExtranetUserService extranetUserService,
                                    ITPJobItemService jobItemService, IRepository<LocalLanguageInfo> localLangInfo,
                                    ITPClientReviewService tPClientReview, ITPContactsLogic _contactService,
                                    ITPOrgsLogic _orgService, ITPBrandsService _brandsService,
                                    IEmailUtilsService _emailUtils, ITPMiscResourceService _miscResourceService,
                                    ITPEmployeesService _employeesService,
                                    ITPflowplusLicencingLogic _flowplusLicencingService)
        {
            this.jobOrderService = jobOrderService;
            this.extranetUserService = extranetUserService;
            this.jobItemService = jobItemService;
            this.localLangRepo = localLangInfo;
            this.reviewService = tPClientReview;
            this.contactService = _contactService;
            this.orgService = _orgService;
            this.brandsService = _brandsService;
            this.emailService = _emailUtils;
            this.miscResourceService = _miscResourceService;
            this.employeesService = _employeesService;
            this.flowplusLicencingService = _flowplusLicencingService;
        }

        public async Task<IActionResult> ReviewStatus()
        {
            var extranetUserDetails = await extranetUserService.GetCurrentExtranetUser();
            ViewBag.DataObjectID = extranetUserDetails.DataObjectId;
            ViewBag.DataObjectTypeId = extranetUserDetails.DataObjectTypeId;

            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserDetails.UserName);

            var FreeTrailCutOffDate = new DateTime(2023, 04, 01);
            if (extranetUserDetails.DataObjectTypeId == 1 && GeneralUtils.GetCurrentUKTime() >= FreeTrailCutOffDate)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserDetails.UserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserDetails.UserName);
                var reviewPlusLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var reviewPlusLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (reviewPlusLicencingGroupLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingGroupLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (reviewPlusLicencingOrgLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingOrgLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/review%20plus");
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
            ViewBag.isAllowedToChangeReviewer = allowToChangeReviewer;

            ViewBag.extranetUserTimeZone = extranetUserDetails.DefaultTimeZone;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetPendingDocumentsToBeReviewed([FromBody] DataTablesReviewPlus dataParams)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var filteredResult = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);
            //var filteredResult = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, 2, "desc");
            var result = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.SentToReviewerAwaitingAcceptance, extranetUserName, (bool)dataParams.loadAssigned);
            var data = filteredResult.Skip(0).ToList();
            return Ok(new { data, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        }

        [HttpPost]
        public async Task<IActionResult> GetDocumentsUnderReview([FromBody] DataTablesReviewPlus dataParams)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var filteredResult = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.ReviewInProgress, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir);
            //var filteredResult = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.ReviewInProgress, extranetUserName, (bool)dataParams.loadAssigned, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value,2, "desc");

            var result = await reviewService.GetPendingAndInprogressReviewDocuments(Enumerations.ReviewStatus.ReviewInProgress, extranetUserName, (bool)dataParams.loadAssigned);
            var data = filteredResult.Skip(0).ToList();
            return Ok(new { data, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        }

        [HttpPost]
        public async Task<IActionResult> GetPendingOrInProgressReviewDocuments()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var data = await reviewService.GetPendingAndInprogressReviewJobItems(extranetUserName);
            return Ok(data);
        }

        [HttpGet]
        [Route("[controller]/[action]/{jobItemId}")]
        public async Task<IActionResult> ReviewPlusSignOff(int jobItemId)
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

            var FreeTrailCutOffDate = new DateTime(2023, 04, 01);
            if (extranetUserObj.DataObjectTypeId == 1 && GeneralUtils.GetCurrentUKTime() >= FreeTrailCutOffDate)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserObj.UserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserObj.UserName);
                var reviewPlusLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var reviewPlusLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (reviewPlusLicencingGroupLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingGroupLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (reviewPlusLicencingOrgLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingOrgLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/review%20plus");
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

                ReviewSignOffModel model = await jobOrderService.GetSignOffData(jobItemId);
                return View(model);
            }
            else
            {
                return Redirect("/Page/Locked");
            }

        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> ApproveReviewAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            int jobItemId = Convert.ToInt32(allFields[0]);
            string reviewComments = allFields[1];
            int result = await jobOrderService.ApproveReview(extranetUserName, jobItemId, reviewComments);
            Redirect("/ReviewPlus/ReviewStatus");
            return Ok(result);
        }

        [HttpGet]
        [Route("[controller]/[action]/{jobItemId}")]
        public async Task<IActionResult> ReviewPlusEdit(int jobItemId)
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

            var FreeTrailCutOffDate = new DateTime(2023, 04, 01);
            if (extranetUserObj.DataObjectTypeId == 1 && GeneralUtils.GetCurrentUKTime() >= FreeTrailCutOffDate)
            {
                var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserObj.UserName);
                var currentGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserObj.UserName);
                var reviewPlusLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentGroup.Id, 3);
                var reviewPlusLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(currentOrg.Id, 2);

                bool AccessPermitted = false;
                if (reviewPlusLicencingGroupLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingGroupLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (reviewPlusLicencingOrgLevel != null)
                {
                    var reviewPlusLicence = await flowplusLicencingService.GetflowPlusLicenceObj(reviewPlusLicencingOrgLevel.reviewPlusLicenceID.Value);
                    if (reviewPlusLicence != null)
                    {
                        AccessPermitted = reviewPlusLicence.IsEnabled;
                    }
                }

                if (AccessPermitted == false)
                {
                    return Redirect("/Page/AccessDisabled/review%20plus");
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

        //[HttpPost]
        //public async Task<IActionResult> GetReviewDataTable([FromBody] DataTablesReviewPlus dataParams)
        //{
        //    var result = await reviewService.GetReviewTranslationSegments((int)dataParams.jobItemId, dataParams.showTagVal);
        //    int pageNum = dataParams.parameters.draw;
        //    int length = dataParams.parameters.length;
        //    var data = result.Skip((pageNum - 1) * length).Take(length).ToList();
        //    return Ok(new { result, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        //}

        [HttpPost]
        public async Task<IActionResult> GetReviewDataTable([FromBody] DataTablesReviewPlus dataParams)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            int firstSegmentOnPage = dataParams.parameters.start + 1;
            int pageNum = 1;

            if (firstSegmentOnPage % dataParams.parameters.length == 0)
            {
                pageNum = firstSegmentOnPage / dataParams.parameters.length;
            }
            else
            {
                pageNum = (int)(Math.Truncate((decimal)firstSegmentOnPage / dataParams.parameters.length)) + 1;
            }

            var filteredResult = await reviewService.GetReviewTranslationSegments((int)dataParams.jobItemId, extranetUserName,
                                                                                    dataParams.showTagVal,
                                                                                    dataParams.parameters.search.value,
                                                                                    dataParams.parameters.columns[1].search.value,
                                                                                    dataParams.parameters.columns[2].search.value,
                                                                                    pageNum, dataParams.parameters.length);
            var result = await reviewService.GetReviewTranslationSegments((int)dataParams.jobItemId, extranetUserName,
                                                                                    dataParams.showTagVal,
                                                                                    dataParams.parameters.search.value,
                                                                                    dataParams.parameters.columns[1].search.value,
                                                                                    dataParams.parameters.columns[2].search.value);
            var data = filteredResult.Skip(0).ToList();
            return Ok(new { data, recordsTotal = result.Count(), recordsFiltered = result.Count() });
        }

        [HttpPost]
        public async Task<IActionResult> GetAllReviewerForTargetLang()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jobItemId = int.Parse(content.Result);

            var thisItem = await jobItemService.GetById(jobItemId);

            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var thisOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            var accessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            List<Contact> allReviewer = new List<Contact>();

            if (accessLevel.CanRerouteReviewJobsToOtherGroupExtranetUsers == true)
            {
                if (thisOrg.OrgGroupId != null)
                {
                    allReviewer = await reviewService.GetAllReviewersForTargetLangForGroup(thisOrg.OrgGroupId.Value, thisItem.TargetLanguageIanacode);
                }
                else
                {
                    allReviewer = await reviewService.GetAllReviewersForTargetLangForOrg(thisOrg.Id, thisItem.TargetLanguageIanacode);
                }

            }
            else
            {
                allReviewer = await reviewService.GetAllReviewersForTargetLangForOrg(thisOrg.Id, thisItem.TargetLanguageIanacode);
            }

            return Ok(allReviewer);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeReviewer()
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

            string CCReviewManagerAddress = "";
            if (thisOrg.OrgName.ToLower().Contains("brammer") == true)
            {
                CCReviewManagerAddress = ", Cheryl Botchway <Cheryl.Botchway@brammer.biz>";
            }

            var newReviewer = await contactService.GetById(reviewerId);

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

                string subjectLine = String.Format(miscResourceService.GetMiscResourceByName("ReviewReassignmentEmailSubject", "en").Result.StringContent,
                                                   jobItemId, loggedInContact.Name);
                string inProgressString = "";
                if (thisItem.SupplierAcceptedWorkDateTime != null)
                {
                    inProgressString = miscResourceService.GetMiscResourceByName("WarningReroutingReviewInProgress", "en").Result.StringContent.Replace("<br /><br />", "");
                    textToReturn = "inProgress";
                }
                string emailBody = String.Format(miscResourceService.GetMiscResourceByName("ReviewNotificationEmailBody", "en").Result.StringContent.Replace("{tp brand}", currentBrand.CompanyNameToShow).Replace("i plus", currentBrand.ApplicationName).Replace("iplus.{tpbrand}.com", currentBrand.DomainName),
                                                 newReviewer.Name, jobItemId.ToString(), thisOrder.Id.ToString(), thisOrder.JobName, String.Format("{0:HH:mm}", thisItem.SupplierCompletionDeadline),
                                                 String.Format("{0:dddd d MMMM yyyy}", thisItem.SupplierCompletionDeadline), thisContact.Name, thisOrg.OrgName,
                                                 inProgressString + "<br /><br />");

                var projectManager = await employeesService.IdentifyCurrentUserById(thisOrder.ProjectManagerEmployeeId);

                emailService.SendMail("flow plus <flowplus@translateplus.com>",
                                      contactService.EmailAddressesForNotification(newReviewer.Id).Result,
                                      subjectLine, emailBody, MsgIsHTML: true,
                                      SuppressSignatureForMarketingReasons: false,
                                      CCRecipients: loggedInContact.EmailAddress + ", " + projectManager.EmailAddress + ", " +
                                      previousReviewer.EmailAddress + ", " + CCReviewManagerAddress,
                                      IsExternalNotification: true);


            }
            catch
            {
                //do not error out if couldn't send e - mail
            }


            return Ok(textToReturn);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSegmentAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jSonObj = JsonConvert.DeserializeObject<ReviewSegmentModel>(content.Result);

            var thisRvTranslation = await reviewService.GetReviewTranslation(jSonObj.JobItemId, jSonObj.Segment);
            bool collapsedTag = jSonObj.ShowCollapsedTag;

            var isValid = await reviewService.ValidateTargetText(collapsedTag, jSonObj.TargetText, thisRvTranslation.Id);

            string invalidString = "";
            if (isValid == false)
            {
                invalidString = miscResourceService.GetMiscResourceByName("ReviewPlusTagsValidator", "en").Result.StringContent;
            }
            else
            {
                if (collapsedTag == true)
                {

                    await reviewService.UpdateReviewDocumentTranslationUnit(jSonObj.JobItemId, thisRvTranslation.FileName, jSonObj.Segment,
                                                                  null, jSonObj.TargetText, thisRvTranslation.SourceText, jSonObj.AutoSave, extranetUserName,
                                                                  thisRvTranslation.Id, jSonObj.CommentText);
                }
                else
                {
                    await reviewService.UpdateReviewDocumentTranslationUnit(jSonObj.JobItemId, thisRvTranslation.FileName, jSonObj.Segment,
                                                                 jSonObj.TargetText, null, thisRvTranslation.SourceText, jSonObj.AutoSave, extranetUserName,
                                                                 thisRvTranslation.Id, jSonObj.CommentText);
                }

                //update the number of segments updated in the database

                await reviewService.UpdateJobItemReviewSegments(jSonObj.JobItemId);
            }



            return Ok(invalidString);
        }

        [HttpPost]
        public async Task<IActionResult> GetSegmentDetails()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var jSonObj = JsonConvert.DeserializeObject<ReviewSegmentModel>(content.Result);

            var thisRvTranslation = await reviewService.GetReviewTranslation(jSonObj.JobItemId, jSonObj.Segment);

            var thisRvComment = await reviewService.GetReviewTranslationCommentDetails(jSonObj.JobItemId, jSonObj.Segment);

            var thisJobItem = await jobItemService.GetById(jSonObj.JobItemId);

            List<int> allSegmentsWithSameText = await reviewService.GetAllSegmentsWithSameText(jSonObj.JobItemId, jSonObj.Segment, thisRvTranslation.FileName);

            var thisRvTranslationModel = new ReviewSegmentModel
            {
                ReviewTranslationId = thisRvTranslation.Id,
                JobItemId = thisRvTranslation.JobItemId,
                TargetText = jSonObj.ShowCollapsedTag == true ? thisRvTranslation.TranslationDuringReviewCollapsedTags : thisRvTranslation.TranslationDuringReview,
                CommentText = thisRvComment == null ? "" : (thisJobItem.LanguageServiceId == 1 ? thisRvComment.TranslateOnlineComment : thisRvComment.Comment),
                LastModifiedBy = thisRvTranslation.LastModifiedUserName == null ? "" : extranetUserService.GetExtranetUserContact(thisRvTranslation.LastModifiedUserName).Result.Name,
                LastModifiedDateTime = thisRvTranslation.LastModifiedDateTime == null ? null : thisRvTranslation.LastModifiedDateTime,
                SegmentsWithSameSourceText = allSegmentsWithSameText
            };

            return Ok(thisRvTranslationModel);
        }


    }
}

