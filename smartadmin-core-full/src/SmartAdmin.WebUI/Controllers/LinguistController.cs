using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services;
using ViewModels;
using Services.Interfaces;
using System.IO;
using Global_Settings;
using ViewModels.LinguisticSupplier;
using Data.Repositories;
using System.Globalization;
using Microsoft.Data.SqlClient;
using System.Data;
using ViewModels.Common;

namespace SmartAdmin.WebUI.Controllers
{
    public class LinguistController : Controller
    {


        private IConfiguration Configuration;
        private readonly ICommonCachedService cachedService;
        private ITPLinguistService linguistData;
        private ITPEmployeesService employeesService;
        private ITPEmployeesService tpservice;
        private ITPLinguistsAltairDetails linguistAltairservice;
        private ITPLinguisticSupplierInvoices invoiciesService;
        private readonly ITPCurrenciesLogic currencyservice;
        private readonly ITPLocalCurrencyInfo currencyinfoservice;
        private readonly IRepository<LinguisticSupplierSoftwareApplication> linguistSupplierSoftwareApplicationRepository;
        private readonly IRepository<PlanningCalendarAppointment> planningCalendarAppointment;
        private readonly ITPTasksLogic tasksService;
        private readonly IRepository<ExtranetUser> extranetUserService;
        private readonly ITPLinguisticSupplierInvoiceTemplate invoicetemplateService;
        private ITPContactsLogic tpcontactService;
        private readonly IEmailUtilsService emailUtils;
        private readonly ITPLinguisticSupplierPoliceCheckDetails linguisticSupplierPoliceCheckService;

        public LinguistController(IConfiguration _configuration, ITPEmployeesService _empservice,
            ICommonCachedService cachedService, ITPLinguistService linguistData, ITPEmployeesService employeesService, ITPLinguistsAltairDetails linguistAltairservice, ITPLinguisticSupplierInvoices invoiciesService, ITPCurrenciesLogic currencyservice,
            ITPLocalCurrencyInfo currencyinfoservice, IRepository<LinguisticSupplierSoftwareApplication> linguistSupplierSoftwareApplicationRepository, IRepository<PlanningCalendarAppointment> planningCalendarAppointment, ITPTasksLogic tasksService, IRepository<ExtranetUser> extranetUserService, ITPLinguisticSupplierInvoiceTemplate invoicetemplateService, ITPContactsLogic _tpcontactService, IEmailUtilsService emailUtilsService, ITPLinguisticSupplierPoliceCheckDetails linguisticSupplierPoliceCheckService)
        {
            Configuration = _configuration;
            tpservice = _empservice;
            this.cachedService = cachedService;
            this.linguistData = linguistData;
            this.employeesService = employeesService;
            this.linguistAltairservice = linguistAltairservice;
            this.invoiciesService = invoiciesService;
            this.currencyservice = currencyservice;
            this.currencyinfoservice = currencyinfoservice;
            this.linguistSupplierSoftwareApplicationRepository = linguistSupplierSoftwareApplicationRepository;
            this.planningCalendarAppointment = planningCalendarAppointment;
            this.tasksService = tasksService;
            this.extranetUserService = extranetUserService;
            this.invoicetemplateService = invoicetemplateService;
            tpcontactService = _tpcontactService;
            this.emailUtils = emailUtilsService;
            this.linguisticSupplierPoliceCheckService = linguisticSupplierPoliceCheckService;
        }


        public async Task<IActionResult> Index(int Id)
        {
            CultureInfo us = new CultureInfo("en-US");

            var model = new ViewModels.LinguisticSupplier.LinguistViewModel();
            var result = linguistData.GetById(Id);
            model.Id = result.Result.Id;
            model.MainContactFirstName = result.Result.MainContactFirstName;
            model.MainContactSurname = result.Result.MainContactSurname;
            model.MobileNumber = result.Result.MobileNumber;
            model.GdpracceptedDateTime = result.Result.GdpracceptedDateTime;
            model.Vatnumber = result.Result.Vatnumber;
            model.OperationSystems = await cachedService.GetOperatingSystems();
            model.OperationSystemVersion = await cachedService.GetOperatingSystemVersions();
            model.AgreedPaymentMethodId = result.Result.AgreedPaymentMethodId;
            model.CurrencyId = result.Result.CurrencyId;
            model.CreatedDate = result.Result.CreatedDate;
            model.SupplierTypeId = result.Result.SupplierTypeId;
            model.AgencyCompanyRegistrationNumber = result.Result.AgencyCompanyRegistrationNumber;
            model.AgencyNumberOfLinguists = result.Result.AgencyNumberOfLinguists;
            model.AgencyNumberOfDtporMultimediaOps = result.Result.AgencyNumberOfDtporMultimediaOps;
            model.MainContactGender = result.Result.MainContactGender;
            model.MainContactNationalityCountryId = result.Result.MainContactNationalityCountryId;
            model.Address1 = result.Result.Address1;
            model.CountyOrState = result.Result.CountyOrState;
            model.EmailAddress = result.Result.EmailAddress;
            model.MainContactDob = result.Result.MainContactDob.GetValueOrDefault().Date;
            model.SkypeId = result.Result.SkypeId;
            model.MainLandlineNumber = result.Result.MainLandlineNumber;
            model.MobileNumber = result.Result.MobileNumber;
            model.SecondaryEmailAddress = result.Result.SecondaryEmailAddress;
            model.HasEncryptedComputer = result.Result.HasEncryptedComputer;
            model.AllLinguistTop5Items = await linguistData.GetLinguistTop5Items(Id.ToString());
            model.AllLinguistTop5Words = await linguistData.GetLinguistTop5Words(Id.ToString());
            model.PrimaryOperatingSystemId = result.Result.PrimaryOperatingSystemId;
            model.MotherTongueLanguageIanacode = result.Result.MotherTongueLanguageIanacode;
            model.IfBilingualOtherMotherTongueIanacode = result.Result.IfBilingualOtherMotherTongueIanacode;
            model.SubjectAreaSpecialismsAsDescribedBySupplier = result.Result.SubjectAreaSpecialismsAsDescribedBySupplier;
            model.WebAddress = result.Result.WebAddress;
            model.Referee1Name = result.Result.Referee1Name;
            model.Referee1Phone = result.Result.Referee1Phone;
            model.Referee1EmailAddress = result.Result.Referee1EmailAddress;
            model.Referee2Name = result.Result.Referee2Name;
            model.Referee2Phone = result.Result.Referee2Phone;
            model.Referee2EmailAddress = result.Result.Referee2EmailAddress;
            model.CreatedByEmployeeId = result.Result.CreatedByEmployeeId;
            model.LastModifiedDate = result.Result.LastModifiedDate;
            model.LastModifiedByEmployeeId = result.Result.LastModifiedByEmployeeId;
            model.Notes = result.Result.Notes;
            model.RatesNotes = result.Result.RatesNotes;
            model.WouldSignWitnessStatementString = result.Result.WouldSignWitnessStatement == null ? null : result.Result.WouldSignWitnessStatement.ToString();
            model.MemoryRateFor50To74Percent = result.Result.MemoryRateFor50To74Percent;
            model.MemoryRateFor75To84Percent = result.Result.MemoryRateFor75To84Percent;
            model.MemoryRateFor85To94Percent = result.Result.MemoryRateFor85To94Percent;
            model.MemoryRateFor95To99Percent = result.Result.MemoryRateFor95To99Percent;
            model.MemoryRateForExactMatches = result.Result.MemoryRateForExactMatches;
            model.MemoryRateForPerfectMatches = result.Result.MemoryRateForPerfectMatches;
            model.MemoryRateForRepetitions = result.Result.MemoryRateForRepetitions;
            model.PostcodeOrZip = result.Result.PostcodeOrZip;
            model.PrimaryOperatingSystemVersionId = result.Result.PrimaryOperatingSystemVersionId;
            model.AllCountries = await cachedService.GetAllCountriesCached();
            model.linguistTypes = await cachedService.getLingTypes();
            model.linguistVendorTypes = await cachedService.getLingVendorTypes();
            model.AllRates = await linguistData.GetLinguistRates(Id.ToString());
            model.AllItems = linguistData.GetLinguistItems(Id.ToString()).Result.OrderByDescending(o => o.itemID).ToList();
            model.AgreedPaymentMethodId = result.Result.AgreedPaymentMethodId;
            model.PaymentMethods = await cachedService.GetAllPaymentMethodsCached();
            var PaymentCurrencies = await cachedService.GetAllCurrencies();
            model.PaymentCurrencies = PaymentCurrencies.Where(x => x.Id == 4 || x.Id == 5 || x.Id == 6 || x.Id == 8 || x.Id == 10).Select(x => new DropdownOptionViewModel() { Id = x.Id, Name = x.Name }).ToList();
            model.CountryId = result.Result.CountryId;
            model.SupplierTypeId = result.Result.SupplierTypeId;
            model.SupplierStatusId = result.Result.SupplierStatusId;
            model.supplierStatus = await cachedService.GetSupplierStatusCached();
            model.AllLinguistApprovedOrgs = await linguistData.GetApprovedOrgs(Id.ToString());
            model.AllLinguistBlockedOrgs = await linguistData.GetBlockedOrgs(Id.ToString());
            model.HasAccessToCar = result.Result.HasAccessToCar;
            model.Gdprstatus = result.Result.Gdprstatus;
            if (model.Gdprstatus == 1) { model.pGDPRStatus = "Actively opted in"; model.LinguistGDPRStatus = "This linguist has given us consent to keep their details in compliance with GDPR."; }
            else if (model.Gdprstatus == 2) { model.pGDPRStatus = "Actively opted out"; model.LinguistGDPRStatus = "This linguist has actively requested for us to <b>not keep their details</b>. Please <b>do not contact</b> them."; }
            else if (model.Gdprstatus == 3) { model.pGDPRStatus = "New pending addition"; model.LinguistGDPRStatus = "This linguist has not yet given us consent to keep their details.<br/><br/>When contacting this linguist, you should always let them know where you got their details from and ask for their consent to keep their records on our systems."; }
            else if (model.Gdprstatus == 4) { model.pGDPRStatus = "Okay to contact (previously worked with us)"; model.LinguistGDPRStatus = "This linguist has not yet directly given us consent to keep their details.<br/><br/>However, since we already have business relations with them, we are ok to contact them."; }
            else if (model.Gdprstatus == 5) { model.pGDPRStatus = "Okay to contact (signed NDA with us)"; model.LinguistGDPRStatus = "This linguist has not yet directly given us consent to keep their details.<br/><br/>However, since they have signed NDA, we are ok to contact them."; }
            else { model.pGDPRStatus = "No response received"; }
            model.GdpracceptedDateTime = result.Result.GdpracceptedDateTime;
            model.GeoPoliticalGroup = await linguistData.GeoPoliticalGroup((short)result.Result.CountryId);
            if (result.Result.NonEeaclauseAcceptedDateTime == null)
            {
                result.Result.NonEeaclauseAcceptedDateTime = DateTime.MinValue;
            }
            if (result.Result.NonEeaclauseDeclinedDateTime == null)
            {
                result.Result.NonEeaclauseDeclinedDateTime = DateTime.MinValue;
            }

            if (model.GeoPoliticalGroup == 5)
            {
                if (result.Result.NonEeaclauseAcceptedDateTime != DateTime.MinValue && result.Result.NonEeaclauseDeclinedDateTime == DateTime.MinValue)
                {
                    model.NonEEAClauseStatus = 1;
                }
                else if (result.Result.NonEeaclauseAcceptedDateTime == DateTime.MinValue && result.Result.NonEeaclauseDeclinedDateTime != DateTime.MinValue)
                {
                    model.NonEEAClauseStatus = 2;
                }
                else if (result.Result.NonEeaclauseAcceptedDateTime == DateTime.MinValue && result.Result.NonEeaclauseDeclinedDateTime == DateTime.MinValue)
                {
                    model.NonEEAClauseStatus = 3;
                }
                else
                {
                    if (result.Result.NonEeaclauseAcceptedDateTime > result.Result.NonEeaclauseDeclinedDateTime)
                    {
                        model.NonEEAClauseStatus = 1;
                    }
                    else { model.NonEEAClauseStatus = 2; }
                }
            }
            else { model.NonEEAClauseStatus = 4; }
            var ndaFilePath = NDAFilePath(Id.ToString());
            model.NDAFilePath = ndaFilePath.Split("$")[0];
            model.NDAFileName = ndaFilePath.Split("$")[1];
            var contractFilePath = ContractFilePath(Id.ToString());
            model.ContractFilePath = contractFilePath.Split("$")[0];
            model.ContractFileName = contractFilePath.Split("$")[1];
            var gdpRDocPath = GDPRDocPath(Id.ToString());
            model.GDPRDocPath = gdpRDocPath.Split("$")[0];
            model.GDPRDocFileName = gdpRDocPath.Split("$")[1];
            model.Languages = await cachedService.GetAllLanguagesCached();
            model.MotherTongueLanguageIanacode = result.Result.MotherTongueLanguageIanacode;
            model.IfBilingualOtherMotherTongueIanacode = result.Result.IfBilingualOtherMotherTongueIanacode;
            var altairDetails = linguistAltairservice.GetTINNumber(Id);
            model.TINNumber = altairDetails.Result == null ? "" : altairDetails.Result.Tinnumber;
            model.Invoices = invoiciesService.GetInvoicesByLinguistID(Id).Result.OrderByDescending(o => o.DueDate).ToList();
            model.Currency = currencyservice.GetAll().Result.ToList();
            model.CurrencyInfo = currencyinfoservice.GetAll().Result.ToList();
            ViewBag.GetCurrentGMT = GeneralUtils.GetCurrentGMT();
            var CVPath = CVFilePath(Id.ToString());
            model.CVFileName = CVPath.Split("$")[0];
            model.CVName = CVPath.Split("$")[1];
            model.CVExist = CVPath.Split("$")[2];
            var PortfolioPath = PortfolioFilePath(Id.ToString());
            model.PortfolioFileName = PortfolioPath.Split("$")[0];
            model.PortfolioName = PortfolioPath.Split("$")[1];
            model.PortfolioExist = PortfolioPath.Split("$")[2];
            var RegistrationFormPath = RegistrationFormFilePath(Id.ToString());
            model.RegistrationFormFileName = RegistrationFormPath.Split("$")[0];
            model.RegistrationFormName = RegistrationFormPath.Split("$")[1];
            model.RegistrationFormExist = RegistrationFormPath.Split("$")[2];
            var ProofIDPath = ProofIDFilePath(Id.ToString());
            model.ProofIDFileName = ProofIDPath.Split("$")[0];
            model.ProofIDName = ProofIDPath.Split("$")[1];
            model.ProofIDExist = ProofIDPath.Split("$")[2];
            var ProofBankPath = ProofBankFilePath(Id.ToString());
            model.ProofBankFileName = ProofBankPath.Split("$")[0];
            model.ProofBankName = ProofBankPath.Split("$")[1];
            var ProofCompanyPath = ProofCompanyFilePath(Id.ToString());
            model.ProofCompanyFileName = ProofCompanyPath.Split("$")[0];
            model.ProofCompanyName = ProofCompanyPath.Split("$")[1];
            model.SoftwareApplications = await cachedService.GetSoftware();
            var LinguistSupplierSoftwareApplication = linguistSupplierSoftwareApplicationRepository.AllAsNoTracking();
            model.LinguisticSupplierSoftwareApplication = LinguistSupplierSoftwareApplication.Where(o => o.LinguisticSupplierId == Id).Distinct().ToList();
            model.LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            var linguistProfileImage = LinguistProfileImage(Id.ToString());
            model.LinguistProfileImage = linguistProfileImage;
            model.MasterFields = await cachedService.GetMasterFields();
            model.SubFields = await cachedService.GetSubFields();
            model.MediaTypes = await cachedService.GetMediaTypes();
            model.MasterFieldsIds = result.Result.MasterFieldID;
            model.SubFieldsIds = result.Result.SubFieldID;
            model.MediaTypeIds = result.Result.MediaTypeID;
            model.OverallRating = result.Result.OverallRating;
            model.SupplierResponsivenessRating = result.Result.SupplierResponsivenessRating;
            model.SupplierFollowingTheInstructionsRating = result.Result.SupplierFollowingTheInstructionsRating;
            model.SupplierAttitudeRating = result.Result.SupplierAttitudeRating;
            model.SupplierQualityOfWorkRating = result.Result.SupplierQualityOfWorkRating;
            model.SupplierOnTimeDeliveryRating = result.Result.SupplierOnTimeDeliveryRating;
            var compliments = tasksService.GetAllTasksForDataObjectID(model.Id, 4);
            if (compliments.Result.Any())
            {
                model.Compliments = compliments.Result.Where(o => o.TaskTypeId == 9 && o.DeletedDate == null).Select(o => o.Id).ToList().Count();
            }
            else { model.Compliments = 0; }
            var complaints = tasksService.GetAllTasksForDataObjectID(model.Id, 4);
            if (complaints.Result.Any())
            {
                model.Complaints = complaints.Result.Where(o => o.TaskTypeId == 10 && o.DeletedDate == null).Select(o => o.Id).ToList().Count();
            }
            else
            {
                model.Complaints = 0;
            }
            var defaultTimeZone = extranetUserService.All().Where(o => o.UserName == linguistData.getExtranetName(Id)).Select(o => o.DefaultTimeZone).FirstOrDefault();
            model.DefaultTimeZone = defaultTimeZone;
            if (model.DefaultTimeZone != null)
            {
                model.CurrentInfoTimeLocal = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById(model.DefaultTimeZone));
            }

            model.SubjectNotes = "";
            model.CategoryName = "Unknown";
            model.CategoryRGBColor = "";
            if (linguistData.getExtranetName(Id) != null)
            {
                var getCalendarAppointment = GetCalendarAppointment(DateTime.Now, linguistData.getExtranetName(Id));
                if (getCalendarAppointment.Id != 0)
                {
                    model.SubjectNotes = getCalendarAppointment.SubjectLine + "-" + getCalendarAppointment.Notes;
                    model.CategoryName = getCalendarAppointment.CategoryName;
                    var category = planningCalendarAppointment.All().Where(o => o.ExtranetUserName == linguistData.getExtranetName(Id)).Select(o => o.Category).FirstOrDefault();
                    var categoryRGBColor = cachedService.getPlanningCalendarCategories();
                    model.CategoryRGBColor = categoryRGBColor.Result.Where(o => o.Id == category).Select(o => o.CategoryRgbcolor).FirstOrDefault();
                }

            }
            var linguisticSupplierInvoiceTemplate = await invoicetemplateService.GetAllLinguisticSupplierInvoiceTemplates(Id);
            model.LinguisticSupplierInvoiceTemplate = linguisticSupplierInvoiceTemplate.ToList().FirstOrDefault();
            ViewBag.CurrencyName = linguistData.getCurrencyName(Convert.ToInt32(result.Result.CurrencyId));
            ViewBag.PaymentType = linguistData.getPaymentType(Convert.ToInt32(result.Result.AgreedPaymentMethodId));
            ViewBag.LingLastLogin = linguistData.getLastLogin(Id);
            ViewBag.LingExtranetUserName = linguistData.getExtranetName(Id);
            model.AgencyOrTeamName = result.Result.AgencyOrTeamName;
            model.VendorTypeID = result.Result.VendorTypeID;
            model.Referee1ReceivedString = result.Result.Referee1Received == null ? "false" : result.Result.Referee1Received.ToString();
            model.Referee2ReceivedString = result.Result.Referee2Received == null ? "false" : result.Result.Referee2Received.ToString();
            model.NeedApprovalToBeAddedToDb = result.Result.NeedApprovalToBeAddedToDb;
            if (model.NeedApprovalToBeAddedToDb == true)
            {
                model.FindLinguistByNameEmailAddOrSkype = linguistData.FindLinguistByNameEmailAddOrSkype(model.AgencyOrTeamName, model.MainContactFirstName, model.MainContactSurname, model.EmailAddress, model.SecondaryEmailAddress, model.SkypeId).Result;
            }
            var VendorContractPath = VendorContractFilePath(Id.ToString());
            model.VendorContractFileName = VendorContractPath.Split("$")[0];
            model.VendorContractName = VendorContractPath.Split("$")[1];
            model.VendorContractExist = VendorContractPath.Split("$")[2];
            model.LinguistPoliceChecks = await cachedService.getLingPoliceChecks();
            var CountryNames = await tpcontactService.GetContactCountries();
            model.CountryNamesAndPrefix = CountryNames;
            model.MainLandlineCountryId = result.Result.MainLandlineCountryId;
            model.SecondaryLandlineCountryId = result.Result.SecondaryLandlineCountryId;
            model.MobileCountryId = result.Result.MobileCountryId;
            model.LinguisticProfessionalBodies = await cachedService.getLingProfessionalBodies();
            model.ProfessionalBodyID = result.Result.ProfessionalBodyID;
            var currentDirectory = System.IO.Directory.GetCurrentDirectory();
            ViewBag.CurrentDirectory = currentDirectory.Replace('\\', '/') + "/wwwroot";
            model.linguisticSupplierPoliceCheckDetails = linguisticSupplierPoliceCheckService.GetAllChecks(result.Result.Id);
            return View("Views/Linguists/Linguist.cshtml", model);
        }

        [HttpPost("api/LinguistUpdate")]
        public async Task<bool> Update(ViewModels.LinguisticSupplier.LinguistViewModel model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var res = await linguistData.Update(model, LoggedInEmployee.Id);

            return true;
        }
        public byte[] LinguistProfileImage(string linguistID)
        {
            var GlobalVars = new GlobalVariables();

            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            byte[] ProfileImage = null;

            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "UserProfileImages", "myplus", "Suppliers", linguistID + ".png");
            if (System.IO.File.Exists(filePath))
            {
                ProfileImage = System.IO.File.ReadAllBytes(filePath);
            }

            return ProfileImage;
        }
        public string NDAFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var fileName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string NDAFileName = "";
            FileInfo[] NDAFileInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "NDA");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo NDADirectoryInfo = new DirectoryInfo(filePath);
                NDAFileInfos = NDADirectoryInfo.GetFiles("*.*");
                if (NDAFileInfos.Length > 0)
                {
                    NDAFileName = NDAFileInfos[0].FullName;
                    fileName = NDAFileInfos[0].Name;
                }
            }

            return NDAFileName + '$' + fileName;
        }
        public string ContractFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var contractName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string ContractFileName = "";
            FileInfo[] ContractFileInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "Contract");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                ContractFileInfos = ContractDirectoryInfo.GetFiles("*.*");
                if (ContractFileInfos.Length > 0)
                {
                    ContractFileName = ContractFileInfos[0].FullName;
                    contractName = ContractFileInfos[0].Name;
                }
            }

            return ContractFileName + '$' + contractName;
        }
        public string GDPRDocPath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var docName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string GDPRDocFilePath = "";
            FileInfo[] GDPRFileInfo;
            var SupplierDir = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID);
            if (System.IO.File.Exists(SupplierDir + @"\GDPR acceptance.pdf"))
            {
                GDPRDocFilePath = SupplierDir + @"\GDPR acceptance.pdf";
                docName = "GDPR acceptance.pdf";
            }
            else
            {
                if (Directory.Exists(SupplierDir + @"\GDPR acceptance.pdf"))
                {

                    GDPRFileInfo = new DirectoryInfo(SupplierDir + @"\GDPR acceptance.pdf").GetFiles();
                    GDPRDocFilePath = GDPRFileInfo[0].FullName;
                    docName = GDPRFileInfo[0].Name;
                }
            }

            return GDPRDocFilePath + '$' + docName;
        }
        public string CVFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var cvName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string CVFileName = "";
            string CVExist = "true";
            FileInfo[] CVFileInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "CV");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                CVFileInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (CVFileInfos.Length > 0)
                {
                    CVFileName = CVFileInfos[0].FullName;
                    cvName = CVFileInfos[0].Name;
                }
            }
            else
            {
                CVExist = "false";
            }
            return CVFileName + '$' + cvName + '$' + CVExist;
        }
        public string PortfolioFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var portfolioName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string PortfolioFileName = "";
            string PortfolioExist = "true";
            FileInfo[] PortfolioFileInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "Portfolio");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                PortfolioFileInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (PortfolioFileInfos.Length > 0)
                {
                    PortfolioFileName = PortfolioFileInfos[0].FullName;
                    portfolioName = PortfolioFileInfos[0].Name;
                }
            }
            else
            {
                PortfolioExist = "false";
            }
            return PortfolioFileName + '$' + portfolioName + '$' + PortfolioExist;
        }
        public string ProofIDFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var idName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string ProofIDName = "";
            string ProofIDExist = "true";
            FileInfo[] ProofIDInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "ProofID");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                ProofIDInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (ProofIDInfos.Length > 0)
                {
                    ProofIDName = ProofIDInfos[0].FullName;
                    idName = ProofIDInfos[0].Name;
                }
            }
            else
            {
                ProofIDExist = "false";
            }
            return ProofIDName + '$' + idName + '$' + ProofIDExist;
        }
        public string RegistrationFormFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var formName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string RegistrationFormName = "";
            string RegistrationFormExist = "true";
            FileInfo[] RegistrationFormInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "RegistrationForm");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                RegistrationFormInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (RegistrationFormInfos.Length > 0)
                {
                    RegistrationFormName = RegistrationFormInfos[0].FullName;
                    formName = RegistrationFormInfos[0].Name;
                }
            }
            else
            {
                RegistrationFormExist = "false";
            }
            return RegistrationFormName + '$' + formName + '$' + RegistrationFormExist;
        }
        public async Task<string> UploadCV(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "CV")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "CV");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "CV");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }


                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public async Task<string> UploadPortfolio(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "Portfolio")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "Portfolio");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "Portfolio");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public async Task<string> UploadRegistrationForm(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "RegistrationForm")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "RegistrationForm");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "RegistrationForm");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }


                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public async Task<string> UploadProofID(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "ProofID")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "ProofID");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "ProofID");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }


                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public async Task<string> UpdateLinguistExpertise()
        {

            string message = string.Empty;
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            LinguisticSupplier model = new LinguisticSupplier();
            var allFields = stringToProcess.Split("$");
            model.WebAddress = allFields[0];
            model.SubjectAreaSpecialismsAsDescribedBySupplier = allFields[1];
            model.MotherTongueLanguageIanacode = allFields[2];
            model.IfBilingualOtherMotherTongueIanacode = allFields[3];
            if (allFields[4] != "") { model.HasEncryptedComputer = Convert.ToBoolean(Convert.ToInt32(allFields[4])); }
            if (Convert.ToByte(allFields[5]) != 0) { model.PrimaryOperatingSystemId = Convert.ToByte(allFields[5]); }
            if (Convert.ToByte(allFields[6]) != 0) { model.PrimaryOperatingSystemVersionId = Convert.ToByte(allFields[6]); }
            model.Id = Convert.ToInt32(allFields[8]);
            var applications = allFields[7].Split(',').Where(o => o != "").ToList();
            var SoftwareApplications = linguistSupplierSoftwareApplicationRepository.AllAsNoTracking();
            var LinguistSupplierSoftwareApplication = SoftwareApplications.Where(o => o.LinguisticSupplierId == model.Id).Select(o => o.SoftwareApplicationId.ToString()).ToList();
            var newAddedList = applications.Where(i => !LinguistSupplierSoftwareApplication.Contains(i)).ToList();
            var newDeletedList = LinguistSupplierSoftwareApplication.Where(i => !applications.Contains(i)).ToList();
            model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
            model.MasterFieldID = allFields[9];
            model.SubFieldID = allFields[10];
            model.MediaTypeID = allFields[11];
            List<string> PoliceChecks;

            if (allFields[13] != "")
            {
                model.ProfessionalBodyID = allFields[13];
            }
            var result = await linguistData.UpdateLinguistExpertise(model);
            if (result != null)
            {
                message = "Success";
            }
            if (allFields[12] != "")
            {
                var policeChecks = allFields[12].Split("#");
                PoliceChecks = policeChecks.Where(x => x != "").ToList();
                if (PoliceChecks != null)
                {

                    foreach (var item in PoliceChecks)
                    {
                        var data = item.Split(",");
                        var checks = linguisticSupplierPoliceCheckService.GetAllChecks(model.Id);
                        if (checks.Any(o => o.PoliceCheckId == short.Parse(data[0])))
                        {
                            continue;
                        }
                        LinguisticSupplierPoliceCheckDetail detail = new LinguisticSupplierPoliceCheckDetail();
                        detail.LinguistId = model.Id;
                        detail.PoliceCheckId = short.Parse(data[0]);
                        detail.PoliceCheckIssueDate = Convert.ToDateTime(data[1]);
                        detail.PoliceCheckExpiryDate = Convert.ToDateTime(data[1]);
                        detail.LastModifiedByEmployeeId = LoggedInEmployee.Id;

                        await linguisticSupplierPoliceCheckService.AddPoliceCheck(detail);
                    }
                }
            }
            if (newAddedList.Any())
            {


                foreach (var item in newAddedList)
                {
                    LinguisticSupplierSoftwareApplication addedList = new LinguisticSupplierSoftwareApplication();
                    addedList.LinguisticSupplierId = model.Id;
                    addedList.SoftwareApplicationId = short.Parse(item);
                    var data = await linguistData.AddApplications(addedList);
                }
                message = "Success";
            }
            if (newDeletedList.Any())
            {


                foreach (var item in newDeletedList)
                {
                    LinguisticSupplierSoftwareApplication deletedList = new LinguisticSupplierSoftwareApplication();
                    deletedList.LinguisticSupplierId = model.Id;
                    deletedList.SoftwareApplicationId = short.Parse(item);
                    var data = await linguistData.DeleteApplications(deletedList);
                }
                message = "Success";
            }
            return message;
        }

        public async Task<string> UpdateLinguistInvoicing()
        {

            string message = string.Empty;
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            LinguisticSupplier model = new LinguisticSupplier();
            LinguisticSupplierInvoiceTemplate vmodel = new LinguisticSupplierInvoiceTemplate();
            var allFields = stringToProcess.Split("$");
            if (short.Parse(allFields[0]) != 0) { model.AgreedPaymentMethodId = short.Parse(allFields[0]); }
            if (short.Parse(allFields[1]) != 0) { model.CurrencyId = short.Parse(allFields[1]); }
            model.Vatnumber = allFields[2];
            model.Id = Convert.ToInt32(allFields[4]);
            model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
            vmodel.LinguisticSupplierId = Convert.ToInt32(allFields[4]);
            vmodel.Vatnumber = allFields[2];
            vmodel.BankBranchName = allFields[5];
            vmodel.BankBranchAddress = allFields[6];
            vmodel.BankBranchCity = allFields[7];
            var country = await cachedService.GetAllCountriesCached();
            vmodel.BankBranchCountry = country.Where(x => x.CountryName == allFields[8]).Select(x => x.Id).FirstOrDefault().ToString();
            vmodel.BankBranchPostCode = allFields[9];
            vmodel.BankAccountName = allFields[10];
            vmodel.BankAccountNumber = allFields[11];
            vmodel.BankAccountSwiftorBic = allFields[12];
            vmodel.BankAccountIban = allFields[13];
            var result = await linguistData.UpdateInvoicing(model);
            var resultBank = await invoicetemplateService.Update(vmodel);
            if (result != null && resultBank != null)
            {
                message = "Success";
            }
            if (allFields[3] != "")
            {
                var data = await linguistAltairservice.UpdateTINNumber(model.Id, allFields[3]);
                if (data != null)
                {
                    message = "Success";
                }
            }
            return message;
        }
        public async Task<string> UpdateLinguistStudioBreakdown(LinguisticSupplier model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
            string message = string.Empty;
            var result = await linguistData.UpdateLinguistStudioBreakdown(model);
            if (result != null)
            {
                message = "Success";
            }

            return message;
        }
        public PlanningCalendarAppointmentViewModel GetCalendarAppointment(DateTime date, string userName)
        {
            PlanningCalendarAppointmentViewModel model = new PlanningCalendarAppointmentViewModel();
            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlCommand SQLComm = new SqlCommand("procGetCalendarAppointment", SQLConn);

            SQLComm.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            SQLComm.Parameters.Add(new SqlParameter("@P_DATE", SqlDbType.DateTime)).Value = date;
            SQLComm.Parameters.Add(new SqlParameter("@P_EXTRANET_USERNAME", SqlDbType.NVarChar)).Value = userName;

            SqlDataReader SQLReader = null;

            try
            {
                SQLConn.Open();
                SQLReader = SQLComm.ExecuteReader();

                // Retrieve single result and assign data
                if ((SQLReader != null) && SQLReader.Read())
                {
                    model.Id = Convert.ToInt32(SQLReader["ID"]);
                    model.ExtranetUserName = Convert.ToString(SQLReader["ExtranetUserName"]);
                    model.StartDateTime = Convert.ToDateTime(SQLReader["StartDateTime"]);
                    model.EndDateTime = Convert.ToDateTime(SQLReader["EndDateTime"]);
                    model.CategoryName = Convert.ToString(SQLReader["CategoryName"]);
                    model.SubjectLine = Convert.ToString(SQLReader["SubjectLine"]);
                    model.Notes = Convert.ToString(SQLReader["Notes"]);
                }

            }
            finally
            {
                try
                {
                    if (SQLReader != null)
                    {
                        SQLReader.Close();
                    }
                    SQLConn.Dispose();
                }
                catch (SqlException se)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }

            return model;
        }
        public async Task<string> UploadProofBank(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "ProofBank")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "ProofBank");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "ProofBank");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }

                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public string ProofBankFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var idName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string ProofIDName = "";

            FileInfo[] ProofIDInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "ProofBank");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                ProofIDInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (ProofIDInfos.Length > 0)
                {
                    ProofIDName = ProofIDInfos[0].FullName;
                    idName = ProofIDInfos[0].Name;
                }
            }

            return ProofIDName + '$' + idName;
        }
        public async Task<string> UploadProofCompany(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "ProofCompany")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "ProofCompany");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "ProofCompany");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }


                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public string ProofCompanyFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var idName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string ProofIDName = "";

            FileInfo[] ProofIDInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "ProofCompany");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                ProofIDInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (ProofIDInfos.Length > 0)
                {
                    ProofIDName = ProofIDInfos[0].FullName;
                    idName = ProofIDInfos[0].Name;
                }
            }

            return ProofIDName + '$' + idName;
        }
        public async Task<string> UpdateLinguistClientSpecifcBreakdown(LinguisticSupplierRate model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            model.LastModifiedByEmployeeId = LoggedInEmployee.Id;
            string message = string.Empty;
            var result = await linguistData.UpdateLinguistClientSpecifcBreakdown(model);
            if (result != null)
            {
                message = "Success";
            }

            return message;
        }
        public async Task<IActionResult> Add()
        {

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (loggedInEmployee.TeamId != ((byte)Global_Settings.Enumerations.Teams.VendorManagement) && loggedInEmployee.Id != 1246)
            {
                return Redirect("/Page/Locked");
            }
            AddLinguistModel model = new AddLinguistModel();
            model.AllCountries = await cachedService.GetAllCountriesCached();
            model.linguistTypes = await cachedService.getLingTypes();
            model.linguistVendorTypes = await cachedService.getLingVendorTypes();
            var CountryNames = await tpcontactService.GetContactCountries();
            model.CountryNamesAndPrefix = CountryNames;
            return View("Views/Linguists/Add.cshtml", model);
        }
        public async Task<IActionResult> AddLinguist(LinguisticSupplier model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            model.CreatedByEmployeeId = LoggedInEmployee.Id;
            model.VendorTypeID = model.VendorTypeID.Remove(model.VendorTypeID.Length - 1);
            var result = await linguistData.AddLinguist(model);
            if (result != null)
            {
                string EmailRecipients = "LRCore@translateplus.com";
                string emailBody = string.Format(@"<p>New linguistic supplier <a href=""https://myplusbeta.publicisgroupe.net/Linguist?id={0}"">
                                                       {1} {2}</a> has been added by <a href=""https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{3}"">{4} {5}</a>.</p>",
                                                        result.Id, result.MainContactFirstName, result.MainContactSurname, LoggedInEmployee.Id, LoggedInEmployee.FirstName, LoggedInEmployee.Surname);

                emailUtils.SendMail("flow plus <flowplus@translateplus.com>", EmailRecipients,
                                       string.Format(@"New linguistic supplier added: {0} {1}", result.MainContactFirstName, result.MainContactSurname), emailBody);
            }

            return Ok(result.Id);
        }
        public async Task<string> UploadNDA(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "NDA")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "NDA");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "NDA");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                        Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
                        model.SectionToUpdate = "NDA";
                        await linguistData.Update(model, LoggedInEmployee.Id);

                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public async Task<string> UploadContract(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "Contract")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "Contract");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "Contract");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }

                        Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
                        model.SectionToUpdate = "Contract";
                        await linguistData.Update(model, LoggedInEmployee.Id);

                    }

                }
            }

            return filePath + '$' + fileName;
        }
        public string VendorContractFilePath(string linguistID)
        {
            var GlobalVars = new GlobalVariables();
            var vendorContractName = string.Empty;
            Configuration.GetSection(GlobalVariables.GlobalVars).Bind(GlobalVars);
            string VendorContractName = "";
            string VendorContractExist = "true";
            FileInfo[] VendorContractInfos;
            var filePath = Path.Combine(GlobalVars.ExtranetNetworkBaseDirectoryPath, "Suppliers", linguistID, "VendorContract");
            if (Directory.Exists(filePath))
            {
                DirectoryInfo ContractDirectoryInfo = new DirectoryInfo(filePath);
                VendorContractInfos = ContractDirectoryInfo.GetFiles("*.*").OrderByDescending(o => o.CreationTime).ToArray();

                if (VendorContractInfos.Length > 0)
                {
                    VendorContractName = VendorContractInfos[0].FullName;
                    vendorContractName = VendorContractInfos[0].Name;
                }
            }
            else
            {
                VendorContractExist = "false";
            }
            return VendorContractName + '$' + vendorContractName + '$' + VendorContractExist;
        }
        public async Task<string> UploadVendorContract(LinguistViewModel model)
        {

            var file = HttpContext.Request.Form.Files;
            var filePath = string.Empty;
            var fileName = string.Empty;
            if (file.Any())
            {
                if (file[0].Name == "VendorContract")
                {
                    var saveFile = file[0];

                    var config = new GlobalVariables();
                    Configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                    if (saveFile.ContentDisposition != "")
                    {
                        fileName = saveFile.FileName;
                        var path = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString(), "VendorContract");

                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Suppliers", model.Id.ToString()) + @"\" + "VendorContract");
                        }

                        filePath = Path.Combine(path, fileName);

                        using (var stream = new MemoryStream())
                        {
                            await saveFile.CopyToAsync(stream);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }

                }
            }

            return filePath + '$' + fileName;
        }
    }
}
