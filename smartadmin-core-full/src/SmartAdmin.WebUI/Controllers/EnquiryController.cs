using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Global_Settings;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.JobItem;
using ViewModels.LinguisticSupplier;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Web;
using Services;
using Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Globalization;
using ViewModels.EmployeeOwnerships;
using ViewModels.Organisation;
using Data.Repositories;

namespace SmartAdmin.WebUI.Controllers
{
    public class EnquiryController : Controller
    {
        private readonly ITPEnquiriesService enquiriesService;
        private readonly ITPEmployeesService employeesService;
        private readonly ICommonCachedService cachedService;
        private readonly ITPContactsLogic contactsService;
        private readonly ITPOrgsLogic orgsService;
        private readonly ITPOrgGroupsLogic orgGroupsLogic;
        private readonly ITPJobOrderChannelsLogic joborderchannelservice;
        private readonly ITPEmployeeOwnershipsLogic employeeOwnershipsLogic;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly ITPLanguageService languageService;
        private readonly ITPQuotesLogic quoteService;
        private readonly ITPBrandsService brandService;
        private readonly ITPQuoteTemplatesService quoteTemplatesService;
        private readonly ITPEnquiryQuoteItemsService enquiryQuoteItemsService;
        private readonly ITPFileSystemService fileSystemService;
        private readonly IConfiguration Configuration;
        private readonly ITPQuoteItemsLogic quoteitemService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPExchangeService exchangeService;
        private readonly ITPCurrenciesLogic currencyservice;
        private readonly GlobalVariables globalVariables;
        private readonly ITPJobOrderService jobOrderService;
        private readonly IRepository<ClientDecisionReason> cdsInfoRepo;
        private readonly ITPJobItemService itemService;
        private readonly IQuoteAndOrderDiscountsAndSurcharges quoteAndOrderDiscountsAndSurchargesService;
        private readonly IQuoteAndOrderDiscountsAndSurchargesCategories quoteAndOrderDiscountsAndSurchargesCategoriesService;
        private readonly ITPLinguistService linguistService;
        private readonly ITPMiscResourceService miscResourceService;

        public EnquiryController(ITPEnquiriesService enquiriesService,
            ITPEmployeesService employeesService,
            ICommonCachedService cachedService,
            ITPContactsLogic contactsService,
            ITPOrgsLogic orgsService,
            ITPOrgGroupsLogic orgGroupsLogic,
            ITPJobOrderChannelsLogic joborderchannelservice,
            ITPEmployeeOwnershipsLogic employeeOwnershipsLogic,
            ITPTimeZonesService timeZonesService,
            ITPLanguageService languageService,
            ITPQuotesLogic quoteService,
            ITPBrandsService brandService,
            ITPQuoteTemplatesService quoteTemplatesService,
            ITPEnquiryQuoteItemsService enquiryQuoteItemsService,
            ITPFileSystemService fileSystemService,
            IConfiguration _configuration,
            ITPQuoteItemsLogic quoteitemService,
            IEmailUtilsService emailUtilsService,
            ITPExchangeService exchangeService,
            ITPCurrenciesLogic currencyservice,
            ITPJobOrderService jobOrderService,
            IRepository<ClientDecisionReason> cdsInfoRepo,
            ITPJobItemService itemService,
            IQuoteAndOrderDiscountsAndSurcharges quoteAndOrderDiscountsAndSurchargesService, IQuoteAndOrderDiscountsAndSurchargesCategories quoteAndOrderDiscountsAndSurchargesCategoriesService, ITPLinguistService linguistService, ITPMiscResourceService miscResourceService)
        {
            this.enquiriesService = enquiriesService;
            this.employeesService = employeesService;
            this.cachedService = cachedService;
            this.contactsService = contactsService;
            this.orgsService = orgsService;
            this.orgGroupsLogic = orgGroupsLogic;
            this.joborderchannelservice = joborderchannelservice;
            this.employeeOwnershipsLogic = employeeOwnershipsLogic;
            this.timeZonesService = timeZonesService;
            this.languageService = languageService;
            this.quoteService = quoteService;
            this.brandService = brandService;
            this.quoteTemplatesService = quoteTemplatesService;
            this.enquiryQuoteItemsService = enquiryQuoteItemsService;
            this.fileSystemService = fileSystemService;
            Configuration = _configuration;
            this.quoteitemService = quoteitemService;
            emailService = emailUtilsService;
            this.exchangeService = exchangeService;
            this.currencyservice = currencyservice;
            globalVariables = new GlobalVariables();
            _configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVariables);
            this.jobOrderService = jobOrderService;
            this.cdsInfoRepo = cdsInfoRepo;
            this.itemService = itemService;
            this.quoteAndOrderDiscountsAndSurchargesService = quoteAndOrderDiscountsAndSurchargesService;
            this.quoteAndOrderDiscountsAndSurchargesCategoriesService = quoteAndOrderDiscountsAndSurchargesCategoriesService;
            this.linguistService = linguistService;
            this.miscResourceService = miscResourceService;
        }
        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
                return NotFound();

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var model = await enquiriesService.GetViewModelById(id);

            if (model == null)
                return Content("The enquiry ID you provided (" + id + ") is not a valid number.");

            if (loggedInEmployee == null)
                return NotFound();

            var contact = await contactsService.GetContactDetails(model.ContactId);
            ViewBag.ContactDeleted = "";
            if (contact.DeletedDate != null && contact.DeletedDate != DateTime.MinValue)
            {
                ViewBag.ContactDeleted = "&nbsp;<i>(Deleted)</i>";
            }

            if (contact == null) { return NotFound(); }

            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var group = await orgGroupsLogic.GetOrgGroupDetails((Int32)org.OrgGroupId);

            var Languages = await cachedService.GetAllLanguagesCached();
            model.AllJobOrderChannels = await joborderchannelservice.GetAllJobOrderChannels<JobOrderChannel>();
            //model.JobOrderId = joborder.Id;
            //model.JobOrderName = joborder.JobName;
            model.ContactId = contact.Id;
            model.ContactName = contact.Name;
            model.OrgId = org.Id;
            model.OrgName = org.OrgName;
            model.OrgSLA = org.SLA;

            if (org.SLA != null & org.SLA > 0)
            {
                int SLAHours = (int)org.SLA;
                var currentDate = timeZonesService.GetCurrentGMT();
                var deadline = currentDate.AddHours(SLAHours);

                if (deadline.DayOfWeek == DayOfWeek.Saturday || deadline.DayOfWeek == DayOfWeek.Sunday || (deadline.DayOfWeek == DayOfWeek.Friday && deadline.Hour > 17) || (deadline.DayOfWeek == DayOfWeek.Monday && deadline.Hour < 7))
                {
                    while (deadline.DayOfWeek != DayOfWeek.Monday)
                    {
                        deadline = deadline.AddDays(1);
                    }
                    model.SLAdeadline = new DateTime(deadline.Year, deadline.Month, deadline.Day, 10, 0, 0);
                }
                else
                {
                    model.SLAdeadline = (DateTime)currentDate.AddHours(SLAHours);
                }


            }

            model.OrgGroupId = group.Id;
            model.OrgGroupName = group.Name;
            model.ListOfEmployees = await employeesService.GetAllEmployees<Employee>(false, false);
            var opsOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.EnquiriesLead);
            model.Ownership = (short)(opsOwnership == null ? 0 : opsOwnership.EmployeeId);
            //model.IsVisibleToClient = false;
            //model.CompletionDeadline = joborder.OverallDeliveryDeadline;
            model.LanguageServices = await languageService.GetLanguageServices();
            model.LanguageServiceCategory = await cachedService.getLanguageServiceCategory();
            model.Languages = Languages.OrderBy(o => o.Name).ToList();
            model.EnquiryQuoteItems = await enquiryQuoteItemsService.GetEnquiryQuoteItemsByEnquiryId(id);
            model.QuotesResults = await enquiriesService.GetQuotes(id);
            var currentQuote = model.QuotesResults.Where(o => o.CurrentVersion == true).FirstOrDefault();
            ViewBag.CurrentQuoteID = 0;
            if (currentQuote != null)
            {
                ViewBag.CurrentQuoteID = currentQuote.QuoteID;
                var quoteItems = await quoteService.GetAllQuoteItems(currentQuote.QuoteID);

                model.QuoteItems = quoteItems.OrderBy(o => o.CreatedDateTime).ToList();
            }
            ViewBag.SetUpByEmployee = "<i>(n/a)</i>";
            if (model.CreatedByEmployeeId > 0)
            {
                var setupByEmployee = await employeesService.IdentifyCurrentUserById((int)model.CreatedByEmployeeId);
                ViewBag.SetUpByEmployee = "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/" + setupByEmployee.Id.ToString() + "\">" + setupByEmployee.FirstName + " " + setupByEmployee.Surname + "</a>";
                ViewBag.SetUpByEmployeeImageBase64 = setupByEmployee.ImageBase64;
            }

            ViewBag.LastModifiedByEmployee = "<i>(n/a)</i>";
            if (model.LastModifiedByEmployeeId != null)
            {
                var lastModifiedByEmployee = await employeesService.IdentifyCurrentUserById((int)model.LastModifiedByEmployeeId);
                ViewBag.LastModifiedByEmployee = "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/" + lastModifiedByEmployee.Id.ToString() + "\">" + lastModifiedByEmployee.FirstName + " " + lastModifiedByEmployee.Surname + "</a>";
                ViewBag.LastModifiedByEmployeeImageBase64 = lastModifiedByEmployee.ImageBase64;
            }

            ViewBag.LastModifiedDate = "<i>(never)</i>";
            if (model.LastModifiedDateTime != null)
            {
                DateTime LastModifiedDateTime = (DateTime)model.LastModifiedDateTime;
                ViewBag.LastModifiedDate = LastModifiedDateTime.ToString("dddd dd MMMM yyyy HH:mm");
            }

            var config = new GlobalVariables();
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);
            string serverLocationPath = config.InternalQuoteDriveBaseDirectoryPathForApp;
            model.EnquiryFolder = fileSystemService.GetEnquiryDirectoryPath(serverLocationPath, org.Id, model.Id);
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            var emp = await employeesService.GetEmployeeByUsername(username);
            var department = await employeesService.GetEmployeeDepartment(emp.Id);
            model.IsAllowedToEdit = false;
            model.IsFinalised = false;
            model.MarkForSales = false;
            model.MarkForOps = false;
            model.FinaliseEnquiry = false;
            model.LinkJobOrder = false;
            model.EditPageEnabled = false;
            model.MarkAsPending = false;
            model.WebServiceDelivery = false;
            model.FinaliseEnquiryButtonText = "Approve";
            model.MarkAsPendingText = "Send to client (mark as pending)";
            var EnquiryFolder = fileSystemService.GetEnquiryDirectoryPath(serverLocationPath, org.Id, model.Id);
            var QuotePDFPath = "";
            if (EnquiryFolder != null && EnquiryFolder != "" && currentQuote != null)
            {
                if (Directory.Exists(Path.Combine(EnquiryFolder, "Quote")) == true)
                {
                    var QuoteDir = new DirectoryInfo(Path.Combine(EnquiryFolder, "Quote"));
                    var Filename = String.Format("{0} - {1}.pdf", currentQuote.QuoteID.ToString(), currentQuote.QuoteFileName);

                    try
                    {
                        if (QuoteDir.GetFiles(Filename).Length > 0)
                        {
                            QuotePDFPath = QuoteDir.GetFiles(Filename)[0].FullName;
                        }
                    }
                    catch { }


                }
            }
            if (org.OrgGroupId == 18520) { model.IsAllowedToEdit = true; }
            else
            {
                if (loggedInEmployee.TeamId == 5 || loggedInEmployee.TeamId == 16 || loggedInEmployee.TeamId == 13 || loggedInEmployee.TeamId == 38 || (department.Id != 1 && emp.AttendsBoardMeetings == true && department.Id != (byte)Enumerations.Departments.CompanyDirectors))
                {
                    model.IsAllowedToEdit = false;
                }
                else { model.IsAllowedToEdit = true; }
            }
            if (model.Status == 2 || model.Status == 3) { model.IsFinalised = true; }
            else { model.MarkForSales = true; model.MarkForOps = true; }
            if (model.IsFinalised == false)
            {
                if (model.Status == 1)
                {
                    if (model.IsAllowedToEdit == true || department.Id == (byte)Enumerations.Departments.CompanyDirectors)
                    {
                        model.FinaliseEnquiry = true;
                        model.LinkJobOrder = true;
                    }
                }
            }
            else
            {
                model.FinaliseEnquiryButtonText = "Unlock";
                if (model.IsAllowedToEdit == true || department.Id == (byte)Enumerations.Departments.CompanyDirectors || loggedInEmployee.Id == 586)
                {
                    model.FinaliseEnquiry = true;
                }
            }
            if (model.IsFinalised == false && model.IsAllowedToEdit == true)
            {
                model.EditPageEnabled = true; model.DeleteLink = "https://myplus.publicisgroupe.net/ConfirmEnquiryDeletion.aspx?EnquiryID=" + model.Id;
                model.CreateAutoQuoteLink = "https://myplus.publicisgroupe.net/AutoQuoting.aspx?EnquiryID=" + model.Id;
            }
            if (model.IsFinalised == true) { model.FinaliseEnquiryLink = "https://myplus.publicisgroupe.net/FinaliseEnquiry.aspx?EnquiryID=" + model.Id + "&Unlock=True"; }
            else
            {
                if (model.Status == 1)
                {
                    model.FinaliseEnquiryLink = "https://myplus.publicisgroupe.net/FinaliseEnquiry.aspx?EnquiryID=" + model.Id;
                    model.LinkJobOrderLink = "https://myplus.publicisgroupe.net/FinaliseEnquiry.aspx?EnquiryID=" + model.Id + "&Link=True";
                }
            }
            if (model.MarkForSales == true && model.MarkForOps == true) { model.MarkAsPending = true; }

            if (model.IsFinalised == false)
            {
                if (model.Status == 1)
                {
                    if (currentQuote != null)
                    {
                        model.MarkAsPendingText = "Mark as draft";
                        if (model.IsAllowedToEdit == true) { model.MarkAsPending = true; }
                    }
                }

                if (model.Status == 0)
                {
                    if (model.QuotesResults.Count() > 0)
                    {
                        if (currentQuote != null && model.QuoteItems.Count() > 0)
                        {
                            if (QuotePDFPath != "" && model.IsAllowedToEdit == true)
                            {
                                model.MarkAsPending = true;
                                if (model.OrderChannelId == 7 || model.OrderChannelId == 8 || model.OrderChannelId == 12 || model.OrderChannelId == 15 || model.OrderChannelId == 18 || model.OrderChannelId == 21 | model.OrderChannelId == 22)
                                {
                                    model.WebServiceDelivery = true;
                                }
                                if (model.WebServiceDelivery == true) { model.MarkAsPending = false; }
                                
                            }
                        }
                    }
                }
            }
            if (model.MarkAsPendingText == "Mark as draft") { model.MarkForSales = false; model.MarkForOps = false; }
            if (model.OrderChannelId == 7 || model.OrderChannelId == 8 || model.OrderChannelId == 12 || model.OrderChannelId == 15 || model.OrderChannelId == 18 || model.OrderChannelId == 21 | model.OrderChannelId == 22)
            {
                model.WebServiceDelivery = true;
            }
            if (model.WebServiceDelivery == true) { model.MarkAsPending = false; }
            if (model.MarkAsPending == true) { model.WebServiceDelivery = false; }
            model.WebServiceDeliveryLink = "https://myplus.publicisgroupe.net/SendQuoteConfirmation.aspx?EnquiryID=" + model.Id;
            model.ClientDecisionReason = cdsInfoRepo.All();
            var enquiryStatus = await cachedService.getEnquiryStatuses();
            ViewBag.EnquiryStatusString = enquiryStatus.Where(o => o.Id == model.Status && o.DeletedDateTime == null).Select(o => o.Name).FirstOrDefault();
            ViewBag.EnquiryDecisionReason = cdsInfoRepo.All().Where(o => o.DecisionReasonId == model.DecisionReasonId).Select(o => o.Reason).FirstOrDefault();
            return View(model);
        }

        public async Task<IActionResult> NewEnquiry(int contactID)
        {
            if (contactID == 0)
                return NotFound();

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var contact = await contactsService.GetById(contactID);

            if (contact == null) { return NotFound(); }

            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var group = await orgGroupsLogic.GetOrgGroupDetails((Int32)org.OrgGroupId);


            //if (item == null)
            //    return Content("The Job Order ID you provided (" + OrderID + ") is not a valid number.");

            if (loggedInEmployee == null)
                return NotFound();

            var Languages = await cachedService.GetAllLanguagesCached();
            ViewModels.Enquiries.EnquiriesViewModel model = new ViewModels.Enquiries.EnquiriesViewModel();
            model.AllJobOrderChannels = await joborderchannelservice.GetAllJobOrderChannels<JobOrderChannel>();
            //model.JobOrderId = joborder.Id;
            //model.JobOrderName = joborder.JobName;
            model.ContactId = contact.Id;
            model.ContactName = contact.Name;
            model.OrgId = org.Id;
            model.OrgName = org.OrgName;
            model.OrgSLA = org.SLA;

            if (org.SLA != null & org.SLA > 0)
            {
                int SLAHours = (int)org.SLA;
                var currentDate = timeZonesService.GetCurrentGMT();
                var deadline = currentDate.AddHours(SLAHours);

                if (deadline.DayOfWeek == DayOfWeek.Saturday || deadline.DayOfWeek == DayOfWeek.Sunday || (deadline.DayOfWeek == DayOfWeek.Friday && deadline.Hour > 17) || (deadline.DayOfWeek == DayOfWeek.Monday && deadline.Hour < 7))
                {
                    while (deadline.DayOfWeek != DayOfWeek.Monday)
                    {
                        deadline = deadline.AddDays(1);
                    }
                    model.SLAdeadline = new DateTime(deadline.Year, deadline.Month, deadline.Day, 10, 0, 0);
                }
                else
                {
                    model.SLAdeadline = (DateTime)currentDate.AddHours(SLAHours);
                }


            }

            model.OrgGroupId = group.Id;
            model.OrgGroupName = group.Name;
            model.ListOfEmployees = await employeesService.GetAllEmployees<Employee>(false, false);
            var opsOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.EnquiriesLead);
            model.Ownership = (short)(opsOwnership == null ? 0 : opsOwnership.EmployeeId);
            //model.IsVisibleToClient = false;
            //model.CompletionDeadline = joborder.OverallDeliveryDeadline;
            model.LanguageServices = await languageService.GetLanguageServices();
            model.LanguageServiceCategory = await cachedService.getLanguageServiceCategory();
            model.Languages = Languages.OrderBy(o => o.Name).ToList();

            ViewBag.CurrentDirectory = (Environment.CurrentDirectory + @"\wwwroot").Replace("\\", " /");

            return View(model);
        }


        public async Task<IActionResult> CreateQuote()
        {

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            int currentQuoteID = Convert.ToInt32(allFields[1]);
            int EnquiryID = Convert.ToInt32(allFields[0]);
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            
            Enquiry enquiry = await enquiriesService.GetEnquiryById(EnquiryID);
            Quote submittedQuote = null;

            //JobOrder newRequest = null;
            //newRequest = await enquiriesService.CreateEnquiry(model);
            if (enquiry == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Quote on the database.");
                return Ok(MessagsString);
            }
            else
            {

                var contact = await contactsService.GetById(enquiry.ContactId);

                var currentOrg = await orgsService.GetOrgDetails(contact.OrgId);

                var currentBrand = await brandService.GetBrandById(1);

                if (currentOrg.OrgGroupId != null)
                {
                    currentBrand = await brandService.GetBrandForClient(currentOrg.OrgGroupId.Value);
                }

                string QuoteTitle = "";
                switch (currentOrg.InvoiceLangIanacode)
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
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    case 2:
                        QuoteFileName = "Jackpot Translation - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    default:
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;
                }
                if (currentOrg.InvoiceLangIanacode == null) { currentOrg.InvoiceLangIanacode = "en"; }
                QuoteTemplate defaultQuoteTemplate = await quoteTemplatesService.GetDefaultQuoteTemplate(currentOrg.InvoiceLangIanacode);

                var salesOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead);

                if (salesOwner == null)
                {
                    salesOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead);
                }

                short currencyID = 4;
                if (currentOrg.InvoiceCurrencyId != null)
                {
                    currencyID = (short)currentOrg.InvoiceCurrencyId;
                }


                string Address2 = "";
                if (currentOrg.Address2 != null)
                {
                    Address2 = currentOrg.Address2;
                }
                string Address3 = "";
                if (currentOrg.Address3 != null)
                {
                    Address3 = currentOrg.Address3;
                }
                string Address4 = "";
                if (currentOrg.Address4 != null)
                {
                    Address4 = currentOrg.Address4;
                }
                short salesOwnerEmployeeID = 0;
                if (salesOwner != null)
                {
                    salesOwnerEmployeeID = (short)salesOwner.EmployeeId;
                }
                //else { salesOwnerEmployeeID =}
                submittedQuote = await quoteService.CreateQuote(enquiry.Id, true, currencyID, currentOrg.InvoiceLangIanacode,
                                                                QuoteTitle, QuoteFileName, "", timeZonesService.GetCurrentGMT(), currentOrg.OrgName,
                                                                currentOrg.Address1, Address2, Address3, Address4,
                                                                currentOrg.CountyOrState.ToString(), currentOrg.PostcodeOrZip.ToString(), currentOrg.CountryId,
                                                                contact.Name, defaultQuoteTemplate.OpeningSectionText, defaultQuoteTemplate.ClosingSectionText,
                                                                (byte)Enumerations.TimelineUnits.Days, 0, (byte)Enumerations.WordCountDisplay.ShowMemoryBreakdownStandard, false,
                                                                false, false, false, false, salesOwnerEmployeeID, null,
                                                                null, null, null, false,
                                                                false, false, false, currentOrg.InvoiceBlanketPonumber, loggedInEmployee.Id,
                                                                timeZonesService.GetCurrentGMT(), PrintingProject: (bool)enquiry.PrintingProject);
            }

            //List<EnquiryQuoteItem> eqi = await enquiryQuoteItemsService.GetEnquiryQuoteItemsByEnquiryId(EnquiryID);
            List<QuoteItem> items = await quoteService.GetAllQuoteItems(currentQuoteID);
            List<int> ignoreItems = new List<int>();
            foreach (string data in allFields)
            {
                if (data.Contains("delete") == true)
                {
                    ignoreItems.Add(Convert.ToInt32(data.Substring(0, data.IndexOf("')"))));
                }
            }
            foreach (QuoteItem quoteitem in items)
            {
                bool ignoreItemFound = false;
                foreach (int data in ignoreItems)
                {
                    if (quoteitem.Id == Convert.ToInt32(data))
                    {
                        ignoreItemFound = true;
                        break;
                    }
                }
                if (!ignoreItemFound)
                {
                    QuoteItem quoteItem = await quoteService.CreateQuoteItem(submittedQuote.Id, (byte)quoteitem.LanguageServiceId, quoteitem.SourceLanguageIanacode, quoteitem.TargetLanguageIanacode, 0,
                                                                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                        0, "", 0, loggedInEmployee.Id, 0,
                                                                        0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: quoteitem.LanguageServiceCategoryId == null ? 0 : (int)quoteitem.LanguageServiceCategoryId);
                }
            }
            foreach (string quoteitem in allFields)
            {
                if (quoteitem.Contains(",") == true)
                {

                    await quoteService.CreateQuoteItem(submittedQuote.Id, Convert.ToByte((quoteitem.Split(",")[0])), quoteitem.Split(",")[1], quoteitem.Split(",")[2], 0,
                                                                               0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                               0, "", 0, loggedInEmployee.Id, 0,
                                                                               0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: Convert.ToInt32(quoteitem.Split(",")[3]));
                }
            }


            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlDataAdapter QuoteDataAdapter = new SqlDataAdapter();
            QuoteDataAdapter.UpdateCommand = new SqlCommand("procUpdateQuoteCurrentVersion", SQLConn);

            QuoteDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_QUOTE_ID", SqlDbType.Int)).Value = submittedQuote.Id;
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_ENQUIRY_ID", SqlDbType.Int)).Value = EnquiryID;

            SqlParameter ReturnValParam = QuoteDataAdapter.UpdateCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                QuoteDataAdapter.UpdateCommand.Connection.Open();
                QuoteDataAdapter.UpdateCommand.ExecuteNonQuery();

                if ((int)(QuoteDataAdapter.UpdateCommand.Parameters["@RowCount"].Value) < 1)
                    throw new Exception("Marking quote as current version was not successful.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to mark quote as curent version.");
            }
            finally
            {
                try
                {
                    // Clean up
                    QuoteDataAdapter.UpdateCommand.Connection.Close();
                    QuoteDataAdapter.Dispose();
                }
                catch (Exception SE)
                {
                    throw new Exception("SQL exception: " + SE.Message);
                }
            }

            return Ok($"Success$Quote created successfully.${submittedQuote.Id}");

            //return RedirectToAction("Index", "Quote", submittedQuote.Id);
            //return Redirect("../Quote?id=" + submittedQuote.Id);
            // return View("Views/Quote/Index.cshtml", submittedQuote.Id);
        }


        [HttpPost]
        public async Task<IActionResult> CreateEnquiryAsync()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");
            //model.ContactId = Convert.ToInt32(allFields[0]);
            //model.OrderChannelId = Convert.ToByte(allFields[1]);
            //model.JobName = allFields[2];
            //model.AssignedToEmployeeID = short.Parse(allFields[3]);

            DateTime? deadlineDate = null;
            //deadlineDate = timeZonesService.GetCurrentGMT();
            if (allFields[4] != "" && allFields[4].StartsWith("d") == false)
            {
                deadlineDate = Convert.ToDateTime(allFields[4]);
                //model.DeadlineRequestedByClient = Convert.ToDateTime(allFields[4]);
            }

            DateTime enquirydeadlineDate = timeZonesService.GetCurrentGMT().AddDays(2);
            if (allFields[8] != "" && allFields[8].StartsWith("d") == false)
            {
                enquirydeadlineDate = Convert.ToDateTime(allFields[8]);
                //model.DeadlineRequestedByClient = Convert.ToDateTime(allFields[4]);
            }


            //model.ExternalNotes = allFields[5];
            //model.InternalNotes = allFields[6];
            //model.PrintingProject = Convert.ToBoolean(allFields[7]);
            //model.CreatedByEmployeeId = loggedInEmployee.Id;


            //Enquiry enquiry = await enquiriesService.CreateEnquiry(Convert.ToInt32(allFields[0]), Convert.ToByte(allFields[1]), allFields[5], allFields[6], allFields[2], deadlineDate, loggedInEmployee.Id, Convert.ToBoolean(allFields[7]), false, enquirydeadlineDate, Convert.ToInt32(allFields[3]));
            Enquiry enquiry = await enquiriesService.CreateEnquiry(Convert.ToInt32(allFields[0]), Convert.ToByte(allFields[1]), allFields[5], allFields[6], allFields[2],  loggedInEmployee.Id, deadlineDate, Convert.ToBoolean(allFields[7]), false, enquirydeadlineDate, null);

            Quote submittedQuote = null;

            //JobOrder newRequest = null;
            //newRequest = await enquiriesService.CreateEnquiry(model);
            if (enquiry == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Enquiry on the database.");
                return Ok(MessagsString);
            }
            else
            {

                var contact = await contactsService.GetById(Convert.ToInt32(allFields[0]));

                var currentOrg = await orgsService.GetOrgDetails(contact.OrgId);

                var currentBrand = await brandService.GetBrandById(1);

                if (currentOrg.OrgGroupId != null)
                {
                    currentBrand = await brandService.GetBrandForClient(currentOrg.OrgGroupId.Value);
                }

                string QuoteTitle = "";
                switch (currentOrg.InvoiceLangIanacode)
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
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    case 2:
                        QuoteFileName = "Jackpot Translation - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;

                    default:
                        QuoteFileName = "translate plus - " + currentOrg.OrgName + " - " + timeZonesService.GetCurrentGMT().ToString("d MMM yy");
                        break;
                }

                if (currentOrg.InvoiceLangIanacode == null) { currentOrg.InvoiceLangIanacode = "en"; }
                QuoteTemplate defaultQuoteTemplate = await quoteTemplatesService.GetDefaultQuoteTemplate(currentOrg.InvoiceLangIanacode);

                var salesOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead);

                if (salesOwner == null)
                {
                    salesOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(currentOrg.Id, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead);
                }

                short currencyID = 4;
                if (currentOrg.InvoiceCurrencyId != null)
                {
                    currencyID = (short)currentOrg.InvoiceCurrencyId;
                }


                string Address2 = "";
                if (currentOrg.Address2 != null)
                {
                    Address2 = currentOrg.Address2;
                }
                string Address3 = "";
                if (currentOrg.Address3 != null)
                {
                    Address3 = currentOrg.Address3;
                }
                string Address4 = "";
                if (currentOrg.Address4 != null)
                {
                    Address4 = currentOrg.Address4;
                }
                short salesOwnerEmployeeID = 0;
                if (salesOwner != null)
                {
                    salesOwnerEmployeeID = (short)salesOwner.EmployeeId;
                }
                submittedQuote = await quoteService.CreateQuote(enquiry.Id, true, currencyID, currentOrg.InvoiceLangIanacode,
                                                                QuoteTitle, QuoteFileName, "", timeZonesService.GetCurrentGMT(), currentOrg.OrgName,
                                                                currentOrg.Address1, Address2, Address3, Address4,
                                                                currentOrg.CountyOrState.ToString(), currentOrg.PostcodeOrZip.ToString(), currentOrg.CountryId,
                                                                contact.Name, defaultQuoteTemplate.OpeningSectionText, defaultQuoteTemplate.ClosingSectionText,
                                                                (byte)Enumerations.TimelineUnits.Days, 0, (byte)Enumerations.WordCountDisplay.ShowMemoryBreakdownStandard, false,
                                                                false, false, false, false, salesOwnerEmployeeID, null,
                                                                null, null, null, false,
                                                                false, false, false, currentOrg.InvoiceBlanketPonumber, loggedInEmployee.Id,
                                                                timeZonesService.GetCurrentGMT(), PrintingProject: (bool)enquiry.PrintingProject);

                XmlDocument BatchDoc = new XmlDocument();
                //XmlAttribute BatchDocAttr;
                BatchDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + System.Environment.NewLine +
                                 "<!-- translate plus process automation batch file -->" +
                                 "<translateplusBatch />");

                XmlNode RootNode = BatchDoc.SelectSingleNode("//translateplusBatch");

                // write format version info
                XmlAttribute BatchDocAttr = BatchDoc.CreateAttribute("BatchFormatVersion");
                BatchDocAttr.Value = "1.2";
                RootNode.Attributes.Append(BatchDocAttr);

                BatchDocAttr = BatchDoc.CreateAttribute("OwnerEmployeeName");
                BatchDocAttr.Value = loggedInEmployee.NetworkUserName;
                RootNode.Attributes.Append(BatchDocAttr);

                // write e-mail notification address(es) info
                BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
                BatchDocAttr.Value = loggedInEmployee.EmailAddress;
                RootNode.Attributes.Append(BatchDocAttr);


                XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

                // write Type info 
                BatchDocAttr = BatchDoc.CreateAttribute("Type");
                BatchDocAttr.Value = "SetUpEnquiryFoldersFromInternalServer";
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                // write task number info 
                BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
                BatchDocAttr.Value = "1";
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                // writeenquiry number info 
                BatchDocAttr = BatchDoc.CreateAttribute("EnquiryID");
                BatchDocAttr.Value = enquiry.Id.ToString();
                IndividualTaskNode.Attributes.Append(BatchDocAttr);

                // now append the node to the doc
                RootNode.AppendChild(IndividualTaskNode);

                var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-40";
                var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
                var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
                BatchDoc.Save(BatchFilePath);



            }

            return Ok($"Success$Enquiry created successfully.${enquiry.Id}${submittedQuote.Id}");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnquiryQuoteItems()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            var quote = await quoteService.GetQuoteById(Convert.ToInt32(allFields[1]));
            EnquiryQuoteItem newEnquiryQuoteItem = null;
            foreach (string quoteitem in allFields)
            {
                if (quoteitem.Contains("delete") == true)
                {
                    var quoteID = quoteitem.Substring(0, quoteitem.IndexOf("')"));

                    QuoteItem item = await quoteitemService.RemoveQuoteItem(Convert.ToInt32(quoteID), loggedInEmployee.Id);
                    if (quote.IsCurrentVersion == true)
                    {
                        var enquiryquoteitems = await enquiryQuoteItemsService.GetEnquiryQuoteItemsByEnquiryId(quote.EnquiryId);

                        for (var j = 0; j < enquiryquoteitems.Count(); j++)
                        {
                            if (enquiryquoteitems.ElementAt(j).LanguageServiceId == item.LanguageServiceId &&
                                enquiryquoteitems.ElementAt(j).SourceLanguageIanaCode == item.SourceLanguageIanacode &&
                                enquiryquoteitems.ElementAt(j).TargetLanguageIanaCode == item.TargetLanguageIanacode)
                            {
                                EnquiryQuoteItem enquiryquoteitem = await enquiriesService.RemoveQuoteItem(enquiryquoteitems.ElementAt(j).Id, loggedInEmployee.Id);
                                break;
                            }
                        }
                    }
                }
            }
            foreach (string quoteitem in allFields)
            {
                if (quoteitem.Contains(",") == true)
                {
                    if (quote.IsCurrentVersion == true)
                    {
                        newEnquiryQuoteItem = await enquiriesService.CreateEnquiryQuoteItems(Convert.ToInt32(allFields[0]), Int32.Parse(quoteitem.Split(",")[0]), quoteitem.Split(",")[1], quoteitem.Split(",")[2], loggedInEmployee.Id, Convert.ToInt32(quoteitem.Split(",")[3]));
                    }
                    QuoteItem quoteItem = await quoteService.CreateQuoteItem(Convert.ToInt32(allFields[1]), Convert.ToByte((quoteitem.Split(",")[0])), quoteitem.Split(",")[1], quoteitem.Split(",")[2], 0,
                                                                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                0, "", 0, loggedInEmployee.Id, 0,
                                                                                0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: Convert.ToInt32(quoteitem.Split(",")[3]));
                }
            }


            //JobOrder newRequest = null;
            //newRequest = await enquiriesService.CreateEnquiry(model);
            if (newEnquiryQuoteItem == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Enquiry quote items.");
                return Ok(MessagsString);
            }

            return Ok($"Success$Enquiry quote items created successfully");
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEnquiryQuoteItems()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            var idlist = "";

            EnquiryQuoteItem newEnquiryQuoteItem = null;

            foreach (string quoteitem in allFields)
            {
                if (quoteitem.Contains(",") == true)
                {
                    newEnquiryQuoteItem = await enquiriesService.CreateEnquiryQuoteItems(Convert.ToInt32(allFields[0]), Int32.Parse(quoteitem.Split(",")[0]), quoteitem.Split(",")[1], quoteitem.Split(",")[2], loggedInEmployee.Id);

                    if (idlist == "")
                    {
                        idlist = newEnquiryQuoteItem.Id.ToString();
                    }
                    else
                    {
                        idlist += "," + newEnquiryQuoteItem.Id.ToString();
                    }
                }
            }


            //JobOrder newRequest = null;
            //newRequest = await enquiriesService.CreateEnquiry(model);
            if (newEnquiryQuoteItem == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Enquiry quote items.");
                return Ok(MessagsString);
            }

            return Ok(idlist + "$Success$Enquiry quote items created successfully");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKeyInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            //DateTime deadlineDate = timeZonesService.GetCurrentGMT().AddDays(5);
            DateTime? deadlineDate = null;
            if (allFields[3] != "" && allFields[3].StartsWith("d") == false)
            {
                deadlineDate = Convert.ToDateTime(allFields[3]);
            }

            DateTime enquirydeadlineDate = timeZonesService.GetCurrentGMT().AddDays(2);
            if (allFields[4] != "" && allFields[4].StartsWith("d") == false)
            {
                enquirydeadlineDate = Convert.ToDateTime(allFields[4]);
            }

            //Enquiry enquiry = await enquiriesService.UpdateKeyInformation(Convert.ToInt32(allFields[0]), Convert.ToByte(allFields[1]), allFields[2], deadlineDate, LoggedInEmployee.Id, enquirydeadlineDate, Convert.ToInt32(allFields[3]));
            Enquiry enquiry = await enquiriesService.UpdateKeyInformation(Convert.ToInt32(allFields[0]), Convert.ToByte(allFields[1]), allFields[2],  LoggedInEmployee.Id, deadlineDate, enquirydeadlineDate, null);


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOptionalInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            Enquiry enquiry = await enquiriesService.UpdateOptionalInformation(Convert.ToInt32(allFields[0]), allFields[1], allFields[2], allFields[3], LoggedInEmployee.Id, Convert.ToBoolean(allFields[4]));


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEnquiryQuoteItem()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;


            //EnquiryQuoteItem enquiryquoteitem = await enquiriesService.RemoveQuoteItem(Int32.Parse(stringToProcess), LoggedInEmployee.Id);
            QuoteItem quoteitem = await quoteitemService.RemoveQuoteItem(Convert.ToInt32(stringToProcess), LoggedInEmployee.Id);

            var quote = await quoteService.GetQuoteById(quoteitem.QuoteId);
            if (quote.IsCurrentVersion == true)
            {
                var enquiryquoteitems = await enquiryQuoteItemsService.GetEnquiryQuoteItemsByEnquiryId(quote.EnquiryId);

                for (var j = 0; j < enquiryquoteitems.Count(); j++)
                {
                    if (enquiryquoteitems.ElementAt(j).LanguageServiceId == quoteitem.LanguageServiceId &&
                        enquiryquoteitems.ElementAt(j).SourceLanguageIanaCode == quoteitem.SourceLanguageIanacode &&
                        enquiryquoteitems.ElementAt(j).TargetLanguageIanaCode == quoteitem.TargetLanguageIanacode)
                    {
                        EnquiryQuoteItem enquiryquoteitem = await enquiriesService.RemoveQuoteItem(enquiryquoteitems.ElementAt(j).Id, LoggedInEmployee.Id);
                        break;
                    }
                }

            }


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCurrentVersion()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");

            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlDataAdapter QuoteDataAdapter = new SqlDataAdapter();
            QuoteDataAdapter.UpdateCommand = new SqlCommand("procUpdateQuoteCurrentVersion", SQLConn);

            QuoteDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_QUOTE_ID", SqlDbType.Int)).Value = Convert.ToInt32(allFields[0]);
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_ENQUIRY_ID", SqlDbType.Int)).Value = Convert.ToInt32(allFields[1]);

            SqlParameter ReturnValParam = QuoteDataAdapter.UpdateCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                QuoteDataAdapter.UpdateCommand.Connection.Open();
                QuoteDataAdapter.UpdateCommand.ExecuteNonQuery();

                if ((int)(QuoteDataAdapter.UpdateCommand.Parameters["@RowCount"].Value) < 1)
                    throw new Exception("Marking quote as current version was not successful.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to mark quote as curent version.");
            }
            finally
            {
                try
                {
                    // Clean up
                    QuoteDataAdapter.UpdateCommand.Connection.Close();
                    QuoteDataAdapter.Dispose();
                }
                catch (Exception SE)
                {
                    throw new Exception("SQL exception: " + SE.Message);
                }
            }

            //return RedirectToAction("Index", Convert.ToInt32(allFields[1]));
            // Response.Redirect("Enquiry?id=" + allFields[1]);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SendEnquiry()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var globalVars = new GlobalVariables();
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(globalVars);

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");
            var enquiryID = Convert.ToInt32(allFields[0]);
            ViewModels.Enquiries.EnquiriesViewModel ThisEnquiry = await enquiriesService.GetViewModelById(enquiryID);
            var contact = await contactsService.GetById(ThisEnquiry.ContactId);
            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var CurrentOpsOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.OperationsLead);
            Employee CurrentOpsOwnerDetails = new Employee();
            if (CurrentOpsOwner != null)
            {
                CurrentOpsOwnerDetails = await employeesService.GetEmployeeById(CurrentOpsOwner.EmployeeId);
            }
            var CurrentClientIntroOwner = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.ClientIntroLead);
            Employee CurrentClientIntroOwnerDetails = new Employee();
            if (CurrentClientIntroOwner != null)
            {
                CurrentClientIntroOwnerDetails = await employeesService.GetEmployeeById(CurrentClientIntroOwner.EmployeeId);
            }
            var CurrentSalesOwnersDetails = new Enumerations.EmployeeOwnerships[]
            {
                Enumerations.EmployeeOwnerships.SalesNewBusinessLead,
                Enumerations.EmployeeOwnerships.SalesAccountManagerLead
            };
            var CurrentSalesOwners = await employeeOwnershipsLogic.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(org.Id, Enumerations.DataObjectTypes.Org, CurrentSalesOwnersDetails, null, true);
            var sendTo = allFields[1];
            var currentQuote = await quoteService.GetQuoteById(Convert.ToInt32(allFields[2]));
            var status = 0;
            if (sendTo == "Ops")
            {
                status = 6;
            }
            if (sendTo == "Sales")
            {
                status = 5;
            }
            if (sendTo == "Pending")
            {



                if (ThisEnquiry.IsFinalised == false)
                {
                    if (ThisEnquiry.Status != 1)
                    {
                        {
                            var withBlock = ThisEnquiry;
                            await enquiriesService.UpdateStatus(enquiryID, 1, ThisEnquiry.ContactId, LoggedInEmployee.Id);


                            string EmailRecipients = "";
                            string CCEmailRecipients = globalVars.InternalDirectorsRecipientAddress;
                            if (CurrentOpsOwner != null)
                            {
                                EmailRecipients = CurrentOpsOwnerDetails.EmailAddress;
                                if (CurrentOpsOwnerDetails.IsTeamManager == false)
                                {
                                    if (CurrentOpsOwnerDetails.Manager != null)
                                    {
                                        var CurrentOpsOwnerManagerDetails = await employeesService.GetEmployeeById(Convert.ToInt32(CurrentOpsOwnerDetails.Manager));
                                        EmailRecipients = EmailRecipients + "," + CurrentOpsOwnerManagerDetails.EmailAddress;
                                    }
                                }
                            }
                            else if (CurrentClientIntroOwner != null)
                            {
                                EmailRecipients = CurrentClientIntroOwnerDetails.EmailAddress;
                                if (CurrentClientIntroOwnerDetails.IsTeamManager == false)
                                {
                                    if (CurrentClientIntroOwnerDetails != null)
                                    {
                                        var CurrentClientIntroOwnerManagerDetails = await employeesService.GetEmployeeById(Convert.ToInt32(CurrentClientIntroOwnerDetails.Manager));
                                        EmailRecipients = EmailRecipients + "," + CurrentClientIntroOwnerManagerDetails.EmailAddress;
                                    }
                                }
                            }
                            else
                                EmailRecipients = "GLSOperationsTeamManagers@translateplus.com, T&POperationsTeamManagers@translateplus.com";

                            CCEmailRecipients = CCEmailRecipients + ", GLSOperationsManagement@translateplus.com, T&POperationsManagement@translateplus.com, Elena.Raynova@translateplus.com, Nikolay.nistorov@translateplus.com";
                            var CurrentSalesDepartmentManager = await employeesService.CurrentSalesDepartmentManager();
                            if (CurrentSalesDepartmentManager != null)
                                CCEmailRecipients = CCEmailRecipients + "," + CurrentSalesDepartmentManager.EmailAddress;
                            if (CurrentSalesOwners != null)
                            {
                                foreach (var data in CurrentSalesOwners)
                                {
                                    var SalesOwner = await employeesService.GetEmployeeById(data.EmployeeId);
                                    CCEmailRecipients = CCEmailRecipients + "," + SalesOwner.EmailAddress;
                                }
                            }


                            string EmailBody = "";
                            var Lang = CultureInfo.CreateSpecificCulture(currentQuote.LangIanacode).NumberFormat; ;
                            if (currentQuote.LangIanacode == "en")
                            {
                                Lang = CultureInfo.CreateSpecificCulture("en-GB").NumberFormat;
                            }
                            System.Globalization.NumberFormatInfo LocalNumberFormatProvider = Lang;
                            var EngLang = CultureInfo.CreateSpecificCulture("en-GB").NumberFormat;
                            System.Globalization.NumberFormatInfo GBPNumberFormatProvider = EngLang;
                            var GBPCurrency = await currencyservice.GetById(4);// GBP
                            var ConvertedAmount = 0.0;
                            if (currentQuote.QuoteCurrencyId == 4)
                            {
                                if (currentQuote.OverallChargeToClient >= 20000)
                                    EmailBody = string.Format("<p>A quote to the value of {0} {1} has just been sent to <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={2}\">{3}</a> at <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id{4}\">{5}</a>.<br /><br /><b>" + "<font color=\"green\">As this is over £20k, please consider whether specific operational preparations will be required and/or have " + "been promised as part of the offer, such as the creation of a glossary, suggesting a project start-up meeting with the client, and the " + "appropriate allocation of internal resources.</font></b></p>", GBPCurrency.Symbol, currentQuote.OverallChargeToClient.GetValueOrDefault().ToString("N2", LocalNumberFormatProvider), contact.Id, contact.Name, org.Id, org.OrgName);
                            }
                            else if (GBPCurrency != null)
                            {
                                if (currentQuote.QuoteCurrencyId == 5)
                                {
                                    var USDCurrency = await currencyservice.GetById(5);
                                    if (USDCurrency != null)
                                    {
                                        ConvertedAmount = (double)exchangeService.Convert((short)USDCurrency.Id, (short)GBPCurrency.Id, (decimal)currentQuote.OverallChargeToClient);

                                        if (ConvertedAmount >= 20000)
                                            EmailBody = string.Format("<p>A quote to the value of {0} {1} ({2} {3}) has just been sent to <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={4}\">{5}</a> at <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id{6}\">{7}</a>.<br /><br /><b>" + "<font color=\"green\">As this is over £20k, please consider whether specific operational preparations will be required and/or have " + "been promised as part of the offer, such as the creation of a glossary, suggesting a project start-up meeting with the client, and the " + "appropriate allocation of internal resources.</font></b></p>", USDCurrency.Symbol, currentQuote.OverallChargeToClient.GetValueOrDefault().ToString("N2", LocalNumberFormatProvider), GBPCurrency.Symbol, ConvertedAmount.ToString("N2", GBPNumberFormatProvider), contact.Id, contact.Name, org.Id, org.OrgName);
                                    }
                                }

                                if (currentQuote.QuoteCurrencyId == 6)
                                {
                                    var EURCurrency = await currencyservice.GetById(6);
                                    if (EURCurrency != null)
                                    {
                                        ConvertedAmount = (double)exchangeService.Convert((short)EURCurrency.Id, (short)GBPCurrency.Id, (decimal)currentQuote.OverallChargeToClient);

                                        if (ConvertedAmount >= 20000)
                                            EmailBody = string.Format("<p>A quote to the value of {0} {1} ({2} {3}) has just been sent to <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={4}\">{5}</a> at <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id{6}\">{7}</a>.<br /><br /><b>" + "<font color=\"green\">As this is over £20k, please consider whether specific operational preparations will be required and/or have " + "been promised as part of the offer, such as the creation of a glossary, suggesting a project start-up meeting with the client, and the " + "appropriate allocation of internal resources.</font></b></p>", EURCurrency.Symbol, currentQuote.OverallChargeToClient.GetValueOrDefault().ToString("N2", LocalNumberFormatProvider), GBPCurrency.Symbol, ConvertedAmount.ToString("N2", GBPNumberFormatProvider), contact.Id, contact.Name, org.Id, org.OrgName);
                                    }
                                }

                                if (currentQuote.QuoteCurrencyId == 8)
                                {
                                    var DKKCurrency = await currencyservice.GetById(8);
                                    if (DKKCurrency != null)
                                    {
                                        ConvertedAmount = (double)exchangeService.Convert((short)DKKCurrency.Id, (short)GBPCurrency.Id, (decimal)currentQuote.OverallChargeToClient);

                                        if (ConvertedAmount >= 20000)
                                            EmailBody = string.Format("<p>A quote to the value of {0} {1} ({2} {3}) has just been sent to <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={4}\">{5}</a> at <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id{6}\">{7}</a>.<br /><br /><b>" + "<font color=\"green\">As this is over £20k, please consider whether specific operational preparations will be required and/or have " + "been promised as part of the offer, such as the creation of a glossary, suggesting a project start-up meeting with the client, and the " + "appropriate allocation of internal resources.</font></b></p>", DKKCurrency.Symbol, currentQuote.OverallChargeToClient.GetValueOrDefault().ToString("N2", LocalNumberFormatProvider), GBPCurrency.Symbol, ConvertedAmount.ToString("N2", GBPNumberFormatProvider), contact.Id, contact.Name, org.Id, org.OrgName);
                                    }
                                }

                                if (currentQuote.QuoteCurrencyId == 10)
                                {
                                    var SEKCurrency = await currencyservice.GetById(10);
                                    if (SEKCurrency != null)
                                    {
                                        ConvertedAmount = (double)exchangeService.Convert(SEKCurrency.Id, GBPCurrency.Id, (decimal)currentQuote.OverallChargeToClient);

                                        if (ConvertedAmount >= 20000)
                                            EmailBody = string.Format("<p>A quote to the value of {0} {1} ({2} {3}) has just been sent to <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid={4}\">{5}</a> at <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id{6}\">{7}</a>.<br /><br /><b>" + "<font color=\"green\">As this is over £20k, please consider whether specific operational preparations will be required and/or have " + "been promised as part of the offer, such as the creation of a glossary, suggesting a project start-up meeting with the client, and the " + "appropriate allocation of internal resources.</font></b></p>", SEKCurrency.Symbol, currentQuote.OverallChargeToClient.GetValueOrDefault().ToString("N2", LocalNumberFormatProvider), GBPCurrency.Symbol, ConvertedAmount.ToString("N2", GBPNumberFormatProvider), contact.Id, contact.Name, org.Id, org.OrgName);
                                    }
                                }
                            }

                            if ((currentQuote.QuoteCurrencyId == 4 & currentQuote.OverallChargeToClient >= 20000) | (currentQuote.QuoteCurrencyId != 4 & ConvertedAmount >= 20000))
                                // send email
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + ThisEnquiry.Id.ToString() + " (for organisation " + org.OrgName + ") has been marked as Pending (awaiting decision) has a value over £20,000", EmailBody, true, false, null/* TODO Change to default(_) if this is not a reference type */, CCEmailRecipients);
                        }
                        return Ok("Success$Enquiry successfully marked as sent to client (marked as pending).");
                    }
                    if (ThisEnquiry.Status == 1)
                    {
                        {
                            await enquiriesService.UpdateStatus(enquiryID, 0, ThisEnquiry.ContactId, LoggedInEmployee.Id);
                        }
                        return Ok("Success$Enquiry successfully marked as draft.");
                    }
                }


            }
            Enquiry enquiry = await enquiriesService.UpdateStatus(enquiryID, Convert.ToByte(status), ThisEnquiry.ContactId, LoggedInEmployee.Id);
            if (enquiry != null)
            {
                if (status == 6)
                {
                    return Ok("Success$Enquiry successfully marked as sent to Ops. Please make sure to contact the Ops team, so they are aware.");
                }
                if (status == 5)
                {
                    return Ok("Success$Enquiry successfully marked as sent to sales. Please make sure to contact the sales team, so they are aware.");
                }
            }

            return Ok("Error$Something went wrong.");
        }

        [HttpPost]
        public async Task<string> ApproveOrRejectEnquiry()
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
                var quoteDetails = await quoteService.GetQuoteById(Convert.ToInt32(allFields[0]));
                var quoteItemDetails = await quoteService.GetAllQuoteItems(Convert.ToInt32(allFields[0]));
                var contactDetails = await contactsService.GetContactDetails(enquiryDetails.ContactId);
                var orgDetails = await orgsService.GetOrgDetails(contactDetails.OrgId);
                var orgGroupDetails = await orgGroupsLogic.GetOrgGroupDetails(orgDetails.OrgGroupId.GetValueOrDefault());
                if (Convert.ToInt32(allFields[2]) == 0) { model.Status = 2; }
                if (Convert.ToInt32(allFields[2]) == 1) { model.Status = 3; }


                var extranetUserFullName = contactDetails.Name;
                //if (extranetUser.DataObjectTypeId == (byte)Enumerations.DataObjectTypes.Contact) { extranetUserFullName = contactDetails.Name; }
                //else if (extranetUser.DataObjectTypeId == (byte)Enumerations.DataObjectTypes.LinguisticSupplier)
                //{
                //    var linguistDetails = await linguistService.GetById(extranetUser.DataObjectTypeId);
                //    if (linguistDetails.SupplierTypeId == 1)
                //    {
                //        extranetUserFullName = linguistDetails.MainContactFirstName + " " + linguistDetails.MainContactSurname;
                //    }
                //    else if (linguistDetails.SupplierStatusId == 2 || linguistDetails.SupplierStatusId == 3 || linguistDetails.SupplierSourceId == 10)
                //    {
                //        extranetUserFullName = linguistDetails.AgencyOrTeamName;
                //    }
                //    else
                //    {
                //        extranetUserFullName = linguistDetails.MainContactFirstName + " " + linguistDetails.MainContactSurname;
                //    }
                //}

                var currencyPrefix = await currencyservice.GetById(quoteDetails.QuoteCurrencyId);
                model.DecisionMadeByContactId = contactDetails.Id;
                model.DecisionReasonId = Convert.ToByte(allFields[3]);
                model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
                string PONumber = "";
                var JONumber = 0;
                if (allFields[4] != "")
                {
                    model.AdditionalDetails = allFields[4];
                }

                if (Convert.ToInt32(allFields[2]) == 1)
                {
                    if (allFields[5] != "")
                    {
                        JONumber = Convert.ToInt32(allFields[5]);
                    }
                }
                if (JONumber != 0)
                {
                    await enquiriesService.UpdateJobOrder(model.Id, JONumber, LoggedInEmployee.Id);
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
                            CustomerSpecificField1Value = quoteDetails.CustomerSpecificField1Value == null ? "": quoteDetails.CustomerSpecificField1Value.ToString();
                            CustomerSpecificField2Value = quoteDetails.CustomerSpecificField2Value == null ? "" : quoteDetails.CustomerSpecificField2Value.ToString();
                            CustomerSpecificField3Value = quoteDetails.CustomerSpecificField3Value == null ? "" : quoteDetails.CustomerSpecificField3Value.ToString();
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
                            var result = await quoteService.UpdateQuote(quoteDetails.Id, quoteDetails.EnquiryId, true, quoteDetails.QuoteCurrencyId, quoteDetails.LangIanacode, quoteDetails.Title, quoteDetails.QuoteFileName, quoteDetails.InternalNotes, quoteDetails.QuoteDate, quoteDetails.QuoteOrgName, quoteDetails.QuoteAddress1, quoteDetails.QuoteAddress2, quoteDetails.QuoteAddress3, quoteDetails.QuoteAddress4, quoteDetails.QuoteCountyOrState, quoteDetails.QuotePostcodeOrZip, (short)Country, quoteDetails.AddresseeSalutationName, quoteDetails.OpeningSectionText, quoteDetails.ClosingSectionText, quoteDetails.TimelineUnit, (decimal)quoteDetails.TimelineValue, quoteDetails.WordCountPresentationOption, quoteDetails.ShowInterpretingDurationInBreakdown, quoteDetails.ShowWorkDurationInBreakdown, quoteDetails.ShowPagesOrSlidesInBreakdown, quoteDetails.ShowNumberOfCharactersInBreakdown, quoteDetails.ShowNumberOfDocumentsInBreakdown, SalesContactEmployee.Id, CustomerSpecificField1Value, quoteDetails.CustomerSpecificField2Value, quoteDetails.CustomerSpecificField3Value, quoteDetails.CustomerSpecificField4Value, quoteDetails.ShowCustomerSpecificField1Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField2Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField3Value.GetValueOrDefault(), quoteDetails.ShowCustomerSpecificField4Value.GetValueOrDefault(), PONumber, DateTime.Now, globalVariables.iplusEmployeeID);

                            if (result > 0)
                            {
                                await quoteService.MarkQuoteCurrentVersion(quoteDetails.Id, quoteDetails.EnquiryId);
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
                        var CurrentOpsOwner = await employeeOwnershipsLogic.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(orgDetails.Id, Enumerations.DataObjectTypes.Org, empOpsOwnerships, timeZone, true);
                        var CurrentSalesOwners = await employeeOwnershipsLogic.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(orgDetails.Id, Enumerations.DataObjectTypes.Org, empSalesOwnerships, timeZone, true);
                        var CurrentClientIntroOwner = await employeeOwnershipsLogic.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(orgDetails.Id, Enumerations.DataObjectTypes.Org, clientIntroOwnerships, timeZone, true);

                        if (CurrentOpsOwner.Any())
                        {
                            ProjectManagerID = CurrentOpsOwner.FirstOrDefault().EmployeeId;
                        }
                        else
                        {
                            ProjectManagerID = globalVariables.TBAEmployeeID;// use "TBA System" as project manager if no ops lead specified
                        }
                        string ReadableStatusString = "";
                        if (Status == 2)
                        {
                            ReadableStatusString = "rejected";
                        }
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

                                SalesOwner = await employeesService.GetEmployeeById(CurrentSalesOwners.FirstOrDefault().EmployeeId);
                                if (EmailRecipients == "")
                                    EmailRecipients = SalesOwner.EmailAddress;
                                else
                                    EmailRecipients += "," + SalesOwner.EmailAddress;
                            }
                        }
                        if (CurrentOpsOwner.Any())
                        {
                            Employee OpsOwner = new Employee();
                            OpsOwner = await employeesService.GetEmployeeById(CurrentOpsOwner.FirstOrDefault().EmployeeId);


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
                                    OpsOwnerManager = await employeesService.GetEmployeeById(Convert.ToInt32(OpsOwner.Manager));
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
                        {
                            EmailRecipients = "Enquiries@translateplus.com";
                        }




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

                                SubmittedJobOrder = await jobOrderService.CreateNewOrder<Order>(contactDetails.Id, (short)ProjectManagerID, (byte)OrderChannelID, withBlock.JobName, withBlock.ExternalNotes, InternalNotes, PONumber, CustomerSpecificField1Value, CustomerSpecificField2Value, CustomerSpecificField3Value, quoteDetails.CustomerSpecificField4Value, Deadline, Deadline.Hour, Deadline.Minute, quoteDetails.QuoteCurrencyId, 0, false, false, false, SetUpBy.Id, false, false, NetworkFolderTemplate, InvoicingNotes, "", "", false, 0, withBlock.PrintingProject.GetValueOrDefault(), 0, 0, 0, 0, false, quoteDetails.DiscountId,quoteDetails.SurchargeId,quoteDetails.DiscountAmount,quoteDetails.SurchargeAmount,quoteDetails.OverallChargeToClient,quoteDetails.SubTotalOverallChargeToClient);

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
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, SetUpBy.Id, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);
                                        else
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, SetUpBy.Id, withBlock1.SupplierWordCountNew.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand1.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand2.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand3.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand4.GetValueOrDefault(), withBlock1.SupplierWordCountExact.GetValueOrDefault(), withBlock1.SupplierWordCountRepetitions.GetValueOrDefault(), withBlock1.SupplierWordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), FromExternalServer: true, LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);

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

                                DateTime Deadline = enquiryDetails.DeadlineRequestedByClient.GetValueOrDefault();
                                DateTime CurrentDateTime = GeneralUtils.GetCurrentGMT();
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

                                UpdatingJobOrder = true;
                                SubmittedJobOrder = await jobOrderService.GetById(enquiryDetails.WentAheadAsJobOrderId.GetValueOrDefault());
                                var JobItems = await jobOrderService.GetAllJobItems(enquiryDetails.WentAheadAsJobOrderId.GetValueOrDefault());
                                CustomerSpecificField1Value = quoteDetails.CustomerSpecificField1Value ==null? "": quoteDetails.CustomerSpecificField1Value.ToString();

                                int EarlyInvoicingEmpID = 0;
                                if (SubmittedJobOrder.EarlyInvoiceByEmpId > 0)
                                {
                                    EarlyInvoicingEmpID = (int)SubmittedJobOrder.EarlyInvoiceByEmpId;
                                }
                                await jobOrderService.UpdateOrder(withBlock.WentAheadAsJobOrderId.GetValueOrDefault(), SubmittedJobOrder.ProjectManagerEmployeeId, withBlock.OrderChannelId, withBlock.JobName, withBlock.ExternalNotes, withBlock.InternalNotes, PONumber, CustomerSpecificField1Value, quoteDetails.CustomerSpecificField2Value.ToString(), quoteDetails.CustomerSpecificField3Value.ToString(), quoteDetails.CustomerSpecificField4Value.ToString(), Convert.ToDateTime(Deadline).Date, Convert.ToDateTime(Deadline).Hour, Convert.ToDateTime(Deadline).Minute, false, quoteDetails.QuoteCurrencyId, 0, 0, false, false, false, InvoicingNotes, SubmittedJobOrder.EarlyInvoiceDateTime.GetValueOrDefault(), EarlyInvoicingEmpID, SubmittedJobOrder.OverdueReasonId.GetValueOrDefault(), SubmittedJobOrder.OverdueComment);

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
                                            SubmittedJobItem = await itemService.CreateItem(SubmittedJobOrder.Id, true, withBlock1.LanguageServiceId, withBlock1.SourceLanguageIanacode, withBlock1.TargetLanguageIanacode, withBlock1.WordCountNew.GetValueOrDefault(), withBlock1.WordCountFuzzyBand1.GetValueOrDefault(), withBlock1.WordCountFuzzyBand2.GetValueOrDefault(), withBlock1.WordCountFuzzyBand3.GetValueOrDefault(), withBlock1.WordCountFuzzyBand4.GetValueOrDefault(), withBlock1.WordCountExact.GetValueOrDefault(), withBlock1.WordCountRepetitions.GetValueOrDefault(), withBlock1.WordCountPerfectMatches.GetValueOrDefault(), Enumerations.TranslationMemoryRequiredValues.NotApplicable, withBlock1.Pages.GetValueOrDefault(), withBlock1.Characters.GetValueOrDefault(), withBlock1.Documents.GetValueOrDefault(), withBlock1.InterpretingExpectedDurationMinutes.GetValueOrDefault(), withBlock1.InterpretingLocationOrgName, withBlock1.InterpretingLocationAddress1, withBlock1.InterpretingLocationAddress2, withBlock1.InterpretingLocationAddress3, withBlock1.InterpretingLocationAddress4, withBlock1.InterpretingLocationCountyOrState, withBlock1.InterpretingLocationPostcodeOrZip, (byte?)Country, withBlock1.AudioMinutes.GetValueOrDefault(), withBlock1.WorkMinutes.GetValueOrDefault(), DateTime.MinValue, DateTime.MinValue, SubmittedJobOrder.OverallDeliveryDeadline, "", "", SupplierIsCientReviewer, 0, withBlock1.ChargeToClient.GetValueOrDefault(), 0, 0, DateTime.MinValue, 0, SetUpBy.Id, withBlock1.SupplierWordCountNew.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand1.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand2.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand3.GetValueOrDefault(), withBlock1.SupplierWordCountFuzzyBand4.GetValueOrDefault(), withBlock1.SupplierWordCountExact.GetValueOrDefault(), withBlock1.SupplierWordCountRepetitions.GetValueOrDefault(), withBlock1.SupplierWordCountPerfectMatches.GetValueOrDefault(), false, WordCountClientSpecific: withBlock1.WordCountClientSpecific.GetValueOrDefault(), SupplierWordCountClientSpecific: withBlock1.SupplierWordCountClientSpecific.GetValueOrDefault(), FromExternalServer: true, LanguageServiceCategoryId: withBlock1.LanguageServiceCategoryId);
                                    }
                                }
                            }

                            // add discount and surcharge to job order if any
                            if (quoteDetails.SurchargeId > 0 && quoteDetails.SurchargeAmount > 0)
                            {
                                QuoteAndOrderDiscountsAndSurcharge CurrentSurcharge = new QuoteAndOrderDiscountsAndSurcharge();
                                CurrentSurcharge = await quoteAndOrderDiscountsAndSurchargesService.GetById(quoteDetails.SurchargeId.GetValueOrDefault());
                                QuoteAndOrderDiscountsAndSurchargesCategory CurrentCategory = new QuoteAndOrderDiscountsAndSurchargesCategory();
                                CurrentCategory = await quoteAndOrderDiscountsAndSurchargesCategoriesService.GetById(CurrentSurcharge.DiscountOrSurchargeCategory);

                                if (SubmittedJobOrder.SurchargeId > 0 && SubmittedJobOrder.SurchargeAmount > 0)
                                {
                                    await quoteAndOrderDiscountsAndSurchargesService.UpdateSurchargeOrDiscount(SubmittedJobOrder.SurchargeId.GetValueOrDefault(), CurrentSurcharge.DiscountOrSurchargeCategory, CurrentSurcharge.Description, CurrentSurcharge.PercentageOrValue, (decimal)SubmittedJobOrder.SurchargeAmount, LoggedInEmployee.Id);
                                }
                                else
                                {
                                    var OrderSurcharge = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(false, CurrentSurcharge.DiscountOrSurchargeCategory, CurrentSurcharge.Description, CurrentSurcharge.PercentageOrValue, (decimal)SubmittedJobOrder.SurchargeAmount, LoggedInEmployee.Id);
                                }
                            }
                            else if (SubmittedJobOrder.SurchargeId > 0 && SubmittedJobOrder.SurchargeAmount > 0)
                                await quoteAndOrderDiscountsAndSurchargesService.RemoveSurchargeOrDiscount(SubmittedJobOrder.SurchargeId.GetValueOrDefault(), LoggedInEmployee.Id);

                            if (quoteDetails.DiscountId > 0 && quoteDetails.DiscountAmount > 0)
                            {
                                QuoteAndOrderDiscountsAndSurcharge CurrentDiscount = new QuoteAndOrderDiscountsAndSurcharge();
                                CurrentDiscount = await quoteAndOrderDiscountsAndSurchargesService.GetById((int)quoteDetails.DiscountId.GetValueOrDefault());
                                QuoteAndOrderDiscountsAndSurchargesCategory CurrentCategory = new QuoteAndOrderDiscountsAndSurchargesCategory();
                                CurrentCategory = await quoteAndOrderDiscountsAndSurchargesCategoriesService.GetById(CurrentDiscount.DiscountOrSurchargeCategory);

                                if (SubmittedJobOrder.DiscountId > 0 && SubmittedJobOrder.DiscountAmount > 0)
                                {
                                    await quoteAndOrderDiscountsAndSurchargesService.UpdateSurchargeOrDiscount(SubmittedJobOrder.DiscountId.GetValueOrDefault(), CurrentDiscount.DiscountOrSurchargeCategory, CurrentDiscount.Description, CurrentDiscount.PercentageOrValue, (decimal)SubmittedJobOrder.DiscountAmount, LoggedInEmployee.Id);
                                }
                                else
                                {
                                    var OrderDiscount = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(true, CurrentDiscount.DiscountOrSurchargeCategory, CurrentDiscount.Description, CurrentDiscount.PercentageOrValue, (decimal)SubmittedJobOrder.DiscountAmount, LoggedInEmployee.Id);
                                }
                            }
                            else if (SubmittedJobOrder.DiscountId > 0 && SubmittedJobOrder.DiscountAmount > 0)
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
                                                        TableRows += "<tr><td><a href=\"http://myplus/Order.aspx?OrderID=" + OrderToCheck.Id.ToString() + "\">" + OrderToCheck.Id.ToString() + " - " + OrderToCheck.JobName + "</a></td><td><a href=\"http://myplus/JobItem.aspx?JobItemID=" + JobItemToCheck.Id.ToString() + "\">" + JobItemToCheck.Id.ToString() + "</a></td><td>" + TargetLang.Name + "</td><td>" + OrderToCheck.SubmittedDateTime.ToString("dddd d MMMM yyyy") + "</td></tr>";
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
                                {
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />The newly created job order for this is: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a>" + ReferenceInfoString + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification:true);
                                }
                                else
                                {
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />The newly created job order for this is: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a>" + ReferenceInfoString + "</p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification:true);
                                }
                            }
                            else
                            {
                                if (withBlock.PrintingProject == true)
                                {
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />This enquiry was unlocked and then approved again, therefore the job order for this is still: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a> which has been updated" + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification:true);
                                }
                                else
                                {
                                    emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients, "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"https://myplusbeta.publicisgroupe.net/enquiry?ID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid==" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + " <br /><br />This enquiry was unlocked and then approved again, therefore the job order for this is still: <a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + SubmittedJobOrder.Id.ToString() + "\">" + SubmittedJobOrder.Id.ToString() + "</a> which has been updated </p>" + MessageBodyCore, true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail,  IsExternalNotification:true);
                                }
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
                            CurrentBrand = await brandService.GetBrandById(1);
                            if (contactDetails != null && contactDetails.Id > 0)
                                CurrentBrand = await brandService.GetBrandById(orgGroupDetails.BrandId);

                            jobOrderService.AnnounceThisOrderCreation(SubmittedJobOrder.Id, CurrentBrand.EmailAddress, miscResourceService.GetMiscResourceByName("QuoteApprovedHeader", "en").Result.StringContent, true, false, "en", QuoteApprovalNotification: true);
                        }
                        else if (Status == 2)
                        {
                            await enquiriesService.ApproveOrRejectEnquiry(model);

                            // for rejections include the current sales manager & Macarena in the notification e-mail
                            var CurrentSalesDepartmentManager = await employeesService.CurrentSalesDepartmentManager();
                            EmailRecipients = EmailRecipients + "," + CurrentSalesDepartmentManager.EmailAddress;





                            // Send an email to notify the reject enquiry
                            if (withBlock.PrintingProject == true)
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients + ",darko.ivanovski@translateplus.com", "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"http://myplus/Enquiry.aspx?EnquiryID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + "<br /><br />This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager. </p>", true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification:true);
                            else
                                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients + ",darko.ivanovski@translateplus.com", "Enquiry " + withBlock.Id.ToString() + " (for organisation " + orgDetails.OrgName + ") has been " + ReadableStatusString + " by " + extranetUserFullName + " in i plus", "<p>" + NoOpsOwnerMsg + "The enquiry <a href=\"http://myplus/Enquiry.aspx?EnquiryID=" + withBlock.Id.ToString() + "\">" + withBlock.Id.ToString() + " - " + quoteDetails.QuoteFileName + "</a> (" + currencyPrefix.Prefix + quoteDetails.OverallChargeToClient.GetValueOrDefault().ToString("N2") + ") has just been " + ReadableStatusString + " by <a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactid=" + contactDetails.Id.ToString() + "\">" + extranetUserFullName + "</a>. " + " <br /><br />The reason provided is: <br /><br /><b>" + DecisionReasonString.Reason + "</b><br/><br/>" + model.AdditionalDetails + "</p>", true, false, null/* TODO Change to default(_) if this is not a reference type */, TeamOpsEmail, IsExternalNotification: true);
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
                    emailService.SendMail("flow plus <flowplus@translateplus.com>", "ArianD@translateplus.com, KavitaJ@translateplus.com, raj.lakhwani@publicismedia.com", "Extranet error occurred", ErrStringForEmail, IsExternalNotification:true);
                    return message = "Error$Something went wrong.";
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
        [HttpPost]
        public async Task<IActionResult> UpdateLogicalInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");
            if (allFields[1] == "") { allFields[1] = null; }
            if (allFields[2] == "") { allFields[2] = null; }
            Enquiry enquiry = await enquiriesService.UpdateLogicalInformation(Convert.ToInt32(allFields[0]), Convert.ToInt32(allFields[1]), Convert.ToDateTime(allFields[2]), LoggedInEmployee.Id);


            return Ok();
        }
        public async Task<IActionResult> CloneQuote()
        {

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            int quoteID = Convert.ToInt32(allFields[0]);
            int EnquiryID = Convert.ToInt32(allFields[1]);
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            Quote quote = await quoteService.GetQuoteById(quoteID);
            Enquiry enquiry = await enquiriesService.GetEnquiryById(EnquiryID);
            Quote submittedQuote = null;


            if (enquiry == null || quote == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while cloning new Quote on the database.");
                return Ok(MessagsString);
            }
            else
            {


                submittedQuote = await quoteService.CreateQuote(enquiry.Id, true, quote.QuoteCurrencyId, quote.LangIanacode,
                                                                quote.Title, quote.QuoteFileName, quote.InternalNotes, timeZonesService.GetCurrentGMT(), quote.QuoteOrgName,
                                                                quote.QuoteAddress1, quote.QuoteAddress2, quote.QuoteAddress3, quote.QuoteAddress4,
                                                                quote.QuoteCountyOrState, quote.QuotePostcodeOrZip.ToString(), quote.QuoteCountryId,
                                                                quote.AddresseeSalutationName, quote.OpeningSectionText, quote.ClosingSectionText,
                                                                quote.TimelineUnit, quote.TimelineValue, quote.WordCountPresentationOption, quote.ShowInterpretingDurationInBreakdown,
                                                                quote.ShowWorkDurationInBreakdown, quote.ShowPagesOrSlidesInBreakdown, quote.ShowNumberOfCharactersInBreakdown, quote.ShowNumberOfDocumentsInBreakdown, quote.SalesContactEmployeeId, quote.CustomerSpecificField1Value,
                                                                quote.CustomerSpecificField2Value, quote.CustomerSpecificField3Value, quote.CustomerSpecificField4Value, quote.ShowCustomerSpecificField1Value.GetValueOrDefault(),
                                                                quote.ShowCustomerSpecificField2Value.GetValueOrDefault(), quote.ShowCustomerSpecificField3Value.GetValueOrDefault(), quote.ShowCustomerSpecificField4Value.GetValueOrDefault(), quote.ClientPonumber, loggedInEmployee.Id,
                                                                timeZonesService.GetCurrentGMT(), PrintingProject: quote.PrintingProject.GetValueOrDefault());
            }
            if (quote.SurchargeId > 0)
            {
                var thisSurcharge = await quoteAndOrderDiscountsAndSurchargesService.GetById((int)quote.SurchargeId);
                if (thisSurcharge != null)
                {
                    var surcharge = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(false, thisSurcharge.DiscountOrSurchargeCategory, thisSurcharge.Description, thisSurcharge.PercentageOrValue, thisSurcharge.DiscountOrSurchargeAmount, loggedInEmployee.Id);
                    await quoteService.UpdateSurchargeID(submittedQuote.Id, surcharge.Id, null);
                }
            }
            if (quote.DiscountId > 0)
            {
                var ThisDisount = await quoteAndOrderDiscountsAndSurchargesService.GetById((int)quote.DiscountId);
                if (ThisDisount != null)
                {
                    var discount = await quoteAndOrderDiscountsAndSurchargesService.CreateSurchargeOrDiscount(true, ThisDisount.DiscountOrSurchargeCategory, ThisDisount.Description, ThisDisount.PercentageOrValue, ThisDisount.DiscountOrSurchargeAmount, loggedInEmployee.Id);
                    await quoteService.UpdateDiscountID(submittedQuote.Id, discount.Id, null);
                }
            }

            //List<EnquiryQuoteItem> eqi = await enquiryQuoteItemsService.GetEnquiryQuoteItemsByEnquiryId(EnquiryID);
            List<QuoteItem> items = await quoteService.GetAllQuoteItems(quoteID);

            foreach (QuoteItem quoteitem in items)
            {

                await quoteService.CreateQuoteItem(submittedQuote.Id, quoteitem.LanguageServiceId, quoteitem.SourceLanguageIanacode, quoteitem.TargetLanguageIanacode, quoteitem.WordCountNew.GetValueOrDefault(0),
                                                                           quoteitem.WordCountFuzzyBand1.GetValueOrDefault(0), quoteitem.WordCountFuzzyBand2.GetValueOrDefault(0), quoteitem.WordCountFuzzyBand3.GetValueOrDefault(0), quoteitem.WordCountFuzzyBand4.GetValueOrDefault(0), quoteitem.WordCountExact.GetValueOrDefault(0), quoteitem.WordCountRepetitions.GetValueOrDefault(0), quoteitem.WordCountPerfectMatches.GetValueOrDefault(0), quoteitem.Pages.GetValueOrDefault(0), quoteitem.Characters.GetValueOrDefault(0), quoteitem.Documents.GetValueOrDefault(0), quoteitem.InterpretingExpectedDurationMinutes.GetValueOrDefault(0), quoteitem.InterpretingLocationOrgName, quoteitem.InterpretingLocationAddress1, quoteitem.InterpretingLocationAddress2, quoteitem.InterpretingLocationAddress3, quoteitem.InterpretingLocationAddress4, quoteitem.InterpretingLocationCountyOrState, quoteitem.InterpretingLocationPostcodeOrZip, quoteitem.InterpretingLocationCountryId, quoteitem.AudioMinutes.GetValueOrDefault(),
                                                                           quoteitem.WorkMinutes.GetValueOrDefault(0), quoteitem.ExternalNotes, quoteitem.ChargeToClient, loggedInEmployee.Id, quoteitem.SupplierWordCountNew.GetValueOrDefault(0),
                                                                           quoteitem.SupplierWordCountFuzzyBand1.GetValueOrDefault(0), quoteitem.SupplierWordCountFuzzyBand2.GetValueOrDefault(0), quoteitem.SupplierWordCountFuzzyBand3.GetValueOrDefault(0), quoteitem.SupplierWordCountFuzzyBand4.GetValueOrDefault(0), quoteitem.SupplierWordCountExact.GetValueOrDefault(0), quoteitem.SupplierWordCountRepetitions.GetValueOrDefault(0), quoteitem.SupplierWordCountPerfectMatches.GetValueOrDefault(0), quoteitem.WordCountClientSpecific.GetValueOrDefault(0), quoteitem.SupplierWordCountClientSpecific.GetValueOrDefault(0), quoteitem.LanguageServiceCategoryId == null ? 0 : (int)quoteitem.LanguageServiceCategoryId);

            }


            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlDataAdapter QuoteDataAdapter = new SqlDataAdapter();
            QuoteDataAdapter.UpdateCommand = new SqlCommand("procUpdateQuoteCurrentVersion", SQLConn);

            QuoteDataAdapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_QUOTE_ID", SqlDbType.Int)).Value = submittedQuote.Id;
            QuoteDataAdapter.UpdateCommand.Parameters.Add(new SqlParameter("@P_ENQUIRY_ID", SqlDbType.Int)).Value = EnquiryID;

            SqlParameter ReturnValParam = QuoteDataAdapter.UpdateCommand.Parameters.Add("@RowCount", SqlDbType.Int);
            ReturnValParam.Direction = ParameterDirection.ReturnValue;

            try
            {
                QuoteDataAdapter.UpdateCommand.Connection.Open();
                QuoteDataAdapter.UpdateCommand.ExecuteNonQuery();

                if ((int)(QuoteDataAdapter.UpdateCommand.Parameters["@RowCount"].Value) < 1)
                    throw new Exception("Marking quote as current version was not successful.");
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to mark quote as curent version.");
            }
            finally
            {
                try
                {
                    // Clean up
                    QuoteDataAdapter.UpdateCommand.Connection.Close();
                    QuoteDataAdapter.Dispose();
                }
                catch (Exception SE)
                {
                    throw new Exception("SQL exception: " + SE.Message);
                }
            }

            return Ok($"Success$Quote cloned successfully and marked as current version.${submittedQuote.Id}");

            //return RedirectToAction("Index", "Quote", submittedQuote.Id);
            //return Redirect("../Quote?id=" + submittedQuote.Id);
            // return View("Views/Quote/Index.cshtml", submittedQuote.Id);
        }
    }
}
