using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Global_Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Services;
using Services.Interfaces;
using ViewModels.OrgGroup;
using System.IO;

namespace SmartAdmin.WebUI.Controllers
{
    public class OrgGroupController : Controller
    {
        private IConfiguration Configuration;
        private ITPOrgGroupsLogic tPOrgGroupsLogic;
        private ITPEmployeesService tPEmployeesService;
        private ITPJobOrderService tpJOservice;
        private readonly ITPriceListsService priceListService;
        private readonly ITPQuoteTemplatesService quoteTemplateService;
        private ITPOrgsLogic tPOrgsLogic;

        public OrgGroupController(IConfiguration _configuration, ITPQuoteTemplatesService _quoteTemplateService, ITPriceListsService _priceListService, ITPJobOrderService _tpJOservice, ITPOrgGroupsLogic _tPOrgGroupsLogic, ITPEmployeesService _tPEmployeesService, ITPOrgsLogic _tPOrgsLogic)
        {
            Configuration = _configuration;
            tPOrgGroupsLogic = _tPOrgGroupsLogic;
            tPEmployeesService = _tPEmployeesService;
            tPOrgsLogic = _tPOrgsLogic;
            tpJOservice = _tpJOservice;
            priceListService = _priceListService;
            quoteTemplateService = _quoteTemplateService;
        }

        public async Task<IActionResult> Index(int GroupID)
        {
            var model = new ViewModels.OrgGroup.OrgGroupViewModel();
            var result = tPOrgGroupsLogic.GetOrgGroupDetails(GroupID);
            model.GroupName = result.Result.Name;
            model.GroupID = result.Result.Id;
            model.groupCreatedOn = result.Result.CreatedDate.ToString("dd MMMM yyyy HH:mm");
            model.Description = result.Result.Description;
            model.groupModified = result.Result.LastModifiedDate?.ToString("dd MMMM yyyy HH:mm");
            model.groupCreatedBy = result.Result.CreatedByEmployeeId;
            model.EncryptedSuppliers = result.Result.OnlyAllowEncryptedSuppliers;
            model.ShowProofreadingOptionToClient = result.Result.ShowProofreadingOptionForClients;
            model.FirstJobDate = result.Result.FirstPaidJobDate?.ToString("dd MMMM yyyy");
            model.JobFilesCountryID = (short?)result.Result.JobFilesToBeSavedWithinRegion;
            model.InvoicedMarginOverLast3Months = String.Format("{0:n}", result.Result.InvoicedMarginOverLast3Months);
            if (model.InvoicedMarginOverLast3Months == "")
            {
                model.InvoicedMarginOverLast3Months = "0";
            }
            model.SpendLast12Months = "£" + String.Format("{0:n}", result.Result.ClientSpendOverLast12Months);
            model.SpendLast3Months = "£" + String.Format("{0:n}", result.Result.ClientSpendOverLast3Months);
            model.SpendLastFinancialYear = "£" + String.Format("{0:n}", result.Result.ClientSpendLastFinancialYear);
            model.SpendThisFinancialYear = "£" + String.Format("{0:n}", result.Result.ClientSpendCurrentFinancialYear);

            var BrandNames = await tPOrgGroupsLogic.GetBrandsList();
            model.BrandList = new SelectList(BrandNames, "ID", "Name");
            model.BrandNames = BrandNames;
            model.HQOrgID = result.Result.HqorgId;
            model.groupDeletedDate = result.Result.DeletedDate;

            Data.Employee LoggedInEmployee = await tPEmployeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            var loggedInEmployeeDept = await tPEmployeesService.GetEmployeeDepartment(LoggedInEmployee.Id);
            var loggedInEmployeeTeam = await tPEmployeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id);
            bool accessToAddOrgs = false;
            if (model.GroupID == 18520)
            {
                //grant access
                accessToAddOrgs = true;
            }
            else
            {
                if ((loggedInEmployeeDept.Id == 10 && LoggedInEmployee.IsTeamManager == true) || loggedInEmployeeTeam.Id == 42)
                {
                    accessToAddOrgs = true;
                }
                else if (loggedInEmployeeTeam.Id == 5 || loggedInEmployeeTeam.Id == 16 || loggedInEmployeeTeam.Id == 38 || loggedInEmployeeTeam.Id == 13 ||
                    (tPEmployeesService.AttendsBoardMeetings(LoggedInEmployee.Id).Result == true && LoggedInEmployee.Id != tPEmployeesService.CurrentSalesDepartmentManager().Result.Id))
                {
                    accessToAddOrgs = false;
                }
                else
                {
                    accessToAddOrgs = true;
                }
            }

            var employee = await tPEmployeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (employee == null) { return Redirect("/Page/Error404"); }
                
            model.LoggedInEmployee = employee;
            if (result.Result.LastModifiedByEmployeeId is not null)
            {
                model.groupModifiedBy = result.Result.LastModifiedByEmployeeId;
                var thisEmployeeModified = tPEmployeesService.IdentifyCurrentUserByIdTerminate((int)model.groupModifiedBy);
                model.groupModifiedByImageBase64 = thisEmployeeModified.Result.ImageBase64;
                model.groupModifiedByName = thisEmployeeModified.Result.FirstName + ' ' + thisEmployeeModified.Result.Surname;
            }

            
            model.EmployeCurrentlyLoggedInID = LoggedInEmployee.Id;
            var thisEmployeeCreated = tPEmployeesService.IdentifyCurrentUserByIdTerminate(model.groupCreatedBy);
            model.groupCreatedByName = thisEmployeeCreated.Result.FirstName + ' ' + thisEmployeeCreated.Result.Surname;
            model.groupCreateByImageBase64 = thisEmployeeCreated.Result.ImageBase64;
            var OrgSearchResults = await tPOrgGroupsLogic.GetOrgs(GroupID.ToString());
            model.OrgResutls = OrgSearchResults;
            model.ApprovedOrBlockedLinguists = await tPOrgsLogic.GetApprovedOrBlockedLinguists(model.GroupID, ((int)Enumerations.DataObjectTypes.OrgGroup));
            model.AllQuoteTemplates = await quoteTemplateService.GetQuoteTemplatesForDataObjectTypeAndId(model.GroupID, (int)Enumerations.DataObjectTypes.OrgGroup);
            model.AllPriceLists = await priceListService.GetAllPriceListstForDataObjectType(model.GroupID, (int)Enumerations.DataObjectTypes.OrgGroup, false);
            model.ContractExpiryDate = result.Result.ContractExpiryDate;
            ViewBag.loggedInEmployeeDeptID = loggedInEmployeeDept.Id;
            return View("Views/flow plus/OrgGroup.cshtml", model);
        }

        [HttpPost("api/GroupUpdate")]
        public async Task<bool> GroupUpdate(OrgGroupViewModel model)
        {
            if (model.HQOrgID != null)
            {
                var res = await tPOrgGroupsLogic.GroupUpdate(model, true);
            }
            else
            {
                var res = await tPOrgGroupsLogic.GroupUpdate(model, false);
            }

            return true;
        }

        [Route("[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateProofreadingOption()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToProcess = content.Result;

            var groupId = Int32.Parse(stringToProcess.Split("$")[0]);
            var showProofreadingOption = Boolean.Parse(stringToProcess.Split("$")[1]);

            await tPOrgGroupsLogic.UpdateShowProofreadingOptionSetting(groupId, showProofreadingOption);

            return Ok();
        }
        [HttpPost("api/GroupContractUpdate")]
        public async Task<bool> GroupContractUpdate(OrgGroupViewModel model)
        {
            if (model.ContractExpiryDate != null)
            {
                var res = await tPOrgGroupsLogic.GroupContractUpdate(model);
            }
            

            return true;
        }

    }
}
