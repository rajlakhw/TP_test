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
using ViewModels.Common;
using Newtonsoft.Json;
using Data.Repositories;
using ViewModels.Quotes;
using Microsoft.EntityFrameworkCore;

namespace SmartAdmin.WebUI.Controllers
{
    public class QuoteController : Controller
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
        private readonly ITPCurrenciesLogic currenciesService;
        private readonly ITPFileSystemService fileSystemService;
        private readonly ITPQuoteItemsLogic quoteitemService;
        private readonly IConfiguration Configuration;
        private readonly ITPWordCountBreakdownBatch wordCountBreakdownBatchService;
        private readonly ITPLanguageLogic languageLogicService;
        private readonly IEmailUtilsService emailService;
        private readonly IQuoteAndOrderDiscountsAndSurchargesCategories discountsurchargeService;
        private readonly IQuoteAndOrderDiscountsAndSurcharges discountorsurchargeService;
        private readonly IRepository<QuoteAndOrderDiscountsAndSurcharge> discountsurchargeRepo;
        private readonly ITPLocalCurrencyInfo currencyinfoservice;

        public QuoteController(ITPEnquiriesService enquiriesService,
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
            ITPCurrenciesLogic currenciesService,
            ITPFileSystemService fileSystemService,
            ITPQuoteItemsLogic quoteitemService,
            IConfiguration _configuration,
            ITPWordCountBreakdownBatch wordCountBreakdownBatchService,
            ITPLanguageLogic languageLogicService,
            IEmailUtilsService emailUtilsService,
            IQuoteAndOrderDiscountsAndSurchargesCategories discountsurchargeService,
            IQuoteAndOrderDiscountsAndSurcharges discountorsurchargeService,
            IRepository<QuoteAndOrderDiscountsAndSurcharge> discountsurchargeRepo,
            ITPLocalCurrencyInfo currencyinfoservice
            )
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
            this.currenciesService = currenciesService;
            this.fileSystemService = fileSystemService;
            this.quoteitemService = quoteitemService;
            Configuration = _configuration;
            this.wordCountBreakdownBatchService = wordCountBreakdownBatchService;
            this.languageLogicService = languageLogicService;
            emailService = emailUtilsService;
            this.discountsurchargeService = discountsurchargeService;
            this.discountorsurchargeService = discountorsurchargeService;
            this.discountsurchargeRepo = discountsurchargeRepo;
            this.currencyinfoservice = currencyinfoservice;
        }


        public async Task<IActionResult> Index(int id,int quoteitemID = 0)
        {
            if (id == 0)
                return NotFound();

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var model = await quoteService.GetViewModelById(id);

            if (model == null)
                return Content("The quote ID you provided (" + id + ") is not a valid number.");

            if (loggedInEmployee == null)
                return NotFound();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            if (stringToProcess != null & stringToProcess != "")
            {
                int quoteid = Convert.ToInt32(stringToProcess);
                if (quoteid > 0)
                {
                    model.QuoteItem = await quoteitemService.GetViewModelById((int)quoteid);

                }
            }

            var enquiry = await enquiriesService.GetEnquiryById(model.EnquiryId);
            var contact = await contactsService.GetContactDetails(enquiry.ContactId);
            ViewBag.ContactDeleted = "";
            if (contact.DeletedDate != null && contact.DeletedDate != DateTime.MinValue)
            {
                ViewBag.ContactDeleted = "&nbsp;<i>(Deleted)</i>";
            }
            if (contact == null) { return NotFound(); }

            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var group = await orgGroupsLogic.GetOrgGroupDetails((Int32)org.OrgGroupId);

            var Languages = await cachedService.GetAllLanguagesCached();
            model.ContactId = contact.Id;
            model.ContactName = contact.Name;
            model.OrgId = org.Id;
            model.OrgName = org.OrgName;
            model.OrgGroupId = group.Id;
            model.OrgGroupName = group.Name;
            model.ListOfEmployees = await employeesService.GetAllEmployees<Employee>(false, false);
            var opsOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.EnquiriesLead);
            model.Ownership = (short)(opsOwnership == null ? 0 : opsOwnership.EmployeeId);
            model.LanguageServices = await languageService.GetLanguageServices();
            model.LanguageServiceCategory = await cachedService.getLanguageServiceCategory();
            model.Languages = Languages.ToList();
            model.Countries = await cachedService.GetAllCountriesCached();
            var quoteItems = await quoteService.GetAllQuoteItems(id);
            model.QuoteItems = quoteItems.OrderBy(o => o.CreatedDateTime).ToList();
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

            var salescontact = await employeesService.IdentifyCurrentUserById(model.SalesContactEmployeeId);
            if (salescontact != null)
            {
                model.SalesContactName = salescontact.FirstName + " " + salescontact.Surname;
                model.SalesContactEmail = salescontact.EmailAddress;
                model.SalesContactNumber = salescontact.LandlineNumber;
            }
            model.Currencies = await currenciesService.GetAllENCurrencies();

            ViewBag.VatLabel = "";
            ViewBag.VatLabel2 = "";
            ViewBag.VatLabel3 = "";
            ViewBag.VatLabel4 = "";

            switch (model.LangIanacode)
            {
                case "en":
                    ViewBag.VatLabel = "Quote is valid for 30 days. All costs are exclusive of any applicable VAT or equivalent taxes.";
                    ViewBag.VatLabel2 = "If your language project is for printing/packaging purposes, please let us know, so we can follow an appropriate workflow. We strongly recommend that you add an additional proofreading step to your printing/packaging project.";
                    ViewBag.VatLabel3 = "Before proceeding with printing any material it has to be sent to translate plus in its final layout for final linguistic approval.";
                    ViewBag.VatLabel4 = "translate plus cannot be held liable for consequential loss resulting from any printing/packaging related matters.";
                    break;
                case "de":
                    ViewBag.VatLabel = "Dieses Angebot hat eine Gültigkeit von 30 Tagen. Alle Kosten verstehen sich exklusive Mehrwertsteuer.";
                    ViewBag.VatLabel2 = "Verständigen Sie uns bitte, falls der Inhalt Ihres Projektes für Drucksachen oder Verpackungsmaterialien verwendet werden soll, damit wir die entsprechenden Arbeitsschritte einleiten können.";
                    ViewBag.VatLabel3 = "Sollte dies der Fall sein, empfiehlt sich für diese Art von Projekten generell ein zusätzlicher Qualitäts-Check, bei dem z. B. der von translate plus übersetzte Text im finalen Layout abgenommen werden kann.";
                    ViewBag.VatLabel4 = "translate plus kann nicht für Folgeschäden bezüglich Drucksachen oder Verpackungsmaterialien haftbar gemacht werden.";
                    break;
                case "sv":
                    ViewBag.VatLabel = "Priserna är exklusive VAT (moms)";
                    ViewBag.VatLabel2 = "Om ditt språkprojekt är avsett för trycknings/förpackningsändamål ber vi dig att meddela oss detta, så att vi kan följa ett lämpligt arbetsflöde. Vi rekommenderar starkt att du lägger till ett ytterligare korrekturläsningssteg till ditt trycknings/förpackningsprojekt.";
                    ViewBag.VatLabel3 = "Innan något material trycks måste det skickas till translate plus med sin slutliga layout för ett sista lingvistiskt godkännande.";
                    ViewBag.VatLabel4 = "translate plus kan inte hållas ansvarigt för indirekta förluster till följd av trycknings/förpackningsrelaterade projekt.";
                    break;
                case "da":
                    ViewBag.VatLabel = "Priserne er ekskl. moms";
                    ViewBag.VatLabel2 = "Hvis dit sprogprojekt er til tryk/emballage, bedes du informere os herom, så vi kan følge et passende workflow. Vi anbefaler meget, at du tilføjer et ekstra korrekturlæsningstrin til dit projekt til tryk/emballage.";
                    ViewBag.VatLabel3 = "Inden du trykker materiale, skal det sendes til translate plus med det endelige layout til afsluttende sproglig godkendelse.";
                    ViewBag.VatLabel4 = "translate plus kan ikke gøres ansvarlig for følgeskader som følge af forhold relaterede til tryk/emballage.";
                    break;
            }

            ViewBag.FuzzyBand1Title = org.FuzzyBand1BottomPercentage.ToString() + " - " + org.FuzzyBand1TopPercentage.ToString() + "% Matches";

            if (org.FuzzyBand2BottomPercentage != null && org.FuzzyBand2BottomPercentage > 0 && org.FuzzyBand2TopPercentage != null)
            {
                ViewBag.FuzzyBand2Title = org.FuzzyBand2BottomPercentage.ToString() + " - " + org.FuzzyBand2TopPercentage.ToString() + "% Matches";

                if (org.FuzzyBand3BottomPercentage != null && org.FuzzyBand3BottomPercentage > 0 && org.FuzzyBand3TopPercentage != null)
                {
                    ViewBag.FuzzyBand3Title = org.FuzzyBand3BottomPercentage.ToString() + " - " + org.FuzzyBand3TopPercentage.ToString() + "% Matches";

                    if (org.FuzzyBand4BottomPercentage != null && org.FuzzyBand4BottomPercentage > 0 && org.FuzzyBand4TopPercentage != null)
                    {
                        ViewBag.FuzzyBand4Title = org.FuzzyBand4BottomPercentage.ToString() + " - " + org.FuzzyBand4TopPercentage.ToString() + "% Matches";
                    }
                }
            }

            ViewBag.Prefix = "";
            ViewBag.CurrencyName = "";

            if (model.QuoteCurrencyId > 0)
            {
                var currency = await currenciesService.GetById((int)model.QuoteCurrencyId);
                ViewBag.Prefix = currency.Prefix;

                var localcurrency = await currenciesService.GetCurrencyInfo((int)model.QuoteCurrencyId, "en");
                ViewBag.CurrencyName = localcurrency.CurrencyName;
            }

            ViewBag.CustomerSpecificData1 = "Customer-specific data 1";
            ViewBag.CustomerSpecificData2 = "Customer-specific data 2";
            ViewBag.CustomerSpecificData3 = "Customer-specific data 3";
            ViewBag.CustomerSpecificData4 = "Customer-specific data 4";

            if (org.CustomerSpecificField1Name != "")
            {
                ViewBag.CustomerSpecificData1 = org.CustomerSpecificField1Name;
            }
            if (org.CustomerSpecificField2Name != "")
            {
                ViewBag.CustomerSpecificData2 = org.CustomerSpecificField2Name;
            }
            if (org.CustomerSpecificField3Name != "")
            {
                ViewBag.CustomerSpecificData3 = org.CustomerSpecificField3Name;
            }
            if (org.CustomerSpecificField4Name != "")
            {
                ViewBag.CustomerSpecificData4 = org.CustomerSpecificField4Name;
            }

            var config = new GlobalVariables();
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);
            string serverLocationPath = config.InternalQuoteDriveBaseDirectoryPathForApp;
            model.EnquiryFolder = fileSystemService.GetEnquiryDirectoryPath(serverLocationPath, org.Id, enquiry.Id);
            model.ArchivedDateTime = enquiry.ArchivedDateTime;
            model.ArchivedToLionBoxDateTime = enquiry.ArchivedToLionBoxDateTime;
            model.ArchivedToAmazonS3dateTime = enquiry.ArchivedToAmazonS3dateTime;

            if (model.EnquiryFolder != null && model.EnquiryFolder != "")
            {
                if (Directory.Exists(Path.Combine(model.EnquiryFolder, "Quote")) == true)
                {
                    var QuoteDir = new DirectoryInfo(Path.Combine(model.EnquiryFolder, "Quote"));
                    var Filename = String.Format("{0} - {1}.pdf", model.Id.ToString(), model.QuoteFileName);

                    try
                    {
                        if (QuoteDir.GetFiles(Filename).Length > 0)
                        {
                            model.QuotePDFPath = QuoteDir.GetFiles(Filename)[0].FullName;
                        }
                    }
                    catch { }


                }
            }

            ViewBag.CurrentDirectory = (Environment.CurrentDirectory + @"\wwwroot").Replace("\\", " /");
            var enquiryModel = await enquiriesService.GetViewModelById(model.EnquiryId);
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            var emp = await employeesService.GetEmployeeByUsername(username);
            var department = await employeesService.GetEmployeeDepartment(emp.Id);
            model.IsAllowedToEdit = false;
            model.EditPageEnabled = false;
            if (enquiryModel.Status == 2 || enquiryModel.Status == 3)
            {
                enquiryModel.IsFinalised = true;
            }
            if (org.OrgGroupId == 18520) { model.IsAllowedToEdit = true; }
            else
            {
                if (loggedInEmployee.TeamId == 5 || loggedInEmployee.TeamId == 16 || loggedInEmployee.TeamId == 38 || (department.Id != 1 && emp.AttendsBoardMeetings == true) || (loggedInEmployee.TeamId == 13 && loggedInEmployee.Id != 1215 && loggedInEmployee.Id != 970))
                {
                    model.IsAllowedToEdit = false;
                }
                else { model.IsAllowedToEdit = true; }
            }
            if (enquiryModel.IsFinalised == false && model.IsAllowedToEdit == true) { model.EditPageEnabled = true; }
            model.SurchargesCategories = await discountsurchargeService.GetAllSurchargeCategories();
            model.DiscountCategories = await discountsurchargeService.GetAllDiscountCategories();
            if (model.SurchargeId != null)
            {
                model.SurchargeDetails = await discountsurchargeRepo.All().Where(x => x.Id == model.SurchargeId)
                    .Select(x => new SurchargeDetails()
                    {
                        SurchargeId = x.Id,
                        SurchargeCategory = x.DiscountOrSurchargeCategory,
                        PercentageOrValue = x.PercentageOrValue,
                        Description = x.Description,
                        SurchargeAmount = x.DiscountOrSurchargeAmount
                    }).FirstOrDefaultAsync();
            }
            if (model.DiscountId != null)
            {
                model.DiscountDetails = await discountsurchargeRepo.All().Where(x => x.Id == model.DiscountId)
                    .Select(x => new DiscountDetails()
                    {
                        DiscountId = x.Id,
                        DiscountCategory = x.DiscountOrSurchargeCategory,
                        PercentageOrValue = x.PercentageOrValue,
                        Description = x.Description,
                        DiscountAmount = x.DiscountOrSurchargeAmount
                    }).FirstOrDefaultAsync();
            }
            ViewBag.Prefix = "";
            ViewBag.CurrencyName = "";
            ViewBag.SurchargeValue = "";
            ViewBag.DiscountValue = "";
            if (model.QuoteCurrencyId > 0)
            {
                var currency = await currenciesService.GetById(model.QuoteCurrencyId);
                if (currency != null)
                {
                    var currencyinfo = await currencyinfoservice.GetById(currency.Id);
                    if (currencyinfo != null)
                    {
                        if (model.SurchargeAmount != null)
                        {
                            ViewBag.SurchargeValue = currency.Prefix + String.Format("{0:N2}", (decimal)model.SurchargeAmount) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.SurchargeValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }
                        if (model.DiscountAmount != null)
                        {
                            ViewBag.DiscountValue = currency.Prefix + String.Format("{0:N2}", (decimal)model.DiscountAmount) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.DiscountValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }

                        ViewBag.Prefix = currency.Prefix;
                        ViewBag.CurrencyName = " (" + currencyinfo.CurrencyName + ")";
                    }
                }
            }
            ViewBag.InvoiceLang = model.LangIanacode;
            model.QuoteItemPopupID = quoteitemID;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuoteInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");

            var openingsectiontext = "";
            var closingsectiontext = "";
            var title = allFields[3];

            try
            {
                Quote quotetocheck = await quoteService.GetQuoteById(Convert.ToInt32(allFields[0]));

                if (quotetocheck.LangIanacode != allFields[2])
                {
                    QuoteTemplate defaultQuoteTemplate = await quoteTemplatesService.GetDefaultQuoteTemplate(allFields[2]);

                    openingsectiontext = defaultQuoteTemplate.OpeningSectionText;
                    closingsectiontext = defaultQuoteTemplate.ClosingSectionText;

                    switch (allFields[2])
                    {
                        case "en":
                            title = "Quotation for translation of:";
                            break;

                        case "de":
                            title = "Angebot zur Übersetzung:";
                            break;

                        case "da":
                            title = "Tilbud på oversættelse af følgende fil(er):";
                            break;

                        case "sv":
                            title = "Offert för översättning:";
                            break;

                        default:
                            title = "Quotation for translation of:";
                            break;
                    }
                }

            }
            catch { }


            Quote quote = await quoteService.UpdateQuoteInformation(Convert.ToInt32(allFields[0]), Convert.ToInt16(allFields[1]), allFields[2], title, allFields[4], allFields[5], openingsectiontext, closingsectiontext, LoggedInEmployee.Id);

            return RedirectToAction("Index", quote.Id);
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuoteItem()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            //Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            QuoteItem quoteitem = await quoteitemService.UpdateQuoteItem(Convert.ToInt32(allFields[0]), Convert.ToByte(allFields[1]), allFields[2], allFields[3], Convert.ToInt32(allFields[4]),
                Convert.ToInt32(allFields[5]), Convert.ToInt32(allFields[6]), Convert.ToInt32(allFields[7]), Convert.ToInt32(allFields[8]), Convert.ToInt32(allFields[9]),
                Convert.ToInt32(allFields[10]), Convert.ToInt32(allFields[11]), Convert.ToDecimal(allFields[12]), Convert.ToInt32(allFields[13]),
                Convert.ToInt32(allFields[14]), Convert.ToInt32(allFields[15]), Convert.ToInt32(allFields[16]), Convert.ToInt32(allFields[17]), Convert.ToInt32(allFields[18]),
                 allFields[19], allFields[20], allFields[21], allFields[22], allFields[23], allFields[24], allFields[25], Convert.ToInt16(allFields[26]), allFields[27], LoggedInEmployee.Id, Convert.ToByte(allFields[28]));


            return RedirectToAction("Index", quoteitem.QuoteId);
            //return Ok();
        }


        public async Task<IActionResult> UpdateQuoteOptions()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            //Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$");

            Quote quote = await quoteService.UpdateQuoteOptions(Convert.ToInt32(allFields[0]), Convert.ToDouble(allFields[1]), Convert.ToByte(allFields[2]), Convert.ToByte(allFields[3]), Convert.ToBoolean(allFields[4]),
                Convert.ToBoolean(allFields[5]), Convert.ToBoolean(allFields[6]), Convert.ToBoolean(allFields[7]), Convert.ToBoolean(allFields[8]), allFields[9],
                allFields[10], allFields[11], allFields[12], allFields[13],
                Convert.ToBoolean(allFields[14]), Convert.ToBoolean(allFields[15]), Convert.ToBoolean(allFields[16]), Convert.ToBoolean(allFields[17]), allFields[18], LoggedInEmployee.Id);


            return RedirectToAction("Index", quote.Id);
            //return Ok();
        }

        public async Task<IActionResult> UpdateQuotePDFDetails()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            //Enquiry model = new Enquiry();
            var allFields = stringToProcess.Split("$tp$");

            Quote quote = await quoteService.UpdateQuotePDFDetails(Convert.ToInt32(allFields[0]), Convert.ToDateTime(allFields[1]), allFields[2], allFields[3], allFields[4],
                allFields[5], allFields[6], allFields[7], allFields[8],
                Convert.ToInt16(allFields[9]), allFields[10], allFields[11], allFields[12],
                Convert.ToInt16(allFields[13]), LoggedInEmployee.Id);


            return RedirectToAction("Index", quote.Id);
            //return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> GeneratePDFQuote()
        {
            Employee loggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var QuoteID = stringToProcess;

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
            BatchDocAttr.Value = "GenerateQuotePDF";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write task number info 
            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
            BatchDocAttr.Value = "1";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // writeenquiry number info 
            BatchDocAttr = BatchDoc.CreateAttribute("QuoteID");
            BatchDocAttr.Value = QuoteID.ToString();
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // now append the node to the doc
            RootNode.AppendChild(IndividualTaskNode);

            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-40";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            BatchDoc.Save(BatchFilePath);


            return Ok();
        }

        public async Task<IActionResult> DeleteQuoteItem()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

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

            return RedirectToAction("Index", quoteitem.QuoteId);
            //return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewQuoteItems()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var allFields = stringToProcess.Split("$");
            QuoteItem newQuoteItem = null;

            foreach (string quoteitem in allFields)
            {
                if (quoteitem.Contains(",") == true)
                {
                    newQuoteItem = await quoteService.CreateQuoteItem(Convert.ToInt32(allFields[0]), Convert.ToByte((quoteitem.Split(",")[0])), quoteitem.Split(",")[1], quoteitem.Split(",")[2], 0,
                                                                                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", "", "", "", "", "", "", null, 0,
                                                                                0, "", 0, loggedInEmployee.Id, 0,
                                                                                0, 0, 0, 0, 0, 0, 0, languageServiceCategoryId: Convert.ToInt32(quoteitem.Split(",")[3]));

                    var quote = await quoteService.GetQuoteById(newQuoteItem.QuoteId);
                    if (quote.IsCurrentVersion == true)
                    {
                        EnquiryQuoteItem newEnquiryQuoteItem = await enquiriesService.CreateEnquiryQuoteItems(quote.EnquiryId, Int32.Parse(quoteitem.Split(",")[0]), quoteitem.Split(",")[1], quoteitem.Split(",")[2], loggedInEmployee.Id, Convert.ToInt32(quoteitem.Split(",")[3]));
                    }
                }
            }


            //JobOrder newRequest = null;
            //newRequest = await enquiriesService.CreateEnquiry(model);
            if (newQuoteItem == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new quote items.");
                return Ok(MessagsString);
            }

            // return Ok(idlist + "$Success$Enquiry quote items created successfully");
            return RedirectToAction("Index", newQuoteItem.QuoteId);
        }

        [HttpPost]
        public async Task<IActionResult> ImportWordCounts()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            string MessagsString = string.Empty;
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            var folderPath = allFields[0];
            var quoteID = Convert.ToInt32(allFields[1]);


            var Quote = await quoteService.GetQuoteById(quoteID);
            var QuoteItems = await quoteService.GetAllQuoteItems(quoteID);
            var Enquiry = await enquiriesService.GetEnquiryById(Quote.EnquiryId);
            var OrderContact = await contactsService.GetContactDetails(Enquiry.ContactId);
            var ParentOrg = await orgsService.GetOrgDetails(OrderContact.OrgId);



            bool IsThereAnyXML = false;
            var DirInfo = new DirectoryInfo(folderPath);
            var XMLCount = DirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly).Count();
            if (XMLCount > 0) { IsThereAnyXML = true; }
            if (DirInfo == null) { MessagsString = "Error$Current folder can't be found"; return Ok(MessagsString); }
            if (IsThereAnyXML)
            {
                var result = await InsertWordCountsInQuoteItems(quoteID, folderPath, loggedInEmployee);
                if (result != null)
                {
                    MessagsString = "Success$Matching quote  items were updated successfully and notification has been sent.";
                    return Ok(MessagsString);
                }
            }
            else
            {
                MessagsString = "Error$There is no xml files in the current folder.";
                return Ok(MessagsString);
            }
            return Ok(MessagsString);
        }
        public async Task<IActionResult> InsertWordCountsInQuoteItems(int quoteID, string ImportFolderPath, ViewModels.EmployeeModels.EmployeeViewModel EmployeeCurrentlyLoggedOn, bool? ImportOnlyForClient = null)
        {
            var DirToBrowse = new DirectoryInfo(ImportFolderPath);
            var OrderedXmlFilesArray = DirToBrowse.GetFiles("*.xml", SearchOption.TopDirectoryOnly).OrderByDescending(o => o.LastWriteTime).ToList();
            System.Data.DataTable NotificationTable = new System.Data.DataTable();
            NotificationTable.Columns.Add("Quote Item ID");
            NotificationTable.Columns.Add("Item Source/Target languages");
            NotificationTable.Columns.Add("XmlName");
            NotificationTable.Columns.Add("XmlDate");

            string[] EnglishCodes = { "en-gb", "en-us" };

            DataTable MissingImportsNotificationTable = new System.Data.DataTable();
            MissingImportsNotificationTable.Columns.Add("Quote Item ID");
            MissingImportsNotificationTable.Columns.Add("Source language");
            MissingImportsNotificationTable.Columns.Add("Target Language");

            var Quote = await quoteService.GetQuoteById(quoteID);
            var QuoteItems = await quoteService.GetAllQuoteItems(quoteID);
            var Enquiry = await enquiriesService.GetEnquiryById(Quote.EnquiryId);
            var OrderContact = await contactsService.GetContactDetails(Enquiry.ContactId);
            var ParentOrg = await orgsService.GetOrgDetails(OrderContact.OrgId);

            foreach (var quoteitem in QuoteItems)
            {
                string SourceLanguageName = string.Empty;
                string TargetLanguageName = string.Empty;

                var SourceLanguage = await cachedService.GetAllLanguagesCached();
                SourceLanguageName = SourceLanguage.Where(o => o.StringValue == quoteitem.SourceLanguageIanacode).Select(o => o.Name).FirstOrDefault();
                var TargetLanguage = await cachedService.GetAllLanguagesCached();
                TargetLanguageName = TargetLanguage.Where(o => o.StringValue == quoteitem.TargetLanguageIanacode).Select(o => o.Name).FirstOrDefault();

                if (quoteitem.LanguageServiceId == 1 || quoteitem.LanguageServiceId == 17 || quoteitem.LanguageServiceId == 46 || quoteitem.LanguageServiceId == 72 || quoteitem.LanguageServiceId == 73 || quoteitem.LanguageServiceId == 74 || quoteitem.LanguageServiceId == 21 || quoteitem.LanguageServiceId == 99)
                {
                    string[] QuoteItemSourceIanaCodeWithRegion = { languageLogicService.MapLangIanaCodeWithRegion(quoteitem.SourceLanguageIanacode).ToLower() };
                    string[] QuoteItemTargetIanaCodeWithRegion = { languageLogicService.MapLangIanaCodeWithRegion(quoteitem.TargetLanguageIanacode).ToLower() };
                    bool isMissingImportFile = true;
                    foreach (FileInfo xmlDoc in OrderedXmlFilesArray)
                    {

                        if (xmlDoc.Name.StartsWith("Analyze Files") == true)
                        {
                            var LangsArr = xmlDoc.Name.Split("_");
                            var XMLSourceIanaCodeWithRegion = LangsArr[0].Substring(LangsArr[0].LastIndexOf(" ") + 1, 5);
                            var XMLTargetIanaCodeWithRegion = LangsArr[1].Substring(0, LangsArr[1].IndexOfAny(new char[] { '(', '.' }));

                            if (SourceLanguageName == "English") { QuoteItemSourceIanaCodeWithRegion = EnglishCodes; }
                            if (TargetLanguageName == "English") { QuoteItemTargetIanaCodeWithRegion = EnglishCodes; }

                            if (QuoteItemSourceIanaCodeWithRegion.Contains(XMLSourceIanaCodeWithRegion.ToLower()) && QuoteItemTargetIanaCodeWithRegion.Contains(XMLTargetIanaCodeWithRegion.ToLower()))
                            {
                                TPWordCountBreakdownBatchModel ItemImport = wordCountBreakdownBatchService.WordCountBreakdownBatch(fileSystemService.UnMapTPNetworkPath(xmlDoc.FullName), ParentOrg);
                                quoteitem.LastModifiedByEmployeeId = EmployeeCurrentlyLoggedOn.Id;
                                var result = await quoteitemService.ApplyToQuoteItem(quoteitem, ItemImport);

                                var SourceAndTargetLangs = SourceLanguageName + " - " + TargetLanguageName;
                                NotificationTable.Rows.Add(quoteitem.Id, SourceAndTargetLangs, xmlDoc.Name, xmlDoc.LastWriteTime);
                                isMissingImportFile = false;
                                break;
                            }
                        }
                    }

                    if (isMissingImportFile == true)
                    {
                        MissingImportsNotificationTable.Rows.Add(quoteitem.Id, SourceLanguageName, TargetLanguageName);
                    }
                }
            }


            if (NotificationTable.Rows != null)
            {
                string EmailBody = String.Format("<p>Dear {0},<br /><br />The following Quote items were updated with the word counts from their corresponding xml file, " +
                                       "which you can find in this folder <a href=\"{1}\">{1}</a><br /><br />" +
                                       "<table border=\"1\" width=\"900\"><tr><td><b>Quote Item ID</b></td><td><b>Job Item Source/Target Language</b></td><td><b>XML document Name</b></td><td><b>Last modified</b></td></tr>", EmployeeCurrentlyLoggedOn.FirstName, ImportFolderPath);

                foreach (DataRow row in NotificationTable.Rows)
                {
                    EmailBody += String.Format("<tr><td><a href=\"http://myplus/QuoteItems.aspx?QuoteItemID={0}\">{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3]);

                }
                EmailBody += "</table>";
                if (MissingImportsNotificationTable.Rows != null)
                {
                    EmailBody += "<p><br /><br />The following Quote items were missing their corresponding xml file, " +
                                       "please update the following items manually.<br /><br />" +
                                       "<table border=\"1\" width=\"800\"><tr><td><b>Job Item ID</b></td><td><b>Source Language</b></td><td><b>Target Language</b></td></tr>";

                    foreach (DataRow rows in MissingImportsNotificationTable.Rows)
                    {
                        EmailBody += String.Format("<tr><td><a href=\"http://myplus/QuoteItems.aspx?QuoteItemID={0}\">{0}</a></td><td>{1}</td><td>{2}</td></tr>", rows.ItemArray[0], rows.ItemArray[1], rows.ItemArray[2]);

                    }
                    EmailBody += "</table>";
                }
                EmailBody += "</p>";
                emailService.SendMail("flow plus <flowplus@translateplus.com>", "cristian-laurentiu.necula@translateplus.com, " + EmployeeCurrentlyLoggedOn.EmailAddress, "Successful word counts import in Quote items",
                                        EmailBody);
                //emailService.SendMail("my plus <myplus@translateplus.com>", EmployeeCurrentlyLoggedOn.EmailAddress, "Successful word counts import in Job items",
                //                        EmailBody);
            }
            else
            {
                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmployeeCurrentlyLoggedOn.EmailAddress, "No corresponding xml files for word counts import in Quote items were found", "<p>No XML files were found that match the Languages of the Quote items.</p>");
            }

            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ImportQuoteItemWordCounts()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            string MessagsString = string.Empty;
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            var filePath = allFields[0];
            var quoteItemID = Convert.ToInt32(allFields[1]);



            var QuoteItem = await quoteitemService.GetQuoteItemById(quoteItemID);
            var Quote = await quoteService.GetQuoteById(QuoteItem.QuoteId);
            var Enquiry = await enquiriesService.GetEnquiryById(Quote.EnquiryId);
            var OrderContact = await contactsService.GetContactDetails(Enquiry.ContactId);
            var ParentOrg = await orgsService.GetOrgDetails(OrderContact.OrgId);



            bool IsThereAnyXML = false;

            if (filePath.EndsWith(".xml") == true && System.IO.File.Exists(filePath) == true) { IsThereAnyXML = true; }
            else { MessagsString = "Error$XML file to import can't be found."; return Ok(MessagsString); }
            var fileName = Path.GetFileName(filePath);

            if (IsThereAnyXML)
            {

                TPWordCountBreakdownBatchModel ItemImport = wordCountBreakdownBatchService.WordCountBreakdownBatch(fileSystemService.UnMapTPNetworkPath(filePath), ParentOrg);
                QuoteItem.LastModifiedByEmployeeId = loggedInEmployee.Id;
                await quoteitemService.ApplyToQuoteItem(QuoteItem, ItemImport);

                var languages = cachedService.GetAllLanguagesCached();
                var sourceLanguage = languages.Result.Where(x => x.StringValue == QuoteItem.SourceLanguageIanacode).Select(x => x.Name).FirstOrDefault();
                var targetLanguage = languages.Result.Where(x => x.StringValue == QuoteItem.TargetLanguageIanacode).Select(x => x.Name).FirstOrDefault();
                MessagsString = "Success$Successfully updated word counts.";


                string EmailBody = String.Format("<p>Dear {0},<br /><br />You have imported \"{1}\" into quote item <a href=\"http://myplus/QuoteItems.aspx?QuoteItemID={2}\">{2} {3} --> {4}.</a>", loggedInEmployee.FirstName, fileName, QuoteItem.Id, sourceLanguage, targetLanguage);
                emailService.SendMail("flow plus <flowplus@translateplus.com>", "cristian-laurentiu.necula@translateplus.com, " + loggedInEmployee.EmailAddress, "Successful word counts import in quote item",
                                        EmailBody);

                //emailService.SendMail("my plus <myplus@translateplus.com>", "raj.lakhwani@publicismedia.com, " + loggedInEmployee.EmailAddress, "Successful word counts import in Job item",
                //                        EmailBody);

            }
            else
            {
                MessagsString = "Error$Please paste in a path to an XML file to import word counts.";
                return Ok(MessagsString);
            }
            return Ok(MessagsString);

        }
        [HttpPost]
        public async Task<IActionResult> UpdateQuoteItems()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;


            var allFields = stringToProcess.Split("$");
            var tableData = allFields[0];
            List<QuoteItem> items = JsonConvert.DeserializeObject<List<QuoteItem>>(tableData);
            foreach (var item in items)
            {
                item.LastModifiedByEmployeeId = LoggedInEmployee.Id;
                await quoteitemService.UpdateQuoteItem(item);
            }
            return Ok();

        }
        [HttpPost]
        public async Task<IActionResult> SaveSurcharge()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var quoteid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var surchargeid = Int32.Parse(stringToUse.Split("$tp$")[1]);
            var surchargecategory = stringToUse.Split("$tp$")[2];
            var Surchargedescription = stringToUse.Split("$tp$")[3];
            var surchargetype = stringToUse.Split("$tp$")[4];
            var surchargeamount = decimal.Parse(stringToUse.Split("$tp$")[5]);

            if (surchargecategory == "-1" && surchargeid != 0)
            {
                var result = await discountorsurchargeService.RemoveSurchargeOrDiscount(surchargeid, LoggedInEmployee.Id);
                var quoteresult = await quoteService.UpdateSurchargeID(quoteid, -1, LoggedInEmployee.Id);
            }

            else
            {
                if (surchargecategory != "-1")
                {
                    bool discountorperc = false;
                    if (surchargetype == "true")
                    { discountorperc = true; }
                    if (surchargeid == 0)
                    {
                        var result = await discountorsurchargeService.CreateSurchargeOrDiscount(false, byte.Parse(surchargecategory), Surchargedescription, discountorperc, surchargeamount, LoggedInEmployee.Id);
                        var quoteresult = await quoteService.UpdateSurchargeID(quoteid, result.Id, LoggedInEmployee.Id);
                    }
                    else
                    {
                        var result = await discountorsurchargeService.UpdateSurchargeOrDiscount(surchargeid, byte.Parse(surchargecategory), Surchargedescription, discountorperc, surchargeamount, LoggedInEmployee.Id);
                    }
                }
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SaveDiscount()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var quoteid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var discountid = Int32.Parse(stringToUse.Split("$tp$")[1]);
            var discountcategory = stringToUse.Split("$tp$")[2];
            var Discountedescription = stringToUse.Split("$tp$")[3];
            var discounttype = stringToUse.Split("$tp$")[4];
            var discountamount = decimal.Parse(stringToUse.Split("$tp$")[5]);

            if (discountcategory == "-1" && discountid != 0)
            {
                var result = await discountorsurchargeService.RemoveSurchargeOrDiscount(discountid, LoggedInEmployee.Id);
                var quoteresult = await quoteService.UpdateDiscountID(quoteid, -1, LoggedInEmployee.Id);
            }

            else
            {
                if (discountcategory != "-1")
                {
                    bool discountorperc = false;
                    if (discounttype == "1")
                    { discountorperc = true; }
                    if (discountid == 0)
                    {
                        var result = await discountorsurchargeService.CreateSurchargeOrDiscount(true, byte.Parse(discountcategory), Discountedescription, discountorperc, discountamount, LoggedInEmployee.Id);
                        var jobresult = await quoteService.UpdateDiscountID(quoteid, result.Id, LoggedInEmployee.Id);
                    }
                    else
                    {
                        var result = await discountorsurchargeService.UpdateSurchargeOrDiscount(discountid, byte.Parse(discountcategory), Discountedescription, discountorperc, discountamount, LoggedInEmployee.Id);
                    }
                }
            }

            return Ok();
        }
        //public async Task<IActionResult> QuoteItemViewModel()
        //{
        //    var DataPassedOver = HttpContext.Request.Body;
        //    var streamreader = new StreamReader(DataPassedOver);
        //    var content = streamreader.ReadToEndAsync();
        //    var stringToProcess = content.Result;
        //    int id = Convert.ToInt32(stringToProcess);

        //    if (id == 0)
        //        return NotFound();

        //   // var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

        //    var quoteitemmodel = await quoteitemService.GetViewModelById(id);

        //    if (quoteitemmodel == null)
        //        return Content("The quote item ID you provided (" + id + ") is not a valid number.");

        //    var model = await quoteService.GetViewModelById(quoteitemmodel.QuoteId);

        //    if (model == null)
        //        return Content("The quote ID you provided (" + id + ") is not a valid number.");

        //    model.QuoteItem = quoteitemmodel;
        //    return View(model);
        //}
    }
}
