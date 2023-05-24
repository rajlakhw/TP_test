using System;
using System.Web;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Interfaces;
using ViewModels.JobOrder;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Global_Settings;
using System.Collections;
using ViewModels.Common;

namespace SmartAdmin.WebUI.Controllers
{
    public class TMSJobOrderController : Controller
    {
        private readonly ITPJobOrderService service;
        private readonly ITPEmployeesService employeeService;
        private readonly IConfiguration configuration;
        private readonly ITPContactsLogic contactService;
        private readonly ITPOrgsLogic orgService;
        private readonly ITPEmployeesService employeesService;
        private readonly ITPJobOrderChannelsLogic joborderchannelservice;
        private readonly ITPCurrenciesLogic currencyservice;
        private readonly ITPLocalCurrencyInfo currencyinfoservice;
        private readonly ITPClientInvoicesLogic clientinvoiceservice;
        private readonly ITPExchangeService exchangeservice;
        private readonly ITPJobItemService jobitemservice;
        private readonly ITPOrgGroupsLogic orgGroupsLogic;
        private readonly ITPLinguistService linguistService;
        private readonly IPGDService pgdService;
        private readonly ITPCurrenciesLogic currenciesService;
        private readonly ITPEndClient endclientService;
        private readonly IQuoteAndOrderDiscountsAndSurchargesCategories discountsurchargeService;
        private readonly IQuoteAndOrderDiscountsAndSurcharges discountorsurchargeService;
        private static int joborderid;
        private static string networkfolderpath;
        private IConfiguration Configuration;
        private readonly ITPEmployeeOwnershipsLogic employeeOwnershipsLogic;
        private readonly ITPTimeZonesService timeZonesService;
        private readonly ITPFileSystemService fileSystemService;
        private readonly ICommonCachedService cachedService;
        private readonly IEmailUtilsService emailService;
        private readonly ITPWordCountBreakdownBatch wordCountBreakdownBatchService;
        private readonly ITPLanguageLogic languageLogicService;
        private readonly ITPLinguisticSupplierInvoiceJobItems invoiceService;

        public TMSJobOrderController(ITPJobOrderService service,
            ITPEmployeesService employeeService,
            IConfiguration configuration,
            ITPContactsLogic contactService,
            ITPOrgsLogic orgService,
            ITPEmployeesService employeesService,
            ITPJobOrderChannelsLogic joborderchannelservice,
            ITPCurrenciesLogic currencyservice,
            ITPLocalCurrencyInfo currencyinfoservice,
            ITPClientInvoicesLogic clientinvoiceservice,
            ITPExchangeService exchangeservice,
            ITPJobItemService jobitemservice,
            ITPOrgGroupsLogic orgGroupsLogic,
            ITPLinguistService linguistService,
            IPGDService pgdService,
            ITPCurrenciesLogic currenciesService,
            ITPEndClient endclientService,
             IQuoteAndOrderDiscountsAndSurchargesCategories discountsurchargeService,
            IQuoteAndOrderDiscountsAndSurcharges discountorsurchargeService,
            IConfiguration _configuration,
             ITPEmployeeOwnershipsLogic employeeOwnershipsLogic,
             ITPTimeZonesService timeZonesService,
             ITPFileSystemService fileSystemService,
            ICommonCachedService cachedService,
            IEmailUtilsService emailUtilsService,
            ITPWordCountBreakdownBatch wordCountBreakdownBatchService,
            ITPLanguageLogic languageLogicService,
            ITPLinguisticSupplierInvoiceJobItems invoiceService
        )

        {
            this.service = service;
            this.employeeService = employeeService;
            this.configuration = configuration;
            this.contactService = contactService;
            this.orgService = orgService;
            this.employeesService = employeesService;
            this.joborderchannelservice = joborderchannelservice;
            this.currencyservice = currencyservice;
            this.currencyinfoservice = currencyinfoservice;
            this.clientinvoiceservice = clientinvoiceservice;
            this.exchangeservice = exchangeservice;
            this.jobitemservice = jobitemservice;
            this.orgGroupsLogic = orgGroupsLogic;
            this.linguistService = linguistService;
            this.pgdService = pgdService;
            this.currenciesService = currenciesService;
            this.endclientService = endclientService;
            this.discountsurchargeService = discountsurchargeService;
            this.discountorsurchargeService = discountorsurchargeService;
            Configuration = _configuration;
            this.employeeOwnershipsLogic = employeeOwnershipsLogic;
            this.timeZonesService = timeZonesService;
            this.fileSystemService = fileSystemService;
            this.cachedService = cachedService;
            emailService = emailUtilsService;
            this.wordCountBreakdownBatchService = wordCountBreakdownBatchService;
            this.languageLogicService = languageLogicService;
            this.invoiceService = invoiceService;
        }

        public async Task<IActionResult> JobOrder(int JobOrderID)
        {
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);


            //if (EmployeeCurrentlyLoggedIn.AccessLevel == 4 || EmployeeCurrentlyLoggedIn.Id == 638 || EmployeeCurrentlyLoggedIn.Id == 646 || EmployeeCurrentlyLoggedIn.Id == 1060 || EmployeeCurrentlyLoggedIn.Id == 1171 || EmployeeCurrentlyLoggedIn.Id == 1184 || EmployeeCurrentlyLoggedIn.Id == 1275 || EmployeeCurrentlyLoggedIn.Id == 1298 || EmployeeCurrentlyLoggedIn.Id == 1340 || EmployeeCurrentlyLoggedIn.Id == 1109 ||
            //    EmployeeCurrentlyLoggedIn.Id == 31 || EmployeeCurrentlyLoggedIn.Id == 1229 || EmployeeCurrentlyLoggedIn.Id == 1269 || EmployeeCurrentlyLoggedIn.Id == 1308 || EmployeeCurrentlyLoggedIn.Id == 1324 || EmployeeCurrentlyLoggedIn.TeamId == 10
            //     || EmployeeCurrentlyLoggedIn.TeamId == 11 || EmployeeCurrentlyLoggedIn.TeamId == 15 || EmployeeCurrentlyLoggedIn.TeamId == 36 || EmployeeCurrentlyLoggedIn.TeamId == 40 || EmployeeCurrentlyLoggedIn.TeamId == 41)
            //{


            ViewBag.CurrentDirectory = (Environment.CurrentDirectory + @"\wwwroot").Replace("\\", " /");

            joborderid = JobOrderID;
            var joborder = await service.GetById(JobOrderID);

            if (joborder == null) { return NotFound(); }

            var contact = await contactService.GetContactDetails(joborder.ContactId);
            var org = await orgService.GetOrgDetails(contact.OrgId);
            var group = await orgGroupsLogic.GetOrgGroupDetails((Int32)org.OrgGroupId);

            if (contact != null && org != null)
            {
                ViewBag.ClientInfo = "<a href=\"https://myplusbeta.publicisgroupe.net/Contact?contactID=" + contact.Id.ToString() + "\">" + contact.Id.ToString() + " - " + contact.Name + "</a> of <a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id=" + org.Id.ToString() + "\">" + org.Id.ToString() + " - " + org.OrgName + "</a>";
            }
            ViewBag.OrgGroupId = org.OrgGroupId;
            ViewBag.LinkedJobOrderID = joborder.LinkedJobOrderId;
            if (joborder.LinkedJobOrderId > 0) { var linkedJobOrder = await service.GetById((int)joborder.LinkedJobOrderId); ViewBag.LinkedJobOrder = "<a href=\"https://myplusbeta.publicisgroupe.net/tmsjoborder/joborder?joborderid=" + linkedJobOrder.Id.ToString() + "\">" + linkedJobOrder.Id.ToString() + " - " + linkedJobOrder.JobName + "</a>"; }
            ViewBag.ContactID = "";
            ViewBag.ContactName = "";
            ViewBag.ContactDeleted = "";
            if (contact != null)
            {
                ViewBag.ContactID = contact.Id.ToString();
                ViewBag.ContactName = contact.Name;

                if (contact.DeletedDate != null && contact.DeletedDate != DateTime.MinValue)
                {
                    ViewBag.ContactDeleted = "&nbsp;<i>(Deleted)</i>";
                }
            }

            ViewBag.OrgID = "";
            ViewBag.OrgName = "";
            ViewBag.OrgDeleted = "";
            ViewBag.InvoiceLang = "en";
            if (org != null)
            {
                ViewBag.OrgID = org.Id.ToString();
                ViewBag.OrgName = org.OrgName;

                if (org.DeletedDate != null && org.DeletedDate != DateTime.MinValue)
                {
                    ViewBag.OrgDeleted = "&nbsp;<i>(Deleted)</i>";
                }

                if (org.InvoiceLangIanacode != null)
                {
                    if (org.InvoiceLangIanacode != "")
                    {
                        ViewBag.InvoiceLang = org.InvoiceLangIanacode;
                    }
                }
            }

            ViewBag.GroupID = "";
            ViewBag.GroupName = "";
            ViewBag.GroupDeleted = "";
            if (org != null)
            {
                ViewBag.GroupID = group.Id.ToString();
                ViewBag.GroupName = group.Name;

                if (group.DeletedDate != null && group.DeletedDate != DateTime.MinValue)
                {
                    ViewBag.GroupDeleted = "&nbsp;<i>(Deleted)</i>";
                }
            }

            ViewBag.NetworkJobFolder = "";
            ViewBag.NetworkJobPath = "";

            byte JobServerLocation = 0;
            if (org.JobServerLocation != null)
            {
                JobServerLocation = (byte)org.JobServerLocation;
            }

            //string networkfolderpath = "";
            networkfolderpath = service.NetworkDirectoryPathForApp(joborder.Id, org.Id, JobServerLocation, false, joborder.SubmittedDateTime);

            if (networkfolderpath == "" && joborder.LionBoxArchivingStatus == 2)
            {
                ViewBag.NetworkJobFolder = "<a href=\"https://lion.app.box.com/folder/72215880016\">LionBox: tp - Archive - Jobs</a>";
                ViewBag.NetworkJobPath = "https://lion.app.box.com/folder/72215880016";
            }
            else if (networkfolderpath == "" && joborder.LionBoxArchivingStatus == 1)
            {
                ViewBag.NetworkJobFolder = "<a href=\"https://lion.app.box.com/folder/94421039140\">Ready to archive - In Staging area</a>";
                ViewBag.NetworkJobPath = "https://lion.app.box.com/folder/94421039140";
            }
            else if (networkfolderpath == "" && joborder.ArchivedToAmazonS3dateTime != null)
            {
                ViewBag.NetworkJobFolder = "Archived to AWS – please contact IT Support for access";
            }
            else if (networkfolderpath == "" && joborder.ArchivedDateTime != null)
            {
                ViewBag.NetworkJobFolder = "Files no longer in production – please contact IT Support";
            }
            else
            {
                ViewBag.NetworkJobFolder = "<a href=\"" + networkfolderpath + "\">" + HttpUtility.HtmlEncode(service.NetworkDirectoryPathForApp(joborder.Id, org.Id, JobServerLocation, true, joborder.SubmittedDateTime)) + "</a>";
                ViewBag.NetworkJobPath = service.NetworkDirectoryPathForApp(joborder.Id, org.Id, JobServerLocation, false, joborder.SubmittedDateTime); ;
            }

            JobOrderViewModel viewResult = await service.GetViewModelById(joborder.Id);
            //JobOrderViewModel viewResult = new JobOrderViewModel()
            //{
            //    Id = joborder.Id,
            //    JobName = System.Web.HttpUtility.HtmlDecode(joborder.JobName),
            //    OverallDeliveryDeadline = joborder.OverallDeliveryDeadline,
            //    GrossMarginPercentage = joborder.GrossMarginPercentage,
            //    AnticipatedGrossMarginPercentage = joborder.AnticipatedGrossMarginPercentage,
            //    SubmittedDateTime = joborder.SubmittedDateTime,
            //    LastModifiedDate = joborder.LastModifiedDate,
            //    ClientNotes = joborder.ClientNotes,
            //    InternalNotes = joborder.InternalNotes,
            //    IsAtrialProject = joborder.IsAtrialProject,
            //    PrintingProject = joborder.PrintingProject,
            //    EscalatedToAccountManager = joborder.EscalatedToAccountManager,
            //    IsHighlyConfidential = joborder.IsHighlyConfidential,
            //    Priority = joborder.Priority,
            viewResult.AllProjectManagerEmployees = await employeeService.GetAllEmployees<Employee>(false, false);
            viewResult.AllJobOrderChannels = await joborderchannelservice.GetAllJobOrderChannels<JobOrderChannel>();
            //    ClientPonumber = joborder.ClientPonumber,
            //    InvoicingNotes = joborder.InvoicingNotes,
            //    CustomerSpecificField1Value = joborder.CustomerSpecificField1Value,
            //    CustomerSpecificField2Value = joborder.CustomerSpecificField2Value,
            //    CustomerSpecificField3Value = joborder.CustomerSpecificField3Value,
            //    CustomerSpecificField4Value = joborder.CustomerSpecificField4Value,
            //    OverallCompletedDateTime = joborder.OverallCompletedDateTime,
            //    ClientInvoiceId = joborder.ClientInvoiceId,
            //    OverdueReasonId = joborder.OverdueReasonId,
            //    OverdueComment = joborder.OverdueComment,
            viewResult.AllTMPaths = service.AllTemplatePaths(joborder.Id, org.Id, JobServerLocation, false, joborder.SubmittedDateTime);
            viewResult.JobItems = await service.GetJobItems(joborder.Id);
            viewResult.Currencies = await currenciesService.GetAllENCurrencies();
            viewResult.EndClients = await endclientService.GetAllEndClients();
            viewResult.Campaigns = await endclientService.GetAllCampaignsForEndClientIDs(joborder.EndClientId.ToString());
            viewResult.Brands = await endclientService.GetAllBrandsForEndClientIDs(joborder.EndClientId.ToString());
            viewResult.Categories = await endclientService.GetAllCategoriesForEndClientIDs(joborder.EndClientId.ToString());
            viewResult.SurchargesCategories = await discountsurchargeService.GetAllSurchargeCategories();
            viewResult.DiscountCategories = await discountsurchargeService.GetAllDiscountCategories();
            viewResult.AllDeadlineChangeReason = await service.GetAllDeadlineChangeReason();
            //};

            if ((viewResult.IsCls == true || viewResult.TypeOfOrder == 3 || viewResult.TypeOfOrder == 4 || viewResult.TypeOfOrder == 5))
            {
                var VOdropdowns = await pgdService.GetAllVODropdownLists();

                //string serviceName = Regex.Replace(viewResult.PGDDetails.ProjectStatus, @"\s+", "");
                var VOdropdown = VOdropdowns.Where(x => x.VodropdownListName.ToLower() == "status").FirstOrDefault();

                int VOdopdownId = 0;
                if (VOdropdown != null)
                    VOdopdownId = VOdropdown.Id;

                var dropdownListItems = await pgdService.GetAllVODropdownListsItems();
                viewResult.AllPGDDropdownItems = dropdownListItems;
                if (VOdopdownId > 0)
                    viewResult.PGDDropdownItems = dropdownListItems.Where(x => x.VodropdownListId == VOdopdownId).Select(x => new ViewModels.Common.DropdownOptionViewModel() { Id = x.Id, Name = x.Value, StringValue = x.Value });

                viewResult.PGDStatuses = dropdownListItems.Where(x => x.VodropdownListId == 10).Select(x => new ViewModels.Common.DropdownOptionViewModel() { Id = x.Id, Name = x.Value });
            }


            ViewBag.SetUpByEmployee = "<i>(n/a)</i>";
            if (joborder.SetupByEmployeeId != null)
            {
                var setupByEmployee = await employeeService.IdentifyCurrentUserById((int)joborder.SetupByEmployeeId);
                ViewBag.SetUpByEmployee = "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/" + setupByEmployee.Id.ToString() + "\">" + setupByEmployee.FirstName + " " + setupByEmployee.Surname + "</a>";
                ViewBag.SetUpByEmployeeImageBase64 = setupByEmployee.ImageBase64;
            }

            ViewBag.LastModifiedByEmployee = "<i>(n/a)</i>";
            if (joborder.LastModifiedByEmployeeId != null)
            {
                var lastModifiedByEmployee = await employeeService.IdentifyCurrentUserById((int)joborder.LastModifiedByEmployeeId);
                ViewBag.LastModifiedByEmployee = "<a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/" + lastModifiedByEmployee.Id.ToString() + "\">" + lastModifiedByEmployee.FirstName + " " + lastModifiedByEmployee.Surname + "</a>";
                ViewBag.LastModifiedByEmployeeImageBase64 = lastModifiedByEmployee.ImageBase64;
            }

            ViewBag.LastModifiedDate = "<i>(never)</i>";
            if (joborder.LastModifiedDate != null)
            {
                DateTime LastModifiedDateTime = (DateTime)joborder.LastModifiedDate;
                ViewBag.LastModifiedDate = LastModifiedDateTime.ToString("dddd dd MMMM yyyy HH:mm");
            }

            var ProjectManagerEmployee = await employeeService.IdentifyCurrentUserById(joborder.ProjectManagerEmployeeId);
            if (ProjectManagerEmployee != null)
            {
                ViewBag.ProjectManager = "<a href=\"http://myplus/Employee.aspx?EmployeeID=" + ProjectManagerEmployee.Id.ToString() + "\">" + ProjectManagerEmployee.FirstName + " " + ProjectManagerEmployee.Surname + "</a>";
                ViewBag.ImageBase64 = ProjectManagerEmployee.ImageBase64;
                ViewBag.ProjectManagerID = ProjectManagerEmployee.Id;
            }

            ViewBag.JobCompletedBool = "";
            if (joborder.OverallCompletedDateTime != null && joborder.OverallCompletedDateTime != DateTime.MinValue)
            {
                ViewBag.JobCompletedBool = "checked";
            }

            ViewBag.JobCompletedReadOnly = "";
            if (joborder.ClientInvoiceId != null && joborder.ClientInvoiceId != 0 && joborder.OverallCompletedDateTime != null && joborder.OverallCompletedDateTime != DateTime.MinValue)
            {
                ViewBag.JobCompletedReadOnly = "disabled";
            }
            //else
            if (joborder.OverallCompletedDateTime == null || joborder.OverallCompletedDateTime == DateTime.MinValue)
            {
                {
                    bool AllComplete = true;
                    foreach (var item in viewResult.JobItems)
                    {
                        if ((item.WeCompletedItemDateTime == null && item.LanguageServiceName.Contains("review") == false) || (item.WeCompletedItemDateTime == null && (item.SupplierCompletionDeadline == null || ((((DateTime)item.SupplierCompletionDeadline).AddDays(7)) < timeZonesService.GetCurrentGMT())) && item.LanguageServiceName.Contains("review") == true))
                        {
                            AllComplete = false;
                            break;
                        }
                    }

                    if (AllComplete == false)
                    {
                        ViewBag.JobCompletedReadOnly = "disabled";
                    }
                }
            }

            var JobOrderChannel = await joborderchannelservice.GetJobOrderChannelDetails(joborder.JobOrderChannelId);
            if (JobOrderChannel != null)
            {
                ViewBag.JobOrderChannel = JobOrderChannel.Name;
                ViewBag.JobOrderChannelID = JobOrderChannel.Id;
            }

            ViewBag.SterlingPaymentToSuppier = "£" + String.Format("{0:N2}", joborder.OverallSterlingPaymentToSuppliers);

            ViewBag.CompletedString = "";
            if (joborder.OverallCompletedDateTime != null && joborder.OverallCompletedDateTime != DateTime.MinValue)
            {
                ViewBag.CompletedString = "<img src=\"img/SuccessIcon.gif\" align=\"middle\" /> &nbsp; Completed " + String.Format("{0:d MMM yy HH:mm}", (DateTime)joborder.OverallCompletedDateTime);
            }
            else
            {
                if (joborder.OverallDeliveryDeadline < GeneralUtils.GetCurrentGMT())
                {
                    ViewBag.CompletedString = "<img src=\"img/WarningIcon.png\" align=\"middle\" />&nbsp;Overdue";
                }
                else
                {
                    ViewBag.CompletedString = "<img src=\"img/InformationIcon.png\" align=\"middle\" />&nbsp;In progress";
                }
            }

            //ViewBag.Priority = "";
            //if (joborder.Priority != null)
            //{
            //    var priotityno = joborder.Priority;

            //    if ((int)priotityno == 0)
            //    {
            //        ViewBag.Priority = "";
            //    }
            //    else if ((int)priotityno == 1)
            //    {
            //        ViewBag.Priority = "<font color=\"red\"><b>High</b></font>";
            //    }
            //    else if ((int)priotityno == 2)
            //    {
            //        ViewBag.Priority = "<font color=\"green\"><b>Medium</b></font>";
            //    }
            //    else if ((int)priotityno == 3)
            //    {
            //        ViewBag.Priority = "Low";
            //    }
            //    else
            //    {
            //        ViewBag.Priority = "";
            //    }
            //}

            ViewBag.EscalatedToAccountManagerStatus = "This overdue job has <b>not</b> been escalated to the Sales lead";
            ViewBag.EscalatedToAccountManagerBool = "";
            if (joborder.EscalatedToAccountManager != null)
            {
                if ((bool)joborder.EscalatedToAccountManager == true)
                {
                    ViewBag.EscalatedToAccountManagerStatus = "This overdue job has been escalated to the Sales lead after following the established process";
                    ViewBag.EscalatedToAccountManagerBool = "checked";
                }
            }

            ViewBag.IsAtrialProjectStatus = "No";
            ViewBag.IsAtrialProjectBool = "";
            if (joborder.IsAtrialProject == true)
            {
                ViewBag.IsAtrialProjectStatus = "This is a <a href=\"TrialProjects.aspx\">trial project/test piece</a> which the client will be using to evaluate our services";
                ViewBag.IsAtrialProjectBool = "checked";
            }

            ViewBag.PrintingProjectStatus = "Not a printing/packaging project";
            ViewBag.PrintingProjectBool = "";
            if (joborder.PrintingProject == true)
            {
                ViewBag.PrintingProjectStatus = "This is a printing/packaging project, and the relevant workflow stated in the Operations Guidebook must be followed at all times. Please speak to your line manager";
                ViewBag.PrintingProjectBool = "checked";
            }

            ViewBag.IsHighlyConfidentialStatus = "Normal";
            ViewBag.IsHighlyConfidentialBool = "";
            if (joborder.IsHighlyConfidential)
            {
                ViewBag.IsHighlyConfidentialStatus = "<font color=\"red\"><b>Highly confidential</b></font>";
                ViewBag.IsHighlyConfidentialBool = "checked";
            }

            ViewBag.TotalOrderValue = "";
            ViewBag.SubTotalOrderValue = "";
            ViewBag.SurchargeValue = "";
            ViewBag.DiscountValue = "";
            ViewBag.Currency = "";
            ViewBag.Prefix = "";
            ViewBag.CurrencyName = "";
            ViewBag.AnticipatedOrderValue = "";
            ViewBag.AnticipatedOrderValueVisible = "";
            if (joborder.ClientCurrencyId > 0)
            {
                var currency = await currencyservice.GetById(joborder.ClientCurrencyId);


                if (currency != null)
                {
                    var currencyinfo = await currencyinfoservice.GetById(currency.Id);

                    if (currencyinfo != null)
                    {
                        ViewBag.Currency = " (" + currencyinfo.CurrencyName + ")";

                        if (joborder.OverallChargeToClient != null)
                        {
                            ViewBag.TotalOrderValue = currency.Prefix + String.Format("{0:N2}", (decimal)joborder.OverallChargeToClient) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.TotalOrderValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }

                        if (joborder.SubTotalOverallChargeToClient != null)
                        {
                            ViewBag.SubTotalOrderValue = currency.Prefix + String.Format("{0:N2}", (decimal)joborder.SubTotalOverallChargeToClient) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.SubTotalOrderValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }

                        if (joborder.SurchargeAmount != null)
                        {
                            ViewBag.SurchargeValue = currency.Prefix + String.Format("{0:N2}", (decimal)joborder.SurchargeAmount) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.SurchargeValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }
                        if (joborder.DiscountAmount != null)
                        {
                            ViewBag.DiscountValue = currency.Prefix + String.Format("{0:N2}", (decimal)joborder.DiscountAmount) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.DiscountValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }
                        if (joborder.AnticipatedFinalValueAmount != null)
                        {
                            ViewBag.AnticipatedOrderValue = currency.Prefix + String.Format("{0:N2}", (decimal)joborder.AnticipatedFinalValueAmount) + " (" + currencyinfo.CurrencyName + ")";
                        }
                        else
                        {
                            ViewBag.AnticipatedOrderValue = currency.Prefix + "0.00 (" + currencyinfo.CurrencyName + ")";
                        }
                        if (org.EarlyPaymentDiscount > 0) { ViewBag.AnticipatedOrderValueVisible = "true"; }
                        ViewBag.Prefix = currency.Prefix;
                        ViewBag.CurrencyName = " (" + currencyinfo.CurrencyName + ")";
                    }
                }

            }

            ViewBag.ClientInvoice = "<i>(Not yet invoiced)</i>";
            ViewBag.EarlyInvoicing = "<i>(Not marked for early invoicing)</i>";
            ViewBag.EarlyInvoicingBool = "";
            ViewBag.Margin = "";
            ViewBag.DeleteLink = "javascript:void(0);";
            ViewBag.MoveOrderLink = "javascript:void(0);";
            ViewBag.InvoiceOrderLink = "javascript:void(0);";
            ViewBag.CalculateCostLink = "javascript:void(0);";
            ViewBag.AnticipatedMargin = "";
            decimal AnticipatedOverallSterlingChargeToClient = 0;
            decimal OverallSterlingChargeToClient = 0;
            viewResult.ClientInvoiceIsFinalised = false;
            if (joborder.ClientInvoiceId != null)
            {
                if (joborder.ClientInvoiceId > 0)
                {
                    var link = "<a href=\"http://myplus/ClientInvoice.aspx?ClientInvoiceID=" + joborder.ClientInvoiceId.ToString() + "\">" + "Invoice " + joborder.ClientInvoiceId.ToString() + "</a>";

                    var clientinvoice = await clientinvoiceservice.GetById((int)joborder.ClientInvoiceId);

                    if (clientinvoice != null)
                    {
                        viewResult.ClientInvoiceIsFinalised = clientinvoice.IsFinalised;
                        if (clientinvoice.IsFinalised == false)
                        {
                            link += "&nbsp;<i>(Currently being prepared)</i>";
                            ViewBag.JobCompletedReadOnly = "";
                        }
                        else
                        {
                            link += "&nbsp;<i>(Invoice has been sent to the client)</i>";

                            if (clientinvoice.InvoiceCurrencyId != 4)
                            {
                                if (joborder.OverallChargeToClientForMarginCalucation != 0)
                                {
                                    DateTime DateToGetRateFrom = (DateTime)clientinvoice.InvoiceDate;
                                    if (clientinvoice.FinalisedDateTime != null && (DateTime)clientinvoice.FinalisedDateTime != DateTime.MinValue)
                                    {
                                        DateToGetRateFrom = (DateTime)clientinvoice.FinalisedDateTime;
                                    }

                                    var exchangerate = await exchangeservice.GetHistoricalExchangeRate(4, clientinvoice.InvoiceCurrencyId, DateToGetRateFrom);

                                    if (exchangerate != null)
                                    {
                                        OverallSterlingChargeToClient = (decimal)joborder.OverallChargeToClientForMarginCalucation * (1 / exchangerate.Rate);
                                        AnticipatedOverallSterlingChargeToClient = (decimal)joborder.OverallAnticipatedChargeToClientForMarginCalucation * (1 / exchangerate.Rate);
                                    }
                                }
                            }

                        }

                        if (joborder.EarlyInvoiceDateTime != null && joborder.EarlyInvoiceDateTime != DateTime.MinValue)
                        {
                            ViewBag.EarlyInvoicing = "<i>(Marked and invoiced for early invoicing)</i>";
                            ViewBag.EarlyInvoicingBool = "checked";
                        }
                    }

                    ViewBag.ClientInvoice = link;
                }
                else
                {
                    if (joborder.EarlyInvoiceDateTime != DateTime.MinValue)
                    {
                        ViewBag.EarlyInvoicing = "<i>(Marked for early invoicing, but yet to be invoiced)</i>";
                        ViewBag.EarlyInvoicingBool = "checked";
                    }
                }
            }
            else
            {
                bool canbeinvoiced = true;
                if (joborder.OverallChargeToClientForMarginCalucation != null && joborder.OverallChargeToClientForMarginCalucation != 0)
                {
                    OverallSterlingChargeToClient = exchangeservice.Convert(joborder.ClientCurrencyId, 4, (decimal)joborder.OverallChargeToClientForMarginCalucation);
                    AnticipatedOverallSterlingChargeToClient = exchangeservice.Convert(joborder.ClientCurrencyId, 4, (decimal)joborder.OverallAnticipatedChargeToClientForMarginCalucation);
                }

                if (joborder.EarlyInvoiceDateTime != null && joborder.EarlyInvoiceDateTime != DateTime.MinValue)
                {
                    ViewBag.EarlyInvoicing = "<i>(Marked for early invoicing, but yet to be invoiced)</i>";
                    ViewBag.EarlyInvoicingBool = "checked";
                    canbeinvoiced = false;
                }

                if (joborder.OverallCompletedDateTime == null || joborder.OverallCompletedDateTime == DateTime.MinValue)
                {
                    ViewBag.DeleteLink = "http://myplus/ConfirmJobDeletion.aspx?ID=" + joborder.Id.ToString() + "&Delete=Order";
                }
                else
                {
                    canbeinvoiced = false;
                }

                if (joborder.ContactId != 176771 && joborder.ContactId != 183210 && joborder.ContactId != 176237)
                {
                    ViewBag.MoveOrderLink = "http://myplus/MoveOrder.aspx?JobOrderID=" + joborder.Id.ToString();
                }

                if (canbeinvoiced == true)
                {
                    if ((joborder.AnticipatedGrossMarginPercentage >= 50 && joborder.AnticipatedGrossMarginPercentage <= 100) || (joborder.InternalHighLowMarginApprovedDateTime != null && joborder.InternalHighLowMarginApprovedDateTime != DateTime.MinValue))
                    {
                        ViewBag.InvoiceOrderLink = "http://myplus/AddJobOrdersToClientInvoice.aspx?JobOrderIDs=" + joborder.Id.ToString();
                    }
                }

                ViewBag.CalculateCostLink = "http://myplus/CalculateJobCosts.aspx?OrderID=" + joborder.Id.ToString();
            }

            ViewBag.DeleteJobFilesLink = "javascript:void(0);";

            if (joborder.EonfilesDeletedDateTime == null || joborder.EonfilesDeletedDateTime == DateTime.MinValue)
            {
                ViewBag.DeleteJobFilesLink = "http://myplus/ConfirmToDeleteJobFiles.aspx?JobOrderID=" + joborder.Id.ToString();
            }

            ViewBag.Margin = "£" + String.Format("{0:N2}", (OverallSterlingChargeToClient - joborder.OverallSterlingPaymentToSuppliersForMarginCalc)) + " (" + String.Format("{0:N2}", joborder.GrossMarginPercentage) + "%)";
            ViewBag.AnticipatedMargin = "£" + String.Format("{0:N2}", (AnticipatedOverallSterlingChargeToClient - joborder.OverallSterlingPaymentToSuppliersForMarginCalc)) + " (" + String.Format("{0:N2}", joborder.AnticipatedGrossMarginPercentage) + "%)";
            ViewBag.HeaderCustomerSpecificField1Label = "Customer-specific data 1";
            ViewBag.HeaderCustomerSpecificField2Label = "Customer-specific data 2";
            ViewBag.HeaderCustomerSpecificField3Label = "Customer-specific data 3";
            ViewBag.HeaderCustomerSpecificField4Label = "Customer-specific data 4";
            if (org != null)
            {
                if (org.CustomerSpecificField1Name != "")
                {
                    ViewBag.HeaderCustomerSpecificField1Label = org.CustomerSpecificField1Name;
                }
                if (org.CustomerSpecificField2Name != "")
                {
                    ViewBag.HeaderCustomerSpecificField2Label = org.CustomerSpecificField2Name;
                }
                if (org.CustomerSpecificField3Name != "")
                {
                    ViewBag.HeaderCustomerSpecificField3Label = org.CustomerSpecificField3Name;
                }
                if (org.CustomerSpecificField4Name != "")
                {
                    ViewBag.HeaderCustomerSpecificField4Label = org.CustomerSpecificField4Name;
                }
            }
            ViewBag.BSHFMNumber = org.HfmcodeBs == null ? "" : org.HfmcodeBs.ToString();
            ViewBag.ISNumber = org.HfmcodeIs == null ? "" : org.HfmcodeIs.ToString();

            var department = await employeeService.GetEmployeeDepartment(EmployeeCurrentlyLoggedIn.Id);

            var emp = await employeeService.GetEmployeeByUsername(username);
            ViewBag.AllowedToAddJobItem = "1";
            viewResult.IsAllowedToEdit = true;
            if (org.OrgGroupId != 18520)
            {
                if (EmployeeCurrentlyLoggedIn.TeamId == 5 || EmployeeCurrentlyLoggedIn.TeamId == 16 || EmployeeCurrentlyLoggedIn.TeamId == 13 || EmployeeCurrentlyLoggedIn.TeamId == 38 || department.Id == 1 || emp.AttendsBoardMeetings == true)
                {
                    ViewBag.AllowedToAddJobItem = "0";
                    viewResult.IsAllowedToEdit = false;
                }
            }
            viewResult.EditPageEnabled = false;
            viewResult.AllowPriority = false;
            viewResult.AllowEarlyInvoicing = false;
            viewResult.BMWSendTranslation = false;
            if (((((viewResult.EarlyInvoiceDateTime != null || viewResult.EarlyInvoiceDateTime != DateTime.MinValue) && (viewResult.OverallCompletedDateTime == null || viewResult.OverallCompletedDateTime == DateTime.MinValue)) || viewResult.ClientInvoiceIsFinalised==false) && (ViewBag.OrgGroupId !=18523 || (ViewBag.OrgGroupId == 18523 && viewResult.OnHold == false))) && viewResult.IsAllowedToEdit == true)
            {
                viewResult.EditPageEnabled = true;
            }
            if (org.AllowPriority == true) {
                viewResult.AllowPriority = true;
            }
            if(joborder.ClientInvoiceId == null && (viewResult.OverallCompletedDateTime == null || viewResult.OverallCompletedDateTime == DateTime.MinValue) && org.AllowEarlyClientInvoicing == true)
            {
                viewResult.AllowEarlyInvoicing = true;
            }
            if ((joborder.ClientInvoiceId == null || viewResult.ClientInvoiceIsFinalised == false) && viewResult.JobItems.Count() > 0) {
                ViewBag.CalculateCostLink = "http://myplus/CalculateJobCosts.aspx?OrderID=" + joborder.Id.ToString();
            }
            if (viewResult.ClientInvoiceIsFinalised == false && viewResult.IsAllowedToEdit == true) {
                ViewBag.DeleteLink = "http://myplus/ConfirmJobDeletion.aspx?ID=" + joborder.Id.ToString() + "&Delete=Order";
            }
            if (org != null)
            {
                if ((org.Id == 139233 || org.Id == 139232 || org.Id == 83923) && viewResult.JobItems.Count() > 0)
                {
                    foreach (var item in viewResult.JobItems)
                    {
                        var invoice = await invoiceService.GetInvoicesByJobItemID(item.Id);
                        if (invoice.Count() == 0 && (item.WeCompletedItemDateTime == null || item.WeCompletedItemDateTime == DateTime.MinValue))
                        {
                            viewResult.BMWSendTranslation = true;
                        }
                    }
                }
            }
            viewResult.RateTheLinguist = false;
            if (org.LinguistRatingEnabled == true) {
                viewResult.RateTheLinguist = true;
            }
            return View("Views/TMS/JobOrder.cshtml", viewResult);

            //}

            //else
            //{
            //    return Redirect("/Page/Locked");
            //}
        }


        //[HttpPost]
        //public async Task<IActionResult> UpdateKeyInformation(JobOrderViewModel model, string completedcheckbox)
        //{
        //    string username = HttpContext.User.Identity.Name;
        //    username = GeneralUtils.GetUsernameFromNetwokUsername(username);
        //    Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);

        //    if (!ModelState.IsValid)
        //    {
        //        return this.Redirect("Job");
        //    }

        //    bool markuncompleted = false;
        //    if ((model.OverallCompletedDateTime != null && model.OverallCompletedDateTime != DateTime.MinValue) && (model.ClientInvoiceId == null || model.ClientInvoiceId == 0))
        //    {
        //        if (completedcheckbox == null)
        //        {
        //            markuncompleted = true;
        //        }
        //    }

        //    bool markcompleted = false;
        //    if ((model.OverallCompletedDateTime == null || model.OverallCompletedDateTime != DateTime.MinValue) && (model.ClientInvoiceId == null || model.ClientInvoiceId == 0))
        //    {
        //        if (completedcheckbox != null)
        //        {
        //            markcompleted = true;
        //        }
        //    }

        //    var result = await service.UpdateKeyInformation<JobOrderViewModel>(model.Id, model.JobName, model.OverallDeliveryDeadline, EmployeeCurrentlyLoggedIn.Id, markuncompleted, markcompleted);
        //    return this.RedirectToAction("JobOrder", new { JobOrderID = model.Id });
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateProjectInformation(JobOrderViewModel model, string escalatedcheckbox, string trialcheckbox, string PrintingProjectheckbox, string IsHighlyConfidentialcheckbox)
        //{
        //    string username = HttpContext.User.Identity.Name;
        //    username = GeneralUtils.GetUsernameFromNetwokUsername(username);
        //    Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);

        //    if (!ModelState.IsValid)
        //    {
        //        return this.RedirectToAction("Index");
        //    }

        //    if (escalatedcheckbox != null)
        //    {
        //        model.EscalatedToAccountManager = true;
        //    }
        //    else
        //    {
        //        if (model.EscalatedToAccountManager != null)
        //        {
        //            model.EscalatedToAccountManager = false;
        //        }
        //        else
        //        {
        //            model.EscalatedToAccountManager = null;
        //        }
        //    }

        //    if (trialcheckbox != null)
        //    {
        //        model.IsAtrialProject = true;
        //    }
        //    else
        //    {
        //        model.IsAtrialProject = false;
        //    }

        //    if (PrintingProjectheckbox != null)
        //    {
        //        model.PrintingProject = true;
        //    }
        //    else
        //    {
        //        if (model.PrintingProject != null)
        //        {
        //            model.PrintingProject = false;
        //        }
        //        else
        //        {
        //            model.PrintingProject = null;
        //        }
        //    }


        //    if (IsHighlyConfidentialcheckbox != null)
        //    {
        //        model.IsHighlyConfidential = true;
        //    }
        //    else
        //    {
        //        model.IsHighlyConfidential = false;
        //    }

        //    if (model.ClientNotes == null) { model.ClientNotes = ""; }
        //    if (model.InternalNotes == null) { model.InternalNotes = ""; }

        //    var result = await service.UpdateProjectInformation<JobOrderViewModel>(model.Id, model.ProjectManagerEmployeeId, model.JobOrderChannelId, model.ClientNotes, model.InternalNotes, model.EscalatedToAccountManager, model.IsAtrialProject, model.PrintingProject, model.IsHighlyConfidential, model.Priority, EmployeeCurrentlyLoggedIn.Id);
        //    return this.RedirectToAction("JobOrder", new { JobOrderID = model.Id });
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateFinancialInformation(JobOrderViewModel model, string earlyinvoicingcheckbox)
        //{
        //    string username = HttpContext.User.Identity.Name;
        //    username = GeneralUtils.GetUsernameFromNetwokUsername(username);
        //    Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);

        //    if (!ModelState.IsValid)
        //    {
        //        return this.RedirectToAction("Index");
        //    }

        //    if (earlyinvoicingcheckbox != null)
        //    {
        //        if (model.EarlyInvoiceDateTime == null || model.EarlyInvoiceDateTime == DateTime.MinValue)
        //        {
        //            model.EarlyInvoiceDateTime = GeneralUtils.GetCurrentUKTime();
        //            model.EarlyInvoiceByEmpId = EmployeeCurrentlyLoggedIn.Id;
        //        }
        //    }
        //    else
        //    {
        //        if (model.EarlyInvoiceDateTime != null || model.EarlyInvoiceDateTime != DateTime.MinValue)
        //        {
        //            model.EarlyInvoiceDateTime = null;
        //            model.EarlyInvoiceByEmpId = null;
        //        }
        //    }

        //    if (model.InvoicingNotes == null) { model.InvoicingNotes = "";  }
        //    if (model.ClientPonumber == null) { model.ClientPonumber = ""; }
        //    if (model.CustomerSpecificField1Value == null) { model.CustomerSpecificField1Value = ""; }
        //    if (model.CustomerSpecificField2Value == null) { model.CustomerSpecificField2Value = ""; }
        //    if (model.CustomerSpecificField3Value == null) { model.CustomerSpecificField3Value = ""; }
        //    if (model.CustomerSpecificField4Value == null) { model.CustomerSpecificField4Value = ""; }

        //    var result = await service.UpdateFinancialInformation<JobOrderViewModel>(model.Id, model.InvoicingNotes, model.ClientPonumber, model.EarlyInvoiceDateTime, model.EarlyInvoiceByEmpId, model.CustomerSpecificField1Value, model.CustomerSpecificField2Value, model.CustomerSpecificField3Value, model.CustomerSpecificField4Value, EmployeeCurrentlyLoggedIn.Id);
        //    return this.RedirectToAction("JobOrder", new { JobOrderID = model.Id });
        //}

        [HttpPost]
        public async Task<IActionResult> CreateTradosStudioTask()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var TMPath = stringToUse.Split("$tp$")[0];
            var UsePrepFiles = stringToUse.Split("$tp$")[1];
            var JobOrderID = stringToUse.Split("$tp$")[2];

            //string errormessage = "";
            //Boolean process = true;

            //var translationtoPrepFolder = Path.Combine(networkfolderpath, "02 Translation\\01 To\\Prep file");
            //var allFiles = Path.Combine(translationtoPrepFolder, "AllFiles");
            //DirectoryInfo PrepFileFolder = new DirectoryInfo(translationtoPrepFolder);

            //if (Directory.Exists(allFiles) == false)
            //{
            //    return Problem("There is no AllFiles folder in the Translation/To/Prep file folder", title:"Error");
            //}







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
            BatchDocAttr.Value = LoggedInEmployee.NetworkUserName;
            RootNode.Attributes.Append(BatchDocAttr);

            // write e-mail notification address(es) info
            BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
            BatchDocAttr.Value = LoggedInEmployee.EmailAddress;
            RootNode.Attributes.Append(BatchDocAttr);


            XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

            // write Type info 
            BatchDocAttr = BatchDoc.CreateAttribute("Type");
            BatchDocAttr.Value = "AutoProcessJobs";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write task number info 
            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
            BatchDocAttr.Value = "1";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write job order number info 
            BatchDocAttr = BatchDoc.CreateAttribute("JobOrderID");
            BatchDocAttr.Value = JobOrderID;
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write TradosProjectTemplatePath info 
            BatchDocAttr = BatchDoc.CreateAttribute("TradosProjectTemplatePath");
            BatchDocAttr.Value = TMPath;
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write ToPreTranslateAfterAnalysis info 
            BatchDocAttr = BatchDoc.CreateAttribute("ToPreTranslateAfterAnalysis");
            BatchDocAttr.Value = "False";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write IsFromJobOrderPage info 
            BatchDocAttr = BatchDoc.CreateAttribute("IsFromJobOrderPage");
            BatchDocAttr.Value = "True";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write UseFilesForAllItems info 
            BatchDocAttr = BatchDoc.CreateAttribute("UseFilesForAllItems");
            BatchDocAttr.Value = UsePrepFiles;
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // now append the node to the doc
            RootNode.AppendChild(IndividualTaskNode);

            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-39";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            BatchDoc.Save(BatchFilePath);

            return Ok();
        }

        [HttpPost("api/RunFEMergeAutomation")]
        public async Task<bool> RunFEMergeAutomation(JobOrderViewModel model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            await service.AutomateFinancialExpressMergeExcel(LoggedInEmployee.Id, model.Id);
            return true;
        }


        [HttpPost("api/RunAutomation")]
        public async Task<bool> RunAutomation(JobOrderViewModel model)
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            await service.AutomateFinancialExpressExcel(LoggedInEmployee.Id, model.Id);
            return true;
        }

        [HttpPost]
        public async Task<IActionResult> RunFinanceAuditLogTask()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

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
            BatchDocAttr.Value = "ITSupport";
            RootNode.Attributes.Append(BatchDocAttr);

            // write e-mail notification address(es) info
            BatchDocAttr = BatchDoc.CreateAttribute("NotifyByEmail");
            BatchDocAttr.Value = "ITSupport@translateplus.com";
            RootNode.Attributes.Append(BatchDocAttr);


            XmlNode IndividualTaskNode = BatchDoc.CreateElement("task");

            // write Type info 
            BatchDocAttr = BatchDoc.CreateAttribute("Type");
            BatchDocAttr.Value = "ExportJobAuditLog";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write task number info 
            BatchDocAttr = BatchDoc.CreateAttribute("TaskNumber");
            BatchDocAttr.Value = "1";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write DataObjectID info 
            BatchDocAttr = BatchDoc.CreateAttribute("DataObjectID");
            BatchDocAttr.Value = joborderid.ToString();
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write DataObjectsType info 
            BatchDocAttr = BatchDoc.CreateAttribute("DataObjectsType");
            BatchDocAttr.Value = "8";
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // write EmployeeLoggedInID info 
            BatchDocAttr = BatchDoc.CreateAttribute("EmployeeLoggedInID");
            BatchDocAttr.Value = LoggedInEmployee.Id.ToString();
            IndividualTaskNode.Attributes.Append(BatchDocAttr);

            // now append the node to the doc
            RootNode.AppendChild(IndividualTaskNode);

            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-38";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            BatchDoc.Save(BatchFilePath);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> SaveJobItem()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var jobitemid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var DeadlineString = stringToUse.Split("$tp$")[1];
            //var SupplierOrContact = Boolean.Parse(stringToUse.Split("$tp$")[2]);
            //int SupplierOrReviewerID = 0;

            //if (stringToUse.Split("$tp$")[3] != "")
            //{
            //    SupplierOrReviewerID = Int32.Parse(stringToUse.Split("$tp$")[3]);
            //}

            var ChargeToClient = decimal.TryParse(stringToUse.Split("$tp$")[2], out decimal parsedValue) ? parsedValue : 0;
            var PaymentToSupplier = decimal.TryParse(stringToUse.Split("$tp$")[3], out decimal parsedPaymentValue) ? parsedPaymentValue : 0;
            var MinSupplierChargeApplied = Boolean.Parse(stringToUse.Split("$tp$")[4]);
            var WorkTimeHour = Int32.Parse(stringToUse.Split("$tp$")[5]);
            var WorkTimeMinutes = Int32.Parse(stringToUse.Split("$tp$")[6]);
            var NewClientWordCount = Int32.Parse(stringToUse.Split("$tp$")[7]);
            var NewSupplierWordCount = Int32.Parse(stringToUse.Split("$tp$")[8]);
            var Visible = Boolean.Parse(stringToUse.Split("$tp$")[9]);
            var SupplierID = stringToUse.Split("$tp$")[10];

            var WorkMinutes = ((WorkTimeHour * 60) + WorkTimeMinutes);

            short suppliercurrencyid = -1;
            if (SupplierID != "")
            {
                var linguist = await linguistService.GetById(Int32.Parse(SupplierID));

                if (linguist != null)
                {
                    if (linguist.CurrencyId != null)
                    {
                        suppliercurrencyid = (short)linguist.CurrencyId;
                    }
                }
            }

            var Deadline = DateTime.MinValue;
            if (DeadlineString != "")
            {
                Deadline = DateTime.Parse(DeadlineString);
            }


            var result = await jobitemservice.UpdateJobItemKeyInformation(jobitemid, Deadline, ChargeToClient, PaymentToSupplier, MinSupplierChargeApplied, WorkMinutes, NewClientWordCount, NewSupplierWordCount, Visible, suppliercurrencyid);

            var totalwordcount = (result.WordCountNew + result.WordCountExact + result.WordCountFuzzyBand1 + result.WordCountFuzzyBand2 + result.WordCountFuzzyBand3 + result.WordCountFuzzyBand4 + result.WordCountRepetitions + result.WordCountPerfectMatches);
            var totalsupplierwordcount = (result.SupplierWordCountNew + result.SupplierWordCountExact + result.SupplierWordCountFuzzyBand1 + result.SupplierWordCountFuzzyBand2 + result.SupplierWordCountFuzzyBand3 + result.SupplierWordCountFuzzyBand4 + result.SupplierWordCountRepetitions + result.SupplierWordCountPerfectMatches);

            var jobitemhidden = "true";
            if (result.IsVisibleToClient == false)
            {
                jobitemhidden = "false";
            }
            return Ok(totalwordcount + ";" + totalsupplierwordcount + ";" + jobitemhidden);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateKeyInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var jobname = stringToUse.Split("$tp$")[1];
            var Deadline = DateTime.Parse(stringToUse.Split("$tp$")[2]);
            var completed = Boolean.Parse(stringToUse.Split("$tp$")[3]);
            var OverdueReasonIDString = stringToUse.Split("$tp$")[4];
            var OverdueReasonComment = stringToUse.Split("$tp$")[5];
            var updatedeadlines = stringToUse.Split("$tp$")[6];
            var IsCls = stringToUse.Split("$tp$")[7];
            var escalated = Boolean.Parse(stringToUse.Split("$tp$")[8]);
            var deadlineChangeReason = stringToUse.Split("$tp$")[9];
            var linkedJobOrder = stringToUse.Split("$tp$")[10];
            byte? OverdueReasonID = null;
            if (OverdueReasonIDString != "" && OverdueReasonIDString != "-1")
            {
                OverdueReasonID = byte.Parse(OverdueReasonIDString);
            }
            string? DeadlineChangeReason = null;
            if (deadlineChangeReason != "" && deadlineChangeReason != "-1")
            {
                DeadlineChangeReason = deadlineChangeReason;
            }
            bool? isCls = null;
            int? typeOfOrder = null;
            if (IsCls == "1" || IsCls == "0")
            {
                isCls = false;
                if (IsCls == "1")
                {
                    isCls = true;
                }
            }
            else
            {
                typeOfOrder = Convert.ToInt32(IsCls);
            }
            int? LinkedJobOrder = null;
            if (linkedJobOrder != "" && linkedJobOrder != "-1")
            {
                LinkedJobOrder = Convert.ToInt32(linkedJobOrder);
            }
            //if (!ModelState.IsValid)
            //{
            //    return this.Redirect("Job");
            //}

            //bool markuncompleted = false;
            //if ((model.OverallCompletedDateTime != null && model.OverallCompletedDateTime != DateTime.MinValue) && (model.ClientInvoiceId == null || model.ClientInvoiceId == 0))
            //{
            //    if (completedcheckbox == null)
            //    {
            //        markuncompleted = true;
            //    }
            //}

            //bool markcompleted = false;
            //if ((model.OverallCompletedDateTime == null || model.OverallCompletedDateTime != DateTime.MinValue) && (model.ClientInvoiceId == null || model.ClientInvoiceId == 0))
            //{
            //    if (completedcheckbox != null)
            //    {
            //        markcompleted = true;
            //    }
            //}

            var result = await service.UpdateKeyInformation<JobOrderViewModel>(joborderid, jobname, Deadline, LoggedInEmployee.Id, completed, OverdueReasonID, OverdueReasonComment, isCls, typeOfOrder, escalated, DeadlineChangeReason, LinkedJobOrder);

            var CompletedString = "";
            if (result.OverallCompletedDateTime != null && result.OverallCompletedDateTime != DateTime.MinValue)
            {
                CompletedString = "<img src=\"img/SuccessIcon.gif\" align=\"middle\" /> &nbsp; Completed " + String.Format("{0:d MMM yy HH:mm}", (DateTime)result.OverallCompletedDateTime);
            }
            else
            {
                if (result.OverallDeliveryDeadline < GeneralUtils.GetCurrentGMT())
                {
                    CompletedString = "<img src=\"img/WarningIcon.png\" align=\"middle\" />&nbsp;Overdue";
                }
                else
                {
                    CompletedString = "<img src=\"img/InformationIcon.png\" align=\"middle\" />&nbsp;In progress";
                }
            }

            if (updatedeadlines == "1")
            {
                System.Collections.Generic.IEnumerable<JobItemsDataTableViewModel> JobItems = await service.GetJobItems(joborderid);

                foreach (var item in JobItems)
                {
                    if (item.WeCompletedItemDateTime == null || item.WeCompletedItemDateTime == DateTime.MinValue)
                    {
                        //var jobitem = await jobitemservice.GetById(item.Id);
                        //jobitem.OurCompletionDeadline = Deadline;

                        //short suppliercurrencyid = -1;
                        //if (jobitem.PaymentToSupplierCurrencyId != null && jobitem.PaymentToSupplierCurrencyId > 0)
                        //{
                        //    suppliercurrencyid = (short)jobitem.PaymentToSupplierCurrencyId;
                        //}
                        var jobitemresult = await jobitemservice.UpdateJobItemClientDeadline(item.Id, Deadline);
                    }
                }
            }

            return Ok(CompletedString);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProjectInformation()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var PMEmpID = Int16.Parse(stringToUse.Split("$tp$")[1]);
            var JobOrderChannel = Byte.Parse(stringToUse.Split("$tp$")[2]);
            var ClientNotes = stringToUse.Split("$tp$")[3];
            var InternalNotesinput = stringToUse.Split("$tp$")[4];
            var trial = Boolean.Parse(stringToUse.Split("$tp$")[5]);
            var printingproject = Boolean.Parse(stringToUse.Split("$tp$")[6]);
            var highlyconfidential = Boolean.Parse(stringToUse.Split("$tp$")[7]);
            var priority = Byte.Parse(stringToUse.Split("$tp$")[8]);

            var result = await service.UpdateProjectInformation<JobOrderViewModel>(joborderid, PMEmpID, JobOrderChannel, ClientNotes, InternalNotesinput, trial, printingproject, highlyconfidential, priority, LoggedInEmployee.Id);

            var url = "";
            var empID = "";
            var ProjectManagerEmployee = await employeeService.IdentifyCurrentUserById(PMEmpID);
            if (ProjectManagerEmployee != null)
            {
                url = ProjectManagerEmployee.ImageBase64;
                empID = PMEmpID.ToString();
            }

            return Ok(empID + "$" + url);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateFinancialInformation()
        {
            //string username = HttpContext.User.Identity.Name;
            //username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            //Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);

            //if (!ModelState.IsValid)
            //{
            //    return this.RedirectToAction("Index");
            //}

            //if (earlyinvoicingcheckbox != null)
            //{
            //    if (model.EarlyInvoiceDateTime == null || model.EarlyInvoiceDateTime == DateTime.MinValue)
            //    {
            //        model.EarlyInvoiceDateTime = GeneralUtils.GetCurrentUKTime();
            //        model.EarlyInvoiceByEmpId = EmployeeCurrentlyLoggedIn.Id;
            //    }
            //}
            //else
            //{
            //    if (model.EarlyInvoiceDateTime != null || model.EarlyInvoiceDateTime != DateTime.MinValue)
            //    {
            //        model.EarlyInvoiceDateTime = null;
            //        model.EarlyInvoiceByEmpId = null;
            //    }
            //}

            //if (model.InvoicingNotes == null) { model.InvoicingNotes = ""; }
            //if (model.ClientPonumber == null) { model.ClientPonumber = ""; }
            //if (model.CustomerSpecificField1Value == null) { model.CustomerSpecificField1Value = ""; }
            //if (model.CustomerSpecificField2Value == null) { model.CustomerSpecificField2Value = ""; }
            //if (model.CustomerSpecificField3Value == null) { model.CustomerSpecificField3Value = ""; }
            //if (model.CustomerSpecificField4Value == null) { model.CustomerSpecificField4Value = ""; }

            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var InvoicingNotes = stringToUse.Split("$tp$")[1];
            var ClientPonumber = stringToUse.Split("$tp$")[2];
            var earlyinvoicing = Boolean.Parse(stringToUse.Split("$tp$")[3]);
            var CustomerSpecificField1Value = stringToUse.Split("$tp$")[4];
            var CustomerSpecificField2Value = stringToUse.Split("$tp$")[5];
            var CustomerSpecificField3Value = stringToUse.Split("$tp$")[6];
            var CustomerSpecificField4Value = stringToUse.Split("$tp$")[7];
            var OrgHfmcodeIs = stringToUse.Split("$tp$")[10];
            var OrgHfmcodeBs = stringToUse.Split("$tp$")[11];
            var ClientCurrency = stringToUse.Split("$tp$")[12];
            short? EarlyInvoiceEmpId = null;

            if (stringToUse.Split("$tp$")[8] != "")
            {
                EarlyInvoiceEmpId = Int16.Parse(stringToUse.Split("$tp$")[8]);
            }

            DateTime? EarlyInvoiceDate = null;

            if (stringToUse.Split("$tp$")[9] != "")
            {
                EarlyInvoiceDate = DateTime.Parse(stringToUse.Split("$tp$")[9]);
            }

            DateTime? EarlyInvoiceDateTime = null;
            short? EarlyInvoiceByEmpId = null;
            if (earlyinvoicing == true)
            {
                if (EarlyInvoiceDate == null || EarlyInvoiceDate == DateTime.MinValue)
                {
                    EarlyInvoiceDateTime = GeneralUtils.GetCurrentUKTime();
                    EarlyInvoiceByEmpId = LoggedInEmployee.Id;
                }
                else
                {
                    EarlyInvoiceDateTime = EarlyInvoiceDate;
                    EarlyInvoiceByEmpId = EarlyInvoiceEmpId;
                }
            }


            var result = await service.UpdateFinancialInformation<JobOrderViewModel>(joborderid, InvoicingNotes, ClientPonumber, EarlyInvoiceDateTime, EarlyInvoiceByEmpId, CustomerSpecificField1Value, CustomerSpecificField2Value, CustomerSpecificField3Value, CustomerSpecificField4Value, LoggedInEmployee.Id, OrgHfmcodeIs, OrgHfmcodeBs,short.Parse(ClientCurrency));

            var earlyinvoicingtext = "<i>(Not marked for early invoicing)</i>";
            if (result.EarlyInvoiceDateTime != null && result.EarlyInvoiceDateTime != DateTime.MinValue)
            {
                earlyinvoicingtext = "<i>(Marked for early invoicing)</i>";
            }


            return Ok(earlyinvoicingtext);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCLSInformation()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var ThirdPartyID = stringToUse.Split("$tp$")[1];
            var ProductionContact = stringToUse.Split("$tp$")[2];
            var pgdStatus = stringToUse.Split("$tp$")[3];
            var ICPOnumber = stringToUse.Split("$tp$")[4];
            var glossaryupdated = Boolean.Parse(stringToUse.Split("$tp$")[5]);

            var endclientid = Int32.Parse(stringToUse.Split("$tp$")[6]);
            var campaignid = Int32.Parse(stringToUse.Split("$tp$")[7]);
            var brandid = Int32.Parse(stringToUse.Split("$tp$")[8]);
            var categoryid = Int32.Parse(stringToUse.Split("$tp$")[9]);

            var result = await pgdService.UpdateCLSJobOrderInformation(joborderid, ThirdPartyID, ProductionContact, pgdStatus, ICPOnumber, glossaryupdated);

            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            var endclientresult = await service.UpdateEndclientInformation<JobOrderViewModel>(joborderid, endclientid, campaignid, brandid, categoryid, LoggedInEmployee.Id);

            var glossarystatus = "<font color=\"red\">Glossary and reference material <b>not</b> updated</font>";
            if (result.GlossaryUpdated == true)
            {
                glossarystatus = "<font color=\"green\">Glossary and reference material updated</font>";
            }

            return Ok(glossarystatus);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePGDInformation()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var ApprovedEndClientCharge = decimal.Parse(stringToUse.Split("$tp$")[1] == "" ? "0" : stringToUse.Split("$tp$")[1]);
            var endclientCurrencyID = Int16.Parse(stringToUse.Split("$tp$")[2]);
            var BSHFnumber = stringToUse.Split("$tp$")[3];
            var ISnumber = stringToUse.Split("$tp$")[4];

            decimal ApprovedEndClientChargeGBP = 0;
            ApprovedEndClientChargeGBP = exchangeservice.Convert(endclientCurrencyID, 4, ApprovedEndClientCharge);

            var result = await pgdService.UpdatePGDJobOrderInformation(joborderid, ApprovedEndClientCharge, endclientCurrencyID, BSHFnumber, ISnumber, ApprovedEndClientChargeGBP);

            var ApprovedEndClientChargeGBPstring = result.ApprovedEndClientChargeGbp.ToString("N2");
            var Variance = "";

            if (result.Variance != null)
            {
                Variance = ((decimal)result.Variance).ToString("N2");
            }


            return Ok(ApprovedEndClientChargeGBPstring + "$" + Variance + "$" + result.Bshfmnumber + "$" + result.Isnumber);
        }

        [HttpPost]
        public async Task<IActionResult> EndclientUpdate()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var endclientid = stringToUse.Split("$tp$")[0];

            var Campaigns = await endclientService.GetAllCampaignsForEndClientIDs(endclientid);
            var Brands = await endclientService.GetAllBrandsForEndClientIDs(endclientid);
            var Categories = await endclientService.GetAllCategoriesForEndClientIDs(endclientid);

            var campaignlist = "";
            foreach (var campaign in Campaigns)
            {
                if (campaignlist != "")
                {
                    campaignlist += ",";
                }

                campaignlist += campaign.Id + "-" + campaign.DataObjectName;
            }

            var brandlist = "";
            foreach (var brand in Brands)
            {
                if (brandlist != "")
                {
                    brandlist += ",";
                }

                brandlist += brand.Id + "-" + brand.DataObjectName;
            }

            var categorylist = "";
            foreach (var category in Categories)
            {
                if (categorylist != "")
                {
                    categorylist += ",";
                }

                categorylist += category.Id + "-" + category.DataObjectName;
            }

            return Ok(campaignlist + "$" + brandlist + "$" + categorylist);
        }


        [HttpPost]
        public async Task<IActionResult> SaveSurcharge()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var surchargeid = Int32.Parse(stringToUse.Split("$tp$")[1]);
            var surchargecategory = stringToUse.Split("$tp$")[2];
            var Surchargedescription = stringToUse.Split("$tp$")[3];
            var surchargetype = stringToUse.Split("$tp$")[4];
            var surchargeamount = decimal.Parse(stringToUse.Split("$tp$")[5]);

            if (surchargecategory == "-1" && surchargeid != 0)
            {
                var result = await discountorsurchargeService.RemoveSurchargeOrDiscount(surchargeid, LoggedInEmployee.Id);
                var jobresult = await service.UpdateSurchargeID<JobOrderViewModel>(joborderid, -1, LoggedInEmployee.Id);
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
                        var jobresult = await service.UpdateSurchargeID<JobOrderViewModel>(joborderid, result.Id, LoggedInEmployee.Id);
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

            var joborderid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var discountid = Int32.Parse(stringToUse.Split("$tp$")[1]);
            var discountcategory = stringToUse.Split("$tp$")[2];
            var Discountedescription = stringToUse.Split("$tp$")[3];
            var discounttype = stringToUse.Split("$tp$")[4];
            var discountamount = decimal.Parse(stringToUse.Split("$tp$")[5]);

            if (discountcategory == "-1" && discountid != 0)
            {
                var result = await discountorsurchargeService.RemoveSurchargeOrDiscount(discountid, LoggedInEmployee.Id);
                var jobresult = await service.UpdateDiscountID<JobOrderViewModel>(joborderid, -1, LoggedInEmployee.Id);
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
                        var jobresult = await service.UpdateDiscountID<JobOrderViewModel>(joborderid, result.Id, LoggedInEmployee.Id);
                    }
                    else
                    {
                        var result = await discountorsurchargeService.UpdateSurchargeOrDiscount(discountid, byte.Parse(discountcategory), Discountedescription, discountorperc, discountamount, LoggedInEmployee.Id);
                    }
                }
            }

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> GetSurchargeDiscountDescription()
        {
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var categoryid = Int32.Parse(stringToUse.Split("$tp$")[0]);
            var invoicelang = stringToUse.Split("$tp$")[1];
            var description = "";

            string connectionstring = Configuration.GetConnectionString("DefaultConnection");
            SqlConnection SQLConn = new SqlConnection(connectionstring);
            SqlDataAdapter SQLComm = new SqlDataAdapter("procGetDiscountOrSurchargeLocalDescription", connectionstring);

            SQLComm.SelectCommand.CommandType = CommandType.StoredProcedure;
            // Add & set the date parameters
            SQLComm.SelectCommand.Parameters.Add(new SqlParameter("@DiscountOrSurchargeID", SqlDbType.Int)).Value = categoryid;
            SQLComm.SelectCommand.Parameters.Add(new SqlParameter("@LangIANACode", SqlDbType.NVarChar, -1)).Value = invoicelang;
            //If GroupTypeDropdown = "External" Then

            try
            {
                DataSet LocalDescription = new DataSet();
                SQLComm.Fill(LocalDescription);
                description = LocalDescription.Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Error running stored procedure to retrieve local translation of the discount or surcharge description.");
            }
            finally
            {
                try
                {
                    // Clean up
                    SQLComm.SelectCommand.Connection.Close();
                    SQLComm.Dispose();
                }
                catch (SqlException e)
                {
                    // Log an event in the Application Event Log.
                    throw;
                }
            }



            //SqlDataReader SQLReader = null;

            //try
            //{
            //    SQLConn.Open();
            //    SQLReader = SQLComm.ExecuteReader();

            //    // Retrieve single result and assign data
            //    if ((SQLReader != null) && SQLReader.Read())
            //    {
            //        description = Convert.ToDouble(SQLReader["TotalGBPValue"]);
            //    }
            //    else
            //    {
            //        throw new Exception("Total order value could not be calculated due to a database stored procedure error.");
            //    }
            //}
            //finally
            //{
            //    try
            //    {
            //        if (SQLReader != null)
            //        {
            //            SQLReader.Close();
            //        }
            //        SQLConn.Dispose();
            //    }
            //    catch (SqlException se)
            //    {
            //        // Log an event in the Application Event Log.
            //        throw;
            //    }
            //}

            return Ok(description);
        }

        public async Task<IActionResult> CreateJobOrder(int ContactID)
        {
            if (ContactID == 0)
                return NotFound();
            string username = HttpContext.User.Identity.Name;
            username = GeneralUtils.GetUsernameFromNetwokUsername(username);
            Employee EmployeeCurrentlyLoggedIn = await employeeService.IdentifyCurrentUser<Employee>(LogonUserName: username);
            var contact = await contactService.GetContactDetails(ContactID);
            var org = await orgService.GetOrgDetails(contact.OrgId);
            var department = await employeeService.GetEmployeeDepartment(EmployeeCurrentlyLoggedIn.Id); var emp = await employeeService.GetEmployeeByUsername(username);
            if (org.OrgGroupId != 18520)
            {
                if (EmployeeCurrentlyLoggedIn.TeamId == 5 || EmployeeCurrentlyLoggedIn.TeamId == 16 || EmployeeCurrentlyLoggedIn.TeamId == 13 || EmployeeCurrentlyLoggedIn.TeamId == 38 || department.Id == 1 || emp.AttendsBoardMeetings == true)
                {
                    return Redirect("/Page/Locked");
                }
            }
            JobOrderCreationViewModel model = new JobOrderCreationViewModel();
            model.ContactId = contact.Id;
            model.ContactName = contact.Name;
            model.OrgId = org.Id;
            model.OrgName = org.OrgName;
            model.ListOfEmployees = await employeesService.GetAllEmployees<Employee>(false, false);
            model.AllAvailableCurrencies = await currenciesService.GetAllENCurrencies();
            model.JobOrdeChannels = await joborderchannelservice.GetAllJobOrderChannels<JobOrderChannel>();
            model.SurchargesCategories = await discountsurchargeService.GetAllSurchargeCategories();
            model.DiscountCategories = await discountsurchargeService.GetAllDiscountCategories();
            model.EndClients = await endclientService.GetAllEndClients();
            model.InvoiceCurrencyId = org.InvoiceCurrencyId == null ? 0 : org.InvoiceCurrencyId;
            var opsOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(org.Id,
               Enumerations.DataObjectTypes.Org,
               Enumerations.EmployeeOwnerships.OperationsLead);
            model.Ownership = (short)(opsOwnership == null ? 0 : opsOwnership.EmployeeId);
            return View("Views/TMS/CreateJobOrder.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateJobOrderAsync()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobOrder model = new JobOrder();
            var allFields = stringToProcess.Split("$");
            model.ContactId = Convert.ToInt32(allFields[0]);
            model.TypeOfOrder = Convert.ToInt32(allFields[1]);
            model.JobOrderChannelId = Convert.ToByte(allFields[2]);
            model.JobName = allFields[3];
            model.ProjectManagerEmployeeId = short.Parse(allFields[4]);
            model.OverallDeliveryDeadline = Convert.ToDateTime(allFields[5]);
            model.ClientCurrencyId = short.Parse(allFields[6]);
            model.IsAtrialProject = Convert.ToBoolean(allFields[7]);
            model.ClientPonumber = allFields[10];
            model.ClientNotes = allFields[11];
            model.InternalNotes = allFields[12];
            model.PrintingProject = Convert.ToBoolean(allFields[13]);
            model.Priority = Convert.ToByte(allFields[14]);
            model.IsHighlyConfidential = Convert.ToBoolean(allFields[15]);
            if (allFields[16] != "")
            {
                model.LinkedJobOrderId = Convert.ToInt32(allFields[16]);
            }
            var contact = await contactService.GetContactDetails(model.ContactId);
            var org = await orgService.GetOrgDetails(contact.OrgId);
            if (org.OrgGroupId == 72112 || org.OrgGroupId == 72113 || org.OrgGroupId == 72114 || org.OrgGroupId == 72115)
            {
                model.OrgHfmcodeBs = org.HfmcodeBs == null || org.HfmcodeBs == "" ? "GB8042" : org.HfmcodeBs;
                model.OrgHfmcodeIs = org.HfmcodeIs == null || org.HfmcodeIs == "" ? "GB8042" : org.HfmcodeIs;
            }
            if(org.InvoiceBlanketPonumber!=null && org.InvoiceBlanketPonumber != "")
            {
                model.ClientPonumber = org.InvoiceBlanketPonumber;
            }
            var folderTemplate = allFields[17];
            model.SetupByEmployeeId = loggedInEmployee.Id;
            JobOrder newRequest = null;
            newRequest = await service.CreateJobOrder(model);
            if (newRequest == null)
            {
                string MessagsString = String.Format("Error$Something went wrong while saving new Job Order on the database.");
                return Ok(MessagsString);
            }

            return Ok($"Success$Job order created successfully.${newRequest.Id}${folderTemplate}");
        }
        public async Task<IActionResult> ConfigureNetworkFolders()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            JobOrder model = new JobOrder();
            var allFields = stringToProcess.Split("$");
            var jobOrderID = Convert.ToInt32(allFields[0]);
            var folderTemplate = allFields[1];
            await service.configureNetworkFolders(folderTemplate, jobOrderID);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> ImportWordCounts()
        {
            var loggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            string MessagsString = string.Empty;
            bool? importRadio = null;
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;
            var allFields = stringToProcess.Split("$");
            var folderPath = allFields[0];
            var importWordCountsRadio = allFields[1];
            if (importWordCountsRadio != "2")
            {
                importRadio = Convert.ToBoolean(Convert.ToInt32(allFields[1]));
            }
            var jobOrderID = Convert.ToInt32(allFields[2]);
            bool IsThereAnyXML = false;
            var DirInfo = new DirectoryInfo(folderPath);
            var XMLCount = DirInfo.GetFiles("*.xml", SearchOption.TopDirectoryOnly).Count();
            if (XMLCount > 0) { IsThereAnyXML = true; }
            if (DirInfo == null) { MessagsString = "Error$Current folder can't be found"; return Ok(MessagsString); }
            if (IsThereAnyXML)
            {
                var result = await InsertWordCountsInJobItems(jobOrderID, folderPath, loggedInEmployee, importRadio);
                if (result != null)
                {
                    MessagsString = "Success$Matching job items were updated successfully and notification has been sent.";
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

        public async Task<IActionResult> InsertWordCountsInJobItems(int JobOrderID, string ImportFolderPath, ViewModels.EmployeeModels.EmployeeViewModel EmployeeCurrentlyLoggedOn, bool? ImportOnlyForClient = null)
        {

            var DirToBrowse = new DirectoryInfo(ImportFolderPath);
            var OrderedXmlFilesArray = DirToBrowse.GetFiles("*.xml", SearchOption.TopDirectoryOnly).OrderByDescending(o => o.LastWriteTime).ToList();
            System.Data.DataTable NotificationTable = new System.Data.DataTable();
            NotificationTable.Columns.Add("Job Item ID");
            NotificationTable.Columns.Add("Item Source/Target languages");
            NotificationTable.Columns.Add("XmlName");
            NotificationTable.Columns.Add("XmlDate");

            string[] EnglishCodes = { "en-gb", "en-us" };

            DataTable MissingImportsNotificationTable = new System.Data.DataTable();
            MissingImportsNotificationTable.Columns.Add("Job Item ID");
            MissingImportsNotificationTable.Columns.Add("Source language");
            MissingImportsNotificationTable.Columns.Add("Target Language");

            var JobItems = await jobitemservice.GetByJobOrderId(JobOrderID);
            var ParentJobOrder = await service.GetById(JobOrderID);
            var OrderContact = await contactService.GetContactDetails(ParentJobOrder.ContactId);
            var ParentOrg = await orgService.GetOrgDetails(OrderContact.OrgId);
            var ParentOrgGroup = await orgGroupsLogic.GetOrgGroupDetails((int)ParentOrg.OrgGroupId);
            foreach (var jobItem in JobItems)
            {
                string SourceLanguageName = string.Empty;
                string TargetLanguageName = string.Empty;

                var SourceLanguage = await cachedService.GetAllLanguagesCached();
                SourceLanguageName = SourceLanguage.Where(o => o.StringValue == jobItem.SourceLanguageIanacode).Select(o => o.Name).FirstOrDefault();
                var TargetLanguage = await cachedService.GetAllLanguagesCached();
                TargetLanguageName = TargetLanguage.Where(o => o.StringValue == jobItem.TargetLanguageIanacode).Select(o => o.Name).FirstOrDefault();

                if (jobItem.LanguageServiceId == 1 || jobItem.LanguageServiceId == 17 || jobItem.LanguageServiceId == 46 || jobItem.LanguageServiceId == 72 || jobItem.LanguageServiceId == 73 || jobItem.LanguageServiceId == 74 || jobItem.LanguageServiceId == 21 || jobItem.LanguageServiceId == 99)
                {
                    string[] JobItemSourceIanaCodeWithRegion = { languageLogicService.MapLangIanaCodeWithRegion(jobItem.SourceLanguageIanacode).ToLower() };
                    string[] JobItemTargetIanaCodeWithRegion = { languageLogicService.MapLangIanaCodeWithRegion(jobItem.TargetLanguageIanacode).ToLower() };
                    bool isMissingImportFile = true;
                    foreach (FileInfo xmlDoc in OrderedXmlFilesArray)
                    {

                        if (xmlDoc.Name.StartsWith("Analyze Files") == true)
                        {
                            var LangsArr = xmlDoc.Name.Split("_");
                            var XMLSourceIanaCodeWithRegion = LangsArr[0].Substring(LangsArr[0].LastIndexOf(" ") + 1, 5);
                            var XMLTargetIanaCodeWithRegion = LangsArr[1].Substring(0, LangsArr[1].IndexOfAny(new char[] { '(', '.' }));

                            if (SourceLanguageName == "English") { JobItemSourceIanaCodeWithRegion = EnglishCodes; }
                            if (TargetLanguageName == "English") { JobItemTargetIanaCodeWithRegion = EnglishCodes; }

                            if (JobItemSourceIanaCodeWithRegion.Contains(XMLSourceIanaCodeWithRegion.ToLower()) && JobItemTargetIanaCodeWithRegion.Contains(XMLTargetIanaCodeWithRegion.ToLower()))
                            {
                                TPWordCountBreakdownBatchModel ItemImport = wordCountBreakdownBatchService.WordCountBreakdownBatch(fileSystemService.UnMapTPNetworkPath(xmlDoc.FullName), ParentOrg, ParentJobOrder, Global_Settings.Enumerations.MemoryApplications.NoneOrUnknown, false, jobItem);
                                if (ImportOnlyForClient == true)
                                {
                                    var result = await jobitemservice.ApplyToJobItem(jobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, EmployeeCurrentlyLoggedOn, ItemImport, true, true);
                                }
                                else if (ImportOnlyForClient == false)
                                {
                                    var result = await jobitemservice.ApplyToJobItem(jobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, EmployeeCurrentlyLoggedOn, ItemImport, false);
                                }
                                else
                                {
                                    var result = await jobitemservice.ApplyToJobItem(jobItem.Id, ParentOrgGroup.Id, ParentOrg.Id, EmployeeCurrentlyLoggedOn, ItemImport);
                                }
                                var SourceAndTargetLangs = SourceLanguageName + " - " + TargetLanguageName;
                                NotificationTable.Rows.Add(jobItem.Id, SourceAndTargetLangs, xmlDoc.Name, xmlDoc.LastWriteTime);
                                isMissingImportFile = false;
                                break;
                            }
                        }
                    }
                    if (isMissingImportFile == true)
                    {
                        MissingImportsNotificationTable.Rows.Add(jobItem.Id, SourceLanguageName, TargetLanguageName);
                    }
                }
            }

            string ImportOptionString = "The word counts were updated for <b>both the client and the linguist</b>";
            if (ImportOnlyForClient == true) { ImportOptionString = "The word counts were updated for <b>Client</b>"; }
            else if (ImportOnlyForClient == false) { ImportOptionString = "The word counts were updated for <b>Linguist</b>"; }
            if (NotificationTable.Rows != null)
            {
                string EmailBody = String.Format("<p>Dear {0},<br /><br />The following Job items were updated with the word counts from their corresponding xml file, " +
                                       "which you can find in this folder <a href=\"{1}\">{1}</a><br /><br />{2}.<br /><br />" +
                                       "<table border=\"1\" width=\"900\"><tr><td><b>Job Item ID</b></td><td><b>Job Item Source/Target Language</b></td><td><b>XML document Name</b></td><td><b>Last modified</b></td></tr>", EmployeeCurrentlyLoggedOn.FirstName, ImportFolderPath, ImportOptionString);

                foreach (DataRow row in NotificationTable.Rows)
                {
                    EmailBody += String.Format("<tr><td><a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id={0}\">{0}</a></td><td>{1}</td><td>{2}</td><td>{3}</td></tr>", row.ItemArray[0], row.ItemArray[1], row.ItemArray[2], row.ItemArray[3]);

                }
                EmailBody += "</table>";
                if (MissingImportsNotificationTable.Rows != null)
                {
                    EmailBody += "<p><br /><br />The following Job items were missing their corresponding xml file, " +
                                       "please update the following items manually.<br /><br />" +
                                       "<table border=\"1\" width=\"800\"><tr><td><b>Job Item ID</b></td><td><b>Source Language</b></td><td><b>Target Language</b></td></tr>";

                    foreach (DataRow rows in MissingImportsNotificationTable.Rows)
                    {
                        EmailBody += String.Format("<tr><td><a href=\"https://myplusbeta.publicisgroupe.net/JobItem?Id={0}\">{0}</a></td><td>{1}</td><td>{2}</td></tr>", rows.ItemArray[0], rows.ItemArray[1], rows.ItemArray[2]);

                    }
                    EmailBody += "</table>";
                }
                EmailBody += "</p>";
                emailService.SendMail("flow plus <flowplus@translateplus.com>", "cristian-laurentiu.necula@translateplus.com, " + EmployeeCurrentlyLoggedOn.EmailAddress, "Successful word counts import in Job items",
                                        EmailBody);
                //emailService.SendMail("my plus <myplus@translateplus.com>", EmployeeCurrentlyLoggedOn.EmailAddress, "Successful word counts import in Job items",
                //                        EmailBody);
            }
            else
            {
                emailService.SendMail("flow plus <flowplus@translateplus.com>", EmployeeCurrentlyLoggedOn.EmailAddress, "No corresponding xml files for word counts import in Job items were found", "<p>No XML files were found that match the Languages of the Job items.</p>");
            }

            return Ok();
        }

        }
}
