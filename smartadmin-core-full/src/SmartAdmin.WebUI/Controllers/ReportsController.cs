using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Data;
using Services;
using Services.Interfaces;
using ViewModels.Reports;
using System.IO;
using Newtonsoft.Json;

namespace SmartAdmin.WebUI.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ITPEmployeesService employeesService;
        private readonly ITPOrgsLogic orgService;
        private readonly IOrgGroupService orgGroupService;
        private readonly ITPEndClient endClientService;
        private readonly IEmailUtilsService emailService;

        public ReportsController(ITPEmployeesService service1, ITPOrgsLogic service2, ITPEndClient service3,
            IOrgGroupService service4, IEmailUtilsService service5)
        {
            this.employeesService = service1;
            this.orgService = service2;
            this.endClientService = service3;
            this.orgGroupService = service4;
            this.emailService = service5;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllOrgsForOrgGroupsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var AllOrgsForOrgGroup = await orgService.GetAllOrgsForOrgGroupString(stringToUse);
            return Ok(AllOrgsForOrgGroup);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllOrgsForInternalExternalFilterAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var AllOrgsForOrgGroup = await orgService.GetAllOrgsForInternalExternalFilters(stringToUse);
            return Ok(AllOrgsForOrgGroup);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllGroupsForInternalExternalFilterAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;
            var AllOrgGroups = await orgGroupService.GetAllGroupsForInternalExternalFilters(stringToUse);
            return Ok(AllOrgGroups);
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> ReportingToolAsync()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            //Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>("saucecca");
            EmployeeTeam employeeTeam = await employeesService.GetEmployeeTeam<EmployeeTeam>(LoggedInEmployee.Id);
            EmployeeDepartment employeeDept = await employeesService.GetEmployeeDepartment(LoggedInEmployee.Id);
            //DateTime EndDate = DateTime.Today;
            //DateTime StartDate = EndDate.AddMonths(-1);

            
            //Access level ID = Director and Senior management
            //Team IT or Nuno
            //then give access
            if (LoggedInEmployee.TeamId == 41 || LoggedInEmployee.TeamId == 15 || LoggedInEmployee.AccessLevel == 3 || LoggedInEmployee.AccessLevel == 2 || LoggedInEmployee.AccessLevel == 4 || LoggedInEmployee.Id == 203)
            {
                //allow access
            }
            else
            {
                return Redirect("/Page/Locked");
            }

            ViewBag.AllowToViewAllDepartments = false;
            ViewBag.AllowToViewAllTeamsInSelectedDept = false;

            ViewBag.CurrentTeam = employeeTeam;
            ViewBag.CurrentDept = employeeDept;


            List<EndClient> allEndClientsToShow = await endClientService.GetAllEndClient();
            List<EndClientData> allBrandsToShow = await endClientService.GetAllBrands();
            List<EndClientData> allCategoriesToShow = await endClientService.GetAllCategories();
            List<EndClientData> allCampaignsToShow = await endClientService.GetAllCampaigns();

            ViewBag.AllowToViewAllDepartments = true;
            ViewBag.AllowToViewAllTeamsInSelectedDept = true;

            var results = new ReportingToolModel()
            {
                //AllEmployees = await employeesService.GetAllEmployees<Employee>(false, false),
                //AllDepartments = await employeesService.GetAllDepartments<EmployeeDepartment>(showDeptWithNoEmployees: false),
                //AllTeams = await employeesService.GetAllTeams<EmployeeTeam>(showTeamsWithNoEmployees: false),
                AllOrgGroupsIdAndName = await orgGroupService.GetAllOrgGroupsIdAndName(true),
                AllOrgsIdAndName = await orgService.GetAllOrgsIdAndName(true),
                AllEndClients = allEndClientsToShow,
                AllBrands = allBrandsToShow,
                AllCategories = allCategoriesToShow,
                AllCampaigns = allCampaignsToShow
            };


            return View(results);

        }

        [HttpPost]
        public async Task<IActionResult> GetAllBrandsForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllBrandsForEndClient = await endClientService.GetAllBrandsForEndClientIDs(stringToUse);
            return Ok(AllBrandsForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> GetAllCategoriesForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllCategoriesForEndClient = await endClientService.GetAllCategoriesForEndClientIDs(stringToUse);
            return Ok(AllCategoriesForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> GetAllCampaignsForEndClientsAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var AllCampaignsForEndClient = await endClientService.GetAllCampaignsForEndClientIDs(stringToUse);
            return Ok(AllCampaignsForEndClient);


        }

        [HttpPost]
        public async Task<IActionResult> ExportReportAsync()
        {
            var DataPassedOver = HttpContext.Request.Body;
            var streamreader = new StreamReader(DataPassedOver);
            var content = streamreader.ReadToEndAsync();
            var stringToUse = content.Result;

            var JsonSettings = new JsonSerializerSettings { Culture = new System.Globalization.CultureInfo("en-GB") };
            List<ReportExportModel> AllDetails = JsonConvert.DeserializeObject<List<ReportExportModel>>(stringToUse, JsonSettings);


            var BatchFileDirPath = "\\\\fredcpappdm0004\\translateplusSystemFiles\\ProcessAutomationTP-SS-PROD-Archive";
            //var BatchFileDirPath = "C:\\Users\\kavjayas\\Desktop";
            var FileName = DateTime.Now.AddHours(-1).ToString("dd-MM-yy_HH-mm-ss_") + System.Guid.NewGuid().ToString() + ".tpbatch";
            var BatchFilePath = Path.Combine(BatchFileDirPath, FileName);
            string tempBatchFilePath = Path.Combine("\\\\FREDCPAPPDM0004\\translateplusSystemFiles\\Temporary zip folder\\temp batch files", FileName);
            System.IO.File.WriteAllText(tempBatchFilePath, System.String.Empty);
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));

            System.IO.File.AppendAllText(tempBatchFilePath, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, "<!-- translate plus process automation batch file -->" + Environment.NewLine);

            System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<translateplusBatch BatchFormatVersion=\"1.2\" OwnerEmployeeName=\"{0}\" NotifyByEmail=\"{1}\">",
                                                                       LoggedInEmployee.NetworkUserName, LoggedInEmployee.EmailAddress) + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, String.Format("<task Type=\"ReportingToolTask\" TaskNumber=\"1\" StartDate=\"{0}\" EndDate=\"{1}\"" +
                                                                        " InternalExtenalOrAll=\"{11}\" OrgGroupString=\"{2}\" OrgString=\"{3}\" EndClientString=\"{4}\" BrandString=\"{5}\"" +
                                                                        " CategoryString=\"{6}\" CampaignString=\"{7}\" EmployeeLoggedInID=\"{8}\" JobItemOrJobOrderString=\"{9}\"" +
                                                                        " FilterByDate=\"{10}\" ResultsToInclude=\"{12}\"/>",
                                                                      AllDetails[0].StartDate, AllDetails[0].EndDate, AllDetails[0].OrgGroupString, AllDetails[0].OrgString, AllDetails[0].EndClientString,
                                                                      AllDetails[0].BrandString, AllDetails[0].CategoryString, AllDetails[0].CampaignString, LoggedInEmployee.Id,
                                                                      AllDetails[0].FilterByJobItemOrJobOrder, AllDetails[0].FilterByDate, AllDetails[0].InternalExternalOrAll,
                                                                      AllDetails[0].ResultsToInclude)
                                                        + Environment.NewLine);
            System.IO.File.AppendAllText(tempBatchFilePath, "</translateplusBatch>" + Environment.NewLine);

            System.IO.File.Copy(tempBatchFilePath, BatchFilePath);
            System.IO.File.Delete(tempBatchFilePath);
            return Ok();
        }


    }
}
