using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ViewModels.flowPlusExternal;
using Data;
using Services.Interfaces;
using Global_Settings;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ViewModels.FileSystem;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Xml;


namespace flowPlusExternal.Controllers
{
    public class ServicesController : Controller
    {
        private readonly ICommonCachedService cachedService;
        private readonly ITPTimeZonesService timezoneService;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPCurrenciesLogic currencyService;
        private readonly ITPBrandsService brandsService;
        private readonly IConfiguration configuration;
        private readonly ITPEmployeeOwnershipsLogic ownershipService;
        private readonly ITPJobOrderService orderService;
        private readonly GlobalVariables GlobalVars;
        private readonly IEmailUtilsService emailService;
        private readonly ITPContactsLogic contactService;
        private readonly ITPJobItemService jobItemService;
        private readonly ITPEnquiriesService enquiriesService;
        private readonly ITPQuotesLogic quoteService;
        private readonly ITPQuoteTemplatesService quoteTemplatesService;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly ITPflowplusLicencingLogic flowplusLicencingService;

        public ServicesController(ICommonCachedService service, ITPTimeZonesService service1,
                              ITPExtranetUserService service2, UserManager<ExtranetUsersTemp> userManager,
                              ITPCurrenciesLogic service3, ITPBrandsService service4, IConfiguration configuration1,
                              ITPEmployeeOwnershipsLogic ownershipService1, ITPJobOrderService tPJobOrderService,
                              IEmailUtilsService emailUtils, ITPContactsLogic tPContactsLogic,
                              ITPJobItemService tPJobItemService, ITPEnquiriesService enqService,
                              ITPQuotesLogic tPQuoteService, ITPQuoteTemplatesService tPQuoteTemplatesService,
                              ITPEmployeesService tPEmployeesService, ITPMiscResourceService tPMiscResourceService,
                              ITPflowplusLicencingLogic flowplusLicencingService)
        {
            this.cachedService = service;
            this.timezoneService = service1;
            this.extranetUserService = service2;
            this._userManager = userManager;
            this.currencyService = service3;
            this.brandsService = service4;
            this.configuration = configuration1;
            this.ownershipService = ownershipService1;
            this.orderService = tPJobOrderService;
            GlobalVars = new GlobalVariables();
            configuration1.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            this.emailService = emailUtils;
            this.contactService = tPContactsLogic;
            this.jobItemService = tPJobItemService;
            this.enquiriesService = enqService;
            this.quoteService = tPQuoteService;
            this.quoteTemplatesService = tPQuoteTemplatesService;
            this.employeesService = tPEmployeesService;
            this.miscResourceService = tPMiscResourceService;
            this.flowplusLicencingService = flowplusLicencingService;
        }

        [Route("[controller]/[action]/{isQuoteRequest:bool=false}")]
        public async Task<IActionResult> NewProjectAsync(bool isQuoteRequest)
        {

            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            ViewBag.jobVsQuote = "";
            if (isQuoteRequest == true)
            {
                ViewBag.jobVsQuote = "quote";
            }

            ViewBag.DataObjectID = await extranetUserService.GetPermittedDataObjectID(extranetUserName);
            ViewBag.DataObjectTypeId = await extranetUserService.GetPermittedDataObjectTypeID(extranetUserName);

            var extranetUserObject = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            //var thisOrg = extranetUserService.GetExtranetUserOrg(extranetUserName).Result;
            //ViewBag.ClientOrg = thisOrg.OrgName;
            //ViewBag.ClientOrgID = thisOrg.Id;
            //ViewBag.ClientGroupID = thisOrg.OrgGroupId;
            //ViewBag.ClientInvoiceCurrencyID = thisOrg.InvoiceCurrencyId;
            //var contactObject = 
            //ViewBag.ContactName = contactObject.Name;
            //ViewBag.ContactId = contactObject.Id;

            var currentUserAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var userOrgObj = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            ViewBag.CanRequestWrittenServicesWork = currentUserAccessLevel.CanRequestWrittenServicesWork;
            ViewBag.CanRequestInterpretingServicesWork = currentUserAccessLevel.CanRequestInterpretingServicesWork;

            var userGroupObj = await extranetUserService.GetExtranetUserOrgGroup(extranetUserName);
            var MTLicencingGroupLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(userGroupObj.Id, 3);
            var MTLicencingOrgLevel = await flowplusLicencingService.GetflowPlusLicencingDetailsForDataObject(userOrgObj.Id, 2);

            bool MTEnabled = false;
            if (MTLicencingGroupLevel != null)
            {
                var MTLicence = await flowplusLicencingService.GetflowPlusLicenceObj(MTLicencingGroupLevel.AIOrMTLicenceID.Value);
                if (MTLicence != null)
                {
                    MTEnabled = MTLicence.IsEnabled;
                }
            }

            if (MTLicencingOrgLevel != null)
            {
                var MTLicence = await flowplusLicencingService.GetflowPlusLicenceObj(MTLicencingOrgLevel.AIOrMTLicenceID.Value);
                if (MTLicence != null)
                {
                    MTEnabled = MTLicence.IsEnabled;
                }
            }
            ViewBag.MTEnabled = MTEnabled;

            if (currentUserAccessLevel.CanRequestWrittenServicesWork == false &&
                currentUserAccessLevel.CanRequestInterpretingServicesWork == false)
            {
                return Redirect("/Page/Locked");
            }

            ViewBag.showQuoteOption = true;
            if (userOrgObj.OrgGroupId == 1621 || userOrgObj.OrgGroupId == 70650)
            {
                ViewBag.showQuoteOption = false;
            }

            ProjectCreationModel Result = new ProjectCreationModel()
            {
                Languages = await cachedService.GetMostUsedLanguagesCached(Enumerations.ServiceCategory.TranslationAndOtherWrittenServices, true, "en"),
                AllTimeZones = timezoneService.GetAllTimeZonesForDisplay(),
                JobRequestedByContact = extranetUserObject,
                AllAvailableCurrencies = await currencyService.GetAllENCurrencies(),
                ParentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName),
                ContactObject = await extranetUserService.GetExtranetUserContact(extranetUserName),
                ParentOrgGroup = await extranetUserService.GetExtranetUserOrgGroup(extranetUserName)
            };

            return View(Result);
        }


        [HttpPost]
        public async Task<IActionResult> GetLanguagesAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var showCommonLang = bool.Parse(content.Result);

            var allSourceLangs = await cachedService.GetMostUsedLanguagesCached(Enumerations.ServiceCategory.TranslationAndOtherWrittenServices, showCommonLang, "en");

            return Ok(allSourceLangs);
        }

        [HttpPost]
        public async Task<IActionResult> GetMTLanguagesAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var showCommonLang = bool.Parse(content.Result.Split("$")[0]);

            var MtEngine = content.Result.Split("$")[1];

            var allLangs = await cachedService.GetMostUsedLanguagesCached(Enumerations.ServiceCategory.TranslationAndOtherWrittenServices, showCommonLang, "en", GetLanguagesForMTEngine: MtEngine);

            return Ok(allLangs);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllCountries()
        {

            var allCountries = await cachedService.GetAllCountriesCached();

            return Ok(allCountries);

        }

        [HttpPost]
        public async Task<IActionResult> GetAllTradosTemplates()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var thisOrg = extranetUserService.GetExtranetUserOrg(extranetUserName).Result;

            var allTemplates = await cachedService.GetAllTradosTemplatesForAnOrg(thisOrg.Id, true);

            return Ok(allTemplates);

        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> CreateTranslationRequest(ProjectCreationModel projectCreationModel)
        {
            string errorStringToReturn = "";

            var extranetUserObject = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserObject.UserName;

            var contactObject = await contactService.GetById(extranetUserObject.DataObjectId);

            var currentOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            bool isAnyFileInDTPFormat = false;
            //get brand of the logged in user
            var currentBrand = await brandsService.GetBrandById(1);

            if (currentOrg.OrgGroupId != null)
            {
                currentBrand = await brandsService.GetBrandForClient(currentOrg.OrgGroupId.Value);
            }
            byte languageServiceCategoryId = 0;
            if (projectCreationModel.LanguageServiceSelected == 1 || projectCreationModel.LanguageServiceSelected == 7 || projectCreationModel.LanguageServiceSelected == 46 || projectCreationModel.LanguageServiceSelected == 2)
            {
                languageServiceCategoryId = 1;
            }
            if (projectCreationModel.LanguageServiceSelected == 5)
            {
                languageServiceCategoryId = 2;
            }
            if (projectCreationModel.LanguageServiceSelected == 6)
            {
                languageServiceCategoryId = 3;
            }
            DateTime SubmittedDateTime = timezoneService.GetCurrentGMT();

            var overallDeadline = timezoneService.GetCurrentGMT().AddDays(7);
            var fixedDeadlineString = "<p>Client selected <b>they do not have a fixed deadline</b>, therefore the applied deadline is automatically calculated 1 week from the submission. Please confirm job order delivery deadline to the client upon analyzing this job order.</p>";
            if (projectCreationModel.FixedDeadlineRequestedByClient == true)
            {
                overallDeadline = new DateTime(projectCreationModel.JobDeadline.Year, projectCreationModel.JobDeadline.Month, projectCreationModel.JobDeadline.Day,
                                               projectCreationModel.JobDeadlineHours, projectCreationModel.JobDeadlineMinutes, 0);

                if (projectCreationModel.JobDeadlineTimeZone != "GMT Standard Time")
                {
                    overallDeadline = System.TimeZoneInfo.ConvertTime(overallDeadline, System.TimeZoneInfo.FindSystemTimeZoneById(projectCreationModel.JobDeadlineTimeZone),
                                                                  System.TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

                }

                fixedDeadlineString = "<p>Client selected <b>fixed deadline</b>, so please observe stated deadline</p>";
            }

            string JobName = "";

            if (projectCreationModel.ProjectName == "" || projectCreationModel.ProjectName == null)
            {
                JobName = "i plus request submitted " + SubmittedDateTime.ToString("d MMM yy") + " - " + SubmittedDateTime.ToString("HH:mm");
            }
            else
            {
                JobName = HttpUtility.HtmlEncode(projectCreationModel.ProjectName).Trim();
            }


            byte orderChannelId = 25;  //by defualt this is set to flow plus extranet

            if (projectCreationModel.ReviewRequired == true && projectCreationModel.ReviewPlusOrDesignPlus == "DesignPlus")
            {
                orderChannelId = 21;

            }



            var projectManagerId = GlobalVars.TBAEmployeeID;
            var opsOwner = ownershipService.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.OperationsLead);

            if (opsOwner != null)
            {
                projectManagerId = opsOwner.Result.EmployeeId;
            }

            bool IsAQuoteRequest = false;

            if (projectCreationModel.QuoteOrJobRequest == "Quote")
            {
                IsAQuoteRequest = true;
            }

            string networkFolderTemplate = "";
            //job request
            if (IsAQuoteRequest == false)
            {
                JobOrder submittedJobOrder = null;
                //if it is a translation job order
                if (projectCreationModel.LanguageServiceSelected == 1)
                {
                    networkFolderTemplate = "Translation - standard";

                    if (projectCreationModel.ReviewRequired == true)
                    {
                        networkFolderTemplate = "Translation - with client review";
                        if (projectCreationModel.DTPRequired == true)
                        {
                            networkFolderTemplate = "Translation - with DTP + client review";
                        }
                    }
                    else
                    {
                        if (projectCreationModel.DTPRequired == true)
                        {
                            networkFolderTemplate = "Translation - with DTP";
                        }
                    }

                    List<JobOrder> allJobOrders = new List<JobOrder>();
                    if (projectCreationModel.CreateDifferentItemsForLangs == true)
                    {
                        for (int i = 0; i < projectCreationModel.TargetLangIANACode.Count(); i++)
                        {
                            submittedJobOrder = await orderService.CreateNewOrder<JobOrder>(extranetUserObject.DataObjectId, projectManagerId, orderChannelId,
                                                                                    JobName, projectCreationModel.Notes, "", projectCreationModel.PONumber,
                                                                                    projectCreationModel.CustomerSpecificData1, projectCreationModel.CustomerSpecificData2,
                                                                                    null, null, overallDeadline, overallDeadline.Hour, overallDeadline.Minute,
                                                                                    projectCreationModel.InvoiceCurrencyID, 0,
                                                                                    false, false, projectCreationModel.IsExtraConfidential,
                                                                                    GlobalVars.iplusEmployeeID, IsAQuoteRequest, false,
                                                                                    networkFolderTemplate, "",
                                                                                    extranetNotifyClientReviewersOfDeliveries: projectCreationModel.NotifyReviewerOfDelivery,
                                                                                    priority: projectCreationModel.Priority,
                                                                                    printingProject: projectCreationModel.IsPrintingPackagingProject);
                            allJobOrders.Add(submittedJobOrder);

                            int GoodFilesUploadedCount = 0;
                            string BaseFolderPathForThisRequest = orderService.ExtranetAndWSDirectoryPathForApp(submittedJobOrder.Id);
                            if (BaseFolderPathForThisRequest == "")
                            {
                                BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisRequest, submittedJobOrder.Id.ToString() + " - " + submittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));
                            }

                            if (Directory.Exists(BaseFolderPathForThisRequest) == false)
                            {
                                Directory.CreateDirectory(BaseFolderPathForThisRequest);
                            }

                            string BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                            if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                            {
                                Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                            }

                            foreach (FileModel sourcefile in projectCreationModel.SourceFiles)
                            {
                                string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount + 1).ToString());
                                if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                                {
                                    Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                                }

                                //check if the file path on external job drive is also less than 259
                                string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourcefile.file.FileName;

                                //System.IO.File.Move(Path.Combine(System.IO.Path.GetTempPath(), sourcefile.file.Name), Path.Combine(BaseFolderPathForThisSourceFile, sourcefile.file.Name.Split(@"\")[1]));

                                using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                                {
                                    await sourcefile.file.CopyToAsync(fileStream);


                                }



                                GoodFilesUploadedCount += 1;

                                // to check if the file is in one of the DTP formats, if previous files were in DTP format, dont need to check the file type again
                                if (projectCreationModel.DTPRequired == true && isAnyFileInDTPFormat == false)
                                {
                                    string currentFileFormat = Path.GetExtension(sourcefile.file.FileName);

                                    if (currentFileFormat == ".indd" || currentFileFormat == ".ind" || currentFileFormat == ".inx" ||
                                        currentFileFormat == ".idml" || currentFileFormat == ".tag" || currentFileFormat == ".qxd" ||
                                        currentFileFormat == ".qxp" || currentFileFormat == ".sit" || currentFileFormat == ".sitx" ||
                                        currentFileFormat == ".hqx" || currentFileFormat == ".pub" || currentFileFormat == ".dwg" ||
                                        currentFileFormat == ".dxf" || currentFileFormat == ".rar" || currentFileFormat == ".zip")
                                    {
                                        isAnyFileInDTPFormat = true;
                                    }
                                }


                            }

                            if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                            {
                                errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                        "Warning: user " + contactObject.Name +
                                                        " just tried to submit a job but they uploaded an empty file",
                                                        "<p>You may want to wait and see if they upload a new job with the correct file or call them to check if they were having problems.",
                                                        IsExternalNotification: true);
                                ModelState.AddModelError("sourceFileError", errorStringToReturn);
                                return Ok(projectCreationModel);
                            }
                            else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                            {
                                errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                        "Warning: user " + contactObject.Name +
                                                        " just tried to submit a job but they uploaded only empty files",
                                                        "<p>You may want to wait and see if they upload a new job with the correct files or call them to check if they were having problems.",
                                                        IsExternalNotification: true);
                                ModelState.AddModelError("sourceFileError", errorStringToReturn);
                                return Ok(projectCreationModel);
                            }

                            string ReferenceUploadedMessage = "";
                            if (projectCreationModel.ReferenceFiles.Count > 0)
                            {
                                string ExternalRefFolderPath = BaseFolderPathForThisRequest + @"\Reference";
                                int RefFilesUploaded = orderService.CopyReferenceFiles(projectCreationModel.ReferenceFiles, ExternalRefFolderPath);

                                if (RefFilesUploaded < projectCreationModel.ReferenceFiles.Count)
                                {
                                    ReferenceUploadedMessage = "<p><font color=\"red\"><b>Warning:</b> This job was uploaded with empty reference file(s)." +
                                                           " Please check with the client to send the correct reference files.</font><br /></p>";
                                }
                                else
                                {
                                    ReferenceUploadedMessage = "<p><font color=\"green\">This job was uploaded with reference file(s).";
                                }
                            }

                            // before creating target-language translation job items, set up a source-source proofreading job item, if required
                            string ProofreadSourceBeforeTranslationInfo = "";
                            if (projectCreationModel.ProofreadingRequired == true)
                            {
                                JobItem SubmittedProofreadingJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 7, projectCreationModel.SourceLangIANACode,
                                                                                                 projectCreationModel.SourceLangIANACode, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", false, 0, 0, 0, 0, DateTime.MinValue, 0,
                                                                                                 (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);


                                ProofreadSourceBeforeTranslationInfo = "<br /><br /><font color=\"red\"><b>Important: </b></font>the client requested that we proofread the source text prior to starting translation, so a proofreading job item (ID <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" +
                                                                    SubmittedProofreadingJobItem.Id.ToString() + "\">" + SubmittedProofreadingJobItem.Id.ToString() + "</a>) has been automatically set up too.";
                            }

                            for (var j = 0; j < projectCreationModel.TargetLangIANACode.Count(); j++)
                            {

                                bool supplierIsClientReview = false;
                                if (projectCreationModel.TranslateOnlineSelected == true)
                                {
                                    supplierIsClientReview = true;
                                }
                                JobItem submittedJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 1, projectCreationModel.SourceLangIANACode,
                                                                                                 projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", supplierIsClientReview, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                                 (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);

                                if (projectCreationModel.TargetProofReadingRequired == true)
                                {
                                    JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 7, projectCreationModel.SourceLangIANACode,
                                                                                                 projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", true, null, 0, 0, null, DateTime.MinValue, 0,
                                                                                                 (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);
                                }

                                if (projectCreationModel.ReviewRequired == true)
                                {
                                    JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 21, projectCreationModel.SourceLangIANACode,
                                                                                                 projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", true, null, 0, 0, null, DateTime.MinValue, 0,
                                                                                                 (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);
                                }

                                if (projectCreationModel.DTPRequired == true)
                                {
                                    JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 4, projectCreationModel.SourceLangIANACode,
                                                                                                 projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                                 Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                                 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                                 overallDeadline, "", "", true, null, 0, 0, null, DateTime.MinValue, 0,
                                                                                                 (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 7);
                                }


                            }

                            ////var BatchFileDirPath = "\\\\FREDCPAPPDM0001\\translateplusSystemFiles\\ProcessAutomationTP-SS-QA-1\\";
                            //var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                            ////var BatchFileDirPath = "C:\\Users\\kavjayas\\Desktop";
                            //var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                            //var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                            //string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
                            //System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);

                            //System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
                            //System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

                            //System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                            //                                                           GlobalVars.iplusEmployeeID, "ITSupport@translateplus.com") + Environment.NewLine);
                            //System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"SetUpJobFoldersFromExternalServer\" TaskNumber=\"1\" JobOrderID=\"{0}\" AdditionalText=\"{1}\"/>",
                            //                                                               submittedJobOrder.Id, networkFolderTemplate) + Environment.NewLine);
                            //System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

                            //System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
                            //System.IO.File.Delete(tempBatchFilePath);

                            XmlDocument BatchDoc = new XmlDocument();
                            BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                             "<!-- translate plus process automation batch file -->" + Environment.NewLine +
                                             "<translateplusBatch />");

                            XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

                            XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
                            BatchDocAttr.Value = "1.0";
                            RootNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
                            BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                            RootNode.Attributes.Append(BatchDocAttr);

                            //write e-mail notification address(es) info
                            BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                            BatchDocAttr.Value = "ITSupport@translateplus.com";
                            RootNode.Attributes.Append(BatchDocAttr);


                            XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                            //write task type info
                            BatchDocAttr = BatchDoc.CreateAttribute("Type");
                            BatchDocAttr.Value = "SetUpJobFoldersFromExternalServer";
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            //write task number info 
                            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                            BatchDocAttr.Value = "1";
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                            BatchDocAttr.Value = submittedJobOrder.Id.ToString();
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                            BatchDocAttr.Value = networkFolderTemplate;
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            //now append the node to the doc
                            RootNode.AppendChild(IndividualTaskNode);

                            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                            BatchDoc.Save(BatchFilePath);

                            if (projectCreationModel.DTPRequired == true)
                            {
                                orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                        "New job submitted by " + contactObject.Name, false, false,
                                                                        CustomerSpecificMessage: ProofreadSourceBeforeTranslationInfo + ReferenceUploadedMessage + fixedDeadlineString,
                                                                        IsFileInDTPFormat: isAnyFileInDTPFormat, IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);
                            }
                            else
                            {
                                orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                        "New job submitted by " + contactObject.Name, false, false,
                                                                        CustomerSpecificMessage: ProofreadSourceBeforeTranslationInfo + ReferenceUploadedMessage + fixedDeadlineString,
                                                                        IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);
                            }

                            orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                        miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailSubj", "en").Result.StringContent, true, false,
                                                                       UILangIANACode: "en", IsPrintingProject: projectCreationModel.IsPrintingPackagingProject,
                                                                       extranetUser: extranetUserObject);

                        }

                        return Redirect("/JobManagement/ProjectStatus");
                    }
                    else
                    {
                        submittedJobOrder = await orderService.CreateNewOrder<JobOrder>(extranetUserObject.DataObjectId, projectManagerId, orderChannelId,
                                                                                    JobName, projectCreationModel.Notes, "", projectCreationModel.PONumber,
                                                                                    projectCreationModel.CustomerSpecificData1, projectCreationModel.CustomerSpecificData2,
                                                                                    null, null, overallDeadline, overallDeadline.Hour, overallDeadline.Minute,
                                                                                    projectCreationModel.InvoiceCurrencyID, 0,
                                                                                    false, false, projectCreationModel.IsExtraConfidential,
                                                                                    GlobalVars.iplusEmployeeID, IsAQuoteRequest, false,
                                                                                    networkFolderTemplate, "",
                                                                                    extranetNotifyClientReviewersOfDeliveries: projectCreationModel.NotifyReviewerOfDelivery,
                                                                                    priority: projectCreationModel.Priority,
                                                                                    printingProject: projectCreationModel.IsPrintingPackagingProject);

                        int GoodFilesUploadedCount = 0;
                        string BaseFolderPathForThisRequest = orderService.ExtranetAndWSDirectoryPathForApp(submittedJobOrder.Id);
                        if (BaseFolderPathForThisRequest == "")
                        {
                            BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisRequest, submittedJobOrder.Id.ToString() + " - " + submittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));
                        }

                        if (Directory.Exists(BaseFolderPathForThisRequest) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisRequest);
                        }

                        string BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                        if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                        }

                        foreach (FileModel sourcefile in projectCreationModel.SourceFiles)
                        {
                            string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount + 1).ToString());
                            if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                            {
                                Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                            }

                            //check if the file path on internal job drive is also less than 259
                            string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourcefile.file.FileName;

                            //System.IO.File.Move(Path.Combine(System.IO.Path.GetTempPath(), sourcefile.file.Name), Path.Combine(BaseFolderPathForThisSourceFile, sourcefile.file.Name.Split(@"\")[1]));

                            using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                            {
                                await sourcefile.file.CopyToAsync(fileStream);


                            }

                            GoodFilesUploadedCount += 1;

                            // to check if the file is in one of the DTP formats, if previous files were in DTP format, dont need to check the file type again
                            if (projectCreationModel.DTPRequired == true && isAnyFileInDTPFormat == false)
                            {
                                string currentFileFormat = Path.GetExtension(sourcefile.file.FileName);

                                if (currentFileFormat == ".indd" || currentFileFormat == ".ind" || currentFileFormat == ".inx" ||
                                    currentFileFormat == ".idml" || currentFileFormat == ".tag" || currentFileFormat == ".qxd" ||
                                    currentFileFormat == ".qxp" || currentFileFormat == ".sit" || currentFileFormat == ".sitx" ||
                                    currentFileFormat == ".hqx" || currentFileFormat == ".pub" || currentFileFormat == ".dwg" ||
                                    currentFileFormat == ".dxf" || currentFileFormat == ".rar" || currentFileFormat == ".zip")
                                {
                                    isAnyFileInDTPFormat = true;
                                }
                            }


                        }



                        //report error if no files could actually be saved
                        //(note that for now this doesn't delete the job or do anything other
                        //than notify us internally)


                        if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                        {
                            errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                            emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                    "Warning: user " + contactObject.Name +
                                                    " just tried to submit a job but they uploaded an empty file",
                                                    "<p>You may want to wait and see if they upload a new job with the correct file or call them to check if they were having problems.",
                                                    IsExternalNotification: true);
                            ModelState.AddModelError("sourceFileError", errorStringToReturn);
                            return Ok(projectCreationModel);
                        }
                        else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                        {
                            errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                            emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                    "Warning: user " + contactObject.Name +
                                                    " just tried to submit a job but they uploaded only empty files",
                                                    "<p>You may want to wait and see if they upload a new job with the correct files or call them to check if they were having problems.",
                                                    IsExternalNotification: true);
                            ModelState.AddModelError("sourceFileError", errorStringToReturn);
                            return Ok(projectCreationModel);
                        }


                        string ReferenceUploadedMessage = "";
                        if (projectCreationModel.ReferenceFiles.Count > 0)
                        {
                            string ExternalRefFolderPath = BaseFolderPathForThisRequest + @"\Reference";
                            int RefFilesUploaded = orderService.CopyReferenceFiles(projectCreationModel.ReferenceFiles, ExternalRefFolderPath);

                            if (RefFilesUploaded < projectCreationModel.ReferenceFiles.Count)
                            {
                                ReferenceUploadedMessage = "<p><font color=\"red\"><b>Warning:</b> This job was uploaded with empty reference file(s)." +
                                                       " Please check with the client to send the correct reference files.</font><br /></p>";
                            }
                            else
                            {
                                ReferenceUploadedMessage = "<p><font color=\"green\">This job was uploaded with reference file(s).";
                            }
                        }

                        // before creating target-language translation job items, set up a source-source proofreading job item, if required
                        string ProofreadSourceBeforeTranslationInfo = "";
                        if (projectCreationModel.ProofreadingRequired == true)
                        {
                            JobItem SubmittedProofreadingJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 7, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.SourceLangIANACode, 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", false, 0, 0, 0, 0, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);


                            ProofreadSourceBeforeTranslationInfo = "<br /><br /><font color=\"red\"><b>Important: </b></font>the client requested that we proofread the source text prior to starting translation, so a proofreading job item (ID <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" +
                                                                SubmittedProofreadingJobItem.Id.ToString() + "\">" + SubmittedProofreadingJobItem.Id.ToString() + "</a>) has been automatically set up too.";
                        }

                        for (var j = 0; j < projectCreationModel.TargetLangIANACode.Count(); j++)
                        {
                            bool supplierIsClientReview = false;
                            if (projectCreationModel.TranslateOnlineSelected == true)
                            {
                                supplierIsClientReview = true;
                            }


                            JobItem submittedJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 1, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", supplierIsClientReview, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);

                            if (projectCreationModel.TargetProofReadingRequired == true)
                            {
                                JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 7, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, 0, "", "", "", "", "", "", "", null, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", true, null, 0, 0, null, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                             FromExternalServer: true, LanguageServiceCategoryId: 1);
                            }

                            if (projectCreationModel.ReviewRequired == true)
                            {
                                JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 21, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", true, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 1);
                            }

                            if (projectCreationModel.DTPRequired == true)
                            {
                                JobItem clientReviewJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, 4, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", true, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: 7);
                            }
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
                        BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                        RootNode.Attributes.Append(BatchDocAttr);

                        //write e-mail notification address(es) info
                        BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                        BatchDocAttr.Value = "ITSupport@translateplus.com";
                        RootNode.Attributes.Append(BatchDocAttr);


                        XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                        //write task type info
                        BatchDocAttr = BatchDoc.CreateAttribute("Type");
                        BatchDocAttr.Value = "SetUpJobFoldersFromExternalServer";
                        IndividualTaskNode.Attributes.Append(BatchDocAttr);

                        //write task number info 
                        BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                        BatchDocAttr.Value = "1";
                        IndividualTaskNode.Attributes.Append(BatchDocAttr);

                        BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                        BatchDocAttr.Value = submittedJobOrder.Id.ToString();
                        IndividualTaskNode.Attributes.Append(BatchDocAttr);

                        BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                        BatchDocAttr.Value = networkFolderTemplate;
                        IndividualTaskNode.Attributes.Append(BatchDocAttr);

                        //now append the node to the doc
                        RootNode.AppendChild(IndividualTaskNode);

                        var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                        var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                        var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                        BatchDoc.Save(BatchFilePath);

                        if (projectCreationModel.DTPRequired == true)
                        {
                            orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                    "New job submitted by " + contactObject.Name, false, false,
                                                                    CustomerSpecificMessage: ProofreadSourceBeforeTranslationInfo + ReferenceUploadedMessage + fixedDeadlineString,
                                                                    IsFileInDTPFormat: isAnyFileInDTPFormat, IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);
                        }
                        else
                        {
                            orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                    "New job submitted by " + contactObject.Name, false, false,
                                                                    CustomerSpecificMessage: ProofreadSourceBeforeTranslationInfo + ReferenceUploadedMessage + fixedDeadlineString,
                                                                    IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);
                        }

                        orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                        miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailSubj", "en").Result.StringContent, true, false,
                                                                       UILangIANACode: "en", IsPrintingProject: projectCreationModel.IsPrintingPackagingProject,
                                                                       extranetUser: extranetUserObject);

                    }

                    return Redirect("/JobManagement/OrderDetails/" + submittedJobOrder.Id.ToString());

                    // attempt to upload the files
                    //string BaseFolderPathForThisContact = Path.Combine(Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Contacts"), extranetUserObject.DataObjectId.ToString());
                }
                else if (projectCreationModel.LanguageServiceSelected == 7 ||
                         projectCreationModel.LanguageServiceSelected == 5 ||
                         projectCreationModel.LanguageServiceSelected == 6)
                {
                    if (projectCreationModel.LanguageServiceSelected == 7 || projectCreationModel.LanguageServiceSelected == 6)
                    {
                        networkFolderTemplate = "Proofreading only";
                    }
                    else if (projectCreationModel.LanguageServiceSelected == 5)
                    {
                        networkFolderTemplate = "Transcription";
                    }


                    submittedJobOrder = await orderService.CreateNewOrder<JobOrder>(extranetUserObject.DataObjectId, projectManagerId, orderChannelId,
                                                                                    JobName, projectCreationModel.Notes, "", projectCreationModel.PONumber,
                                                                                    projectCreationModel.CustomerSpecificData1, projectCreationModel.CustomerSpecificData2,
                                                                                    null, null, overallDeadline, overallDeadline.Hour, overallDeadline.Minute,
                                                                                    projectCreationModel.InvoiceCurrencyID, 0,
                                                                                    false, false, projectCreationModel.IsExtraConfidential,
                                                                                    GlobalVars.iplusEmployeeID, IsAQuoteRequest, false,
                                                                                    networkFolderTemplate, "",
                                                                                    extranetNotifyClientReviewersOfDeliveries: projectCreationModel.NotifyReviewerOfDelivery,
                                                                                    priority: projectCreationModel.Priority,
                                                                                    printingProject: projectCreationModel.IsPrintingPackagingProject);

                    int GoodFilesUploadedCount = 0;
                    string BaseFolderPathForThisRequest = orderService.ExtranetAndWSDirectoryPathForApp(submittedJobOrder.Id);
                    if (BaseFolderPathForThisRequest == "")
                    {
                        BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisRequest, submittedJobOrder.Id.ToString() + " - " + submittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));
                    }

                    if (Directory.Exists(BaseFolderPathForThisRequest) == false)
                    {
                        Directory.CreateDirectory(BaseFolderPathForThisRequest);
                    }

                    string BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                    if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                    {
                        Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                    }

                    foreach (FileModel sourcefile in projectCreationModel.SourceFiles)
                    {
                        string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount + 1).ToString());
                        if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                        }

                        //check if the file path on internal job drive is also less than 259
                        string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourcefile.file.FileName;

                        //System.IO.File.Move(Path.Combine(System.IO.Path.GetTempPath(), sourcefile.file.Name), Path.Combine(BaseFolderPathForThisSourceFile, sourcefile.file.Name.Split(@"\")[1]));

                        using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                        {
                            await sourcefile.file.CopyToAsync(fileStream);


                        }

                        GoodFilesUploadedCount += 1;

                    }

                    //report error if no files could actually be saved
                    //(note that for now this doesn't delete the job or do anything other
                    //than notify us internally)
                    if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit a job but they uploaded an empty file",
                                                "<p>You may want to wait and see if they upload a new job with the correct file or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }
                    else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit a job but they uploaded only empty files",
                                                "<p>You may want to wait and see if they upload a new job with the correct files or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }


                    string ReferenceUploadedMessage = "";
                    if (projectCreationModel.ReferenceFiles.Count > 0)
                    {
                        string ExternalRefFolderPath = BaseFolderPathForThisRequest + @"\Reference";
                        int RefFilesUploaded = orderService.CopyReferenceFiles(projectCreationModel.ReferenceFiles, ExternalRefFolderPath);

                        if (RefFilesUploaded < projectCreationModel.ReferenceFiles.Count)
                        {
                            ReferenceUploadedMessage = "<p><font color=\"red\"><b>Warning:</b> This job was uploaded with empty reference file(s)." +
                                                   " Please check with the client to send the correct reference files.</font><br /></p>";
                        }
                        else
                        {
                            ReferenceUploadedMessage = "<p><font color=\"green\">This job was uploaded with reference file(s).";
                        }
                    }


                    for (var j = 0; j < projectCreationModel.TargetLangIANACode.Count(); j++)
                    {

                        JobItem submittedJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, (byte)projectCreationModel.LanguageServiceSelected, projectCreationModel.SourceLangIANACode,
                                                                                         projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                         Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                         0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                         overallDeadline, "", "", false, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                         (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                         FromExternalServer: true, LanguageServiceCategoryId: languageServiceCategoryId);

                    }

                    string ForReportingNoDeadlineSelected = "<p>Client selected <b>fixed deadline</b>, so please observe stated deadline<p>";
                    if (projectCreationModel.FixedDeadlineRequestedByClient == false)
                    {
                        ForReportingNoDeadlineSelected = "<br /><p><font color=\"red\">The user selected \"I don’t have a fixed deadline by when I need the translation\", so my plus has defaulted the deadline to 1 week from today. It is essential that you actively update the deadline after analysing the word count.</font></p><br /><br />";
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
                    BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                    RootNode.Attributes.Append(BatchDocAttr);

                    //write e-mail notification address(es) info
                    BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                    BatchDocAttr.Value = "ITSupport@translateplus.com";
                    RootNode.Attributes.Append(BatchDocAttr);


                    XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                    //write task type info
                    BatchDocAttr = BatchDoc.CreateAttribute("Type");
                    BatchDocAttr.Value = "SetUpJobFoldersFromExternalServer";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //write task number info 
                    BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                    BatchDocAttr.Value = "1";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                    BatchDocAttr.Value = submittedJobOrder.Id.ToString();
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                    BatchDocAttr.Value = networkFolderTemplate;
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //now append the node to the doc
                    RootNode.AppendChild(IndividualTaskNode);

                    var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                    var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                    var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                    BatchDoc.Save(BatchFilePath);


                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>", "New job submitted by " + contactObject.Name, false, false,
                        CustomerSpecificMessage: ReferenceUploadedMessage + ForReportingNoDeadlineSelected);

                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                    miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailSubj", "en").Result.StringContent, true, false,
                                                                   UILangIANACode: "en", IsPrintingProject: projectCreationModel.IsPrintingPackagingProject,
                                                                   extranetUser: extranetUserObject, CustomerSpecificMessage: ReferenceUploadedMessage);

                    return Redirect("/JobManagement/OrderDetails/" + submittedJobOrder.Id.ToString());


                }
                else if (projectCreationModel.LanguageServiceSelected == 46)
                {
                    overallDeadline = timezoneService.GetCurrentGMT().AddDays(2);
                    byte jobItemLanguageService = 46;
                    networkFolderTemplate = "Translation - standard";

                    if (projectCreationModel.IsPostEditingSelected == true)
                    {
                        overallDeadline = new DateTime(projectCreationModel.PEDeadline.Year, projectCreationModel.PEDeadline.Month, projectCreationModel.PEDeadline.Day,
                                                       projectCreationModel.PEDeadlineHours, projectCreationModel.PEDeadlineMinutes, 0);

                        if (projectCreationModel.JobDeadlineTimeZone != "GMT Standard Time")
                        {
                            overallDeadline = System.TimeZoneInfo.ConvertTime(overallDeadline, System.TimeZoneInfo.FindSystemTimeZoneById(projectCreationModel.JobDeadlineTimeZone),
                                                                          System.TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                        }

                        jobItemLanguageService = projectCreationModel.PostEditingOption;
                    }

                    submittedJobOrder = await orderService.CreateNewOrder<JobOrder>(extranetUserObject.DataObjectId, projectManagerId, orderChannelId,
                                                                                   JobName, projectCreationModel.Notes, "", projectCreationModel.PONumber,
                                                                                   projectCreationModel.CustomerSpecificData1, projectCreationModel.CustomerSpecificData2,
                                                                                   null, null, overallDeadline, overallDeadline.Hour, overallDeadline.Minute,
                                                                                   projectCreationModel.InvoiceCurrencyID, 0,
                                                                                   false, false, projectCreationModel.IsExtraConfidential,
                                                                                   GlobalVars.iplusEmployeeID, IsAQuoteRequest, false,
                                                                                   networkFolderTemplate, "",
                                                                                   extranetNotifyClientReviewersOfDeliveries: projectCreationModel.NotifyReviewerOfDelivery,
                                                                                   priority: projectCreationModel.Priority,
                                                                                   printingProject: projectCreationModel.IsPrintingPackagingProject);



                    await orderService.UpdateMachineTranslationTemplateDetails(submittedJobOrder.Id, projectCreationModel.PEPreTranslateFrom,
                                                                 projectCreationModel.PESaveTranslationTo, projectCreationModel.MTEngineSelected);

                    int GoodFilesUploadedCount = 0;
                    string BaseFolderPathForThisRequest = orderService.ExtranetAndWSDirectoryPathForApp(submittedJobOrder.Id);
                    if (BaseFolderPathForThisRequest == "")
                    {
                        BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisRequest, submittedJobOrder.Id.ToString() + " - " + submittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));
                    }

                    if (Directory.Exists(BaseFolderPathForThisRequest) == false)
                    {
                        Directory.CreateDirectory(BaseFolderPathForThisRequest);
                    }

                    string BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                    if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                    {
                        Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                    }

                    foreach (FileModel sourcefile in projectCreationModel.SourceFiles)
                    {
                        string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount + 1).ToString());
                        if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                        }

                        //check if the file path on internal job drive is also less than 259
                        string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourcefile.file.FileName;

                        //System.IO.File.Move(Path.Combine(System.IO.Path.GetTempPath(), sourcefile.file.Name), Path.Combine(BaseFolderPathForThisSourceFile, sourcefile.file.Name.Split(@"\")[1]));

                        using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                        {
                            await sourcefile.file.CopyToAsync(fileStream);


                        }

                        GoodFilesUploadedCount += 1;

                        // to check if the file is in one of the DTP formats, if previous files were in DTP format, dont need to check the file type again
                        if (projectCreationModel.DTPRequired == true && isAnyFileInDTPFormat == false)
                        {
                            string currentFileFormat = Path.GetExtension(sourcefile.file.FileName);

                            if (currentFileFormat == ".indd" || currentFileFormat == ".ind" || currentFileFormat == ".inx" ||
                                currentFileFormat == ".idml" || currentFileFormat == ".tag" || currentFileFormat == ".qxd" ||
                                currentFileFormat == ".qxp" || currentFileFormat == ".sit" || currentFileFormat == ".sitx" ||
                                currentFileFormat == ".hqx" || currentFileFormat == ".pub" || currentFileFormat == ".dwg" ||
                                currentFileFormat == ".dxf" || currentFileFormat == ".rar" || currentFileFormat == ".zip")
                            {
                                isAnyFileInDTPFormat = true;
                            }
                        }


                    }



                    //report error if no files could actually be saved
                    //(note that for now this doesn't delete the job or do anything other
                    //than notify us internally)


                    if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit a job but they uploaded an empty file",
                                                "<p>You may want to wait and see if they upload a new job with the correct file or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }
                    else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit a job but they uploaded only empty files",
                                                "<p>You may want to wait and see if they upload a new job with the correct files or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }


                    string ReferenceUploadedMessage = "";
                    if (projectCreationModel.ReferenceFiles.Count > 0)
                    {
                        string ExternalRefFolderPath = BaseFolderPathForThisRequest + @"\Reference";
                        int RefFilesUploaded = orderService.CopyReferenceFiles(projectCreationModel.ReferenceFiles, ExternalRefFolderPath);

                        if (RefFilesUploaded < projectCreationModel.ReferenceFiles.Count)
                        {
                            ReferenceUploadedMessage = "<p><font color=\"red\"><b>Warning:</b> This job was uploaded with empty reference file(s)." +
                                                   " Please check with the client to send the correct reference files.</font><br /></p>";
                        }
                        else
                        {
                            ReferenceUploadedMessage = "<p><font color=\"green\">This job was uploaded with reference file(s).";
                        }
                    }

                    for (var j = 0; j < projectCreationModel.TargetLangIANACode.Count(); j++)
                    {

                        JobItem submittedJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, jobItemLanguageService, projectCreationModel.SourceLangIANACode,
                                                                                         projectCreationModel.TargetLangIANACode.ElementAt(j), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                         Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                         0, 0, 0, 0, "", "", "", "", "", "", "", 0, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                         overallDeadline, "", "", false, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                         (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                             FromExternalServer: true, LanguageServiceCategoryId: languageServiceCategoryId);
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
                    BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                    RootNode.Attributes.Append(BatchDocAttr);

                    //write e-mail notification address(es) info
                    BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                    BatchDocAttr.Value = "ITSupport@translateplus.com";
                    RootNode.Attributes.Append(BatchDocAttr);


                    XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                    //write task type info
                    BatchDocAttr = BatchDoc.CreateAttribute("Type");
                    BatchDocAttr.Value = "SetUpJobFoldersFromExternalServer";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //write task number info 
                    BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                    BatchDocAttr.Value = "1";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                    BatchDocAttr.Value = submittedJobOrder.Id.ToString();
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                    BatchDocAttr.Value = networkFolderTemplate;
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //now append the node to the doc
                    RootNode.AppendChild(IndividualTaskNode);

                    var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                    var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                    var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                    BatchDoc.Save(BatchFilePath);

                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                            "New job submitted by " + contactObject.Name, false, false,
                                                            CustomerSpecificMessage: ReferenceUploadedMessage,
                                                            IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);


                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                    miscResourceService.GetMiscResourceByName("TransRequestReceivedEmailSubj", "en").Result.StringContent, true, false,
                                                                   UILangIANACode: "en", IsPrintingProject: projectCreationModel.IsPrintingPackagingProject,
                                                                   extranetUser: extranetUserObject);

                    return Redirect("/JobManagement/OrderDetails/" + submittedJobOrder.Id.ToString());
                }

                else if (projectCreationModel.LanguageServiceSelected == 2)
                {
                    var totalInterpretingMinutes = (projectCreationModel.InterpretingHours * 60) + projectCreationModel.InterpretingMinutes;
                    networkFolderTemplate = "Interpreting - standard";

                    submittedJobOrder = await orderService.CreateNewOrder<JobOrder>(extranetUserObject.DataObjectId, projectManagerId, orderChannelId,
                                                                                    JobName, projectCreationModel.Notes, "", projectCreationModel.PONumber,
                                                                                    projectCreationModel.CustomerSpecificData1, projectCreationModel.CustomerSpecificData2,
                                                                                    null, null, overallDeadline, overallDeadline.Hour, overallDeadline.Minute,
                                                                                    projectCreationModel.InvoiceCurrencyID, 0,
                                                                                    false, false, projectCreationModel.IsExtraConfidential,
                                                                                    GlobalVars.iplusEmployeeID, IsAQuoteRequest, false,
                                                                                    networkFolderTemplate, "",
                                                                                    extranetNotifyClientReviewersOfDeliveries: projectCreationModel.NotifyReviewerOfDelivery,
                                                                                    priority: projectCreationModel.Priority,
                                                                                    printingProject: projectCreationModel.IsPrintingPackagingProject);

                    JobItem submittedJobItem = await jobItemService.CreateItem(submittedJobOrder.Id, true, (byte)projectCreationModel.LanguageServiceSelected, projectCreationModel.SourceLangIANACode,
                                                                                             projectCreationModel.TargetLangIANACode.ElementAt(0), 0, 0, 0, 0, 0, 0, 0, 0,
                                                                                             Enumerations.TranslationMemoryRequiredValues.NotKnownForHistoricalReasons,
                                                                                             0, 0, 0, totalInterpretingMinutes, projectCreationModel.InterpretingOrg,
                                                                                             projectCreationModel.InterpretingAddressLine1, projectCreationModel.InterpretingAddressLine2,
                                                                                             projectCreationModel.InterpretingAddressLine3, projectCreationModel.InterpretingAddressLine4,
                                                                                             projectCreationModel.InterpretingCountyOrState, projectCreationModel.InterpretingPostOrZip,
                                                                                             (byte)projectCreationModel.InterpretingCountryId, 0, 0, DateTime.MinValue, DateTime.MinValue,
                                                                                             overallDeadline, "", "", false, null, 0, 0, 0, DateTime.MinValue, 0,
                                                                                             (byte)GlobalVars.iplusEmployeeID, 0, 0, 0, 0, 0, 0, 0, 0, false,
                                                                                                 FromExternalServer: true, LanguageServiceCategoryId: languageServiceCategoryId);

                    XmlDocument BatchDoc = new XmlDocument();
                    BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                     "<!-- translate plus process automation batch file -->" + Environment.NewLine +
                                     "<translateplusBatch />");

                    XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

                    XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
                    BatchDocAttr.Value = "1.0";
                    RootNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
                    BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                    RootNode.Attributes.Append(BatchDocAttr);

                    //write e-mail notification address(es) info
                    BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                    BatchDocAttr.Value = "ITSupport@translateplus.com";
                    RootNode.Attributes.Append(BatchDocAttr);


                    XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                    //write task type info
                    BatchDocAttr = BatchDoc.CreateAttribute("Type");
                    BatchDocAttr.Value = "SetUpJobFoldersFromExternalServer";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //write task number info 
                    BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                    BatchDocAttr.Value = "1";
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                    BatchDocAttr.Value = submittedJobOrder.Id.ToString();
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                    BatchDocAttr.Value = networkFolderTemplate;
                    IndividualTaskNode.Attributes.Append(BatchDocAttr);

                    //now append the node to the doc
                    RootNode.AppendChild(IndividualTaskNode);

                    var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                    var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                    var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                    BatchDoc.Save(BatchFilePath);


                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                            "New interpreting booking submitted by " + contactObject.Name, false, false,
                                                            IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);


                    orderService.AnnounceThisOrderCreation(submittedJobOrder.Id, "flow plus <flowplus@translateplus.com>",
                                                                    miscResourceService.GetMiscResourceByName("InterpretingRequestReceivedEmailSubj", "en").Result.StringContent, true, false,
                                                                   UILangIANACode: "en",
                                                                   extranetUser: extranetUserObject);

                    return Redirect("/JobManagement/OrderDetails/" + submittedJobOrder.Id.ToString());
                }

            }


            else
            {
                Enquiry submittedEnquiry = null;

                submittedEnquiry = await enquiriesService.CreateEnquiry(contactObject.Id,
                                               orderChannelId, projectCreationModel.Notes, "", JobName,

                                                GlobalVars.iplusEmployeeID, overallDeadline, projectCreationModel.IsPrintingPackagingProject);

                Quote submittedQuote = null;

                string QuoteTitle = "";
                string ContactName = contactObject.Name;
                switch (extranetUserObject.PreferredExtranetUilangIanacode)
                {
                    case "en":
                        QuoteTitle = "Quotation for translation of:";
                        break;

                    case "de":
                        QuoteTitle = "Angebot zur Übersetzung:";
                        break;

                    case "da":
                        QuoteTitle = "Tilbud på oversættelse af følgende fil(er):";
                        break;

                    case "sv":
                        QuoteTitle = "Offert för översättning:";
                        break;

                    default:
                        QuoteTitle = "Quotation for translation of:";
                        break;
                }


                string QuoteFileName = "";
                switch (currentBrand.Id)
                {
                    case 1: //Translate plus
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timezoneService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    case 2:
                        QuoteFileName = "Jackpot Translation - " + currentOrg.OrgName + " - " + timezoneService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    default:
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timezoneService.GetCurrentGMT().ToString("d MMM yy");
                        break;
                }


                string OpeningSection = "";
                string ClosingSection = "";

                QuoteTemplate defaultQuoteTemplate = await quoteTemplatesService.GetDefaultQuoteTemplate("en");

                int SalesEmpID = (await employeesService.CurrentSalesDepartmentManager()).Id;

                var salesOwner = await ownershipService.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead);

                if (salesOwner == null)
                {
                    salesOwner = await ownershipService.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead);
                }

                submittedQuote = await quoteService.CreateQuote(submittedEnquiry.Id, true, projectCreationModel.InvoiceCurrencyID, "en",
                                                                QuoteTitle, QuoteFileName, "", timezoneService.GetCurrentGMT(), currentOrg.OrgName,
                                                                currentOrg.Address1, currentOrg.Address2, currentOrg.Address3, currentOrg.Address4,
                                                                currentOrg.CountyOrState, currentOrg.PostcodeOrZip, currentOrg.CountryId,
                                                                ContactName, defaultQuoteTemplate.OpeningSectionText, defaultQuoteTemplate.ClosingSectionText,
                                                                (byte)Enumerations.TimelineUnits.Days, 0, (byte)Enumerations.WordCountDisplay.ShowTotalsOnly, false,
                                                                false, false, false, false, (short)salesOwner.EmployeeId, projectCreationModel.CustomerSpecificData1,
                                                                projectCreationModel.CustomerSpecificData2, null, null, false,
                                                                false, false, false, projectCreationModel.PONumber, GlobalVars.iplusEmployeeID,
                                                                timezoneService.GetCurrentGMT(), PrintingProject: projectCreationModel.IsPrintingPackagingProject);

                string BaseFolderPathForThisContact = Path.Combine(Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Contacts"), contactObject.Id.ToString());
                string BaseFolderPathForThisRequest = enquiriesService.ExtranetAndWSDirectoryPathForApp(submittedEnquiry.Id);
                string BaseFolderPathForThisRequestSource = "";

                if (projectCreationModel.LanguageServiceSelected == 1 || projectCreationModel.LanguageServiceSelected == 0)
                {
                    if (projectCreationModel.ProofreadingRequired == true)
                    {
                        QuoteItem SubmittedProofreadingQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, 7, projectCreationModel.SourceLangIANACode,
                                                                                                      projectCreationModel.SourceLangIANACode, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "",
                                                                                                      "", "", "", "", "", "", 0, 0, 0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID,
                                                                                                      0, 0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: 1);
                    }

                    bool IsAnyFileInDTPFormat = false;

                    if (projectCreationModel.DTPRequired == true)
                    {
                        foreach (FileModel sourceFile in projectCreationModel.SourceFiles)
                        {
                            string fileExtension = Path.GetExtension(sourceFile.file.FileName);

                            if (fileExtension == ".indd" || fileExtension == ".ind" || fileExtension == ".inx" ||
                                fileExtension == ".idml" || fileExtension == ".tag" || fileExtension == ".qxd" ||
                                fileExtension == ".qxp" || fileExtension == ".sit" || fileExtension == ".sitx" ||
                                fileExtension == ".hqx" || fileExtension == ".pub" || fileExtension == ".dwg" ||
                                fileExtension == ".dxf" || fileExtension == ".rar" || fileExtension == ".zip")
                            {
                                IsAnyFileInDTPFormat = true;
                            }
                        }
                    }

                    for (int i = 0; i < projectCreationModel.TargetLangIANACode.Count(); i++)
                    {
                        QuoteItem submittedQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, 1, projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(i), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: 1);

                        if (projectCreationModel.TargetProofReadingRequired == true)
                        {
                            QuoteItem proofreadingQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, 7, projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(i), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: 1);
                        }

                        if (projectCreationModel.ReviewRequired == true)
                        {
                            QuoteItem reviewQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, 21, projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(i), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: 1);
                        }

                        if (projectCreationModel.DTPRequired == true)
                        {
                            QuoteItem dtpQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, 4, projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(i), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: 7);
                        }
                    }


                    //attempt to upload the files
                    int GoodFilesUploadedCount = 0;

                    if (BaseFolderPathForThisRequest == "")
                    {
                        BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisContact, submittedEnquiry.Id.ToString() + " - " +
                                                         submittedEnquiry.CreatedDateTime.ToString("d MMM yy"));

                        if (System.IO.Directory.Exists(BaseFolderPathForThisRequest) == false)
                        {
                            System.IO.Directory.CreateDirectory(BaseFolderPathForThisRequest);
                        }


                        BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                        if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                        }

                    }

                    foreach (FileModel sourceFile in projectCreationModel.SourceFiles)
                    {
                        string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount.ToString() + 1).ToString());

                        if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                        }

                        string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourceFile.file.FileName;

                        using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                        {
                            await sourceFile.file.CopyToAsync(fileStream);
                        }

                        GoodFilesUploadedCount += 1;
                    }

                    if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit an enquiry but they uploaded an empty file",
                                                "<p>You may want to wait and see if they upload a new enquiry with the correct file or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }
                    else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit an enquiry but they uploaded only empty files",
                                                "<p>You may want to wait and see if they upload a new enquiry with the correct files or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }

                }
                else if (projectCreationModel.LanguageServiceSelected == 7 ||
                         projectCreationModel.LanguageServiceSelected == 5 ||
                         projectCreationModel.LanguageServiceSelected == 6)
                {
                    for (int i = 0; i < projectCreationModel.TargetLangIANACode.Count(); i++)
                    {
                        QuoteItem submittedQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, (byte)projectCreationModel.LanguageServiceSelected,
                                                                                    projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(i), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: languageServiceCategoryId);
                    }


                    //attempt to upload the files
                    int GoodFilesUploadedCount = 0;

                    if (BaseFolderPathForThisRequest == "")
                    {
                        BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisContact, submittedEnquiry.Id.ToString() + " - " +
                                                         submittedEnquiry.CreatedDateTime.ToString("d MMM yy"));

                        if (System.IO.Directory.Exists(BaseFolderPathForThisRequest) == false)
                        {
                            System.IO.Directory.CreateDirectory(BaseFolderPathForThisRequest);
                        }


                        BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");
                        if (Directory.Exists(BaseFolderPathForThisRequestSource) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisRequestSource);
                        }

                    }

                    foreach (FileModel sourceFile in projectCreationModel.SourceFiles)
                    {
                        string BaseFolderPathForThisSourceFile = Path.Combine(BaseFolderPathForThisRequestSource, "File" + (GoodFilesUploadedCount.ToString() + 1).ToString());

                        if (Directory.Exists(BaseFolderPathForThisSourceFile) == false)
                        {
                            Directory.CreateDirectory(BaseFolderPathForThisSourceFile);
                        }

                        string FullPathOfFileInInternalDrive = BaseFolderPathForThisSourceFile + @"\" + sourceFile.file.FileName;

                        using (Stream fileStream = new FileStream(FullPathOfFileInInternalDrive, FileMode.Create))
                        {
                            await sourceFile.file.CopyToAsync(fileStream);
                        }

                        GoodFilesUploadedCount += 1;
                    }

                    if (projectCreationModel.SourceFiles.Count == 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "The source file you uploaded was an empty (zero-byte) file. Please select a valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit an enquiry but they uploaded an empty file",
                                                "<p>You may want to wait and see if they upload a new enquiry with the correct file or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }
                    else if (projectCreationModel.SourceFiles.Count > 1 && GoodFilesUploadedCount == 0)
                    {
                        errorStringToReturn = "All of the source files you uploaded were empty (zero-byte) files. Please select one or more valid source file with content for translation.";
                        emailService.SendMail("flow plus <flowplus@translateplus.com>", "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com",
                                                "Warning: user " + contactObject.Name +
                                                " just tried to submit an enquiry but they uploaded only empty files",
                                                "<p>You may want to wait and see if they upload a new enquiry with the correct files or call them to check if they were having problems.",
                                                IsExternalNotification: true);
                        ModelState.AddModelError("sourceFileError", errorStringToReturn);
                        return Ok(projectCreationModel);
                    }
                }
                else if (projectCreationModel.LanguageServiceSelected == 2)
                {
                    var totalInterpretingMinutes = (projectCreationModel.InterpretingHours * 60) + projectCreationModel.InterpretingMinutes;

                    QuoteItem submittedQuoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, (byte)projectCreationModel.LanguageServiceSelected,
                                                                                    projectCreationModel.SourceLangIANACode,
                                                                                    projectCreationModel.TargetLangIANACode.ElementAt(0), 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, 0, 0, 0, totalInterpretingMinutes,
                                                                                    projectCreationModel.InterpretingOrg, projectCreationModel.InterpretingAddressLine1,
                                                                                    projectCreationModel.InterpretingAddressLine2, projectCreationModel.InterpretingAddressLine3,
                                                                                    projectCreationModel.InterpretingAddressLine4, projectCreationModel.InterpretingCountyOrState,
                                                                                    projectCreationModel.InterpretingPostOrZip, (byte)projectCreationModel.InterpretingCountryId, 0,
                                                                                    0, projectCreationModel.Notes, 0, GlobalVars.iplusEmployeeID, 0,
                                                                                    0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: languageServiceCategoryId);
                }


                string ReferenceUploadedMessage = "";
                if (projectCreationModel.ReferenceFiles.Count > 0)
                {
                    string ExternalRefFolderPath = BaseFolderPathForThisRequest + @"\Reference";
                    int RefFilesUploaded = orderService.CopyReferenceFiles(projectCreationModel.ReferenceFiles, ExternalRefFolderPath);

                    if (RefFilesUploaded < projectCreationModel.ReferenceFiles.Count)
                    {
                        ReferenceUploadedMessage = "<p><font color=\"red\"><b>Warning:</b> This enquiry was uploaded with empty reference file(s)." +
                                               " Please check with the client to send the correct reference files.</font><br /></p>";
                    }
                    else
                    {
                        ReferenceUploadedMessage = "<p><font color=\"green\">This enquiry was uploaded with reference file(s).";
                    }
                }

                string ForReportingNoDeadlineSelected = "<p>Client selected <b>fixed deadline</b>, so please observe stated deadline<p>";
                if (projectCreationModel.FixedDeadlineRequestedByClient == false)
                {
                    ForReportingNoDeadlineSelected = "<br /><p><font color=\"red\">The user selected \"I don’t have a fixed deadline by when I need the translation\", so my plus has defaulted the deadline to 1 week from today. It is essential that you actively update the deadline after analysing the word count.</font></p><br /><br />";
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
                BatchDocAttr.Value = GlobalVars.iplusEmployeeID.ToString();
                RootNode.Attributes.Append(BatchDocAttr);

                //write e-mail notification address(es) info
                BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                BatchDocAttr.Value = "ITSupport@translateplus.com";
                RootNode.Attributes.Append(BatchDocAttr);


                XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                //write task type info
                BatchDocAttr = BatchDoc.CreateAttribute("Type");
                BatchDocAttr.Value = "SetUpEnquiryFoldersFromExternalServer";
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                //write task number info 
                BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                BatchDocAttr.Value = "1";
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                BatchDocAttr = BatchDoc.CreateAttribute("EnquiryID");
                BatchDocAttr.Value = submittedEnquiry.Id.ToString();
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                BatchDocAttr.Value = networkFolderTemplate;
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                //now append the node to the doc
                RootNode.AppendChild(IndividualTaskNode);

                var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                BatchDoc.Save(BatchFilePath);

                if (projectCreationModel.LanguageServiceSelected == '2')
                {
                    enquiriesService.AnnounceThisEnquiryCreation(submittedQuote.Id, "flow plus <flowplus@translateplus.com>",
                                                        "New interpreting enquiry submitted by " + contactObject.Name,
                                                        false, true);
                }
                else
                {
                    enquiriesService.AnnounceThisEnquiryCreation(submittedQuote.Id, "flow plus <flowplus@translateplus.com>",
                                                        "New enquiry submitted by " + contactObject.Name,
                                                        false, true, CustomerSpecificMessage: ReferenceUploadedMessage + ForReportingNoDeadlineSelected,
                                                        IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);
                }




                enquiriesService.AnnounceThisEnquiryCreation(submittedQuote.Id, "flow plus <flowplus@translateplus.com>",
                                                            miscResourceService.GetMiscResourceByName("QuoteRequestReceivedEmailSubj", "en").Result.StringContent,
                                                            true, true, UILangIANACode: "en",
                                                            IsPrintingProject: projectCreationModel.IsPrintingPackagingProject);


                return Redirect("/JobManagement/Quote/" + submittedEnquiry.Id.ToString());
            }

            return Ok();


        }
    }

}

;
