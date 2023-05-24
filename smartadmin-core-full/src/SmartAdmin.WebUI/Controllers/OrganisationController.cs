using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.Organisation;
using ViewModels.EmployeeModels;
using System.Linq;
using Global_Settings;
using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using ViewModels.flowplusLicences;
using Data;

namespace SmartAdmin.WebUI.Controllers
{
    public class OrganisationController : Controller
    {
        private readonly ITPOrgsLogic orgsService;
        private readonly ICommonCachedService commonCachedService;
        private readonly ITPCurrenciesLogic curencyService;
        private readonly ITPEmployeeOwnershipsLogic employeeOwnershipsLogic;
        private readonly ITPEmployeesService employeeService;
        private readonly ITPContactsLogic contatsService;
        private readonly IOrgGroupService orgGroupService;
        private readonly ITPJobOrderService jobOrderService;
        private readonly ITPriceListsService priceListService;
        private readonly ITPQuoteTemplatesService quoteTemplateService;
        private readonly ITPFileSystemService fileSystemService;
        private readonly ITPClientInvoicesLogic clientInvoicesService;
        private readonly IConfiguration configuration;
        private readonly ITPOrgGroupsLogic orgGroupsLogic;
        private readonly ITPflowplusLicencingLogic flowplusLicencingLogic;
        private readonly ITPJobItemService jobItemService;
        private readonly IEmailUtilsService emailService;

        public OrganisationController(ITPOrgsLogic orgsService,
            ICommonCachedService commonCachedService,
            ITPCurrenciesLogic curencyService,
            ITPEmployeeOwnershipsLogic employeeOwnershipsLogic,
            ITPEmployeesService employeeService,
            ITPContactsLogic contatsService,
            IOrgGroupService orgGroupService,
            ITPJobOrderService jobOrderService,
            ITPriceListsService priceListService,
            ITPQuoteTemplatesService quoteTemplateService,
            ITPFileSystemService fileSystemService,
            ITPClientInvoicesLogic clientInvoicesService,
            IConfiguration configuration,
            ITPOrgGroupsLogic orgGroupsLogic,
            ITPflowplusLicencingLogic _flowplusLicencingLogic,
            ITPJobItemService _jobItemService,
            IEmailUtilsService _emailService)
        {
            this.orgsService = orgsService;
            this.commonCachedService = commonCachedService;
            this.curencyService = curencyService;
            this.employeeOwnershipsLogic = employeeOwnershipsLogic;
            this.employeeService = employeeService;
            this.contatsService = contatsService;
            this.orgGroupService = orgGroupService;
            this.jobOrderService = jobOrderService;
            this.priceListService = priceListService;
            this.quoteTemplateService = quoteTemplateService;
            this.fileSystemService = fileSystemService;
            this.clientInvoicesService = clientInvoicesService;
            this.configuration = configuration;
            this.orgGroupsLogic = orgGroupsLogic;
            this.flowplusLicencingLogic = _flowplusLicencingLogic;
            this.jobItemService = _jobItemService;
            this.emailService = _emailService;
        }

        public async Task<IActionResult> Index(int id)
        {
            var org = await orgsService.GetOrg(id);
            if (org == null)
                return Redirect("/Page/Error404");

            var employee = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (employee == null)
                return Redirect("/Page/Error404");

            var viewModel = new OrgPageViewModel();
            try
            {
                var config = new GlobalVariables();
                configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

                // Check who is allowed to edit certain fields
                //viewModel.AllowedToEdit = IsAllowedToEdit(employee, org.OrgGroupId.GetValueOrDefault());
                //viewModel.IsEnabledAllowPriorityCheckbox = await EnableOrDisableCertainCheckboxes(employee, org.Id);
                //viewModel.IsEnabledLinguistRatingCheckbox = IsEnabledLinguistRatingCheckbox(employee) == false ? "disabled" : null;
                //viewModel.IsEnabledForClientAutomationCheckbox = IsEnabledForClientAutomationCheckbox(employee) == false ? "disabled" : null;

                viewModel.LoggedInEmployee = employee;
                viewModel.Organisation = org;
                viewModel.Currencies = await curencyService.GetAll();
                viewModel.AltairCorporateGroups = await orgGroupService.GetAllAltairCorporateGroups();
                viewModel.OrgIndustryRelationships = await orgsService.GetOrgIdustryRelationship(id);
                viewModel.OrgTechnologyRelationships = await orgsService.GetOrgTechnologyRelationship(id);
                viewModel.Organisation.DefaultInvoiceContacts = await orgsService.GetDefaultInvoiceContanctsForOrg(org.Id, org.OrgGroupId);
                viewModel.AllContacts = await contatsService.GetAllContactsForOrg(org.Id);
                viewModel.AllPriceLists = await priceListService.GetAllPriceListstForDataObjectType(org.Id, (int)Enumerations.DataObjectTypes.Org, false);
                viewModel.AllQuoteTemplates = await quoteTemplateService.GetQuoteTemplatesForDataObjectTypeAndId(org.Id, (int)Enumerations.DataObjectTypes.Org);
                string pathToSharedDocs = Path.Combine(config.ExtranetNetworkBaseDirectoryPath, "Orgs", org.Id.ToString(), "SharedDocs");
                viewModel.SharedIplusFiles = await fileSystemService.GetDownloadableDirectoryContents(pathToSharedDocs, org.Id, ((int)Enumerations.DataObjectTypes.Org));
                string serverLocationPath = string.Empty;
                if (org.JobServerLocation == null || org.JobServerLocation == 0)
                {
                    serverLocationPath = config.LondonJobDriveBaseDirectoryPathForApp;
                }
                else if (org.JobServerLocation == 1)
                {
                    serverLocationPath = config.SofiaJobDriveBaseDirectoryPathForApp;
                }
                else if (org.JobServerLocation == 2)
                {
                    serverLocationPath = config.ParisJobDriveBaseDirectoryPathForApp;
                }
                viewModel.KeyClientInfoFolder = fileSystemService.GetNetworkKeyClientInfoDirectoryPath(serverLocationPath, org.Id);
                viewModel.ApprovedOrBlockedLinguists = await orgsService.GetApprovedOrBlockedLinguists(org.Id, ((int)Enumerations.DataObjectTypes.Org));
                viewModel.OrgSLA = org.OrgSLA;
                viewModel.Countries = await commonCachedService.GetAllCountriesCached();
                viewModel.Regions = await commonCachedService.GetAllAltairRegionsCached();
                viewModel.OrgIndustrySectors = await commonCachedService.GetAllOrgIndustrySectorsCached();
                viewModel.OrgSalesCategories = await commonCachedService.GetAllOrgSalesCategoriesCached();
                viewModel.ClientTechnologies = await commonCachedService.GetAllClientTechnologiesCached();
                viewModel.OrgMainIndustries = await commonCachedService.GetAllOrgMainIndustriesCached();
                viewModel.OrgLegalStatusCategories = await commonCachedService.GetAllOrgLegalStatusCategoriesCached();
                viewModel.OrgIntroSources = await commonCachedService.GetAllOrgIntroductionSourcesCached();
                viewModel.DecisionReasons = await commonCachedService.GetDecisionReasonsType1Cached();

                bool accessToAddContact = false;
                if (org.DeletedDate == null)
                {
                    if (org.OrgGroupId == 18520)
                    {
                        //grant access
                        accessToAddContact = true;
                    }
                    else
                    {
                        var CurrentSalesDepartmentManager = await employeeService.CurrentSalesDepartmentManager();

                        if (employee.TeamId == 16 || employee.TeamId == 38 ||
                            (await employeeService.AttendsBoardMeetings(employee.Id) == true && employee.Id != CurrentSalesDepartmentManager.Id && employee.Id != 475))
                        {
                            accessToAddContact = false;
                        }
                        else
                        {
                            accessToAddContact = true;
                        }
                    }
                }
                else
                {
                    accessToAddContact = false;
                }

                viewModel.AllowedToAddContact = accessToAddContact;

                viewModel.Organisation.NetworkPriceListInfoDirectoryPathForApp = orgsService.NetworkPriceListInfoDirectoryPathForApp(org.Id, org.JobServerLocation);
            }
            catch (System.Exception ex)
            {
                var a = ex.Message;
                throw;
            }
            return View(viewModel);
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<bool> Update(OrgPageUpdateModel model)
        {
            if (!ModelState.IsValid)
                return false;

            try
            {
                var res = await orgsService.Update(model);
            }
            catch (Exception)
            {

                throw;
            }

            return true;
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateFlowPlusLicencingInfo(int Id, int AccessForDataObjectID, byte AccessForDataObjectTypeID,
                                                                     bool CreateSingleOrderForAllLicences, string Notes,
                                                                     flowPlusLicenceModel flowPlusLicence,
                                                                     flowPlusLicenceModel reviewPlusLicence,
                                                                     flowPlusLicenceModel translateOnlineLicence,
                                                                     flowPlusLicenceModel designPlusLicence,
                                                                     flowPlusLicenceModel AIOrMTLicence,
                                                                     flowPlusLicenceModel CMSLicence
                                                                     )
        {
            var loggedInEmployee = await employeeService.GetLoggedInEmployeeFromSessionOrDatabase();
            var newJobOrderCreated = false;
            var config = new GlobalVariables();
            configuration.GetSection(GlobalVariables.GlobalVars).Bind(config);

            string recipients = loggedInEmployee.EmailAddress;
            var allSalesManagers = await employeeService.GetAllManagersInDeparmenet(1, AnyTeamIDsToExclude:"5,4");
            foreach (Employee emp in allSalesManagers)
            {
                recipients += ", " + emp.EmailAddress;
            }

            if (flowPlusLicence.DemoEnabled == true)
            {
                reviewPlusLicence.DemoEnabled = true;
                translateOnlineLicence.DemoEnabled = true;
                designPlusLicence.DemoEnabled = true;
                AIOrMTLicence.DemoEnabled = true;
                CMSLicence.DemoEnabled = true;
            }

            if (Id == 0)
            //create a new flowplus Licencing mapping
            {
                var flowplusLicenceDetails = new flowPlusLicences();
                if (flowPlusLicence.IsEnabled == true)
                {
                    flowplusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(1, flowPlusLicence.AppCost, flowPlusLicence.DemoEnabled, flowPlusLicence.IsEnabled, loggedInEmployee.Id, flowPlusLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && flowplusLicenceDetails.AppCost > 0 && flowplusLicenceDetails.DemoEnabled == false && flowplusLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: flowplusLicenceDetails.Id);

                        await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(flowplusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var reviewPlusLicenceDetails = new flowPlusLicences();

                if (reviewPlusLicence.IsEnabled == true)
                {
                    reviewPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(2, reviewPlusLicence.AppCost, reviewPlusLicence.DemoEnabled, reviewPlusLicence.IsEnabled, loggedInEmployee.Id, reviewPlusLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && reviewPlusLicenceDetails.AppCost > 0 && reviewPlusLicenceDetails.DemoEnabled == false && reviewPlusLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: reviewPlusLicenceDetails.Id);
                        var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(reviewPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var translateOnlineLicenceDetails = new flowPlusLicences();

                if (translateOnlineLicence.IsEnabled == true)
                {
                    translateOnlineLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(3, translateOnlineLicence.AppCost, translateOnlineLicence.DemoEnabled, translateOnlineLicence.IsEnabled, loggedInEmployee.Id, translateOnlineLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && translateOnlineLicenceDetails.AppCost > 0 && translateOnlineLicenceDetails.DemoEnabled == false && translateOnlineLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: translateOnlineLicenceDetails.Id);
                        var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(translateOnlineLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var designPlusLicenceDetails = new flowPlusLicences();

                if (designPlusLicence.IsEnabled == true)
                {
                    designPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(4, designPlusLicence.AppCost, designPlusLicence.DemoEnabled, designPlusLicence.IsEnabled, loggedInEmployee.Id, designPlusLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && designPlusLicenceDetails.AppCost > 0 && designPlusLicenceDetails.DemoEnabled == false && designPlusLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: designPlusLicenceDetails.Id);
                        var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(designPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var AIOrMTPlusLicenceDetails = new flowPlusLicences();
                if (AIOrMTLicence.IsEnabled == true)
                {
                    AIOrMTPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(5, AIOrMTLicence.AppCost, AIOrMTLicence.DemoEnabled, AIOrMTLicence.IsEnabled, loggedInEmployee.Id, AIOrMTLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && AIOrMTPlusLicenceDetails.AppCost > 0 && AIOrMTPlusLicenceDetails.DemoEnabled == false && AIOrMTPlusLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: AIOrMTPlusLicenceDetails.Id);
                        var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(AIOrMTPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var CMSLicenceDetails = new flowPlusLicences();
                if (CMSLicence.IsEnabled == true)
                {
                    CMSLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(6, CMSLicence.AppCost, CMSLicence.DemoEnabled, CMSLicence.IsEnabled, loggedInEmployee.Id, CMSLicence.OrderContactID);
                    if (CreateSingleOrderForAllLicences == false && CMSLicenceDetails.AppCost > 0 && CMSLicenceDetails.DemoEnabled == false && CMSLicenceDetails.OrderContactID != null)
                    {
                        newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: CMSLicenceDetails.Id);
                        var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(CMSLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                    }
                }

                var licenceMapping = await flowplusLicencingLogic.CreateFlowPlusLicenceMapping(AccessForDataObjectID, AccessForDataObjectTypeID, CreateSingleOrderForAllLicences, Notes,
                                                                          flowplusLicenceDetails == null ? null : flowplusLicenceDetails.Id,
                                                                          reviewPlusLicenceDetails == null ? null : reviewPlusLicenceDetails.Id,
                                                                          translateOnlineLicenceDetails == null ? null : translateOnlineLicenceDetails.Id,
                                                                          designPlusLicenceDetails == null ? null : designPlusLicenceDetails.Id,
                                                                          AIOrMTPlusLicenceDetails == null ? null : AIOrMTPlusLicenceDetails.Id,
                                                                          CMSLicenceDetails == null ? null : CMSLicenceDetails.Id);

                //set up job order if all details are filled in correctly
                if (CreateSingleOrderForAllLicences == true)
                {
                    if (licenceMapping.flowplusLicenceID != null)
                    {
                        var flowplusLicence = await flowplusLicencingLogic.GetflowPlusLicence(licenceMapping.flowplusLicenceID.Value);
                        if (flowplusLicence.AppCost > 0 && flowplusLicence.DemoEnabled == false &&
                            flowplusLicence.IsEnabled == true && flowplusLicence.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(true, LicenceMappingId: licenceMapping.Id);
                            var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(flowplusLicence.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                }


                //send email notification to sales and Ad of the new licence set up

                if (AccessForDataObjectTypeID == 3)
                {
                    var group = await orgGroupsLogic.GetOrgGroupDetails(AccessForDataObjectID);

                    emailService.SendMail("flow plus <flowplus@translateplus.com>", recipients,
                                          String.Format("flow plus licencing enabled for group \"{0}\"", group.Name),
                                          String.Format("<p>flow plus licencing has been enabled for group " +
                                          "\"<a href=\"https://myplusbeta.publicisgroupe.net/OrgGroup?groupid={1}\">{0}</a>\" " +
                                          "by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{2}\">{3}</a></p>",
                                          group.Name, group.Id, loggedInEmployee.Id, loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                          CCRecipients:config.InternalDirectorsRecipientAddress);
                }
                else
                {
                    //add sales owner to the notification
                    var AMOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(AccessForDataObjectID, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead);
                    if (AMOwnership != null)
                    {
                        recipients = employeeService.IdentifyCurrentUserById(AMOwnership.EmployeeId).Result.EmailAddress + ", " + recipients;
                    }

                    var NewBusinessOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(AccessForDataObjectID, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead);
                    if (NewBusinessOwnership != null)
                    {
                        recipients = employeeService.IdentifyCurrentUserById(NewBusinessOwnership.EmployeeId).Result.EmailAddress + ", " + recipients;
                    }

                    var org = await orgsService.GetOrgDetails(AccessForDataObjectID);

                    emailService.SendMail("flow plus <flowplus@translateplus.com>", recipients,
                                          String.Format("flow plus licencing enabled for org \"{0}\"", org.OrgName),
                                          String.Format("<p>flow plus licencing has been enabled for org " +
                                          "\"<a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={1}\">{0}</a>\" " +
                                          "by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{2}\">{3}</a></p>",
                                          org.OrgName, org.Id, loggedInEmployee.Id, loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                          CCRecipients: config.InternalDirectorsRecipientAddress);
                }

            }
            else
            {
                var flowPlusMappingObjPreUpdate = await flowplusLicencingLogic.GetflowPlusLicencingMappingDetails(Id);
                var flowplusLicenceDetails = new flowPlusLicences();

                flowPlusLicences flowPlusLicencePreUpdate = null;
                if (flowPlusLicence != null)
                {
                    flowPlusLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(flowPlusLicence.Id);
                }


                if (flowPlusLicence.IsEnabled == true)
                {

                    if (flowPlusLicence.Id == 0)
                    {
                        flowplusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(1, flowPlusLicence.AppCost, flowPlusLicence.DemoEnabled, flowPlusLicence.IsEnabled, loggedInEmployee.Id, flowPlusLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && flowplusLicenceDetails.AppCost > 0 && flowplusLicenceDetails.DemoEnabled == false && flowplusLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: flowplusLicenceDetails.Id);
                            await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(flowplusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                    else
                    {
                        flowplusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(flowPlusLicence.Id, flowPlusLicence.AppCost, flowPlusLicence.DemoEnabled, flowPlusLicence.IsEnabled, loggedInEmployee.Id, flowPlusLicence.OrderContactID);

                        if (flowPlusLicencePreUpdate.IsEnabled == false && flowplusLicenceDetails.IsEnabled == true)
                        {
                            if (CreateSingleOrderForAllLicences == false && flowplusLicenceDetails.AppCost > 0 && flowplusLicenceDetails.DemoEnabled == false && flowplusLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: flowplusLicenceDetails.Id);
                                await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(flowplusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }

                    }
                }
                else
                {
                    if (flowPlusLicence.Id != 0)
                    {
                        flowplusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(flowPlusLicence.Id, flowPlusLicence.AppCost, flowPlusLicence.DemoEnabled, flowPlusLicence.IsEnabled, loggedInEmployee.Id, flowPlusLicence.OrderContactID);
                    }
                }


                var reviewPlusLicenceDetails = new flowPlusLicences();
                if (reviewPlusLicence.IsEnabled == true)
                {
                    if (reviewPlusLicence.Id == 0)
                    {

                        reviewPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(2, reviewPlusLicence.AppCost, reviewPlusLicence.DemoEnabled, reviewPlusLicence.IsEnabled, loggedInEmployee.Id, reviewPlusLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && reviewPlusLicenceDetails.AppCost > 0 && reviewPlusLicenceDetails.DemoEnabled == false && reviewPlusLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: reviewPlusLicenceDetails.Id);
                            await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(reviewPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }

                    }
                    else
                    {
                        var reviewPlusLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(reviewPlusLicence.Id);
                        reviewPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(reviewPlusLicence.Id, reviewPlusLicence.AppCost, reviewPlusLicence.DemoEnabled, reviewPlusLicence.IsEnabled, loggedInEmployee.Id, reviewPlusLicence.OrderContactID);
                        if ((reviewPlusLicencePreUpdate.IsEnabled == false && reviewPlusLicenceDetails.IsEnabled == true) ||
                            (flowPlusMappingObjPreUpdate.CreateSingleOrderForAllLicences == true && CreateSingleOrderForAllLicences == false))
                        {
                            if (CreateSingleOrderForAllLicences == false && reviewPlusLicenceDetails.AppCost > 0 && reviewPlusLicenceDetails.DemoEnabled == false && reviewPlusLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: reviewPlusLicenceDetails.Id);
                                await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(reviewPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }
                    }
                }
                else
                {
                    if (reviewPlusLicence.Id != 0)
                    {
                        reviewPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(reviewPlusLicence.Id, reviewPlusLicence.AppCost, reviewPlusLicence.DemoEnabled, reviewPlusLicence.IsEnabled, loggedInEmployee.Id, reviewPlusLicence.OrderContactID);
                    }
                }

                var translateOnlineLicenceDetails = new flowPlusLicences();
                if (translateOnlineLicence.IsEnabled == true)
                {
                    if (translateOnlineLicence.Id == 0)
                    {
                        translateOnlineLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(3, translateOnlineLicence.AppCost, translateOnlineLicence.DemoEnabled, translateOnlineLicence.IsEnabled, loggedInEmployee.Id, translateOnlineLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && translateOnlineLicenceDetails.AppCost > 0 && translateOnlineLicenceDetails.DemoEnabled == false && translateOnlineLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: translateOnlineLicenceDetails.Id);
                            var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(translateOnlineLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                    else
                    {
                        var translateOnlineLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(translateOnlineLicence.Id);
                        translateOnlineLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(translateOnlineLicence.Id, translateOnlineLicence.AppCost, translateOnlineLicence.DemoEnabled, translateOnlineLicence.IsEnabled, loggedInEmployee.Id, translateOnlineLicence.OrderContactID);
                        if ((translateOnlineLicencePreUpdate.IsEnabled == false && translateOnlineLicenceDetails.IsEnabled == true) ||
                             (flowPlusMappingObjPreUpdate.CreateSingleOrderForAllLicences == true && CreateSingleOrderForAllLicences == false))
                        {
                            if (CreateSingleOrderForAllLicences == false && translateOnlineLicenceDetails.AppCost > 0 && translateOnlineLicenceDetails.DemoEnabled == false && translateOnlineLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: translateOnlineLicenceDetails.Id);
                                var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(translateOnlineLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }
                    }
                }
                else
                {
                    if (translateOnlineLicence.Id != 0)
                    {
                        translateOnlineLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(translateOnlineLicence.Id, translateOnlineLicence.AppCost, translateOnlineLicence.DemoEnabled, translateOnlineLicence.IsEnabled, loggedInEmployee.Id, translateOnlineLicence.OrderContactID);
                    }
                }

                var designPlusLicenceDetails = new flowPlusLicences();
                if (designPlusLicence.IsEnabled == true)
                {
                    if (designPlusLicence.Id == 0)
                    {
                        designPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(4, designPlusLicence.AppCost, designPlusLicence.DemoEnabled, designPlusLicence.IsEnabled, loggedInEmployee.Id, designPlusLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && designPlusLicenceDetails.AppCost > 0 && designPlusLicenceDetails.DemoEnabled == false && designPlusLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: designPlusLicenceDetails.Id);
                            var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(designPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                    else
                    {
                        var designPlusLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(designPlusLicence.Id);
                        designPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(designPlusLicence.Id, designPlusLicence.AppCost, designPlusLicence.DemoEnabled, designPlusLicence.IsEnabled, loggedInEmployee.Id, designPlusLicence.OrderContactID);

                        if ((designPlusLicencePreUpdate.IsEnabled == false && designPlusLicenceDetails.IsEnabled == true) ||
                             (flowPlusMappingObjPreUpdate.CreateSingleOrderForAllLicences == true && CreateSingleOrderForAllLicences == false))
                        {
                            if (CreateSingleOrderForAllLicences == false && designPlusLicenceDetails.AppCost > 0 && designPlusLicenceDetails.DemoEnabled == false && designPlusLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: designPlusLicenceDetails.Id);
                                var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(designPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }
                    }
                }
                else
                {
                    if (designPlusLicence.Id != 0)
                    {
                        designPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(designPlusLicence.Id, designPlusLicence.AppCost, designPlusLicence.DemoEnabled, designPlusLicence.IsEnabled, loggedInEmployee.Id, designPlusLicence.OrderContactID);
                    }
                }

                var AIOrMTPlusLicenceDetails = new flowPlusLicences();
                if (AIOrMTLicence.IsEnabled == true)
                {
                    if (AIOrMTLicence.Id == 0)
                    {
                        AIOrMTPlusLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(5, AIOrMTLicence.AppCost, AIOrMTLicence.DemoEnabled, AIOrMTLicence.IsEnabled, loggedInEmployee.Id, AIOrMTLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && AIOrMTPlusLicenceDetails.AppCost > 0 && AIOrMTPlusLicenceDetails.DemoEnabled == false && AIOrMTPlusLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: AIOrMTPlusLicenceDetails.Id);
                            var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(AIOrMTPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                    else
                    {
                        var AIOrMTLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(AIOrMTLicence.Id);
                        AIOrMTPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(AIOrMTLicence.Id, AIOrMTLicence.AppCost, AIOrMTLicence.DemoEnabled, AIOrMTLicence.IsEnabled, loggedInEmployee.Id, AIOrMTLicence.OrderContactID);

                        if ((AIOrMTLicencePreUpdate.IsEnabled == false && AIOrMTPlusLicenceDetails.IsEnabled == true) ||
                             (flowPlusMappingObjPreUpdate.CreateSingleOrderForAllLicences == true && CreateSingleOrderForAllLicences == false))
                        {
                            if (CreateSingleOrderForAllLicences == false && AIOrMTPlusLicenceDetails.AppCost > 0 && AIOrMTPlusLicenceDetails.DemoEnabled == false && AIOrMTPlusLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: AIOrMTPlusLicenceDetails.Id);
                                var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(AIOrMTPlusLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }
                    }
                }
                else
                {
                    if (AIOrMTLicence.Id != 0)
                    {
                        AIOrMTPlusLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(AIOrMTLicence.Id, AIOrMTLicence.AppCost, AIOrMTLicence.DemoEnabled, AIOrMTLicence.IsEnabled, loggedInEmployee.Id, AIOrMTLicence.OrderContactID);
                    }

                }

                var CMSLicenceDetails = new flowPlusLicences();
                if (CMSLicence.IsEnabled == true)
                {
                    if (CMSLicence.Id == 0)
                    {
                        CMSLicenceDetails = await flowplusLicencingLogic.CreateFlowPlusLicence(6, CMSLicence.AppCost, CMSLicence.DemoEnabled, CMSLicence.IsEnabled, loggedInEmployee.Id, CMSLicence.OrderContactID);
                        if (CreateSingleOrderForAllLicences == false && CMSLicenceDetails.AppCost > 0 && CMSLicenceDetails.DemoEnabled == false && CMSLicenceDetails.OrderContactID != null)
                        {
                            newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: CMSLicenceDetails.Id);
                            var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(CMSLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                        }
                    }
                    else
                    {
                        var CMSLicencePreUpdate = await flowplusLicencingLogic.GetflowPlusLicenceObj(CMSLicence.Id);
                        CMSLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(CMSLicence.Id, CMSLicence.AppCost, CMSLicence.DemoEnabled, CMSLicence.IsEnabled, loggedInEmployee.Id, CMSLicence.OrderContactID);
                        if ((CMSLicencePreUpdate.IsEnabled == false && CMSLicenceDetails.IsEnabled == true) ||
                             (flowPlusMappingObjPreUpdate.CreateSingleOrderForAllLicences == true && CreateSingleOrderForAllLicences == false))
                        {
                            if (CreateSingleOrderForAllLicences == false && CMSLicenceDetails.AppCost > 0 && CMSLicenceDetails.DemoEnabled == false && CMSLicenceDetails.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(false, LicenceId: CMSLicenceDetails.Id);
                                var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(CMSLicenceDetails.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }
                    }
                }
                else
                {
                    if (CMSLicence.Id != 0)
                    {
                        CMSLicenceDetails = await flowplusLicencingLogic.UpdateFlowPlusLicence(CMSLicence.Id, CMSLicence.AppCost, CMSLicence.DemoEnabled, CMSLicence.IsEnabled, loggedInEmployee.Id, CMSLicence.OrderContactID);
                    }

                }

                var licenceMapping = await flowplusLicencingLogic.UpdateFlowPlusLicenceMapping(AccessForDataObjectID, AccessForDataObjectTypeID, CreateSingleOrderForAllLicences, Notes,
                                                                          flowplusLicenceDetails == null ? null : flowplusLicenceDetails.Id,
                                                                          reviewPlusLicenceDetails == null ? null : reviewPlusLicenceDetails.Id,
                                                                          translateOnlineLicenceDetails == null ? null : translateOnlineLicenceDetails.Id,
                                                                          designPlusLicenceDetails == null ? null : designPlusLicenceDetails.Id,
                                                                          AIOrMTPlusLicenceDetails == null ? null : AIOrMTPlusLicenceDetails.Id,
                                                                          CMSLicenceDetails == null ? null : CMSLicenceDetails.Id);

                if (CreateSingleOrderForAllLicences == true)
                {
                    if (licenceMapping.flowplusLicenceID != null)
                    {
                        var flowplusLicence = await flowplusLicencingLogic.GetflowPlusLicence(licenceMapping.flowplusLicenceID.Value);
                        if (flowPlusLicencePreUpdate.IsEnabled == false && flowPlusLicence.IsEnabled == true)
                        {
                            if (flowplusLicence.AppCost > 0 && flowplusLicence.DemoEnabled == false &&
                            flowplusLicence.IsEnabled == true && flowplusLicence.OrderContactID != null)
                            {
                                newJobOrderCreated = await jobItemService.SetupFlowPlusLicencingJobOrder(true, LicenceMappingId: licenceMapping.Id);
                                var result1 = await flowplusLicencingLogic.UpdateOrderSetUpDatesOfLicence(flowplusLicence.Id, DateTime.Now, DateTime.Now.AddMonths(1));
                            }
                        }

                    }
                }

                //send email notification to sales and Ad of the new licence set up

                if (AccessForDataObjectTypeID == 3)
                {
                    var group = await orgGroupsLogic.GetOrgGroupDetails(AccessForDataObjectID);


                    emailService.SendMail("flow plus <flowplus@translateplus.com>", recipients,
                                          String.Format("flow plus licencing updated for group \"{0}\"", group.Name),
                                          String.Format("<p>flow plus licencing has been updated for group " +
                                          "\"<a href=\"https://myplusbeta.publicisgroupe.net/OrgGroup?groupid={1}\">{0}</a>\" " +
                                          "by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{2}\">{3}</a></p>",
                                          group.Name, group.Id, loggedInEmployee.Id, loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                          CCRecipients: config.InternalDirectorsRecipientAddress);
                }
                else
                {
                    //add sales owner to the notification
                    var AMOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(AccessForDataObjectID, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesAccountManagerLead);
                    if (AMOwnership != null)
                    {
                        recipients = employeeService.IdentifyCurrentUserById(AMOwnership.EmployeeId).Result.EmailAddress + ", " + recipients;
                    }

                    var NewBusinessOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(AccessForDataObjectID, Enumerations.DataObjectTypes.Org, Enumerations.EmployeeOwnerships.SalesNewBusinessLead);
                    if (NewBusinessOwnership != null)
                    {
                        recipients = employeeService.IdentifyCurrentUserById(NewBusinessOwnership.EmployeeId).Result.EmailAddress + ", " + recipients;
                    }


                    var org = await orgsService.GetOrgDetails(AccessForDataObjectID);
                    
                    emailService.SendMail("flow plus <flowplus@translateplus.com>", recipients,
                                          String.Format("flow plus licencing updated for org \"{0}\"", org.OrgName),
                                          String.Format("<p>flow plus licencing has been updated for org " +
                                          "\"<a href=\"https://myplusbeta.publicisgroupe.net/Organisation?id={1}\">{0}</a>\" " +
                                          "by <a href=\"https://myplusbeta.publicisgroupe.net/HR/EmployeeProfile/{2}\">{3}</a></p>",
                                          org.OrgName, org.Id, loggedInEmployee.Id, loggedInEmployee.FirstName + " " + loggedInEmployee.Surname),
                                          CCRecipients: config.InternalDirectorsRecipientAddress);
                }

            }


            return Ok(newJobOrderCreated);
        }

        public IActionResult SaveFile(string filePath, string clientNameOfTheFile)
        {
            var fileStream = new FileStream(filePath, FileMode.Open);

            if (fileStream == null)
                return NotFound();

            return File(fileStream, "application/octet-stream", clientNameOfTheFile);
        }

        [HttpPost()]
        public IActionResult DeleteFile(string filePath)
        {
            try
            {
                if (System.IO.File.Exists(filePath) == false)
                    throw new Exception("File not found!");

                new FileInfo(filePath).Delete();

                // attempt to delete the metadata file with it
                try
                {
                    new FileInfo(filePath + ".tpfiledesc").Delete();
                }
                catch (Exception)
                {
                }
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static bool IsAllowedToEdit(EmployeeViewModel employee, int orgGroupId = 0)
        {
            var teamIds = new int[] { 5, 16, 38, 13 };

            if (orgGroupId == 18520 || employee.Id == 1041)
                return true;

            bool AllowedToEdit;
            if (teamIds.Contains(employee.TeamId) ||
                (employee.AttendsBoardMeetings == true && employee.Id != 22)) //Svenja
                AllowedToEdit = false;
            else
                AllowedToEdit = true;

            return AllowedToEdit;
        }
        private async Task<bool> EnableOrDisableCertainCheckboxes(EmployeeViewModel employee, int orgId)
        {
            int? salesOwnerId = 0;

            var salesOwners = await employeeOwnershipsLogic.GetSalesEmployeeOwnershipForDataObjectAndOwnershipType(orgId,
                Enumerations.DataObjectTypes.Org,
                new Enumerations.EmployeeOwnerships[] { Enumerations.EmployeeOwnerships.SalesNewBusinessLead,
                                                            Enumerations.EmployeeOwnerships.SalesAccountManagerLead });

            if (salesOwners.Count() > 0)
                salesOwnerId = salesOwners?.FirstOrDefault(x => x.EmployeeId == employee.Id)?.EmployeeId;

            var opsOwnership = await employeeOwnershipsLogic.GetEmployeeOwnershipForDataObjectAndOwnershipType(orgId,
                Enumerations.DataObjectTypes.Org,
                Enumerations.EmployeeOwnerships.OperationsLead);

            int opsOwnerId = opsOwnership != null ? opsOwnership.EmployeeId : 0;

            if (employee.Department.Id == ((byte)Enumerations.Departments.CompanyDirectors) ||
                (
                    employee.IsTeamManager == true &&
                    (employee.Department.Id == ((byte)Enumerations.Departments.GeneralLinguisticServices) ||
                    employee.Department.Id == ((byte)Enumerations.Departments.TranscreationAndProduction))
                ) ||
                employee.Id == salesOwnerId ||
                employee.Id == opsOwnerId &&
                IsAllowedToEdit(employee))
            {
                return true;
            }

            return false;
        }
        private static bool IsEnabledLinguistRatingCheckbox(EmployeeViewModel employee)
        {
            // allow Linguist rating setting to be changed only by Ops management, LR, IT or Directors
            if (employee.Department.Id == ((byte)Enumerations.Departments.CompanyDirectors) ||
            employee.Department.Id == ((byte)Enumerations.Departments.IT) ||
            employee.TeamId == ((byte)Enumerations.Teams.VendorManagement) ||
            employee.TeamId == ((byte)Enumerations.Teams.TPOperationsManagement) ||
            employee.TeamId == ((byte)Enumerations.Teams.OperationsManagement) &&
            IsAllowedToEdit(employee))
                return true;

            return false;
        }
        private static bool IsEnabledForClientAutomationCheckbox(EmployeeViewModel employee)
        {
            // allow enabling/disabling auto job setup setting only to Ops management, TMs, IT or Directors
            if (employee.Department.Id == ((byte)Enumerations.Departments.CompanyDirectors) ||
            employee.Department.Id == ((byte)Enumerations.Departments.IT) ||
            (
                (
                    employee.Department.Id == ((byte)Enumerations.Departments.GeneralLinguisticServices) ||
                    employee.Department.Id == ((byte)Enumerations.Departments.TranscreationAndProduction)
                ) &&
                employee.IsTeamManager == true
            ) ||
            employee.TeamId == ((byte)Enumerations.Teams.TPOperationsManagement) ||
            employee.TeamId == ((byte)Enumerations.Teams.OperationsManagement) &&
            IsAllowedToEdit(employee))
                return true;

            return false;
        }


        [HttpPost]
        public async Task<IActionResult> GetCurrencySymbol()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var orgId = Int32.Parse(stringToProcess);

            var invoiceCurrencyId = orgsService.GetOrgDetails(orgId).Result.InvoiceCurrencyId;

            var currencyPrefix = "";

            if (invoiceCurrencyId != null)
            {
                currencyPrefix = curencyService.GetById(invoiceCurrencyId.Value).Result.Prefix;
            }


            return Ok(currencyPrefix);

        }
    }
}
