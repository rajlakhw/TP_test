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
using ViewModels.Common;
using System.Collections;
using System.Xml;

namespace SmartAdmin.WebUI.Controllers
{
    public class JobItemController : Controller
    {
        //private readonly int[] employeeAccessForTesting = new int[] { 31, 1229, 1269, 1308, 1324, 638, 646, 1060, 1171, 1184, 1275, 1298, 1340, 1109 };
        private readonly ITPJobOrderService service;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPJobItemService jobItemService;
        private readonly ICommonCachedService cachedService;
        private readonly ITPCurrenciesLogic currenciesService;
        private readonly IPGDService pgdService;
        private readonly ITPLinguistService linguistService;
        private readonly ITPClientInvoicesLogic clientInvoicesLogic;
        private readonly IOpsDashboardService opsDashboardService;
        private readonly ITPContactsLogic contactsService;
        private readonly ITPOrgsLogic orgsService;
        private readonly ITPJobOrderService jobordersService;
        private readonly ITPLinguisticSupplierInvoiceTemplate invoicetemplateService;
        private readonly ITPOrgGroupsLogic orgGroupsLogic;
        private readonly ITPLanguageService languageService;
        private readonly ITPFileSystemService fileSystemService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPWordCountBreakdownBatch wordCountBreakdownBatchService;

        public JobItemController(ITPJobOrderService service, ITPJobItemService jobItemService,
            ICommonCachedService cachedService,
            ITPCurrenciesLogic currenciesService,
            IPGDService pgdService,
            ITPEmployeesService employeesService,
            ITPLinguistService linguistService,
            ITPClientInvoicesLogic clientInvoicesLogic,
            IOpsDashboardService opsDashboardService,
            ITPContactsLogic contactsService,
            ITPOrgsLogic orgsService,
            ITPJobOrderService jobordersService,
            ITPOrgGroupsLogic orgGroupsLogic,
            ITPLanguageService languageService,
            ITPLinguisticSupplierInvoiceTemplate invoicetemplateService,
            ITPFileSystemService fileSystemService,
            IEmailUtilsService emailUtilsService, ITPWordCountBreakdownBatch wordCountBreakdownBatchService)
        {
            this.service = service;
            this.jobItemService = jobItemService;
            this.cachedService = cachedService;
            this.currenciesService = currenciesService;
            this.pgdService = pgdService;
            this.employeesService = employeesService;
            this.linguistService = linguistService;
            this.clientInvoicesLogic = clientInvoicesLogic;
            this.opsDashboardService = opsDashboardService;
            this.contactsService = contactsService;
            this.orgsService = orgsService;
            this.jobordersService = jobordersService;
            this.orgGroupsLogic = orgGroupsLogic;
            this.languageService = languageService;
            this.invoicetemplateService = invoicetemplateService;
            this.fileSystemService = fileSystemService;
            emailService = emailUtilsService;
            this.wordCountBreakdownBatchService = wordCountBreakdownBatchService;
        }

        public async Task<IActionResult> Index(int id)
        {
            if (id == 0)
                return NotFound();

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var item = await jobItemService.GetViewModelById(id);

            if (item == null)
                return Content("The Job item ID you provided (" + id + ") is not a valid number.");

            if (loggedInEmployee == null)
                return NotFound();

            //if (loggedInEmployee.AccessLevel == 4 || employeeAccessForTesting.Contains(loggedInEmployee.Id) || loggedInEmployee.TeamId == 10 || loggedInEmployee.TeamId == 11 || loggedInEmployee.TeamId == 15 || loggedInEmployee.TeamId == 36 || loggedInEmployee.TeamId == 40 || loggedInEmployee.TeamId == 41)
            //{

            item.LoggedInEmployeeId = loggedInEmployee.Id;
            item.Countries = await cachedService.GetAllCountriesCached();
            item.Languages = (await cachedService.GetAllLanguagesCached()).OrderBy(o=>o.Name).ToList();
            item.LanguageServices = await cachedService.GetAllLanguageServicesCached();
            item.LanguageServiceCategory = await cachedService.getLanguageServiceCategory();
            item.PaymentMethods = await cachedService.GetAllPaymentMethodsCached();
            item.Currencies = await currenciesService.GetAllENCurrencies();
            item.AdditionalDetails.OrderCurrencyName = item.Currencies.Where(x => x.Id == item.AdditionalDetails.OrderCurrencyId).FirstOrDefault().Name;

            ViewBag.IsACMSProject = 0;
            var joborder = await jobordersService.GetById(item.JobOrderId);
            if (joborder != null)
            {
                if (joborder.IsAcmsproject == true)
                {
                    ViewBag.IsACMSProject = 1;
                }
            }
            var contact = await contactsService.GetContactDetails(joborder.ContactId);
            var org = await orgsService.GetOrgDetails(contact.OrgId);
            if ((item.AdditionalDetails.IsCLSJob == true || item.AdditionalDetails.TypeOfOrder == 3 || item.AdditionalDetails.TypeOfOrder == 4 || item.AdditionalDetails.TypeOfOrder == 5))
            {
                var VOdropdowns = await pgdService.GetAllVODropdownLists();

                //string serviceName = Regex.Replace(item.PGDDetails.Service, @"\s+", "");
                //var VOdropdown = VOdropdowns.Where(x => x.VodropdownListName.ToLower() == serviceName.ToLower()).FirstOrDefault();

                //int VOdopdownId = 0;
                //if (VOdropdown != null)
                //    VOdopdownId = VOdropdown.Id;

                var dropdownListItems = await pgdService.GetAllVODropdownListsItems();
                item.AllPGDDropdownItems = dropdownListItems;
                // if (VOdopdownId > 0)
                item.PGDDropdownItems = dropdownListItems.Where(x => x.VodropdownListId == 12).Select(x => new ViewModels.Common.DropdownOptionViewModel() { Id = x.Id, Name = x.Value, StringValue = x.Value });

                item.PGDServices = dropdownListItems.Where(x => x.VodropdownListId == 2).Select(x => new ViewModels.Common.DropdownOptionViewModel() { Id = x.Id, Name = x.Value });
            }

            if (item.LanguageServiceId == 2 || item.LanguageServiceId == 3 || item.LanguageServiceId == 10)
                item.AdditionalDetails.IsInterpreting = true;

            var jobItemInvoice = await linguistService.GetSupplierInvoiceFromJobItemId(item.Id);
            ViewBag.JobItemInvoice = jobItemInvoice;
            var clientInvoice = await clientInvoicesLogic.GetViewModelById(item.AdditionalDetails.ClientInvoiceId);

            if (item.SupplierCompletedItemDateTime != null && jobItemInvoice == null)
                item.SupplierInvoiceFieldsEnabled = true;

            var department = await opsDashboardService.GetDepartmentFromTeamId(loggedInEmployee.TeamId);
            bool hasClientReviewer = false;

            if (item.SupplierIsClientReviewer != null && item.SupplierIsClientReviewer == false && item.LinguisticSupplierOrClientReviewerId != null)
                hasClientReviewer = true;

            if (jobItemInvoice == null)
            {
                if (hasClientReviewer || department.Id == ((byte)Enumerations.Departments.CompanyDirectors) ||
                    ((department.Id == ((byte)Enumerations.Departments.GeneralLinguisticServices) ||
                    department.Id == ((byte)Enumerations.Departments.TranscreationAndProduction)) &&
                    loggedInEmployee.IsTeamManager == true) ||
                    loggedInEmployee.TeamId == ((byte)Enumerations.Teams.OperationsManagement) ||
                    loggedInEmployee.TeamId == ((byte)Enumerations.Teams.TPOperationsManagement) ||
                    loggedInEmployee.Id == await employeesService.GetCurrentLinguisticResourcesTeamManagerID())
                {
                    item.SupplierInvoiceCurrencyEnabled = true;
                }
            }

            if (item.Category == Enumerations.ServiceCategory.ClientReview || item.LanguageServiceId == 67)
            {
                item.ShowSignOffComments = true;
                var ExtranetUser = await contactsService.GetExtranetUser(item.AdditionalDetails.ContactId);

                if (item.AdditionalDetails.JobOrderChannelId == 21 &&
                    item.WeCompletedItemDateTime == null &&
                    ExtranetUser.UserName != null &&
                    (ExtranetUser.DesignplusEnabled == true || item.AdditionalDetails.ContactEmailAddress.Contains("@translateplus.com") == true) &&
                    item.AdditionalDetails.JobOrderDesignPlusFileId != null &&
                    item.AdditionalDetails.JobOrderDesignPlusFileId > 0 &&
                    item.LanguageServiceId == 21)
                {
                    var reviewersList = await orgsService.GetAllExtranetUsersForOrg(item.AdditionalDetails.OrgId);
                    item.ClientReviewers = new System.Collections.Generic.List<ViewModels.Contact.ContactModel>();

                    foreach (var reviewer in reviewersList)
                    {
                        if (reviewer.ExtranetUserName != null && reviewer.ExtranetUserName != "" ||
                            reviewer.ExtranetUserAccessLevel.CanReviewOwnJobsInOwnLanguageCombos == true ||
                            reviewer.ExtranetUserAccessLevel.CanReviewOtherOrgJobsInOwnLanguageCombos == true ||
                            reviewer.ExtranetUserAccessLevel.CanReviewOtherGroupJobsInOwnLanguageCombos == true ||
                            reviewer.ExtranetUserAccessLevel.CanReviewOtherGroupJobsInAnyLanguageCombo == true)
                        {
                            if (reviewer.ExtranetUserDesignPlusEnabled == true)
                                item.ClientReviewers.Add(reviewer);
                        }
                    }
                }
            }

            bool hasLinguisticSupplier = item.SupplierIsClientReviewer != null && item.SupplierIsClientReviewer == true && item.LinguisticSupplierOrClientReviewerId != null;

            //if ((item.WeCompletedItemDateTime == null && item.SupplierCompletedItemDateTime != null && item.SupplierAcceptedWorkDateTime != null &&
            //    item.SupplierSentWorkDateTime != null && item.SupplierCompletionDeadline != null && (item.LanguageServiceId == 21 || item.LanguageServiceId == 67) &&
            //    CheckPGDStatus() == true) || (item.WeCompletedItemDateTime == null && item.SupplierCompletedItemDateTime != null &&
            //    item.SupplierAcceptedWorkDateTime != null && item.SupplierSentWorkDateTime != null && item.SupplierCompletionDeadline != null &&
            //    (item.LanguageServiceId != 21 && item.LanguageServiceId != 67) && (hasLinguisticSupplier == true || hasClientReviewer == true) &&
            //    jobItemInvoice == null && CheckPGDStatus() == true) ||
            //    (item.WeCompletedItemDateTime != null && item.AdditionalDetails.OrderOverallCompletedDateTime == null && jobItemInvoice == null && CheckPGDStatus() == true))
            //{
            //    item.CompleteCheckboxEnabled = true;
            //}
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            var emp = await employeesService.GetEmployeeByUsername(username);
            bool isAllowedToEdit = IsAllowedToEdit();
            if (org.OrgGroupId != 18520)
            {
                if (loggedInEmployee.TeamId == 5 || loggedInEmployee.TeamId == 16 || loggedInEmployee.TeamId == 13 || loggedInEmployee.TeamId == 38 || department.Id == 1 || emp.AttendsBoardMeetings == true)
                {
                    isAllowedToEdit = false;
                }
            }
            if ((item.WeCompletedItemDateTime == null && (item.LanguageServiceId == 21 || item.LanguageServiceId == 67) &&
                    CheckPGDStatus() == true) || (item.WeCompletedItemDateTime == null &&
                    (item.LanguageServiceId != 21 && item.LanguageServiceId != 67) && (hasLinguisticSupplier == true || hasClientReviewer == true) &&
                    jobItemInvoice == null && CheckPGDStatus() == true) ||
                    (item.WeCompletedItemDateTime != null && item.AdditionalDetails.OrderOverallCompletedDateTime == null && jobItemInvoice == null && CheckPGDStatus() == true))
            {
                item.CompleteCheckboxEnabled = true;
            }

            if ((jobItemInvoice == null || (clientInvoice == null || (clientInvoice != null && clientInvoice.IsFinalised == false))) &&
                   (item.AdditionalDetails.OrgGroupId != 18523 || (item.AdditionalDetails.OrgGroupId == 18523 && item.AdditionalDetails.OrderOnHold == false)) &&
                   (item.LanguageServiceId == 21 || item.LanguageServiceId == 67 || item.AdditionalDetails.OrderOverallCompletedDateTime == null) &&
                   isAllowedToEdit == true)
            {
                item.EditPageEnabled = true;
            }

            if ((clientInvoice == null || (clientInvoice != null && clientInvoice.IsFinalised == false)) && (item.AdditionalDetails.EarlyInvoiceDateTime == null || item.AdditionalDetails.EarlyInvoiceDateTime != null &&
                    item.ChargeToClient == 0) && isAllowedToEdit == true)
            {
                item.DeleteButtonEnabled = true;
            }


            if (item.SupplierIsClientReviewer == true && item.LinguisticSupplierOrClientReviewerId != null)
            {
                var list = await contactsService.SearchByNameOrId(item.LinguisticSupplierOrClientReviewerId.ToString());
                if (list.FirstOrDefault() != null)
                    item.SupplierOrContactName = list.FirstOrDefault().Name;
            }
            else
            {
                if (item.SupplierIsClientReviewer == false && item.LinguisticSupplierOrClientReviewerId != null)
                {
                    var list = await linguistService.SearchByNameOrIdOnly(item.LinguisticSupplierOrClientReviewerId.ToString());
                    if (list.FirstOrDefault() != null)
                    {
                        if (list.FirstOrDefault().AgencyOrTeamName != null && list.FirstOrDefault().AgencyOrTeamName != "")
                        {
                            item.SupplierOrContactName = list.FirstOrDefault().AgencyOrTeamName;
                        }
                        else
                        {
                            if (list.FirstOrDefault().MainContactFirstName != null && list.FirstOrDefault().MainContactFirstName != "")
                                item.SupplierOrContactName = list.FirstOrDefault().MainContactFirstName + " " + list.FirstOrDefault().MainContactSurname;
                        }
                    }


                }
                else if (item.LinguisticSupplierOrClientReviewerId != null)
                {
                    var contactslist = await contactsService.SearchByNameOrId(item.LinguisticSupplierOrClientReviewerId.ToString());
                    if (contactslist.FirstOrDefault() != null)
                        item.SupplierOrContactName = contactslist.FirstOrDefault().Name;
                }
            }

            //}
            //else
            //{
            //    return Redirect("/Page/Locked");
            //}
            ViewBag.IsAllowedToEdit = isAllowedToEdit;
            ViewBag.OrderOverallCompletedDateTime = item.AdditionalDetails.OrderOverallCompletedDateTime;
            return View(item);
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<int> Update(JobItemUpdateModel updateModel)
        {
            var item = await jobItemService.GetViewModelById(updateModel.Item.Id);
            bool redirttoratelinguist = false;
            if (updateModel.Item.IsCompleted == true)
            {
                if (item.WeCompletedItemDateTime == null || item.WeCompletedItemDateTime == DateTime.MinValue)
                {
                    if (item.SupplierIsClientReviewer == false && (item.LinguisticSupplierOrClientReviewerId != 19568 && item.LinguisticSupplierOrClientReviewerId != 19833 && item.LinguisticSupplierOrClientReviewerId != 11256 && item.LinguisticSupplierOrClientReviewerId != 13983 && item.LinguisticSupplierOrClientReviewerId != 14746 && item.LinguisticSupplierOrClientReviewerId != 15488))
                    {
                        redirttoratelinguist = true;
                    }
                }
            }

            if (updateModel.Item.SupplierIsClientReviewer == false && (updateModel.Item.LinguisticSupplierOrClientReviewerId != null && updateModel.Item.LinguisticSupplierOrClientReviewerId != 0) && ((item.LinguisticSupplierOrClientReviewerId == null || updateModel.Item.LinguisticSupplierOrClientReviewerId == 0) || ((int)updateModel.Item.LinguisticSupplierOrClientReviewerId != (int)item.LinguisticSupplierOrClientReviewerId)))
            {
                var linguist = await linguistService.GetById((int)updateModel.Item.LinguisticSupplierOrClientReviewerId);

                if (linguist != null)
                {
                    if (linguist.CurrencyId != null)
                    {
                        updateModel.Item.PaymentToSupplierCurrencyId = linguist.CurrencyId;
                    }
                }
            }
            string warning = "";
            string pgdbuyoutwarning = "";
            string pgdcopydeckwarning = "";
            string linguistNDAwarning = "";
            string linguistContractwarning = "";
            if (updateModel.Item.LinguisticSupplierOrClientReviewerId > 0)
            {
                var Linguist = await linguistService.GetById((int)updateModel.Item.LinguisticSupplierOrClientReviewerId);
                if (Linguist != null)
                {
                    if (Linguist.NdauploadedDateTime == null && Linguist.NdaunlockedDateTime == null)
                    {
                        linguistNDAwarning += "This supplier has not signed an NDA, so they cannot currently be used.<br />";
                    }
                    if (Linguist.ContractUploadedDateTime == null && Linguist.ContractUnlockedDateTime == null)
                    {
                        linguistContractwarning += "This supplier has not signed a contract/service agreement, so they cannot currently be used.<br />";
                    }
                }
            }
            if (updateModel.Item.IsCompleted == true && (item.WeCompletedItemDateTime == null || item.WeCompletedItemDateTime == DateTime.MinValue))
            {

                if (item.SupplierSentWorkDateTime == null || item.SupplierSentWorkDateTime == DateTime.MinValue)
                {
                    warning += "- Supplier has not beem sent the work<br />";
                }

                if (item.SupplierAcceptedWorkDateTime == null || item.SupplierAcceptedWorkDateTime == DateTime.MinValue)
                {
                    warning += "- Supplier has not accepted the work<br />";
                }

                if (item.SupplierCompletionDeadline == null || item.SupplierCompletionDeadline == DateTime.MinValue)
                {
                    warning += "- Supplier completion deadline has not been set<br />";
                }

                if (item.SupplierCompletedItemDateTime == null || item.SupplierCompletedItemDateTime == DateTime.MinValue)
                {
                    warning += "- Supplier has not completed the work<br />";
                }


                if (item.LanguageServiceId == 49 && item.AdditionalDetails.IsCLSJob == true && item.PGDDetails.BuyoutAgreementSigned == false)
                {
                    pgdbuyoutwarning += "- Buyout agreement must be signed before completing the job item";
                }

                if (item.LanguageServiceId == 36 && item.AdditionalDetails.IsCLSJob == true && item.PGDDetails.CopydeckStored == false)
                {
                    pgdcopydeckwarning += "- Buyout agreement must be signed before completing the job item";
                }
            }




            if (warning != "")
            {
                return 3;
            }
            if (pgdbuyoutwarning != "")
            {
                return 4;
            }
            if (pgdcopydeckwarning != "")
            {
                return 5;
            }
            if (linguistNDAwarning != "")
            {
                return 6;
            }
            if (linguistContractwarning != "")
            {
                return 7;
            }
            if (updateModel.Item.LanguageServiceId == 0)
            {
                return 8;
            }
            else
            {
                var res = await jobItemService.Update(updateModel);
                if (redirttoratelinguist == true)
                {
                    var org = await orgsService.GetOrg(item.AdditionalDetails.OrgId);

                    if (org.LinguistRatingEnabled == true)
                    {
                        return 1;
                    }
                    else
                    {
                        return 2;
                    }
                }
                else
                {
                    return 2;
                }
            }
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> Search(string searchTerm, bool onlyAllowEncryptedSuppliers, int orgGroupId, bool searchForLinguist, string sourceLangIanaCode, string targetLangIanaCode)
        {
            var resultList = new List<LinguistViewModel>();

            if (searchForLinguist == true)
            {
                var linguistList = await linguistService.SearchByNameOrId(searchTerm, sourceLangIanaCode, targetLangIanaCode);

                List<int> BannedCountries = new List<int>() { 24, 59, 107, 155, 120, 214, 187, 220, 243, 252 };
                List<String> Countries = new List<String>() { "belarus", "cuba", "iran", "myanmar", "north korea", "sudan", "russia", "syria", "venezuela", "zimbabwe" };

                foreach (var linguist in linguistList)
                {
                    if (linguist == null)
                    {
                        linguist.UIMessage = "No supplier with this ID exists";
                        linguist.Disabled = true;
                    }
                    else
                    {

                        if (linguist.NdauploadedDateTime == null && linguist.NdaunlockedDateTime == null)
                        {
                            linguist.UIMessage = "This supplier has not signed an NDA, so they cannot currently be used.";
                            linguist.Disabled = true;
                        }
                        else if (linguist.ContractUploadedDateTime == null && linguist.ContractUnlockedDateTime == null)
                        {
                            linguist.UIMessage = "This supplier has not signed a contract/service agreement, so they cannot currently be used.";
                            linguist.Disabled = true;
                        }
                        else
                        {
                            //check if the linguist needs to be approved by LR
                            //if the linguist needs approval, then we shouldnt allow it to be added to the job item
                            if (linguist.NeedApprovalToBeAddedToDb == true)
                            {
                                linguist.UIMessage = "This linguist needs to be approved by LR.";
                                linguist.Disabled = true;
                            }
                            if (linguist.HasEncryptedComputer.GetValueOrDefault() == false)
                            {
                                bool groupOnlyAllowsEnryptedSuppliers = false;

                                if (onlyAllowEncryptedSuppliers == true)
                                    groupOnlyAllowsEnryptedSuppliers = true;

                                if (groupOnlyAllowsEnryptedSuppliers == true)
                                {
                                    linguist.UIMessage = "This supplier has not encrypted their computer and the org group only allows suppliers who have encrypted their computer, so they cannot currently be used.";
                                    linguist.Disabled = true;
                                }
                                bool IsOrgStrictOnEEAClause = false;

                                //strict org groups - INTRAN, Ramboll, Language shop and Expolink
                                if (orgGroupId == 71521 || orgGroupId == 23055 || orgGroupId == 19049 || orgGroupId == 18496 || orgGroupId == 72099)
                                    IsOrgStrictOnEEAClause = true;

                                if (IsOrgStrictOnEEAClause == true && linguist.NonEeaclauseDeclinedDateTime != null)
                                {
                                    linguist.UIMessage = "This supplier is from outside the EEA region and has declined non-EEA clause. This org only allows suppliers who have accepted the non-EEA clause.";
                                    linguist.Disabled = true;
                                }
                            }
                        }

                        if (BannedCountries.Contains(linguist.CountryId) == true)
                        {
                            linguist.UIMessage = "This supplier is from a sanctioned country, so they cannot currently be used.";
                            linguist.Disabled = true;
                        }
                        else
                        {
                            List<Data.LinguisticSupplierInvoiceTemplate> invoiceTemplates = await invoicetemplateService.GetAllLinguisticSupplierInvoiceTemplates(linguist.Id);

                            if (invoiceTemplates != null && invoiceTemplates.Count > 0)
                            {
                                foreach (Data.LinguisticSupplierInvoiceTemplate invoicetemplate in invoiceTemplates)
                                {
                                    if (invoicetemplate.CountryToShowOnInvoice != null && BannedCountries.Contains((int)invoicetemplate.CountryToShowOnInvoice) == true)
                                    {
                                        linguist.UIMessage = "This supplier is from a sanctioned country, so they cannot currently be used.";
                                        linguist.Disabled = true;
                                    }
                                    else if (invoicetemplate.BankAccountIban != null && (invoicetemplate.BankAccountIban.Trim().ToLower().StartsWith("by") == true || invoicetemplate.BankAccountIban.Trim().ToLower().StartsWith("ir") == true))
                                    {
                                        linguist.UIMessage = "This supplier is from a sanctioned country, so they cannot currently be used.";
                                        linguist.Disabled = true;
                                    }
                                    else if (invoicetemplate.BankBranchCountry != null && (Countries.Contains(invoicetemplate.BankBranchCountry.Trim().ToLower()) == true))
                                    {
                                        linguist.UIMessage = "This supplier is from a sanctioned country, so they cannot currently be used.";
                                        linguist.Disabled = true;
                                    }
                                }
                            }
                        }

                    }
                }
                resultList = linguistList.ToList();
            }
            else
            {
                var contactList = await contactsService.SearchByNameOrId(searchTerm);

                foreach (var contact in contactList)
                {
                    var lingModel = new LinguistViewModel();
                    if (contact == null)
                        lingModel.UIMessage = "No contact with this ID / Name exists";
                    else
                    {
                        var name = contact.Name;
                        lingModel.Id = contact.Id;
                        lingModel.MainContactFirstName = name.Split(" ")[0];
                        if (name.Contains(" "))
                            lingModel.MainContactSurname = name.Substring(lingModel.MainContactFirstName.LastIndexOf(lingModel.MainContactFirstName.Last()) + 2);
                        else
                            lingModel.MainContactSurname = "";
                    }
                    resultList.Add(lingModel);
                }
            }

            resultList.Sort((p, q) => p.Disabled.CompareTo(q.Disabled));

            return Ok(new { results = resultList });
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<string> GenerateBrief(int itemId)
        {
            var message = "Job brief successfully created.";
            try
            {
                await jobItemService.GenerateJobBrief(itemId);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("because it is being used by another process"))
                    message = "The job brief could not be created because someone has the existing job brief file open. Please ensure it is closed before trying again.";
                else
                    message = "The job brief could not be created as the following error was reported: " + ex.Message;
            }

            return message;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCLSItemInformation()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var jobitemid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var Markets = stringToUse.Split("$tp$")[1];
            var Service = stringToUse.Split("$tp$")[2];
            var AssetsOverview = stringToUse.Split("$tp$")[3].Replace("\\", "");
            var CopydeckStored = Boolean.Parse(stringToUse.Split("$tp$")[4]);
            var VOTalent = stringToUse.Split("$tp$")[5];
            var BuyoutAgreementSigned = Boolean.Parse(stringToUse.Split("$tp$")[6]);
            var UsageType = stringToUse.Split("$tp$")[7];
            var UsageDuration = Convert.ToInt32(stringToUse.Split("$tp$")[8]);
            var UsageStartDateinput = stringToUse.Split("$tp$")[9];
            var UsageEndDateinput = stringToUse.Split("$tp$")[10];

            var UsageStartDate = DateTime.MinValue;
            if (UsageStartDateinput != "")
            {
                UsageStartDate = DateTime.Parse(UsageStartDateinput);
            }

            var UsageEndDate = DateTime.MinValue;
            if (UsageEndDateinput != "")
            {
                UsageEndDate = DateTime.Parse(UsageEndDateinput);
            }

            var result = await pgdService.UpdateCLSJobItemInformation(jobitemid, Markets, Service, AssetsOverview, CopydeckStored, VOTalent, BuyoutAgreementSigned, UsageType, UsageDuration, UsageStartDate, UsageEndDate);

            var markets = result.Markets.Split(",");
            var marketheader = String.Format("Markets ({0})", markets.Length.ToString());

            var assets = result.AssetsOverview.Split(",");
            var assetheader = String.Format("Assets overview ({0})", assets.Length.ToString());


            return Ok(marketheader + "$" + assetheader);
        }

        [HttpPost]
        public async Task<IActionResult> CheckforPaymentCurrency()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            short? currencyid = 0;
            var linguistid = Int32.Parse(stringToUse);

            var linguist = await linguistService.GetById(linguistid);

            if (linguist != null)
            {
                if (linguist.CurrencyId != null)
                {
                    currencyid = linguist.CurrencyId;
                }
            }



            return Ok(currencyid);
        }

        // expected changes on PGD
        private bool CheckPGDStatus() => true;
        private bool IsAllowedToEdit() => true;
        public async Task<IActionResult> JobItem(int OrderID)
        {
            if (OrderID == 0)
                return NotFound();

            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            //var item = await jobItemService.GetViewModelById(OrderID);
            var joborder = await service.GetById(OrderID);

            if (joborder == null) { return NotFound(); }

            var contact = await contactsService.GetContactDetails(joborder.ContactId);
            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var group = await orgGroupsLogic.GetOrgGroupDetails((Int32)org.OrgGroupId);


            //if (item == null)
            //    return Content("The Job Order ID you provided (" + OrderID + ") is not a valid number.");

            if (loggedInEmployee == null)
                return NotFound();

            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            var department = await employeesService.GetEmployeeDepartment(loggedInEmployee.Id); var emp = await employeesService.GetEmployeeByUsername(username);
            if (org.OrgGroupId != 18520)
            {
                if (loggedInEmployee.TeamId == 5 || loggedInEmployee.TeamId == 16 || loggedInEmployee.TeamId == 13 || loggedInEmployee.TeamId == 38 || department.Id == 1 || emp.AttendsBoardMeetings == true)
                {
                    return Redirect("/Page/Locked");
                }
            }

            var Languages = await cachedService.GetAllLanguagesCached();
            JobItemCreationViewModel model = new JobItemCreationViewModel();
            model.JobOrderId = joborder.Id;
            model.JobOrderName = joborder.JobName;
            model.ContactId = contact.Id;
            model.ContactName = contact.Name;
            model.OrgId = org.Id;
            model.OrgName = org.OrgName;
            model.OrgGroupId = group.Id;
            model.OrgGroupName = group.Name;
            model.IsVisibleToClient = false;
            model.CompletionDeadline = joborder.OverallDeliveryDeadline;
            model.LanguageServices = await languageService.GetLanguageServices();
            model.LanguageServiceCategory = await cachedService.getLanguageServiceCategory();
            model.Languages = Languages.OrderBy(o => o.Name).ToList();

            return View(model);
        }
        public async Task<IActionResult> CreateJobItemAsync()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobItem model = new JobItem();
            var allFields = stringToProcess.Split("$");
            model.WordCountFuzzyBand1 = 0;
            model.WordCountFuzzyBand2 = 0;
            model.WordCountFuzzyBand3 = 0;
            model.WordCountFuzzyBand4 = 0;
            model.WordCountExact = 0;
            model.WordCountNew = 0;
            model.WordCountRepetitions = 0;
            model.WordCountPerfectMatches = 0;
            model.SupplierWordCountFuzzyBand1 = 0;
            model.SupplierWordCountFuzzyBand2 = 0;
            model.SupplierWordCountFuzzyBand3 = 0;
            model.SupplierWordCountFuzzyBand4 = 0;
            model.SupplierWordCountExact = 0;
            model.SupplierWordCountNew = 0;
            model.SupplierWordCountRepetitions = 0;
            model.SupplierWordCountPerfectMatches = 0;
            model.IsVisibleToClient = Convert.ToBoolean(allFields[0]);
            model.LanguageServiceId = Convert.ToByte(allFields[1]);
            model.SourceLanguageIanacode = allFields[2];
            var targetLanguage = allFields[3];
            var splitTargetLanguage = targetLanguage.Split(",").Where(o => o != "").Distinct().ToList();
            model.OurCompletionDeadline = Convert.ToDateTime(allFields[4]);
            if (Convert.ToBoolean(allFields[5]))
            {
                model.LinguisticSupplierOrClientReviewerId = 19568;
            }

            if (allFields[6] != "")
            {
                model.DescriptionForSupplierOnly = allFields[6];
            }
            if (allFields[7] != "")
            {
                model.SupplierCompletionDeadline = Convert.ToDateTime(allFields[7]);
            }
            if (allFields[8] != "")
            {
                model.SupplierWordCountNew = Convert.ToInt32(allFields[8]);
                model.WordCountNew = Convert.ToInt32(allFields[8]);
            }
            if (allFields[9] != "")
            {
                model.SupplierWordCountFuzzyBand1 = Convert.ToInt32(allFields[9]);
            }
            if (allFields[10] != "")
            {
                model.SupplierWordCountFuzzyBand2 = Convert.ToInt32(allFields[10]);
            }
            if (allFields[11] != "")
            {
                model.SupplierWordCountFuzzyBand3 = Convert.ToInt32(allFields[11]);
            }
            if (allFields[12] != "")
            {
                model.SupplierWordCountFuzzyBand4 = Convert.ToInt32(allFields[12]);
            }
            if (allFields[13] != "" && allFields[13] != "undefined")
            {
                model.SupplierWordCountExact = Convert.ToInt32(allFields[13]);
                model.WordCountExact = Convert.ToInt32(allFields[13]);
            }
            if (allFields[14] != "" && allFields[14] != "undefined")
            {
                model.SupplierWordCountRepetitions = Convert.ToInt32(allFields[14]);
                model.WordCountRepetitions = Convert.ToInt32(allFields[14]);
            }
            if (allFields[15] != "" && allFields[15] != "undefined")
            {
                model.SupplierWordCountPerfectMatches = Convert.ToInt32(allFields[15]);
                model.WordCountPerfectMatches = Convert.ToInt32(allFields[15]);
            }
            if (allFields[16] != "" || allFields[17] != "")
            {
                if (allFields[16] == "")
                {
                    model.WorkMinutes = Convert.ToInt32(allFields[17]);
                }
                else if (allFields[17] == "")
                {
                    model.WorkMinutes = (Convert.ToInt32(allFields[16]) * 60);
                }
                else
                {
                    model.WorkMinutes = (Convert.ToInt32(allFields[16]) * 60) + Convert.ToInt32(allFields[17]);
                }
            }
            model.JobOrderId = Convert.ToInt32(allFields[18]);
            model.LanguageServiceCategoryId = Convert.ToByte(allFields[19]);
            model.CreatedByEmployeeId = loggedInEmployee.Id;
            model.SupplierIsClientReviewer = false;
            var joborder = await service.GetById(model.JobOrderId);
            var contact = await contactsService.GetContactDetails(joborder.ContactId);
            var org = await orgsService.GetOrgDetails(contact.OrgId);
            var errorCount = 0;
            if (model.SupplierWordCountFuzzyBand1 != null || model.SupplierWordCountFuzzyBand2 != null || model.SupplierWordCountFuzzyBand3 != null || model.SupplierWordCountFuzzyBand4 != null)
            {
                if (org.FuzzyBand1BottomPercentage != 0 && org.FuzzyBand1TopPercentage != 0 && org.FuzzyBand2BottomPercentage == 0 && org.FuzzyBand2TopPercentage == 0 && org.FuzzyBand3BottomPercentage == 0 && org.FuzzyBand3TopPercentage == 0 && org.FuzzyBand4BottomPercentage == 0 && org.FuzzyBand4TopPercentage == 0)
                {
                    if (org.FuzzyBand1BottomPercentage == 75 && org.FuzzyBand1TopPercentage == 99)
                    {
                        if (allFields[9] != "")
                        {
                            model.WordCountNew = model.WordCountNew + Convert.ToInt32(allFields[9]);
                        }
                        model.WordCountFuzzyBand1 = model.SupplierWordCountFuzzyBand2 + model.SupplierWordCountFuzzyBand3 + model.SupplierWordCountFuzzyBand4;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
                else if (org.FuzzyBand1BottomPercentage != 0 && org.FuzzyBand1TopPercentage != 0 && org.FuzzyBand2BottomPercentage != 0 && org.FuzzyBand2TopPercentage != 0 && org.FuzzyBand3BottomPercentage == 0 && org.FuzzyBand3TopPercentage == 0 && org.FuzzyBand4BottomPercentage == 0 && org.FuzzyBand4TopPercentage == 0)
                {
                    if (org.FuzzyBand1BottomPercentage == 50 && org.FuzzyBand1TopPercentage == 74 && org.FuzzyBand2BottomPercentage == 75 && org.FuzzyBand2TopPercentage == 99)
                    {
                        model.WordCountFuzzyBand1 = model.SupplierWordCountFuzzyBand1;
                        model.WordCountFuzzyBand2 = model.SupplierWordCountFuzzyBand2 + model.SupplierWordCountFuzzyBand3 + model.SupplierWordCountFuzzyBand4;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
                else if (org.FuzzyBand1BottomPercentage != 0 && org.FuzzyBand1TopPercentage != 0 && org.FuzzyBand2BottomPercentage != 0 && org.FuzzyBand2TopPercentage != 0 && org.FuzzyBand3BottomPercentage != 0 && org.FuzzyBand3TopPercentage != 0 && org.FuzzyBand4BottomPercentage == 0 && org.FuzzyBand4TopPercentage == 0)
                {
                    if (org.FuzzyBand1BottomPercentage == 75 && org.FuzzyBand1TopPercentage == 84 && org.FuzzyBand2BottomPercentage == 85 && org.FuzzyBand2TopPercentage == 94 && org.FuzzyBand3BottomPercentage == 95 && org.FuzzyBand3TopPercentage == 99)
                    {
                        model.WordCountFuzzyBand1 = model.SupplierWordCountFuzzyBand2;
                        model.WordCountFuzzyBand2 = model.SupplierWordCountFuzzyBand3;
                        model.WordCountFuzzyBand3 = model.SupplierWordCountFuzzyBand4;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
                else if (org.FuzzyBand1BottomPercentage != 0 && org.FuzzyBand1TopPercentage != 0 && org.FuzzyBand2BottomPercentage != 0 && org.FuzzyBand2TopPercentage != 0 && org.FuzzyBand3BottomPercentage != 0 && org.FuzzyBand3TopPercentage != 0 && org.FuzzyBand4BottomPercentage != 0 && org.FuzzyBand4TopPercentage != 0)
                {
                    if (org.FuzzyBand1BottomPercentage == 50 && org.FuzzyBand1TopPercentage == 74 && org.FuzzyBand2BottomPercentage == 75 && org.FuzzyBand2TopPercentage == 84 && org.FuzzyBand3BottomPercentage == 85 && org.FuzzyBand3TopPercentage == 94 && org.FuzzyBand3BottomPercentage == 95 && org.FuzzyBand3TopPercentage == 99)
                    {
                        model.WordCountFuzzyBand1 = model.SupplierWordCountFuzzyBand1;
                        model.WordCountFuzzyBand2 = model.SupplierWordCountFuzzyBand2;
                        model.WordCountFuzzyBand3 = model.SupplierWordCountFuzzyBand3;
                        model.WordCountFuzzyBand4 = model.SupplierWordCountFuzzyBand4;
                    }
                    else
                    {
                        errorCount++;
                    }
                }
            }
            string requestId = null;
            JobItem newRequest = null;
            for (int j = 0; j < splitTargetLanguage.Count(); j++)
            {
                model.TargetLanguageIanacode = splitTargetLanguage[j];

                newRequest = await jobItemService.CreateJobItem(model);
                if (newRequest == null)
                {
                    string MessagsString = String.Format("Error$Something went wrong while saving new Job Item(s) on the database.");
                    return Ok(MessagsString);
                }
                requestId = newRequest.Id + "," + requestId;
            }
            if (errorCount > 0)
            {
                return Ok($"Success$Job Item(s) created successfully but found mismatch in fuzzy bands so couldn't save client fuzzy bands on the database.${requestId}");
            }
            return Ok($"Success$Job Item(s) created successfully.${requestId}");
        }
        public async Task<IActionResult> AvailableTargetLanguages()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobItem model = new JobItem();
            var allFields = stringToProcess.Split("$");
            var sourceLanguageSelected = allFields[0];
            var input = allFields[1];
            var jobOrderID = Convert.ToInt32(allFields[2]);
            List<JobItem> newRequest = null;
            newRequest = await jobItemService.CheckAvailableTargetLanguages(sourceLanguageSelected, jobOrderID);
            var targetLanguages = newRequest.Select(o => o.TargetLanguageIanacode).Distinct().ToList();
            var Languages = await cachedService.GetAllLanguagesCached();
            var finalLst = Languages.Where(o => targetLanguages.Contains(o.StringValue)).ToList();
            if (newRequest.Any())
            {
                if (input == "check")
                {
                    return Ok("true");
                }
                else
                {
                    return Ok(finalLst);
                }
            }
            return Ok();
        }
        public async Task<IActionResult> ConfigureNetworkFolders()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobOrder model = new JobOrder();
            var allFields = stringToProcess[0];
            List<string> jobItemID = stringToProcess.Split(",").Where(o => o != "").ToList();
            foreach (var id in jobItemID)
            {
                try
                {
                    await jobItemService.configurenetworkfolders(Convert.ToInt32(id));
                }
                catch { }
            }
            return Ok();
        }
        public async Task<IActionResult> UpdateNetworkFolder(string jobItemID)
        {
            try
            {
                await jobItemService.configurenetworkfolders(Convert.ToInt32(jobItemID));
            }
            catch { }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ImportWordCounts()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            string MessagsString = string.Empty;
            bool? ImportOnlyForClient = null;
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            var filePath = allFields[0];
            var importWordCountsRadio = allFields[1];
            if (importWordCountsRadio != "2")
            {
                ImportOnlyForClient = Convert.ToBoolean(Convert.ToInt32(allFields[1]));
            }
            var jobItemID = Convert.ToInt32(allFields[2]);
            bool IsThereAnyXML = false;

            if (filePath.EndsWith(".xml") == true && System.IO.File.Exists(filePath) == true) { IsThereAnyXML = true; }
            else { MessagsString = "Error$XML file to import can't be found."; return Ok(MessagsString); }
            var fileName = Path.GetFileName(filePath);
            var JobItem = await jobItemService.GetById(jobItemID);
            var ParentJobOrder = await service.GetById(JobItem.JobOrderId);
            var OrderContact = await contactsService.GetContactDetails(ParentJobOrder.ContactId);
            var ParentOrg = await orgsService.GetOrgDetails(OrderContact.OrgId);
            var ParentOrgGroup = await orgGroupsLogic.GetOrgGroupDetails((int)ParentOrg.OrgGroupId);
            if (IsThereAnyXML)
            {

                TPWordCountBreakdownBatchModel ItemImport = wordCountBreakdownBatchService.WordCountBreakdownBatch(fileSystemService.UnMapTPNetworkPath(filePath), ParentOrg, ParentJobOrder, Global_Settings.Enumerations.MemoryApplications.NoneOrUnknown, false, JobItem);
                if (ImportOnlyForClient == true)
                {
                    await jobItemService.ApplyToJobItem(JobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, loggedInEmployee, ItemImport, true, true);
                }
                else if (ImportOnlyForClient == false)
                {
                    await jobItemService.ApplyToJobItem(JobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, loggedInEmployee, ItemImport, false);
                }
                else
                {
                    await jobItemService.ApplyToJobItem(JobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, loggedInEmployee, ItemImport);
                }
                var languages = cachedService.GetAllLanguagesCached();
                var sourceLanguage = languages.Result.Where(x => x.StringValue == JobItem.SourceLanguageIanacode).Select(x => x.Name).FirstOrDefault();
                var targetLanguage = languages.Result.Where(x => x.StringValue == JobItem.TargetLanguageIanacode).Select(x => x.Name).FirstOrDefault();
                MessagsString = "Success$Word counts were updated for both the client and the linguist.";
                if (ImportOnlyForClient == true) { MessagsString = "Success$The word counts were updated for Client."; }
                else if (ImportOnlyForClient == false) { MessagsString = "Success$The word counts were updated for Linguist."; }

                string EmailBody = String.Format("<p>Dear {0},<br /><br />You have imported \"{1}\" into job item <a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id={2}\">{2} {3} --> {4}.</a>", loggedInEmployee.FirstName, fileName, jobItemID, sourceLanguage, targetLanguage);
                emailService.SendMail("flow plus <flowplus@translateplus.com>", "cristian-laurentiu.necula@translateplus.com, " + loggedInEmployee.EmailAddress, "Successful word counts import in Job item",
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
        [HttpPost("api/GetServices")]
        public async Task<IEnumerable<LanguageService>> GetServices(string SelectedField)
        {

            var data = await languageService.GetLanguageServices();
            var services = data.Where(o => o.LanguageServiceCategoryID.Split(',').Contains(SelectedField)).ToList();

            return services;
        }
        public async Task<IActionResult> CloneItem()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobItem model = new JobItem();
            var allFields = stringToProcess.Split("$");
            
            var itemID = Convert.ToInt32(allFields[0]);
            var joborderID = Convert.ToUInt32(allFields[1]);

            model =await jobItemService.GetById(itemID);
            model.ChargeToClient = 0;
            model.PaymentToSupplier = 0;
            model.CreatedByEmployeeId = loggedInEmployee.Id;
            var newItem =await jobItemService.CreateJobItem(model);
            if (newItem == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Job Item on the database.");
                return Ok(MessagsString);
            }
                
            return Ok($"Success${newItem.Id}");
        }

    }
}

