using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Data;
using ViewModels.Organisation;
using ViewModels.JobOrder;
using System.IO;
using System.Globalization;
using Data.Repositories;
using ViewModels.Quote;
using Newtonsoft.Json;
using Services;
using System.IO;
using Global_Settings;
using Microsoft.Extensions.Configuration;
using ViewModels.EmployeeOwnerships;
using ViewModels.JobItem;
using System.Xml;

namespace flowPlusExternal.Controllers
{
    public class JobManagementController : Controller
    {
        private readonly ITPEnquiriesService enquiriesService;
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPContactsLogic contactsService;
        private readonly ITPOrgsLogic orgsService;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPQuotesLogic quotesService;
        private readonly IRepository<LocalCountryInfo> lcInfoRepo;
        private readonly IRepository<LocalLanguageInfo> llInfoRepo;
        private readonly IRepository<LanguageService> lsInfoRepo;
        private readonly IRepository<Currency> cInfoRepo;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPMiscResourceService miscResourceService;
        private readonly ITPOrgGroupsLogic orgGroupsService;
        private readonly IRepository<ClientDecisionReason> cdsInfoRepo;
        private readonly ITPBrandsService brandsService;
        private readonly ICommonCachedService cachedService;
        private readonly ITPJobItemService itemService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPEmployeeOwnershipsLogic ownershipsLogicService;
        private readonly GlobalVariables globalVariables;
        private readonly IConfiguration configuration;
        private readonly IQuoteAndOrderDiscountsAndSurcharges quoteAndOrderDiscountsAndSurchargesService;
        private readonly IQuoteAndOrderDiscountsAndSurchargesCategories quoteAndOrderDiscountsAndSurchargesCategoriesService;
        private readonly ITPLinguistService linguistService;
        public JobManagementController(ITPEnquiriesService service, ITPJobOrderService service1,
                              ITPExtranetUserService service2, UserManager<ExtranetUsersTemp> userManager,
                              ITPContactsLogic contactsLogic, ITPOrgsLogic orgsLogic, ITPQuotesLogic quotesService, IRepository<LocalCountryInfo> lcInfoRepo, IRepository<LocalLanguageInfo> llInfoRepo, IRepository<LanguageService> lsInfoRepo, IRepository<Currency> cInfoRepo, ITPEmployeesService employeesService, ITPMiscResourceService _miscResourceService, ITPOrgGroupsLogic orgGroupsService, IRepository<ClientDecisionReason> cdsInfoRepo, ITPBrandsService brandsService, ICommonCachedService cachedService, ITPJobItemService itemService, IEmailUtilsService emailUtilsService, ITPEmployeeOwnershipsLogic ownershipsLogicService, IConfiguration configuration, IQuoteAndOrderDiscountsAndSurcharges quoteAndOrderDiscountsAndSurchargesService, IQuoteAndOrderDiscountsAndSurchargesCategories quoteAndOrderDiscountsAndSurchargesCategoriesService, ITPLinguistService linguistService)
        {
            this.enquiriesService = service;
            this.jobOrderService = service1;
            this.extranetUserService = service2;
            this._userManager = userManager;
            this.contactsService = contactsLogic;
            this.orgsService = orgsLogic;
            this.quotesService = quotesService;
            this.lcInfoRepo = lcInfoRepo;
            this.llInfoRepo = llInfoRepo;
            this.lsInfoRepo = lsInfoRepo;
            this.cInfoRepo = cInfoRepo;
            this.employeesService = employeesService;
            this.miscResourceService = _miscResourceService;
            this.orgGroupsService = orgGroupsService;
            this.cdsInfoRepo = cdsInfoRepo;
            this.brandsService = brandsService;
            this.cachedService = cachedService;
            this.itemService = itemService;
            emailService = emailUtilsService;
            this.ownershipsLogicService = ownershipsLogicService;
            this.configuration = configuration;
            globalVariables = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.quoteAndOrderDiscountsAndSurchargesService = quoteAndOrderDiscountsAndSurchargesService;
            this.quoteAndOrderDiscountsAndSurchargesCategoriesService = quoteAndOrderDiscountsAndSurchargesCategoriesService;
            this.linguistService = linguistService;
        }

        public async Task<IActionResult> ProjectStatus()
        {

            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();

            ViewBag.DataObjectID = await extranetUserService.GetPermittedDataObjectID(extranetUserName);
            ViewBag.DataObjectTypeId = await extranetUserService.GetPermittedDataObjectTypeID(extranetUserName);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllJobOrdersDataTableComponentData([FromBody] DataTables dataParams)
        {
            try
            {
                var submStartDate = Convert.ToDateTime(dataParams.startDate).ToString("yyyy-MM-dd hh:mm:ss.fff");
                var submEndDate = Convert.ToDateTime(dataParams.endDate).ToString("yyyy-MM-dd hh:mm:ss.fff");
                var data = await jobOrderService.GetAllJobOrdersForOrderStatusPage(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.start, dataParams.parameters.length, dataParams.parameters.search.value, dataParams.parameters.order.FirstOrDefault().column, dataParams.parameters.order.FirstOrDefault().dir, submStartDate, submEndDate);
                int totalRecords;
                int filteredRecords;
                (totalRecords, filteredRecords) = await jobOrderService.GetAllJobOrdersCountForOrderStatusPage(dataParams.dataObjectId, dataParams.dataTypeId, dataParams.parameters.search.value, submStartDate, submEndDate);
                return Ok(new { data, recordsTotal = totalRecords, recordsFiltered = filteredRecords });
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("[controller]/[action]/{jobOrderId}")]
        public async Task<IActionResult> OrderDetailsAsync(int jobOrderId)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var extranetUserObj = await extranetUserService.GetExtranetUserByUsername(extranetUserName);
            var extranetUserAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var orderDetails = await jobOrderService.GetOrderDetails(jobOrderId);
            var orderContact = await contactsService.GetContactDetails(orderDetails.ContactId);
            var orderOrg = await orgsService.GetOrgDetails(orderContact.OrgId);

            var loggedInUserOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            var permittedToViewThisOrder = false;

            if (extranetUserObj.DataObjectTypeId == 1)
            {
                if (orderDetails.ContactId == extranetUserObj.DataObjectId)
                {
                    permittedToViewThisOrder = true;
                }
                else if (loggedInUserOrg.Id == orderOrg.Id && extranetUserAccessLevel.CanViewDetailsOfOtherOrgOrders == true)
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

            if (permittedToViewThisOrder == true)
            {
                var jobItems = await jobOrderService.GetJobItemDetails(jobOrderId);
                JobOrderDetailsViewModel jobOrderDetailsViewModel = new JobOrderDetailsViewModel();
                jobOrderDetailsViewModel.OrderDetails = orderDetails;
                jobOrderDetailsViewModel.JobItems = jobItems;
                return View(jobOrderDetailsViewModel);
            }
            else
            {
                return Redirect("/Page/Locked");
            }

        }

        [HttpGet]
        [Route("[controller]/[action]/{jobOrderId}/{jobItemId}")]
        public async Task<IActionResult> GetDataForReviewAsync(int jobOrderId, int jobItemId)
        {
            var orderDetails = await jobOrderService.GetOrderDetails(jobOrderId);
            var jobItems = await jobOrderService.GetJobItemDetails(jobOrderId);
            var reviewItem = jobItems.Where(x => x.JobItemId == jobItemId);
            return Ok(reviewItem);
        }

        [HttpGet]
        [Route("[controller]/[action]/{jobOrderId}")]
        public async Task<IActionResult> GetJobOrderDataAsync(int jobOrderId)
        {
            var orderDetails = await jobOrderService.GetOrderDetails(jobOrderId);
            return Ok(orderDetails);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> CancelOrderAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            int jobOrderId = Convert.ToInt32(allFields[0]);
            string cancelComments = allFields[1];
            int result = await jobOrderService.CancelJobOrder(jobOrderId, extranetUserName, cancelComments);
            return Ok(result);
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> SubmitReviewAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            int jobItemId = Convert.ToInt32(allFields[0]);
            int reviewerId = Convert.ToInt32(allFields[1]);
            DateTime deadlineDate = Convert.ToDateTime(allFields[2]);
            int result = await jobOrderService.SubmitReview(jobItemId, reviewerId, deadlineDate, extranetUserName);
            return Ok();
        }

        [HttpPost]
        [Route("[controller]/[action]")]
        public async Task<IActionResult> UpdateJobOrderPriorityAsync()
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            int jobOrderId = Convert.ToInt32(allFields[0]);
            string priority = allFields[1];
            var result = await jobOrderService.UpdatePriority(jobOrderId, priority, extranetUserName);
            return Ok(result);
        }

        public IActionResult DownloadJobOrderSourceAndRefFiles(string file)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var allFields = file.Split("$");
            string fileIndex = allFields[0];
            int jobOrderId = Convert.ToInt32(allFields[1]);
            string fileDownloadType = allFields[2];
            FileContentResult result = jobOrderService.DownloadOrderSourceFile(jobOrderId, extranetUserName, fileDownloadType, fileIndex);
            return result;
        }
        public async Task<IActionResult> DownloadJobOrderAsync(string jobOrderId)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            FileContentResult result = await jobOrderService.DownloadJobOrder(Convert.ToInt32(jobOrderId), extranetUserName);
            return result;
        }

        public async Task<IActionResult> DownloadJobItemAsync(string jobItemId)
        {
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            FileContentResult result = await jobOrderService.DownloadJobItem(Convert.ToInt32(jobItemId), extranetUserName);
            return result;
        }

        [HttpGet]

        [Route("[controller]/[action]/{enquiryId}")]
        public async Task<IActionResult> QuoteAsync(int enquiryId)
        {
            ViewBag.EnquiryId = enquiryId.ToString();

            return View();
        }

        [HttpGet]

        public async Task<IActionResult> QuotePrep(int QuoteID)
        {
            var quoteItem = await quotesService.GetAllQuoteItems(QuoteID);
            ViewBag.QuoteID = QuoteID.ToString();

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ApproveQuotes()
        {
            QuoteViewModel model = new QuoteViewModel();
            string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
            var extranetUserObj = await extranetUserService.GetExtranetUserContact(extranetUserName);
            var extranetUserAcess = await extranetUserService.GetAccessLevelInfo(extranetUserName);
            var orgDetails = await orgsService.GetOrgDetails(extranetUserObj.OrgId);
            ViewBag.PendingQuotesText = miscResourceService.GetMiscResourceByName("PendingQuotesText", "en").Result.StringContent.ToString().Replace("<a href=\"ContactUs.aspx\" Class=\"ourservicesblue\">contact us</a>", "contact us");
            ViewBag.ApproveQuotesHeader = miscResourceService.GetMiscResourceByName("ApproveQuotesHeader", "en").Result.StringContent;
            ViewBag.ContactId = 0;
            ViewBag.OrgId = 0;
            ViewBag.OrgGroupId = 0;
            ViewBag.ViewProjectName = 0;
            var enquiries = await enquiriesService.GetEnquiryByContactId(extranetUserObj.Id);
            if (extranetUserName != null)
            {
                if (extranetUserAcess.CanViewPricingAndCosts == true)
                {
                    if (extranetUserObj != null)
                    {
                        ViewBag.ContactId = extranetUserObj.Id;
                    }
                    if (extranetUserAcess.CanViewDetailsOfOtherOrgOrders == true)
                    {
                        if (extranetUserObj.OrgId > 0 && extranetUserObj.OrgId != 0)
                        {
                            ViewBag.OrgId = orgDetails.Id;
                        }
                    }
                    if (extranetUserAcess.CanViewDetailsOfOtherGroupOrders == true)
                    {
                        if (orgDetails.OrgGroupId > 0 && orgDetails.OrgGroupId != 0)
                        {
                            ViewBag.OrgGroupId = orgDetails.OrgGroupId;
                            ViewBag.ViewProjectName = 1;
                        }
                    }

                }
                else
                {
                    return Redirect("/Page/Locked");
                }
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetAllQuotesDataTableComponentData([FromBody] QuotesTables dataParams)
        {
            try
            {
                var orgDetails = await orgsService.GetOrgDetails(dataParams.orgId);
                var result = quotesService.GetAllQuotesForApprovalPage(dataParams.contactId, dataParams.orgId, dataParams.orgGroupId, dataParams.parameters.start, dataParams.parameters.length);
                List<ApproveQuotesViewModel> data = new List<ApproveQuotesViewModel>();
                foreach (var item in result)
                {
                    var quote = await quotesService.GetQuoteById(item);
                    var enquiry = await enquiriesService.GetEnquiryById(quote.EnquiryId);
                    var contact = await contactsService.GetContactDetails(enquiry.ContactId);
                    var org = await orgsService.GetOrgDetails(contact.OrgId);
                    var currencies = cInfoRepo.All().Where(o => o.Id == quote.QuoteCurrencyId).FirstOrDefault();
                    data.Add(new ApproveQuotesViewModel { Id = quote.Id, Date = quote.CreatedDateTime.ToString("dd MMMM yyy") + "  " + quote.CreatedDateTime.ToString("hh:mm tt"), RequestedBy = contact.Name, ProjectName = enquiry.JobName, Division = org.OrgName, Currency = currencies.Symbol, Value = quote.OverallChargeToClient.GetValueOrDefault(0).ToString("00.00") });

                }

                data = data.OrderByDescending(o => o.Id).ToList();
                int totalRecords;
                int filteredRecords;
                totalRecords = result.Count();
                filteredRecords = result.Count();
                return Ok(new { data, recordsTotal = totalRecords, recordsFiltered = filteredRecords });
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("[controller]/[action]/{QuoteID}")]
        public async Task<IActionResult> QuoteDetails(int QuoteID)
        {
            QuoteViewModel model = new QuoteViewModel();
            var result = await quotesService.GetQuoteById(QuoteID);
            model = JsonConvert.DeserializeObject<QuoteViewModel>(JsonConvert.SerializeObject(result));
            var quoteItem = await quotesService.GetAllQuoteItems(QuoteID);
            model.QuoteItems = quoteItem.OrderBy(o => o.Id).ToList();

            var extranetUserObj = await extranetUserService.GetCurrentExtranetUser();
            string extranetUserName = extranetUserObj.UserName;
            var extranetUserAccessLevel = await extranetUserService.GetAccessLevelInfo(extranetUserName);

            var enquiryDetails = await enquiriesService.GetEnquiryById(model.EnquiryId);
            var contactDetails = await contactsService.GetContactDetails(enquiryDetails.ContactId);
            var orgDetails = await orgsService.GetOrgDetails(contactDetails.OrgId);
            var currentBrand = await brandsService.GetBrandForClient(orgDetails.OrgGroupId.GetValueOrDefault());
            var loggedInUserOrg = await extranetUserService.GetExtranetUserOrg(extranetUserName);

            var permittedToViewThisQuote = false;

            if (extranetUserObj.DataObjectTypeId == 1)
            {
                if (contactDetails.Id == extranetUserObj.DataObjectId)
                {
                    permittedToViewThisQuote = true;
                }
                else if (loggedInUserOrg.Id == orgDetails.Id && extranetUserAccessLevel.CanViewDetailsOfOtherOrgOrders == true)
                {
                    permittedToViewThisQuote = true;
                }
                else if (loggedInUserOrg.OrgGroupId != null && orgDetails.OrgGroupId != null)
                {
                    if (loggedInUserOrg.OrgGroupId == orgDetails.OrgGroupId && extranetUserAccessLevel.CanViewDetailsOfOtherGroupOrders == true)
                    {
                        permittedToViewThisQuote = true;
                    }
                }
            }
            //permittedToViewThisQuote = true;
            if (permittedToViewThisQuote == true)
            {
                if (enquiryDetails.Status == 0)
                {
                    return RedirectToAction(nameof(this.QuotePrep), new { QuoteID = QuoteID });
                }
                if (enquiryDetails.Status == 3 || enquiryDetails.Status == 2)
                {
                    var DecisionMadeByContact = await contactsService.GetContactDetails((int)enquiryDetails.DecisionMadeByContactId);
                    ViewBag.DecisionMadeByContactName = DecisionMadeByContact.Name;
                    ViewBag.DecisionMadeDate = enquiryDetails.DecisionMadeDateTime.GetValueOrDefault().ToString("dddd") + " " + enquiryDetails.DecisionMadeDateTime.GetValueOrDefault().ToString("dd MMMM yyy");
                    ViewBag.DecisionMadeTime = enquiryDetails.DecisionMadeDateTime.GetValueOrDefault().ToString("HH:mm");
                    ViewBag.WentAheadAsJobOrderID = enquiryDetails.WentAheadAsJobOrderId;
                }

                model.Ianacode = "en";
                ViewBag.ApproveQuote = miscResourceService.GetMiscResourceByName("ApproveQuoteHeader", model.Ianacode).Result.StringContent;
                ViewBag.RejectQuote = miscResourceService.GetMiscResourceByName("RejectQuoteHeader", model.Ianacode).Result.StringContent;
                ViewBag.ViewSavePDF = miscResourceService.GetMiscResourceByName("ViewSavePDFHeader", model.Ianacode).Result.StringContent;
                ViewBag.ConfirmButton = miscResourceService.GetMiscResourceByName("ConfirmButton", model.Ianacode).Result.StringContent;
                model.Ianacode = result.LangIanacode;
                if (enquiryDetails.PrintingProject == true)
                {
                    if (model.Ianacode == "en")
                    {
                        ViewBag.ClosingSectionText = "<p style='line-height: 1.1em; font-family: Arial; font-size: 11pt;'> If your language project is for printing / packaging purposes, please let us know, so we can follow an appropriate workflow.We strongly recommend that you add an additional proofreading step to your printing / packaging project.<br/> Before proceeding with printing any material it has to be sent to translate plus in its final layout for final linguistic approval.<br/> translate plus cannot be held liable for consequential loss resulting from any printing/packaging related matters. <br /> </p> <br/>";
                    }
                    else if (model.Ianacode == "de")
                    {
                        ViewBag.ClosingSectionText = "<p> Verständigen Sie uns bitte, falls der Inhalt Ihres Projektes für Drucksachen oder Verpackungsmaterialien verwendet werden soll, damit wir die entsprechenden Arbeitsschritte einleiten können. <br/> Sollte dies der Fall sein, empfiehlt sich für diese Art von Projekten generell ein zusätzlicher Qualitäts-Check, bei dem z. B. der von translate plus übersetzte Text im finalen Layout abgenommen werden kann.<br/> translate plus kann nicht für Folgeschäden bezüglich Drucksachen oder Verpackungsmaterialien haftbar gemacht werden. <br /> </p> <br/>";
                    }
                    else if (model.Ianacode == "sv")
                    {
                        ViewBag.ClosingSectionText = "<p> Om ditt språkprojekt är avsett för trycknings/förpackningsändamål ber vi dig att meddela oss detta, så att vi kan följa ett lämpligt arbetsflöde. Vi rekommenderar starkt att du lägger till ett ytterligare korrekturläsningssteg till ditt trycknings/förpackningsprojekt.<br/> Innan något material trycks måste det skickas till translate plus med sin slutliga layout för ett sista lingvistiskt godkännande.<br/> translate plus kan inte hållas ansvarigt för indirekta förluster till följd av trycknings/förpackningsrelaterade projekt. <br /> </p> <br/>";
                    }
                    else if (model.Ianacode == "da")
                    {
                        ViewBag.ClosingSectionText = "<p> Hvis dit sprogprojekt er til tryk/emballage, bedes du informere os herom, så vi kan følge et passende workflow. Vi anbefaler meget, at du tilføjer et ekstra korrekturlæsningstrin til dit projekt til tryk/emballage.<br/> Inden du trykker materiale, skal det sendes til translate plus med det endelige layout til afsluttende sproglig godkendelse.<br/> translate plus kan ikke gøres ansvarlig for følgeskader som følge af forhold relaterede til tryk/emballage. <br /> </p> <br/>";
                    }
                }
                ViewBag.Service = miscResourceService.GetMiscResourceByName("ServiceHeader", model.Ianacode).Result.StringContent;
                ViewBag.SourceLanguage = miscResourceService.GetMiscResourceByName("SourceLangHeader", model.Ianacode).Result.StringContent;
                ViewBag.TargetLanguage = miscResourceService.GetMiscResourceByName("TargetLangSingleHeader", model.Ianacode).Result.StringContent;
                ViewBag.PONumberOptionalHeader = miscResourceService.GetMiscResourceByName("PONumberOptionalHeader", model.Ianacode).Result.StringContent;
                ViewBag.ApproveQuoteText = miscResourceService.GetMiscResourceByName("ApproveQuoteText", model.Ianacode).Result.StringContent.Replace("{tp brand}", currentBrand.Name);
                ViewBag.AdditionalDetailsHeader = miscResourceService.GetMiscResourceByName("AdditionalDetailsHeader", model.Ianacode).Result.StringContent;
                model.EnquiryStatus = enquiryDetails.Status;
                ViewBag.ContactName = contactDetails.Name;
                ViewBag.CountryName = lcInfoRepo.All().Where(o => o.CountryId == model.QuoteCountryId).Select(o => o.CountryName).FirstOrDefault();
                var SalesContactEmployee = await employeesService.IdentifyCurrentUserById(model.SalesContactEmployeeId);
                ViewBag.SalesContactEmployeeFullName = SalesContactEmployee.FirstName + " " + SalesContactEmployee.Surname;
                ViewBag.SalesContactEmployeeEmailAddress = SalesContactEmployee.EmailAddress;
                ViewBag.SalesContactEmployeeLandlineNumber = SalesContactEmployee.LandlineNumber;
                ViewBag.QuoteApprovedHeader = miscResourceService.GetMiscResourceByName("QuoteApprovedHeader", model.Ianacode).Result.StringContent;
                ViewBag.QuoteRejectedHeader = miscResourceService.GetMiscResourceByName("QuoteRejectedHeader", model.Ianacode).Result.StringContent;
                ViewBag.QuoteApprovedText = miscResourceService.GetMiscResourceByName("QuoteApprovedText", model.Ianacode).Result.StringContent;
                ViewBag.QuoteRejectedText = miscResourceService.GetMiscResourceByName("QuoteRejectedText", model.Ianacode).Result.StringContent;
                ViewBag.RejectQuoteText = miscResourceService.GetMiscResourceByName("RejectQuoteText", model.Ianacode).Result.StringContent.Replace("{0}", SalesContactEmployee.FirstName + " " + SalesContactEmployee.Surname).Replace("{1}", SalesContactEmployee.EmailAddress).Replace("{2}", SalesContactEmployee.LandlineNumber);
                model.LocalLanguages = llInfoRepo.All();
                model.LanguageService = lsInfoRepo.All();
                model.Currency = cInfoRepo.All();
                model.ClientDecisionReason = cdsInfoRepo.All();
                ViewBag.ViewPDF = quotesService.ExtranetAndWSDirectoryPathForApp(model.Id);
                if (model.Ianacode == "en")
                {
                    ViewBag.VatLabel = "Quote is valid for 30 days. All costs are exclusive of any applicable VAT or equivalent taxes.";
                    ViewBag.ServiceHeading = "Service";
                    ViewBag.SourceLangHeading = "Source Language";
                    ViewBag.TargetLangHeading = "Target language";
                    ViewBag.NewWordsHeading = "New Words";
                    ViewBag.ExactWordsHeading = "Exact Words";
                    ViewBag.InterpretingDurationHeading = "Duration";
                    ViewBag.PagesOrSlidesHeading = "Pages/slides";
                    ViewBag.PerfectMatchWordsHeading = "match plus™ words";
                    ViewBag.RepetitionWordsHeading = "Repetition words";
                    ViewBag.TotalWordsHeading = "Total words";
                    ViewBag.TPCostHeading = "Cost";
                    ViewBag.WorkTimeHeading = "Time";
                    ViewBag.WackerNeusonWordCountHeader = "Check pre-translation";
                    ViewBag.CharactersHeader = miscResourceService.GetMiscResourceByName("Characters", model.Ianacode).Result.StringContent;
                    ViewBag.DocumentsHeader = miscResourceService.GetMiscResourceByName("Documents", model.Ianacode).Result.StringContent;
                }
                else if (model.Ianacode == "de")
                {
                    ViewBag.VatLabel = "Alle Kosten verstehen sich exklusive Mehrwertsteuer.";
                    ViewBag.ServiceHeading = "Dienstleistung";
                    ViewBag.SourceLangHeading = "Ausgangssprache";
                    ViewBag.TargetLangHeading = "Zielsprache";
                    ViewBag.NewWordsHeading = "Neue Wörter";
                    ViewBag.ExactWordsHeading = "100%-Match";
                    ViewBag.InterpretingDurationHeading = "Dauer";
                    ViewBag.PagesOrSlidesHeading = "Seiten/Folien";
                    ViewBag.PerfectMatchWordsHeading = "match plus™ Wörter";
                    ViewBag.RepetitionWordsHeading = "Wiederholungen";
                    ViewBag.TotalWordsHeading = "Gesamtwortzahl";
                    ViewBag.TPCostHeading = "Betrag";
                    ViewBag.WorkTimeHeading = "Zeit";
                    ViewBag.WackerNeusonWordCountHeader = "Vorübersetzung prüfen";
                    ViewBag.CharactersHeader = miscResourceService.GetMiscResourceByName("Characters", model.Ianacode).Result.StringContent;
                    ViewBag.DocumentsHeader = miscResourceService.GetMiscResourceByName("Documents", model.Ianacode).Result.StringContent;

                }
                else if (model.Ianacode == "sv")
                {
                    ViewBag.VatLabel = "Priserna är exklusive VAT (moms)";
                    ViewBag.ServiceHeading = "Språktjänst";
                    ViewBag.SourceLangHeading = "Källspråk";
                    ViewBag.TargetLangHeading = "Målspråk";
                    ViewBag.NewWordsHeading = "Exakta ord";
                    ViewBag.ExactWordsHeading = "Exakta ord";
                    ViewBag.InterpretingDurationHeading = "Varaktighet";
                    ViewBag.PagesOrSlidesHeading = "Sidor/bilder";
                    ViewBag.PerfectMatchWordsHeading = "match plus™ ord";
                    ViewBag.RepetitionWordsHeading = "Upprepade ord";
                    ViewBag.TotalWordsHeading = "Totalt antal ord";
                    ViewBag.TPCostHeading = "Pris";
                    ViewBag.WorkTimeHeading = "Tid";
                    ViewBag.WackerNeusonWordCountHeader = "";
                    ViewBag.CharactersHeader = miscResourceService.GetMiscResourceByName("Characters", model.Ianacode).Result.StringContent;
                    ViewBag.DocumentsHeader = miscResourceService.GetMiscResourceByName("Documents", model.Ianacode).Result.StringContent;
                }
                else if (model.Ianacode == "da")
                {
                    ViewBag.VatLabel = "Priserne er ekskl. moms";
                    ViewBag.ServiceHeading = "Ydelse";
                    ViewBag.SourceLangHeading = "Kildesprog";
                    ViewBag.TargetLangHeading = "Målsprog";
                    ViewBag.NewWordsHeading = "Nye ord";
                    ViewBag.ExactWordsHeading = "100%-match";
                    ViewBag.InterpretingDurationHeading = "Antal timer";
                    ViewBag.PagesOrSlidesHeading = "Sider/dias";
                    ViewBag.PerfectMatchWordsHeading = "match plus™ ord";
                    ViewBag.RepetitionWordsHeading = "Gentagelser";
                    ViewBag.TotalWordsHeading = "Samlet antal ord";
                    ViewBag.TPCostHeading = "Beløb";
                    ViewBag.WorkTimeHeading = "Tid";
                    ViewBag.WackerNeusonWordCountHeader = "";
                    ViewBag.CharactersHeader = miscResourceService.GetMiscResourceByName("Characters", model.Ianacode).Result.StringContent;
                    ViewBag.DocumentsHeader = miscResourceService.GetMiscResourceByName("Documents", model.Ianacode).Result.StringContent;
                }

                if (orgDetails.FuzzyBand1BottomPercentage > 0 || orgDetails.FuzzyBand1TopPercentage > 0) { model.FuzzyBand1 = orgDetails.FuzzyBand1BottomPercentage + "-" + orgDetails.FuzzyBand1TopPercentage + "% Matches"; }
                if (orgDetails.FuzzyBand2BottomPercentage > 0 || orgDetails.FuzzyBand2TopPercentage > 0) { model.FuzzyBand2 = orgDetails.FuzzyBand2BottomPercentage + "-" + orgDetails.FuzzyBand2TopPercentage + "% Matches"; }
                if (orgDetails.FuzzyBand3BottomPercentage > 0 || orgDetails.FuzzyBand3TopPercentage > 0) { model.FuzzyBand3 = orgDetails.FuzzyBand3BottomPercentage + "-" + orgDetails.FuzzyBand3TopPercentage + "% Matches"; }
                if (orgDetails.FuzzyBand4BottomPercentage > 0 || orgDetails.FuzzyBand4TopPercentage > 0) { model.FuzzyBand4 = orgDetails.FuzzyBand4BottomPercentage + "-" + orgDetails.FuzzyBand4TopPercentage + "% Matches"; }
                return View(model);
            }
            else
            {
                return Redirect("/Page/Locked");
            }

        }

        [HttpPost]
        public async Task<string> ApproveOrRejectQuote()
        {
            try
            {
                string message = string.Empty;
                Employee LoggedInEmployee = await employeesService.GetEmployeeById(globalVariables.iplusEmployeeID);
                var DataPassedOver = HttpContext.Request.Body;
                var streamreader = new StreamReader(DataPassedOver);
                var content = streamreader.ReadToEndAsync();
                var stringToProcess = content.Result;

                Enquiry model = new Enquiry();
                var allFields = stringToProcess.Split("$");
                model.Id = Convert.ToInt32(allFields[1]);
                var enquiryDetails = await enquiriesService.GetEnquiryById(model.Id);
                var quoteDetails = await quotesService.GetQuoteById(Convert.ToInt32(allFields[0]));
                var quoteItemDetails = await quotesService.GetAllQuoteItems(Convert.ToInt32(allFields[0]));
                var contactDetails = await contactsService.GetContactDetails(enquiryDetails.ContactId);
                var orgDetails = await orgsService.GetOrgDetails(contactDetails.OrgId);
                var orgGroupDetails = await orgGroupsService.GetOrgGroupDetails(orgDetails.OrgGroupId.GetValueOrDefault());
                if (Convert.ToInt32(allFields[2]) == 0) { model.Status = 2; }
                if (Convert.ToInt32(allFields[2]) == 1) { model.Status = 3; }
                string extranetUserName = extranetUserService.GetCurrentExtranetUserName();
                var extranetUserObj = await extranetUserService.GetExtranetUserContact(extranetUserName);
                var extranetUser = await extranetUserService.GetCurrentExtranetUser();


                var extranetUserFullName = string.Empty;
                if (extranetUser.DataObjectTypeId == (byte)Enumerations.DataObjectTypes.Contact) { extranetUserFullName = contactDetails.Name; }
                else if (extranetUser.DataObjectTypeId == (byte)Enumerations.DataObjectTypes.LinguisticSupplier)
                {
                    var linguistDetails = await linguistService.GetById(extranetUser.DataObjectTypeId);
                    if (linguistDetails.SupplierTypeId == 1)
                    {
                        extranetUserFullName = linguistDetails.MainContactFirstName + " " + linguistDetails.MainContactSurname;
                    }
                    else if (linguistDetails.SupplierStatusId == 2 || linguistDetails.SupplierStatusId == 3 || linguistDetails.SupplierSourceId == 10)
                    {
                        extranetUserFullName = linguistDetails.AgencyOrTeamName;
                    }
                    else
                    {
                        extranetUserFullName = linguistDetails.MainContactFirstName + " " + linguistDetails.MainContactSurname;
                    }
                }

                var currencyPrefix = cInfoRepo.All().Where(o => o.Id == quoteDetails.QuoteCurrencyId).FirstOrDefault();

                model.DecisionMadeByContactId = extranetUserObj.Id;
                model.DecisionReasonId = Convert.ToByte(allFields[3]);
                model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
                string PONumber = "";
                if (allFields[4] != "")
                {
                    model.AdditionalDetails = allFields[4];
                }

                if (Convert.ToInt32(allFields[2]) == 1)
                {
                    if (allFields[5] != "")
                    {
                        PONumber = allFields[5];
                    }
                }

                var DecisionReasonString = cdsInfoRepo.All().Where(o => o.DecisionReasonId == model.DecisionReasonId).FirstOrDefault();
                var Languages = await cachedService.GetAllLanguagesCached();
                //var result = enquiriesService.ApproveOrRejectEnquiry(model);

                string CustomerSpecificField1Value = "";
                string CustomerSpecificField2Value = "";
                string CustomerSpecificField3Value = "";
                string InvoicingNotes = "";






                Int16 Status;
                Int16 DecisionReason;

                int Country;
                JobOrder SubmittedJobOrder = new JobOrder();
                Enquiry EnquiryToUpdate = new Enquiry();
                bool UpdatingJobOrder = false;

                bool SetUpDesignPlusJob = false;


                // attempt to call update code
                try
                {
                    {

                        var withBlock = enquiryDetails;
                        if (orgDetails.Id != 83923 & orgDetails.OrgGroupId != 16946 & orgDetails.Id != 106160)
                        {
                            CustomerSpecificField1Value = quoteDetails.CustomerSpecificField1Value.ToString();
                            CustomerSpecificField2Value = quoteDetails.CustomerSpecificField2Value.ToString();
                            CustomerSpecificField3Value = quoteDetails.CustomerSpecificField3Value.ToString();
                        }

                        if (((orgDetails.Id != 1621 & PONumber != quoteDetails.ClientPonumber) | (orgDetails.OrgGroupId == 1621 & PONumber != quoteDetails.CustomerSpecificField1Value)) & PONumber != "")
                        {
                            if (quoteDetails.QuoteCountryId > 0)
                                Country = quoteDetails.QuoteCountryId;
                            else
                                Country = 0;

                            if (orgDetails.OrgGroupId == 1621)
                            {
                                PONumber = quoteDetails.ClientPonumber;
                                CustomerSpecificField1Value = PONumber;
                            }
                            else
                                PONumber = PONumber;

                            var SalesContactEmployee = await employeesService.IdentifyCurrentUserById(quoteDetails.SalesContactEmployeeId);

                            // PONumber = TPCommon.TPCommonGeneralUtils.ConvertAmpersandsAndAngledBracketsForXML(Trim(PONumberTextBox.Text))
                            var result = await quotesService.UpdateQuote(quoteDetails.Id, quoteDetails.EnquiryId, true, quoteDetails.QuoteCurrencyId, quoteDetails.LangIanacode, quoteDetails.Title, quoteDetails.QuoteFileName, quoteDetails.InternalNotes, quoteDetails.QuoteDate, quoteDetails.QuoteOrgName, quoteDetails.QuoteAddress1, quoteDetails.QuoteAddress2, quoteDetails.QuoteAddress3, quoteDetails.QuoteAddress4, quoteDetails.QuoteCountyOrState, quoteDetails.QuotePostcodeOrZip, (short)Country, quoteDetails.AddresseeSalutationName, quoteDetails.OpeningSectionText, quoteDetails.ClosingSectionText, quoteDetails.TimelineUnit, (decimal)quoteDetails.TimelineValue, quoteDetails.WordCountPresentationOption, quoteDetails.ShowInterpretingDurationInBreakdown, quoteDetails.ShowWorkDurationInBreakdown, quoteDetails.ShowPagesOrSlidesInBreakdown, quoteDetails.ShowNumberOfCharactersInBreakdown, quoteDetails.ShowNumberOfDocumentsInBreakdown, SalesContactEmployee.Id, CustomerSpecificField1Value, quoteDetails.CustomerSpecificField2Value, quoteDetails.CustomerSpecificField3Value, quoteDetails.CustomerSpecificField4Value, quoteDetails.ShowCustomerSpecificField1Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField2Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField3Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField4Value.GetValueOrDefault(), PONumber, DateTime.Now, globalVariables.iplusEmployeeID);

                            if (result > 0)
                            {
                                await quotesService.MarkQuoteCurrentVersion(quoteDetails.Id, quoteDetails.EnquiryId);
                            }
                        }

                        JobItem SubmittedJobItem = new JobItem();
                        int ProjectManagerID;
                        bool Approved = false;
                        int JobOrder = 0;

                        if (PONumber == "")
                            PONumber = quoteDetails.ClientPonumber;

                        string TargetURLString = "https://flowplus.translateplus.com/jobmanagement/QuoteDetails/Quote.aspx/" + quoteDetails.Id.ToString();

                        if (model.Status == 2)
                        {
                            Status = 2;
                        }
                        else
                        {
                            Status = 3;
                        }
                        DecisionReason = (short)model.DecisionReasonId.GetValueOrDefault();

                        Employee SetUpBy = new Employee();
                        SetUpBy = await employeesService.GetEmployeeById(enquiryDetails.CreatedByEmployeeId);
                        var empSalesOwnerships = new Enumerations.EmployeeOwnerships[]
                        {
                      Enumerations.EmployeeOwnerships.SalesNewBusinessLead,
                 Enumerations.EmployeeOwnerships.SalesAccountManagerLead
                         };
                        var empOpsOwnerships = new Enumerations.EmployeeOwnerships[]
                        {
                Enumerations.EmployeeOwnerships.OperationsLead
                          };
                        var clientIntroOwnerships = new Enumerations.EmployeeOwnerships[]
        {
                Enumerations.EmployeeOwnerships.ClientIntroLead
        };
                        DateTime timeZone = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));
                        var CurrentOpsOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(contactDetails.Id, Enumerations.DataObjectTypes.Org, empOpsOwnerships, timeZone, true);
                        var CurrentSalesOwners = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(contactDetails.Id, Enumerations.DataObjectTypes.Org, empSalesOwnerships, timeZone, true);
                        var CurrentClientIntroOwner = await ownershipsLogicService.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(contactDetails.Id, Enumerations.DataObjectTypes.Org, clientIntroOwnerships, timeZone, true);

                        if (CurrentOpsOwner.Any())
                            ProjectManagerID = CurrentOpsOwner.FirstOrDefault().Id;
                        else
                            ProjectManagerID = globalVariables.TBAEmployeeID;// use "TBA System" as project manager if no ops lead specified

                        string ReadableStatusString = "";
                        if (Status == 2)
                            ReadableStatusString = "rejected";
                        else if (Status == 3)
                        {
                            ReadableStatusString = "approved";
                            Approved = true;
                        }

                        string EmailRecipients = "ariand@translateplus.com";
                        string NoOpsOwnerMsg = "";
                        string TeamOpsEmail = "";


                        EmailRecipients = "";
                        if (CurrentSalesOwners.Any())
                        {
                            foreach (EmployeeOwnershipRelationshipViewModel Owner in CurrentSalesOwners)
                            {
                                Employee SalesOwner = new Employee();

                                SalesOwner = await employeesService.GetEmployeeById(SalesOwner.Id);
                                if (EmailRecipients == "")
                                    EmailRecipients = SalesOwner.EmailAddress;
                                else
                                    EmailRecipients += "," + SalesOwner.EmailAddress;
                            }
                        }
                        if (CurrentOpsOwner.Any())
                        {
                            Employee OpsOwner = new Employee();
                            OpsOwner = await employeesService.GetEmployeeById(CurrentOpsOwner.FirstOrDefault().Id);


                            if (EmailRecipients == "")
                            {
                                EmailRecipients = OpsOwner.EmailAddress;
                            }
                            else
                            {
                                EmailRecipients = EmailRecipients + "," + OpsOwner.EmailAddress;
                            }
                            if (OpsOwner.IsTeamManager == false)
                            {
                                if (OpsOwner.Manager != null)
                                {
                                    Employee OpsOwnerManager = new Employee();
                                    OpsOwnerManager = await employeesService.GetEmployeeById(CurrentOpsOwner.FirstOrDefault().Id);
                                    EmailRecipients = EmailRecipients + "," + OpsOwnerManager.EmailAddress;
                                }
                            }
                        }
                        else if (CurrentClientIntroOwner.Any())
                        {
                            Employee ClientIntroOwner = new Employee();
                            ClientIntroOwner = await employeesService.GetEmployeeById(CurrentClientIntroOwner.FirstOrDefault().Id);
                            if (EmailRecipients == "")
                                EmailRecipients = ClientIntroOwner.EmailAddress;
                            else
                                EmailRecipients = EmailRecipients + "," + ClientIntroOwner;
                        }
                        else
                        {
                            TeamOpsEmail = "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com";
                            NoOpsOwnerMsg = "<br /><font face=\"arial\" color=\"red\">No operations lead ownership is currently assigned for this client, so operations team managers need to assign this project to the appropriate project manager.</font><br /><br />";
                        }
                        if (EmailRecipients == "")
                            EmailRecipients = "Enquiries@translateplus.com";




                        // only create job order and items if the enquiry has gone ahead
                        if (Status == 3)
                        {
                            // set options based on client review vs. no client review
                            string NetworkFolderTemplate = "Translation - standard";

                            foreach (QuoteItem Item in quoteItemDetails)
                            {
                                if (Item.LanguageServiceId == 21)
                                {
                                    NetworkFolderTemplate = "Translation - with client review";
                                    break;
                                }
                            }

                            foreach (QuoteItem TranscreationItem in quoteItemDetails)
                            {
                                if (TranscreationItem.LanguageServiceId == 36)
                                {
                                    NetworkFolderTemplate = "Transcreation - complex";
                                    break;
                                }
                            }

                            if (withBlock.WentAheadAsJobOrderId < 1 || withBlock.WentAheadAsJobOrderId == null)
                            {
                                Int16 OrderChannelID = enquiryDetails.OrderChannelId;
                                //if (InDesignFile != "")
                                //{
                                //    OrderChannelID = 21;
                                //    SetUpDesignPlusJob = true;
                                //}

                                string InternalNotes = withBlock.InternalNotes;

                                if (quoteDetails.InternalNotes != "")
                                {
                                    if (InternalNotes == "")
                                        InternalNotes = quoteDetails.InternalNotes;
                                    else
                                        InternalNotes = "Enquiry notes: " + InternalNotes + "<br /><br />" + "Quote notes: " + quoteDetails.InternalNotes;
                                }

                                if (model.AdditionalDetails != "")
                                {
                                    if (InternalNotes == "")
                                    {
                                        InternalNotes = model.AdditionalDetails;
                                    }
                                    else
                                    {
                                        InternalNotes += "<br /><br />" + "Additional details: " + model.AdditionalDetails;
                                    }
                                }

                                DateTime Deadline = enquiryDetails.DeadlineRequestedByClient.GetValueOrDefault();
                                DateTime CurrentDateTime = GeneralUtils.GetCurrentGMT();
                                if (contactDetails.OrgId == 79702 | contactDetails.OrgId == 116161)
                                {
                                    int DaysNeededForTranslation = 1;

                                    if (CurrentDateTime.AddDays(DaysNeededForTranslation) > withBlock.DeadlineRequestedByClient)
                                        Deadline = CurrentDateTime.AddDays(DaysNeededForTranslation);
                                }
                                else
                                    try
                                    {
                                        if (quoteDetails.TimelineUnit == (byte)Enumerations.TimelineUnits.Days)
                                            Deadline = GeneralUtils.AddWorkingDaysToDate(CurrentDateTime, (int)quoteDetails.TimelineValue);
                                        else
                                            Deadline = CurrentDateTime.AddDays((quoteDetails.TimelineValue * 7));

                                        if (withBlock.DeadlineRequestedByClient > Deadline)
                                            Deadline = enquiryDetails.DeadlineRequestedByClient.GetValueOrDefault();
                                    }
                                    catch
                                    {
                                        Deadline = enquiryDetails.DeadlineRequestedByClient.GetValueOrDefault();
                                    }

                                SubmittedJobOrder = await jobOrderService.CreateNewOrder<Order>(contactDetails.Id, (short)ProjectManagerID, (byte)OrderChannelID, withBlock.JobName, withBlock.ExternalNotes, InternalNotes, PONumber, CustomerSpecificField1Value, CustomerSpecificField2Value, CustomerSpecificField3Value, quoteDetails.CustomerSpecificField4Value, Deadline, Deadline.Hour, Deadline.Minute, quoteDetails.QuoteCurrencyId, 0, false, false, false, SetUpBy.Id, false, false, NetworkFolderTemplate, InvoicingNotes, "", "", false, 0, withBlock.PrintingProject.GetValueOrDefault(), 0, 0, 0, 0, false);

                                model.WentAheadAsJobOrderId = SubmittedJobOrder.Id;
                                await enquiriesService.ApproveOrRejectEnquiry(model);

                                string ContactDirPath = System.IO.Path.Combine(System.IO.Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), contactDetails.Id.ToString());

                                string EnquiryFolder = enquiriesService.ExtranetAndWSDirectoryPathForApp(withBlock.Id);
                                string NewJobOrderFolder = jobOrderService.ExtranetAndWSDirectoryPathForApp(SubmittedJobOrder.Id);

                                if (NewJobOrderFolder == "")
                                    NewJobOrderFolder = Path.Combine(ContactDirPath, SubmittedJobOrder.Id.ToString() + " - " + SubmittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));

                                if (Directory.Exists(NewJobOrderFolder) == false)
                                    Directory.CreateDirectory(NewJobOrderFolder);
                                if (Directory.Exists(EnquiryFolder) == true)
                                    GeneralUtils.CopyDirectory(EnquiryFolder, NewJobOrderFolder, true);

                                string BaseFolderPathForThisRequestCollection = Path.Combine(NewJobOrderFolder, "Collection");
                                if (Directory.Exists(BaseFolderPathForThisRequestCollection) == false)
                                    Directory.CreateDirectory(BaseFolderPathForThisRequestCollection);


                                foreach (QuoteItem Item in quoteItemDetails)
                                {
                                    {

                                        var sourcelang = Languages.Where(o => o.StringValue == Item.SourceLanguageIanacode).ToList().FirstOrDefault();
                                        var targetlang = Languages.Where(o => o.StringValue == Item.TargetLanguageIanacode).ToList().FirstOrDefault();
                                        var withBlock1 = Item;
                                        if (withBlock1.InterpretingLocationCountryId > 0)
                                            Country = (int)withBlock1.InterpretingLocationCountryId;
                                        else
                                            Country = 0;

                                        bool SupplierIsCientReviewer = false;

                                        if (withBlock1.LanguageServiceId == 21)
                                            SupplierIsCientReviewer = true;

                                        if (orgDetails.OrgGroupId == 73512)
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, (byte)SetUpBy.Id, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);
                                        else
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, (byte)SetUpBy.Id, withBlock1.SupplierWordCountNew.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand1.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand2.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand3.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand4.GetValueOrDefault(), withBlock1.SupplierWordCountExact.GetValueOrDefault(), withBlock1.SupplierWordCountRepetitions.GetValueOrDefault(), withBlock1.SupplierWordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), FromExternalServer: true, LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);

                                        if (SubmittedJobItem.LanguageServiceId != 21)
                                        {
                                            // create collection item paths for subsequent delivery
                                            string BaseFolderPathForThisRequestCollectionItem = Path.Combine(BaseFolderPathForThisRequestCollection, GeneralUtils.MakeStringSafeForFileSystemPath(SubmittedJobItem.Id.ToString() + " - " + sourcelang.Name + "-" + targetlang.Name));
                                            if (Directory.Exists(BaseFolderPathForThisRequestCollectionItem) == false)
                                                Directory.CreateDirectory(BaseFolderPathForThisRequestCollectionItem);

                                            // crate status folders for 2 and 3 (hard-coded for now)
                                            string BaseFolderPathForThisRequestCollectionItemStatus = Path.Combine(BaseFolderPathForThisRequestCollectionItem, "Status2");
                                            if (Directory.Exists(BaseFolderPathForThisRequestCollectionItemStatus) == false)
                                                Directory.CreateDirectory(BaseFolderPathForThisRequestCollectionItemStatus);
                                            BaseFolderPathForThisRequestCollectionItemStatus = Path.Combine(BaseFolderPathForThisRequestCollectionItem, "Status3");
                                            if (Directory.Exists(BaseFolderPathForThisRequestCollectionItemStatus) == false)
                                                Directory.CreateDirectory(BaseFolderPathForThisRequestCollectionItemStatus);
                                        }
                                    }
                                }
                            }

                            else if (await jobOrderService.OrderExists(enquiryDetails.WentAheadAsJobOrderId.GetValueOrDefault()))
                            {
                                UpdatingJobOrder = true;
                                SubmittedJobOrder = await jobOrderService.GetById(enquiryDetails.WentAheadAsJobOrderId.GetValueOrDefault());
                                var JobItems = await jobOrderService.GetAllJobItems(enquiryDetails.WentAheadAsJobOrderId.GetValueOrDefault());
                                CustomerSpecificField1Value = quoteDetails.CustomerSpecificField1Value.ToString();

                                int EarlyInvoicingEmpID = 0;
                                if (SubmittedJobOrder.EarlyInvoiceByEmpId > 0)
                                {
                                    EarlyInvoicingEmpID = (int)SubmittedJobOrder.EarlyInvoiceByEmpId;
                                }
                                await jobOrderService.UpdateOrder(withBlock.WentAheadAsJobOrderId.GetValueOrDefault(), SubmittedJobOrder.ProjectManagerEmployeeId, withBlock.OrderChannelId, withBlock.JobName, withBlock.ExternalNotes, withBlock.InternalNotes, PONumber, CustomerSpecificField1Value, quoteDetails.CustomerSpecificField2Value.ToString(), quoteDetails.CustomerSpecificField3Value.ToString(), quoteDetails.CustomerSpecificField4Value.ToString(), Convert.ToDateTime(withBlock.DeadlineRequestedByClient).Date, Convert.ToDateTime(withBlock.DeadlineRequestedByClient).Hour, Convert.ToDateTime(withBlock.DeadlineRequestedByClient).Minute, false, quoteDetails.QuoteCurrencyId, 0, 0, false, false, false, InvoicingNotes, SubmittedJobOrder.EarlyInvoiceDateTime.GetValueOrDefault(), EarlyInvoicingEmpID, SubmittedJobOrder.OverdueReasonId.GetValueOrDefault(), SubmittedJobOrder.OverdueComment);

                                model.WentAheadAsJobOrderId = enquiryDetails.WentAheadAsJobOrderId;
                                await enquiriesService.ApproveOrRejectEnquiry(model);
                                foreach (JobItem JobItem in JobItems)
                                {
                                    JobItemViewModel removeItem = new JobItemViewModel();
                                    removeItem.LoggedInEmployeeId = LoggedInEmployee.Id;
                                    removeItem.Id = JobItem.Id;
                                    removeItem.DeletedFreeTextReason = "Enquiry was unlocked and quote items have changed therefore all job items need to be deleted and " + "new quote items will be turned into job items";
                                    await itemService.RemoveItem(removeItem);
                                }
                                foreach (QuoteItem Item in quoteItemDetails)
                                {
                                    {
                                        var withBlock1 = Item;
                                        if (withBlock1.InterpretingLocationCountryId > 0)
                                            Country = (int)withBlock1.InterpretingLocationCountryId;
                                        else
                                            Country = 0;

                                        bool SupplierIsCientReviewer = false;

                                        if (withBlock1.LanguageServiceId == 21)
                                            SupplierIsCientReviewer = true;

                                        if (await itemService.ItemExists(withBlock1.Id))
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, (byte)SetUpBy.Id, withBlock1.SupplierWordCountNew.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand1.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand2.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand3.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand4.GetValueOrDefault(), withBlock1.SupplierWordCountExact.GetValueOrDefault(), withBlock1.SupplierWordCountRepetitions.GetValueOrDefault(), withBlock1.SupplierWordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), FromExternalServer: true, LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);
                                    }
                                }
                            }

                            // add discount and surcharge to job order if any
                            if (quoteDetails.SurchargeId > 0 & quoteDetails.SurchargeAmount > 0)
                            {
                                QuoteAndOrderDiscountsAndSurcharge CurrentSurcharge = new QuoteAndOrderDiscountsAndSurcharge();
                                CurrentSurcharge = await quoteAndOrderDiscountsAndSurchargesService.GetById(quoteDetails.SurchargeId.GetValueOrDefault());
                                QuoteAndOrderDiscountsAndSurchargesCategory CurrentCategory = new QuoteAndOrderDiscountsAndSurchargesCategory();
                                CurrentCategory = await quoteAndOrderDiscountsAndSurchargesCategoriesService.GetById(CurrentSurcharge.DiscountOrSurchargeCategory);

                                if (SubmittedJobOrder.SurchargeId > 0 & SubmittedJobOrder.SurchargeAmount > 0)
                                {
                                    await quoteAndOrderDiscountsAndSurchargesService.UpdateSurchargeOrDiscount(SubmittedJobOrder.SurchargeId.GetValueOrDefault(), (byte)CurrentCategory.Id, CurrentSurcharge.Description, CurrentSurcharge.PercentageOrValue, CurrentSurcharge.DiscountOrSurchargeAmount, LoggedInEmployee.Id);
                                }
                                else
                                {
                                    var OrderSurcharge = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(false, (byte)CurrentCategory.Id, CurrentSurcharge.Description, CurrentSurcharge.PercentageOrValue, CurrentSurcharge.DiscountOrSurchargeAmount, LoggedInEmployee.Id);
                                }
                            }
                            else if (SubmittedJobOrder.SurchargeId > 0 & SubmittedJobOrder.SurchargeAmount > 0)
                                await quoteAndOrderDiscountsAndSurchargesService.RemoveSurchargeOrDiscount(SubmittedJobOrder.SurchargeId.GetValueOrDefault(), LoggedInEmployee.Id);

                            if (quoteDetails.DiscountId > 0 & quoteDetails.DiscountAmount > 0)
                            {
                                QuoteAndOrderDiscountsAndSurcharge CurrentDiscount = new QuoteAndOrderDiscountsAndSurcharge();
                                CurrentDiscount = await quoteAndOrderDiscountsAndSurchargesService.GetById((int)quoteDetails.DiscountAmount.GetValueOrDefault());
                                QuoteAndOrderDiscountsAndSurchargesCategory CurrentCategory = new QuoteAndOrderDiscountsAndSurchargesCategory();
                                CurrentCategory = await quoteAndOrderDiscountsAndSurchargesCategoriesService.GetById(CurrentDiscount.DiscountOrSurchargeCategory);

                                if (SubmittedJobOrder.DiscountId > 0 & SubmittedJobOrder.DiscountAmount > 0)
                                {
                                    await quoteAndOrderDiscountsAndSurchargesService.UpdateSurchargeOrDiscount(SubmittedJobOrder.DiscountId.GetValueOrDefault(), (byte)CurrentCategory.Id, CurrentDiscount.Description, CurrentDiscount.PercentageOrValue, CurrentDiscount.DiscountOrSurchargeAmount, LoggedInEmployee.Id);
                                }
                                else
                                {
                                    var OrderDiscount = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(true, (byte)CurrentCategory.Id, CurrentDiscount.Description, CurrentDiscount.PercentageOrValue, CurrentDiscount.DiscountOrSurchargeAmount, LoggedInEmployee.Id);
                                }
                            }
                            else if (SubmittedJobOrder.DiscountId > 0 & SubmittedJobOrder.DiscountAmount > 0)
                                await quoteAndOrderDiscountsAndSurchargesService.RemoveSurchargeOrDiscount(SubmittedJobOrder.DiscountId.GetValueOrDefault(), LoggedInEmployee.Id);


                            string MessageBodyCore = "";
                            try
                            {
                                List<int> MatchingOrderIDs = jobOrderService.GetJobOrderIDsByExactNamePOOrNotes(SubmittedJobOrder.JobName, "", "", GeneralUtils.GetCurrentGMT().AddMonths(-12), GeneralUtils.GetCurrentGMT(), OrgID: orgDetails.Id);

                                if (MatchingOrderIDs != null && MatchingOrderIDs.Count > 0)
                                {
                                    string Table = "";
                                    string TableRows = "";
                                    List<string> TargetLangs = new List<string>();
                                    var VisibleToClients = await jobOrderService.GetAllJobItems(SubmittedJobOrder.Id);
                                    var JobItemsVisibleToClients = VisibleToClients.Where(o => o.IsVisibleToClient == true).ToList();
                                    foreach (JobItem JobItem in JobItemsVisibleToClients)
                                    {
                                        if (JobItem.LanguageServiceId == 1)
                                        {
                                            if (TargetLangs.Count == 0)
                                                TargetLangs.Add(JobItem.TargetLanguageIanacode);
                                            else if (TargetLangs.Contains(JobItem.TargetLanguageIanacode) == false)
                                                TargetLangs.Add(JobItem.TargetLanguageIanacode);
                                        }
                                    }


                                    foreach (int OrderID in MatchingOrderIDs)
                                    {
                                        JobOrder OrderToCheck = new JobOrder();
                                        OrderToCheck = await jobOrderService.GetById(OrderID);
                                        var OrderToCheckVisibleToClients = await jobOrderService.GetAllJobItems(OrderToCheck.Id);
                                        var OrderToCheckJobItemsVisibleToClients = VisibleToClients.Where(o => o.IsVisibleToClient == true).ToList();
                                        if (OrderToCheck.Id != SubmittedJobOrder.Id && OrderToCheckJobItemsVisibleToClients.Count > 0)
                                        {
                                            foreach (JobItem JobItemToCheck in OrderToCheckJobItemsVisibleToClients)
                                            {
                                                var SourceLang = Languages.Where(o => o.StringValue == JobItemToCheck.SourceLanguageIanacode).ToList().FirstOrDefault();
                                                var TargetLang = Languages.Where(o => o.StringValue == JobItemToCheck.TargetLanguageIanacode).ToList().FirstOrDefault();
                                                foreach (string LangCode in TargetLangs)
                                                {
                                                    if ((JobItemToCheck.TargetLanguageIanacode.ToLower()) == (LangCode).ToLower() && JobItemToCheck.LanguageServiceId == 1)
                                                    {
                                                        TableRows += "<tr><td><a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + OrderToCheck.Id.ToString() + "\">" + OrderToCheck.Id.ToString() + " - " + OrderToCheck.JobName + "</a></td><td><a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id=" + JobItemToCheck.Id.ToString() + "\">" + JobItemToCheck.Id.ToString() + "</a></td><td>" + TargetLang.Name + "</td><td>" + OrderToCheck.SubmittedDateTime.ToString("dddd d MMMM yyyy") + "</td></tr>";
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (TableRows != "")
                                    {
                                        Table = "<table border=\"1\"><tr><td><b>Job order</b></td><td><b>Job item</b></td><td><b>Language</b></td><td><b>Submitted date</b></td></tr>" + TableRows + "</table>";

                                        MessageBodyCore += "<br/><p>See below for matching orders where we may have previously done the same translation job:<br /><br />" + Table + "</p> <br />";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                string Err = ex.Message;
                            }

                            // Send an email to notify the newly created job order of the approved enquiry
                            if (UpdatingJobOrder == false)
                            {
                                string QuoteRefPath = enquiriesService.ExtranetAndWSDirectoryPathForApp(enquiryDetails.Id) + @"\Reference";
                                string ReferenceInfoString = "";
                                if (Directory.Exists(QuoteRefPath) == true)
                                {
                                    string[] RefFiles = Directory.GetFiles(QuoteRefPath);
                                    // if the directory exists and has any reference files in it
                                    if (Directory.Exists(QuoteRefPath) == true & RefFiles.Length > 0)
                                    {
                                        string ExternalRefFolderPath = jobOrderService.ExtranetAndWSDirectoryPathForApp(SubmittedJobOrder.Id) + @"\Reference";
                                        // copy all ref files from quote ref folder to job order ref folder
                                        ReferenceInfoString = enquiriesService.CopyReferenceFilesFromExternalServer(QuoteRefPath, ExternalRefFolderPath);
                                    }
                                }

                                //if (SetUpDesignPlusJob == true)
                                //{
                                //    DesignPlusProject NewDesignPlusProject = null/* TODO Change to default(_) if this is not a reference type */;
                                //    DesignPlusFolder NewDesignFolder = null/* TODO Change to default(_) if this is not a reference type */;
                                //    ClientDesignFile NewDesignPlusFile = null/* TODO Change to default(_) if this is not a reference type */;

                                //    // create project and folder
                                //    NewDesignPlusProject = TPClientDesignFilesLogic.CreateDesignPlusProject(withBlock.JobName);
                                //    NewDesignFolder = TPDesignPlusFoldersLogic.CreateDeisgnPlusFolder(withBlock.JobName + " - Folder1", NewDesignPlusProject.Id);
                                //    NewDesignPlusFile = UploadDesignPlusFile(NewDesignPlusFile, InDesignFile, NewDesignFolder.Id);

                                //    TPClientDesignFilesLogic.AssignJobOrderIDToDesignPlusFile(NewDesignPlusFile.Id, SubmittedJobOrder.Id);
                                //    TPJobOrdersLogic.UpdateDesignPlusFileDetail(SubmittedJobOrder.Id, NewDesignPlusFile.Id);
                                //    TPClientDesignFilesLogic.UpdateParentDesignPlusIDToItSelf(NewDesignPlusFile.Id, ToUpdateChildrenDesignPlusFiles: true, JobOrderID: SubmittedJobOrder.Id);

                                //    TPClientDesignFile ThisDesignPlusFile = new TPClientDesignFile(NewDesignPlusFile.Id);
                                //    string DesignPlusFilePath = Path.Combine(ThisDesignPlusFile.ExtranetNetworkDrive, ThisDesignPlusFile.UploadedByClient.ID.ToString);
                                //    DesignPlusFilePath = Path.Combine(DesignPlusFilePath, ThisDesignPlusFile.ID.ToString);


                                //    // if there is no saved version of design plus file, take the source file which is the orginal file uploaded by the user
                                //    string DestinationFileName = "";
                                //    if (File.Exists(Path.Combine(DesignPlusFilePath, "4 Delivery file") + @"\" + ThisDesignPlusFile.DocumentName) == false)
                                //    {
                                //        DesignPlusFilePath = Path.Combine(DesignPlusFilePath, "1 Source file" + @"\" + ThisDesignPlusFile.DocumentName);
                                //        DestinationFileName = ThisDesignPlusFile.DocumentName;
                                //    }
                                //    else
                                //    {
                                //        DesignPlusFilePath = Path.Combine(DesignPlusFilePath, "4 Delivery file" + @"\" + ThisDesignPlusFile.DocumentNameWithoutExtension + ".indd");
                                //        DestinationFileName = ThisDesignPlusFile.DocumentNameWithoutExtension + ".indd";
                                //    }

                                //    string BaseFolderPathForThisContact = Path.Combine(Path.Combine(globalVariables.ExtranetNetworkBaseDirectoryPath, "Contacts"), extranetUserDataObjectID.ToString());

                                //    string BaseFolderPathForThisRequest = jobOrderService.ExtranetAndWSDirectoryPathForApp(SubmittedJobOrder.Id);
                                //    if (BaseFolderPathForThisRequest == "")
                                //        BaseFolderPathForThisRequest = Path.Combine(BaseFolderPathForThisContact, SubmittedJobOrder.Id.ToString() + " - " + SubmittedJobOrder.SubmittedDateTime.ToString("d MMM yy"));

                                //    string BaseFolderPathForThisRequestSource = Path.Combine(BaseFolderPathForThisRequest, "Source");

                                //    string BaseFolderPathForThisSourceFile = BaseFolderPathForThisRequestSource; // Path.Combine(BaseFolderPathForThisRequestSource, "File1")
                                //    string ExternalJobDrivePath = Path.Combine(BaseFolderPathForThisSourceFile, DestinationFileName);

                                //    if (System.IO.File.Exists(DesignPlusFilePath) == false)
                                //    {
                                //        FileInfo DesignPlusFile = new FileInfo(DesignPlusFilePath);
                                //        GeneralUtils.CopyDirectory(Path.Combine(BaseFolderPathForThisRequestSource, "File1"), DesignPlusFile.DirectoryName, true, true);
                                //    }

                                //    // if the internal job drive length is within allowed limit which is 247
                                //    if (ExternalJobDrivePath.Length < 259)
                                //        System.IO.File.Copy(DesignPlusFilePath, ExternalJobDrivePath, true);
                                //    else
                                //    {
                                //        string CroppedFilePathForExtJobDrive = Path.Combine(BaseFolderPathForThisSourceFile, ThisDesignPlusFile.DocumentNameWithoutExtension);
                                //        // since 259 is the limit, subtract 5 character length (or 4 for .inx files) from the total limit to be the 
                                //        // allowed path length without extension
                                //        if (ThisDesignPlusFile.DocumentFileType == "inx" & CroppedFilePathForExtJobDrive.Length > 255)
                                //        {
                                //            CroppedFilePathForExtJobDrive = CroppedFilePathForExtJobDrive.Substring(0, 255) + ".inx";
                                //            System.IO.File.Copy(DesignPlusFilePath, CroppedFilePathForExtJobDrive, true);
                                //        }
                                //        else if (ThisDesignPlusFile.DocumentFileType != "inx" & CroppedFilePathForExtJobDrive.Length > 254)
                                //        {
                                //            CroppedFilePathForExtJobDrive = CroppedFilePathForExtJobDrive.Substring(0, 254) + "." + ThisDesignPlusFile.DocumentFileType;
                                //            System.IO.File.Copy(DesignPlusFilePath, CroppedFilePathForExtJobDrive, true);
                                //        }
                                //        else
                                //            System.IO.File.Copy(DesignPlusFilePath, ExternalJobDrivePath, true);
                                //    }

                                //    //var JobItems = await jobOrderService.GetAllJobItems(SubmittedJobOrder.Id);
                                //    //foreach (JobItem JobItem in JobItems)
                                //    //{
                                //    //    if (JobItem.LanguageServiceId == 1)
                                //    //    {
                                //    //        UploadDesignPlusFile(NewDesignPlusFile, InDesignFile, NewDesignFolder.Id, SubmittedJobItem: JobItem);
                                //    //        TPClientDesignFilesLogic.AssignJobOrderIDToDesignPlusFile(NewDesignPlusFile.Id, SubmittedJobOrder.Id);
                                //    //        TPJobOrdersLogic.UpdateDesignPlusFileDetail(SubmittedJobOrder.Id, NewDesignPlusFile.Id);
                                //    //        TPClientDesignFilesLogic.UpdateParentDesignPlusIDToItSelf(NewDesignPlusFile.Id, ToUpdateChildrenDesignPlusFiles: true, JobOrderID: SubmittedJobOrder.ID);
                                //    //    }
                                //    //}
                                //}

                                if (withBlock.PrintingProject == true)
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />The newly created job order for this is: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a>" + ReferenceInfoString + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
                                else
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />The newly created job order for this is: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a>" + ReferenceInfoString + "</p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);

                            }
                            else
                            {
                                if (withBlock.PrintingProject == true)
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />This enquiry was unlocked and then approved again, therefore the job order for this is still: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a> which has been updated" + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
                                else
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid==" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />This enquiry was unlocked and then approved again, therefore the job order for this is still: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a> which has been updated </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
                            }
                            //SubmittedJobOrder = new TPJobOrder(SubmittedJobOrder.Id);

                            //TPProcessAutomationBatch AutoProcessingBatch = new TPProcessAutomationBatch("iplus", "ITSupport@translateplus.com");
                            //TPProcessAutomationTask AutoProcessingTask = new TPProcessAutomationTask(TPProcessAutomationTask.AutomationTaskTypes.SetUpJobFoldersForApprovedEnquiryFromExternalServer, JobOrderID: SubmittedJobOrder.Id, AdditionalText: NetworkFolderTemplate, EnquiryID: EnquiryToUpdate.Id);
                            //AutoProcessingBatch.AppendTask(AutoProcessingTask);
                            //AutoProcessingBatch.Commit();

                            XmlDocument BatchDoc = new XmlDocument();
                            BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine +
                                             "<!-- translate plus process automation batch file -->" + Environment.NewLine +
                                             "<translateplusBatch />");

                            XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

                            XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
                            BatchDocAttr.Value = "1.0";
                            RootNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
                            BatchDocAttr.Value = globalVariables.iplusEmployeeID.ToString();
                            RootNode.Attributes.Append(BatchDocAttr);

                            //write e-mail notification address(es) info
                            BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                            BatchDocAttr.Value = "ITSupport@translateplus.com";
                            RootNode.Attributes.Append(BatchDocAttr);


                            XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                            //write task type info
                            BatchDocAttr = BatchDoc.CreateAttribute("Type");
                            BatchDocAttr.Value = "SetUpJobFoldersForApprovedEnquiryFromExternalServer";
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            //write task number info 
                            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                            BatchDocAttr.Value = "1";
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
                            BatchDocAttr.Value = SubmittedJobOrder.Id.ToString();
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("AdditionalText");
                            BatchDocAttr.Value = NetworkFolderTemplate;
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            BatchDocAttr = BatchDoc.CreateAttribute("EnquiryID");
                            BatchDocAttr.Value = withBlock.Id.ToString();
                            IndividualTaskNode.Attributes.Append(BatchDocAttr);

                            //now append the node to the doc
                            RootNode.AppendChild(IndividualTaskNode);

                            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
                            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                            BatchDoc.Save(BatchFilePath);


                            Brand CurrentBrand = new Brand();
                            CurrentBrand = await brandsService.GetBrandById(1);
                            if (extranetUserObj != null && extranetUserObj.Id > 0)
                                CurrentBrand = await brandsService.GetBrandById(orgGroupDetails.BrandId);

                            jobOrderService.AnnounceThisOrderCreation(SubmittedJobOrder.Id, "flow plus <flowplus@translateplus.com>", miscResourceService.GetMiscResourceByName("QuoteApprovedHeader", "en").Result.StringContent, true, false, "en", QuoteApprovalNotification: true);
                        }
                        else if (Status == 2)
                        {
                            await enquiriesService.ApproveOrRejectEnquiry(model);

                            // for rejections include the current sales manager & Macarena in the notification e-mail
                            var CurrentSalesDepartmentManager = await employeesService.CurrentSalesDepartmentManager();
                            EmailRecipients = EmailRecipients + "," + CurrentSalesDepartmentManager.EmailAddress;





                            // Send an email to notify the reject enquiry
                            if (withBlock.PrintingProject == true)
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>", true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
                            else
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + extranetUserObj.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + "</p>", true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
                        }
                    }
                }

                // report any error
                catch (Exception ex)
                {

                    var ErrorDetailsLabel = miscResourceService.GetMiscResourceByName("QuoteApprovalError", "en").Result.StringContent;
                    string UserIdentityName = "<b>Logged-in user name:</b> ";
                    if ((User.Identity != null) && (User.Identity.Name != null))
                        UserIdentityName += "http://myplus/ExtranetUser.aspx?UserName=" + User.Identity.Name;
                    else
                        UserIdentityName += "(could not be determined)";

                    string ErrStringForEmail = "<p>An error occurred:<br /><br />" + UserIdentityName + "<br />" + "<b>URL</b>: " + HttpContext.Request.Path + "<br />" + "<b>User-agent (browser info)</b>: " + HttpContext.Request.Host + "<br />" + "<b>Source</b>: " + ex.Source + "<br />" + "<b>Message</b>: " + ex.Message + "<br />" + "<b>Inner exception</b>: " + ex.InnerException + "<br />" + "<b>Stack trace</b>: " + ex.StackTrace + "</p>";
                    emailService.SendMail("flow plus <flowplus@translateplus.com>", "ArianD@translateplus.com, KavitaJ@translateplus.com, raj.lakhwani@publicismedia.com", "Extranet error occurred", ErrStringForEmail, IsExternalNotification: true);

                }

                // confirm success 
                if (model.Status == 3)
                {

                    message = "Success$" + SubmittedJobOrder.Id;
                }
                else
                {

                    message = "Rejected$" + quoteDetails.Id;
                }





                return message;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public IActionResult GetFile(string filePath)
        {

            var fileName = Path.GetFileName(filePath);
            byte[] thisFileBytes = System.IO.File.ReadAllBytes(filePath);
            FileContentResult file = new FileContentResult(thisFileBytes, "application/octet-stream")
            {
                FileDownloadName = fileName
            };
            return file;
        }
    }
}
