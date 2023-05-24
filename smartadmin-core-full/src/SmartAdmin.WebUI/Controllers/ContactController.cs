using System.Threading.Tasks;
using Data;
using Global_Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using ViewModels.Contact;
using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace SmartAdmin.WebUI.Controllers
{
    public class ContactController : Controller
    {
        private IConfiguration Configuration;


        private ITPContactsLogic tpcontactService;
        private readonly ITPriceListsService priceListService;
        private ITPEmployeesService tPEmployeesService;
        private ITPJobOrderService tpJOservice;
        private ITPOrgGroupsLogic tpGroupservice;
        private readonly ITPQuoteTemplatesService quoteTemplateService;
        private ITPOrgsLogic tPOrgsLogic;
        private readonly ITPExtranetUserService extranetUserService;
        private readonly ITPBrandsService brandsService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPExtranetAccessLevelService accessLevelService;
        private readonly ITPTimeZonesService timezoneService;
        private readonly UserManager<ExtranetUsersTemp> _userManager;
        private readonly ITPMiscResourceService miscResourceService;

        public ContactController(IConfiguration _configuration,
            ITPQuoteTemplatesService quoteTemplateService,
            ITPJobOrderService _tpJOservice,
            ITPContactsLogic _tpcontactService,
             ITPriceListsService priceListService,
            ITPEmployeesService _tPEmployeesService,
            ITPOrgsLogic _tPOrgsLogic,
            ITPOrgGroupsLogic _tpGroupservice,
            ITPExtranetUserService _extranetUserService,
            ITPBrandsService tPBrandsService,
            IEmailUtilsService emailUtilsService,
            ITPExtranetAccessLevelService _accessLevelService,
            ITPTimeZonesService _timezoneService,
            UserManager<ExtranetUsersTemp> userManager,
            ITPMiscResourceService _miscResourceService)
        {
            Configuration = _configuration;
            tpcontactService = _tpcontactService;
            tPEmployeesService = _tPEmployeesService;
            tPOrgsLogic = _tPOrgsLogic;
            tpJOservice = _tpJOservice;
            this.quoteTemplateService = quoteTemplateService;
            this.priceListService = priceListService;
            tpGroupservice = _tpGroupservice;
            extranetUserService = _extranetUserService;
            brandsService = tPBrandsService;
            emailService = emailUtilsService;
            this.accessLevelService = _accessLevelService;
            this.timezoneService = _timezoneService;
            this._userManager = userManager;
            this.miscResourceService = _miscResourceService;
        }


        public async Task<IActionResult> Index(int ContactID)
        {
            var model = new ViewModels.Contact.ContactViewModel();
            var result = tpcontactService.GetContactDetails(ContactID);



            model.Name = result.Result.Name;
            model.contactDeletedDate = result.Result.DeletedDate;
            model.Id = result.Result.Id;
            model.orgId = result.Result.OrgId;
            model.JobTitle = result.Result.JobTitle;
            model.Department = result.Result.Department;

            model.contactCreatedOn = result.Result.CreatedDate.ToString("dd MMMM yyyy HH:mm");
            model.contactModified = result.Result.LastModifiedDate?.ToString("dd MMMM yyyy HH:mm");
            model.contactCreatedBy = result.Result.CreatedByEmployeeId;

            var thisOrg = await tPOrgsLogic.GetOrgDetails(result.Result.OrgId);
            model.orgDeletedDate = thisOrg.DeletedDate;
            model.orgName = thisOrg.OrgName;
            model.orgDesignPlusStatus = thisOrg.DesignplusStatus;
            model.orgTranslateOnlineStatus = thisOrg.TranslateonlineStatus;
            model.orgDesignPlusRemainingUsers = await tPOrgsLogic.GetDesignPlusRemainingUsers(thisOrg.Id);
            model.orgTranslateOnlineRemainingUsers = await tPOrgsLogic.GetTranslateonlineRemainingUsers(thisOrg.Id);

            if (thisOrg.OrgGroupId != null)
            {
                var thisGroup = tpGroupservice.GetOrgGroupDetails((int)thisOrg.OrgGroupId);

                model.orgGroupName = thisGroup.Result.Name;
                model.orgGroupId = thisGroup.Result.Id;
                model.orgGroupDeletedDate = thisGroup.Result.DeletedDate;

            }


            model.notes = result.Result.Notes;

            model.LogoImageBase64 = thisOrg.LogoImageBase64;
            model.CustomerLogoImageBinary = thisOrg.CustomerLogoImageBinary;
            model.ApprovedHighLowMargin = result.Result.HighLowMarginApprovalNeeded;
            model.ExcludeZeroMargin = result.Result.ExcludeZeroMarginJobsFromApproval;
            model.NotifyOnlyWhenLastJob = result.Result.ExtranetOnlyNotifyOnDeliveryOfLastJobItem;
            model.ContacRelatedAlerts = result.Result.SpendFrequencyAlertDays;
            model.Notifications = result.Result.IncludeInNotificationsOn;
            model.NotificationEmails = result.Result.IncludeInNotifications;
            model.OptedInForMarketingCampaign = (bool)result.Result.OptedInForMarketingCampaign;

            model.iplusName = tpcontactService.GetExtranetUserName(ContactID);
            model.AllPriceLists = await priceListService.GetAllPriceListstForDataObjectType(model.Id, (int)Enumerations.DataObjectTypes.Contact, false);
            model.AllQuoteTemplates = await quoteTemplateService.GetQuoteTemplatesForDataObjectTypeAndId(model.Id, (int)Enumerations.DataObjectTypes.Contact);

            if (model.iplusName != "")
            {
                var extranetUser = await extranetUserService.GetExtranetUserByUsername(model.iplusName);
                var extranetAccessLevel = extranetUserService.GetAccessLevelInfo(model.iplusName).Result;

                if (extranetUser != null)
                {
                    var iplusUserObj = new ViewModels.Contact.iPlusUser();
                    iplusUserObj.username = model.iplusName;
                    var accessLevelObj = new ViewModels.Contact.ExtranetUserAccessLevel();
                    accessLevelObj.AccessLevelId = extranetUser.AccessLevelId;
                    accessLevelObj.AccessLevelName = extranetAccessLevel.Name;
                    iplusUserObj.UserAccessLevel = accessLevelObj;
                    iplusUserObj.translateOnlineEnabled = extranetUser.TranslateonlineAllowed;
                    iplusUserObj.designPlusEnabled = extranetUser.DesignplusEnabled;

                    model.extranetUserObj = iplusUserObj;

                }
                else
                {
                    var oldextranetUser = await extranetUserService.GetOldIPlusUser(model.iplusName);
                    extranetAccessLevel = extranetUserService.GetAccessLevelInfoForOldIplusUser(model.iplusName).Result;

                    if (oldextranetUser != null)
                    {
                        var iplusUserObj = new ViewModels.Contact.iPlusUser();
                        iplusUserObj.username = model.iplusName;
                        var accessLevelObj = new ViewModels.Contact.ExtranetUserAccessLevel();
                        accessLevelObj.AccessLevelId = oldextranetUser.AccessLevelId;
                        accessLevelObj.AccessLevelName = extranetAccessLevel.Name;
                        iplusUserObj.UserAccessLevel = accessLevelObj;
                        iplusUserObj.translateOnlineEnabled = oldextranetUser.TranslateonlineAllowed;
                        iplusUserObj.designPlusEnabled = oldextranetUser.DesignplusEnabled;

                        model.extranetUserObj = iplusUserObj;
                    }

                }


            }

            else
            {
                model.AllAvailableAccess = await accessLevelService.GetAccessLevelsForClientAsync(thisOrg.Id);
                model.AllTimeZones = timezoneService.GetAllTimeZonesForDisplay();
            }


            if (result.Result.TpintroductionSource == null)
            {
                model.TPIntroductionSource = 15;
            }
            else
            {
                model.TPIntroductionSource = result.Result.TpintroductionSource;
            }
            var contactSource = await tpcontactService.GetIntroductionSource();
            model.TPIntroductionSourceList = new SelectList(contactSource, "ID", "SourceName");
            model.TPIntroductionSourceListIdAndName = contactSource;
            model.ApprovedOrBlockedLinguists = await tPOrgsLogic.GetApprovedOrBlockedLinguists(model.Id, ((int)Enumerations.DataObjectTypes.Contact));


            var CountryNames = await tpcontactService.GetContactCountries();
            model.CountryList = new SelectList(CountryNames, "ID", "CountryName");
            model.CountryNamesAndPrefix = CountryNames;

            if (result.Result.LastModifiedByEmployeeId is not null)
            {
                model.contactModifiedBy = result.Result.LastModifiedByEmployeeId;
                var thisEmployeeModified = tPEmployeesService.IdentifyCurrentUserByIdTerminate((int)model.contactModifiedBy);
                model.modifiedByImageBase64 = thisEmployeeModified.Result.ImageBase64;
                model.contactModifiedByName = thisEmployeeModified.Result.FirstName + ' ' + thisEmployeeModified.Result.Surname;
            }


            Employee LoggedInEmployee = await tPEmployeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            model.EmployeCurrentlyLoggedInID = LoggedInEmployee.Id;
            var thisEmployeeCreated = tPEmployeesService.IdentifyCurrentUserByIdTerminate(model.contactCreatedBy);

            model.contactCreatedByName = thisEmployeeCreated.Result.FirstName + ' ' + thisEmployeeCreated.Result.Surname;
            model.createByImageBase64 = thisEmployeeCreated.Result.ImageBase64;


            if (result.Result.Gdprstatus == 1) { model.GDPRStatus = "Actively opted-in"; }
            else if (result.Result.Gdprstatus == 2) { model.GDPRStatus = "Actively opted-out"; }
            else if (result.Result.Gdprstatus == 3) { model.GDPRStatus = "New pending addition"; }
            else if (result.Result.Gdprstatus == 4) { model.GDPRStatus = "Okay to contact(existing client)"; }
            else if (result.Result.Gdprstatus == 5) { model.GDPRStatus = "The GDPR was once verbally accepted by client, but now it has been more than 6 months since verbal approval(therefore the GDPRAcceptedDateTime has been blanked out)"; }
            else if (result.Result.Gdprstatus == 0) { model.GDPRStatus = "N/A"; }
            model.GDPRStatusId = result.Result.Gdprstatus;
            model.FaxCountryID = result.Result.FaxCountryId;
            model.LandlineCountryID = result.Result.LandlineCountryId;
            model.MobileCountryID = result.Result.MobileCountryId;
            model.FaxNumber = result.Result.FaxNumber;
            model.LandlineNumber = result.Result.LandlineNumber;
            model.EmailAddress = result.Result.EmailAddress;
            model.MobileNumber = result.Result.MobileNumber;

            int? GroupID = tpcontactService.GetContactOrgGroup(ContactID);
            model.GroupID = GroupID;
            var loggedInEmployeeDept = await tPEmployeesService.GetEmployeeDepartment(LoggedInEmployee.Id);
            bool accessToEditContact = false;
            if (model.GroupID == 18520)
            {
                //grant access
                accessToEditContact = true;
            }
            else
            {
                var CurrentSalesDepartmentManager = await tPEmployeesService.CurrentSalesDepartmentManager();
                if (LoggedInEmployee.TeamId == 5 || LoggedInEmployee.TeamId == 16 || LoggedInEmployee.TeamId == 38 ||
                    (await tPEmployeesService.AttendsBoardMeetings(LoggedInEmployee.Id) == true && LoggedInEmployee.Id != CurrentSalesDepartmentManager.Id))
                {
                    accessToEditContact = false;
                }
                else
                {
                    accessToEditContact = true;
                }

            }
            bool accessToAddJobOrder = false;
            if (model.contactDeletedDate == null)
            {
                if (model.GroupID == 18520)
                {
                    //grant access
                    accessToAddJobOrder = true;
                }
                else
                {
                    var CurrentSalesDepartmentManager = await tPEmployeesService.CurrentSalesDepartmentManager();
                    if (LoggedInEmployee.TeamId == 5 || LoggedInEmployee.TeamId == 16 || LoggedInEmployee.TeamId == 13 || LoggedInEmployee.TeamId == 38 ||
                        await tPEmployeesService.AttendsBoardMeetings(LoggedInEmployee.Id) == true || loggedInEmployeeDept.Id == (byte)Enumerations.Departments.SalesAndMarketing)
                    {
                        accessToAddJobOrder = false;
                    }
                    else
                    {
                        accessToAddJobOrder = true;
                    }

                }
            }
            bool accessToEditPriceList = false;
            if (model.contactDeletedDate == null)
            {
                if (model.GroupID == 18520)
                {
                    //grant access
                    accessToEditPriceList = true;
                }
                else
                {
                    var CurrentSalesDepartmentManager = await tPEmployeesService.CurrentSalesDepartmentManager();
                    if (LoggedInEmployee.TeamId == 5 || LoggedInEmployee.TeamId == 16 || LoggedInEmployee.TeamId == 13 || LoggedInEmployee.TeamId == 38 ||
                        await tPEmployeesService.AttendsBoardMeetings(LoggedInEmployee.Id) == true || loggedInEmployeeDept.Id == (byte)Enumerations.Departments.GeneralLinguisticServices || loggedInEmployeeDept.Id == (byte)Enumerations.Departments.TranscreationAndProduction)
                    {
                        accessToEditPriceList = false;
                    }
                    else
                    {
                        accessToEditPriceList = true;
                    }

                }
            }
            model.AccessToEditContact = accessToEditContact;
            model.AccessToAddJobOrder = accessToAddJobOrder;
            model.AccessToEditPriceList = accessToEditPriceList;
            model.AccessToEditHighLowMargin = false;
            model.AccessToEditExcludeZeroMargin = false;

            if (LoggedInEmployee.Id == 41 || LoggedInEmployee.Id == 475)
            {
                model.AccessToEditHighLowMargin = true;
                if (model.ApprovedHighLowMargin == true)
                {
                    model.AccessToEditExcludeZeroMargin = true;
                }
            }
            else if ((loggedInEmployeeDept.Id == (byte)Enumerations.Departments.GeneralLinguisticServices || loggedInEmployeeDept.Id == (byte)Enumerations.Departments.TranscreationAndProduction) && LoggedInEmployee.IsTeamManager == true)
            {
                model.AccessToEditHighLowMargin = true;
                if (model.ApprovedHighLowMargin == true)
                {
                    model.AccessToEditExcludeZeroMargin = true;
                }
            }


            return View("Views/flow plus/Contact.cshtml", model);
        }

        [HttpPost("api/ContactUpdate")]
        public async Task<bool> Update(ContactViewModel model)
        {

            var res = await tpcontactService.Update(model);

            return true;
        }

        [HttpPost]
        public async Task<IActionResult> EnableIPlusAccess()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var accessLevelId = Convert.ToInt32(stringToProcess.Split("$")[0]);
            var timeZone = stringToProcess.Split("$")[1];
            var translateOnlineEnabled = Convert.ToBoolean(stringToProcess.Split("$")[2]);
            var designPlusEnabled = Convert.ToBoolean(stringToProcess.Split("$")[3]);
            var contactId = Convert.ToInt32(stringToProcess.Split("$")[4]);

            string randomPassword = Guid.NewGuid().ToString() + "TP$$";

            Employee loggedInEmployee = await tPEmployeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var oldExtranetUser = await extranetUserService.EnableAsUser(1, contactId, randomPassword, accessLevelId, timeZone, translateOnlineEnabled, designPlusEnabled, loggedInEmployee.Id);

            //create user in new ExtranetUser table
            var user = new ExtranetUsersTemp { UserName = oldExtranetUser.UserName, Email = oldExtranetUser.UserName };
            var result = await _userManager.CreateAsync(user, randomPassword);
            if (result.Succeeded)
            {
                await extranetUserService.UpdateflowPlusExternalDetails(oldExtranetUser.UserName);
            }

            var code = Guid.NewGuid();

            await extranetUserService.UpdatePasswordResetCode(oldExtranetUser.UserName, code);

            var thisBrand = await brandsService.GetBrandById(1);

            var contactObject = await tpcontactService.GetById(contactId);

            var currentOrg = await tPOrgsLogic.GetOrgDetails(contactObject.OrgId);


            if (currentOrg.OrgGroupId != null)
            {
                thisBrand = await brandsService.GetBrandForClient(currentOrg.OrgGroupId.Value);
            }

            var flowPlusExternalPswrdLink = "https://flowplus.translateplus.com/Identity/Account/CreatePassword?code=" + code.ToString() + "&username=" + oldExtranetUser.UserName;
            //string EmailBody = await miscResourceService.GetMiscResourceByName("CreateNewIPlusAccountEmailBody", "en").Result.StringContent;
            string EmailBody = "<p>Dear {0}, <br /><br />You now have an account with <b>i plus</b>, " +
                                            "providing 24/7 secure access to language services under an arrangement between {1} and {tp brand}. " +
                                            "<br /><br />To access your account, visit <a href=\"https://iplus.{tpbrand}.com\">https://iplus.{tpbrand}.com</a>." +
                                            "<br /><br /><b>User name:</b> {2}" +
                                            "<br /><br />For security reasons, you will be required to create your password using below link.<br/><br/>{3}" +
                                            "<br /><br />If you copy and paste the link from this message, please take care to ensure you select only the text of the link itself, avoiding any trailing spaces or tab characters." +
                                            "<br /><br />If you have any questions about i plus, please contact {4} via {5} or contact {tp brand} directly (our contact details are shown below)." +
                                            " <br /><br />Yours sincerely, <br /><br />The <b>i plus</b> team <br /><a href=\"https://iplus.{tpbrand}.com\">https://iplus.{tpbrand}.com</a></p>";

            EmailBody = EmailBody.Replace("{tp brand}", thisBrand.CompanyNameToShow).Replace("i plus", thisBrand.ApplicationName).Replace("iplus.{tpbrand}.com", thisBrand.DomainName);

            EmailBody = String.Format(EmailBody, contactObject.Name, currentOrg.OrgName, oldExtranetUser.UserName, flowPlusExternalPswrdLink,
                                             loggedInEmployee.FirstName + " " + loggedInEmployee.Surname,
                                             loggedInEmployee.EmailAddress);

            string ExtranetUserLangIANA = "en";

            if (oldExtranetUser.PreferredExtranetUilangIanacode != null)
            {
                ExtranetUserLangIANA = oldExtranetUser.PreferredExtranetUilangIanacode;
            }

            string SubjectLine = String.Format(miscResourceService.GetMiscResourceByName("NewUserAddedMsgSubj", ExtranetUserLangIANA).Result.StringContent.Replace("i plus", thisBrand.ApplicationName)
                                            , loggedInEmployee.FirstName + " " + loggedInEmployee.Surname);

            //for TRANSLATE accounts, make subject is grammatically correct
            if (ExtranetUserLangIANA == "en" && SubjectLine.Contains("an TRANSLATE") == true)
            {
                SubjectLine = SubjectLine.Replace("an TRANSLATE", "a TRANSLATE");
            }

            else if (ExtranetUserLangIANA == "de" && SubjectLine.Contains("TRANSLATE Konto") == true)
            {
                SubjectLine = SubjectLine.Replace("TRANSLATE Konto", "TRANSLATE-Konto");
            }

            else if (ExtranetUserLangIANA == "da" && SubjectLine.Contains("TRANSLATE konto") == true)
            {
                SubjectLine = SubjectLine.Replace("TRANSLATE konto", "TRANSLATE-konto");
            }



            emailService.SendMail("flow plus <flowplus@translateplus.com>", contactObject.EmailAddress, SubjectLine,
                                        EmailBody, CCRecipients: loggedInEmployee.EmailAddress,
                                        IsExternalNotification:true);

            return Ok(oldExtranetUser.UserName);

        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordResetEmail()
        {

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var username = content.Result;

            var code = Guid.NewGuid();

            var user = await extranetUserService.GetExtranetUserByUsername(username);
            var oldUser = await extranetUserService.GetOldIPlusUser(username);
            bool sendmail = false;
            int contactId = 0;
            if (user == null)
            {
                await extranetUserService.UpdatePasswordResetCodeForOldIplus(username, code);
                contactId = oldUser.DataObjectId;
                sendmail = true;
            }
            else
            {
                await extranetUserService.UpdatePasswordResetCode(username, code);
                contactId = user.DataObjectId;
                sendmail = true;
            }

            if (sendmail == true)
            {
                var contactObject = await tpcontactService.GetById(contactId);

                var currentOrg = await tPOrgsLogic.GetOrgDetails(contactObject.OrgId);

                var currentBrand = await brandsService.GetBrandById(1);

                if (currentOrg.OrgGroupId != null)
                {
                    currentBrand = await brandsService.GetBrandForClient(currentOrg.OrgGroupId.Value);
                }

                var flowPlusExternalPswrdResetLink = "https://flowplus.translateplus.com/Identity/Account/ResetPassword?code=" + code.ToString() + "&username=" + username + "&returnLink=iplus";
                //string EmailBody = await miscResourceService.GetMiscResourceByName("ResetPasswordEmailBody", "en").Result.StringContent;
                string EmailBody = String.Format("<p>Dear {0}, <br/><br/>We have created a request to reset your <b>i plus</b> account password.<br/><br/>" +
                                                "Please click the link below to reset your password<br/><br/>" +
                                                "<a href=\"{1}\">{1}</a><br/><br/>" +
                                                "If you copy and paste this link, please take care to ensure you select only the link itself, avoiding any trailing spaces or tab characters." +
                                                "<br /><br />Yours sincerely, <br /><br />The <b>i plus</b> team</p>",
                                                contactObject.Name, flowPlusExternalPswrdResetLink);

                Employee loggedInEmployee = await tPEmployeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

                emailService.SendMail("flow plus <flowplus@translateplus.com>", contactObject.EmailAddress, "Your password is ready to reset",
                                            EmailBody, CCRecipients: loggedInEmployee.EmailAddress,
                                            IsExternalNotification:true);
            }


            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllContactsForOrg()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var orgId = Int32.Parse(stringToProcess);

            var allContactStrings = await tpcontactService.GetAllContactIdAndNameStringForOrg(orgId);

            return Ok(allContactStrings);

        }
    }
}
