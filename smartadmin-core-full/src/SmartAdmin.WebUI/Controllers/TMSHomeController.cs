using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using ViewModels.TMS.Ops_mgmt;
using ViewModels.TMS.PmMgmt;
using ViewModels.TMS.TeamsMgmt;

namespace SmartAdmin.WebUI.Controllers
{
    public class TMSHomeController : Controller
    {
        private readonly ITPEmployeesService employeesService;
        private readonly IOpsDashboardService operationsService;
        private readonly IPMDashboardService pMService;
        private readonly ITeamsDashboardService teamsMgmtService;
        
        // Access permissions
        private readonly int[] employeesIDs = new int[] { 1218, 41, 475 };  // Martin, Ad, Umer
        
        public TMSHomeController(ITPEmployeesService employeesService,
            IOpsDashboardService operationsService,
            IPMDashboardService pMService,
            ITeamsDashboardService teamsMgmtService)
        {
            this.employeesService = employeesService;
            this.operationsService = operationsService;
            this.pMService = pMService;
            this.teamsMgmtService = teamsMgmtService;
        }

        public async Task<IActionResult> Index()
        {
            var employee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase(); //await employeesService.IdentifyCurrentUserById(1368);

            if (employee.IsTeamManager == true && employee.TeamId != 19 && employee.TeamId != 42)
                return RedirectToAction(nameof(this.TeamMgmtDashboard), new { teamId = employee.TeamId });

            if (employee.IsTeamManager == false && employee.TeamId != 19 && employee.TeamId != 42)
                return RedirectToAction(nameof(this.PMDashboard), new { pmId = employee.Id });

            if (employee.TeamId == 19 || employee.TeamId == 42 || employeesIDs.Contains(employee.Id))
            {

                return RedirectToAction(nameof(this.OpsMgmtDashboard), new { employeeID = employee.Id });
                //var department = await employeesService.GetEmployeeDepartment(employee.Id);

                //if (employeesIDs.Contains(employee.Id))
                //    department.Id = 9;

                //var opsMgmtModel = new OpsDashboardViewModel()
                //{
                //    departmentName = department.Name,
                //    groupBreakdowns = await operationsService.GetAllGroupsDataForOpsMgmt(department.Id),
                //    groupItemsBreakdowns = await operationsService.GetAllItemsByGroupsForOpsMgmt(department.Id),
                //    teamBreakdowns = await operationsService.GetAllTeamsDataForOpsMgmt(department.Id),
                //    teamItemsBreakdowns = await operationsService.GetAllItemsByTeamsForOpsMgmt(department.Id),
                //    invoicedRevenue = await operationsService.GetChartInvoicedRevenueForOpsMgmt(department.Id),
                //    peopleHeadcountBreakdownForToday = await operationsService.GetPeopleDataForTodayByDepartmentForOpsMgmt(department.Id),
                //    peopleHeadcountBreakdownForTwoWeeks = await operationsService.GetPeopleDataForTwoWeeksByDepartmentForOpsMgmt(department.Id),
                //    departmentId = department.Id
                //};

                //if (department.Id == 9)
                //{
                //    var tbaSystemEmp = await operationsService.GetTBAEmployeeJobsForOpsMgmt();
                //    if (tbaSystemEmp.Id != 0)
                //    {
                //        var teamBreakdownList = opsMgmtModel.teamBreakdowns.ToList();
                //        teamBreakdownList.Add(tbaSystemEmp);
                //        opsMgmtModel.teamBreakdowns = teamBreakdownList;
                //    }

                //    var tbaItemsSystemEmp = await operationsService.GetTBAEmployeeItemsForOpsMgmt();
                //    if (tbaItemsSystemEmp.Id != 0)
                //    {
                //        var teamBreakdownList = opsMgmtModel.teamItemsBreakdowns.ToList();
                //        teamBreakdownList.Add(tbaItemsSystemEmp);
                //        opsMgmtModel.teamItemsBreakdowns = teamBreakdownList;
                //    }

                //    opsMgmtModel = this.ReorderOpsMgmtTables(opsMgmtModel);
                //}

                //return View("Views/TMS/OpsMgmtHome.cshtml", opsMgmtModel);
            }
            return Redirect("/Page/Locked");
        }

        [HttpGet("api/[controller]/[action]")]
        public async Task<IActionResult> GetOpsRevenueTable(int departmentId)
        {
            var data = await operationsService.GetRevenueForOpsMgmt(departmentId);

            if (departmentId == 9)
            {
                data = data.ToList().MoveAndUpdateList(19, data.Count() - 1);   // Ops mgmt team
                data = data.ToList().MoveAndUpdateList(21, data.Count() - 2);   // QA team
            }

            return Ok(data);
        }

        [HttpGet("api/TMrevenueTable")]
        public async Task<IActionResult> GetTMRevenueTable(int teamId)
        {
            var data = await teamsMgmtService.GetRevenue(teamId);
            return Ok(data);
        }

        public async Task<IActionResult> GetJobOrdersDataTableData(int teamId, string jobType)
        {
            var data = await GetDataTableDataAsync(teamId, jobType);
            return Ok(new { data });
        }

        public async Task<IActionResult> GetTMJobOrdersDataTableData(int pmId, string jobType)
        {
            var data = await GetDataTableDataAsyncForPM(pmId, jobType);
            return Ok(new { data });
        }

        public async Task<IActionResult> TeamMgmtDashboard(int teamId)
        {
            var employee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase(); //await employeesService.IdentifyCurrentUserById(638);
            //if (employee.IsTeamManager == false && (employee.TeamId != 19 && employee.TeamId != 42))
            //    return Redirect("/Page/Locked");

            var team = await teamsMgmtService.GetTeam(teamId);
            var teamMgmtDashboard = new TMDashboardViewModel()
            {
                TeamName = team.Name,
                PMBreakdowns = await teamsMgmtService.GetStatusPerPM(teamId, team.Name),
                teamItemsBreakdowns = await teamsMgmtService.GetAllItemsByPMs(teamId, team.Name),
                invoicedRevenue = await teamsMgmtService.GetInvoicedRevenueForTMs(teamId),
                groupBreakdowns = await teamsMgmtService.GetAllClientsData(teamId),
                groupItemsBreakdowns = await teamsMgmtService.GetAllItemsByClients(teamId),
                holidaysBreakdownForToday = await teamsMgmtService.HolidaysBreakdowForTeam(teamId),
                holidaysBreakdownForTwoWeeks = await teamsMgmtService.HolidaysBreakdowNextTwoWeeksForTeam(teamId),
                LoggedInEmployee = employee,
                TeamID = teamId,
                DeptID = team.DepartmentId
            };

            return View("Views/TMS/TeamMgmtHome.cshtml", teamMgmtDashboard);
        }

        public async Task<IActionResult> PMDashboard(int pmId)
        {
            var LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            var team = await teamsMgmtService.GetTeam(LoggedInEmployee.TeamId);
            
            var employee = await employeesService.IdentifyCurrentUserById(pmId);
            if (employee == null)
                return NotFound();

            //if (LoggedInEmployee.IsTeamManager == true && (LoggedInEmployee.TeamId != 19 || LoggedInEmployee.TeamId != 42))
            //    return Redirect("/Page/Locked");

            var model = new PMDashboard();
            try
            {
                model.AllJobs = await pMService.GetAllJobOrdersForPm(pmId);
                model.TodayDeliveries = await pMService.GetTodaysDeliveriesJobs(pmId);
                model.OverdueJobs = await pMService.GetOverdueJobs(pmId);
                model.ApproachingDeadlines = await pMService.GetApproachingDeadlinesInNextThreeDaysJobs(pmId);
                model.ClientReviewJobs = await pMService.GetClientReviewJobs(pmId);
                model.JobOrdersClientBreakdown = await pMService.GetJobOrdersByClients(pmId);
                model.JobItemsClientBreakdown = await pMService.GetJobItemsByClients(pmId);
                model.TeamName = team.Name;
                model.LoggedInEmployee = LoggedInEmployee;
                model.DeptID = team.DepartmentId;
            }
            catch (Exception ex)
            {
                var a = 0;
                throw;
            }


            model.PmId = employee.Id;
            model.PmName = employee.FirstName + " " + employee.Surname;

            return View("Views/TMS/PmMgmtHome.cshtml", model);
        }

        private async Task<List<JobOrdersDataTableViewModel>> GetDataTableDataAsync(int id, string jobType)
        {
            var teamId = id;
            var data = new List<JobOrdersDataTableViewModel>();

            if (jobType == "openJobs")
            {
                data = await operationsService.GetOpenJobOrdersDataForOpsMgmt(teamId) as List<JobOrdersDataTableViewModel>;
            }
            else if (jobType == "overdueJobs")
            {
                data = await operationsService.GetOverdueJobOrdersDataForOpsMgmt(teamId) as List<JobOrdersDataTableViewModel>;
            }
            else if (jobType == "dueTodayJobs")
            {
                data = await operationsService.GetDueTodayJobOrdersDataForOpsMgmt(teamId) as List<JobOrdersDataTableViewModel>;
            }

            var departmentId = teamId;
            switch (jobType)
            {
                case "departmentOpenJobs":
                    data = await operationsService.GetOpenJobOrdersDataByDepartmentForOpsMgmt(departmentId) as List<JobOrdersDataTableViewModel>;
                    break;

                case "departmentOverdueJobs":
                    data = await operationsService.GetOverdueJobOrdersDataByDepartmentForOpsMgmt(departmentId) as List<JobOrdersDataTableViewModel>;
                    break;

                case "departmentDueTodayJobs":
                    data = await operationsService.GetDueTodayJobOrdersDataByDepartmentForOpsMgmt(departmentId) as List<JobOrdersDataTableViewModel>;
                    break;

                default:
                    break;
            }


            switch (jobType)
            {
                case "TBAOpenJobs":
                    data = await operationsService.GetTBAEmployeeOpenJobsForOpsMgmt() as List<JobOrdersDataTableViewModel>;
                    break;

                case "TBAOverdueJobs":
                    data = await operationsService.GetTBAEmployeeOverdueJobsForOpsMgmt() as List<JobOrdersDataTableViewModel>;
                    break;

                case "TBADueTodayJobs":
                    data = await operationsService.GetTBAEmployeeDueTodayJobsForOpsMgmt() as List<JobOrdersDataTableViewModel>;
                    break;

                default:
                    break;
            }
            return data;
        }
        private async Task<List<JobOrdersDataTableTMViewModel>> GetDataTableDataAsyncForPM(int pmId, string jobType)
        {
            var data = new List<JobOrdersDataTableTMViewModel>();

            if (jobType == "openJobs")
            {
                data = await teamsMgmtService.GetOpenJobOrdersDataForPM(pmId) as List<JobOrdersDataTableTMViewModel>;
            }
            else if (jobType == "overdueJobs")
            {
                data = await teamsMgmtService.GetOverdueJobOrdersDataForPM(pmId) as List<JobOrdersDataTableTMViewModel>;
            }
            else if (jobType == "dueTodayJobs")
            {
                data = await teamsMgmtService.GetDueTodayJobOrdersDataForPM(pmId) as List<JobOrdersDataTableTMViewModel>;
            }

            var teamId = pmId;
            switch (jobType)
            {
                case "teamOpenJobs":
                    data = await teamsMgmtService.GetOpenJobOrdersDataByTeam(teamId) as List<JobOrdersDataTableTMViewModel>;
                    break;

                case "teamOverdueJobs":
                    data = await teamsMgmtService.GetOverdueJobOrdersDataByTeam(teamId) as List<JobOrdersDataTableTMViewModel>;
                    break;

                case "teamDueTodayJobs":
                    data = await teamsMgmtService.GetDueTodayJobOrdersDataByTeam(teamId) as List<JobOrdersDataTableTMViewModel>;
                    break;

                default:
                    break;
            }
            return data;
        }
        private OpsDashboardViewModel ReorderOpsMgmtTables(OpsDashboardViewModel opsMgmtModel)
        {
            // 19 - Operations mgmt , 21 = Quality assurance , 12 - Vendor mgmt, 435 - TBA System employee

            opsMgmtModel.teamBreakdowns = opsMgmtModel.teamBreakdowns.ToList().MoveAndUpdateList(435, 1);
            opsMgmtModel.teamBreakdowns = opsMgmtModel.teamBreakdowns.ToList().MoveAndUpdateList(19, opsMgmtModel.teamBreakdowns.Count() - 1);
            opsMgmtModel.teamBreakdowns = opsMgmtModel.teamBreakdowns.ToList().MoveAndUpdateList(21, opsMgmtModel.teamBreakdowns.Count() - 2);

            opsMgmtModel.teamItemsBreakdowns = opsMgmtModel.teamItemsBreakdowns.ToList().MoveAndUpdateList(435, 1);
            opsMgmtModel.teamItemsBreakdowns = opsMgmtModel.teamItemsBreakdowns.ToList().MoveAndUpdateList(19, opsMgmtModel.teamItemsBreakdowns.Count() - 1);
            opsMgmtModel.teamItemsBreakdowns = opsMgmtModel.teamItemsBreakdowns.ToList().MoveAndUpdateList(21, opsMgmtModel.teamItemsBreakdowns.Count() - 2);

            opsMgmtModel.peopleHeadcountBreakdownForToday = opsMgmtModel.peopleHeadcountBreakdownForToday.ToList().MoveAndUpdateList(19, opsMgmtModel.peopleHeadcountBreakdownForToday.Count() - 1);
            opsMgmtModel.peopleHeadcountBreakdownForToday = opsMgmtModel.peopleHeadcountBreakdownForToday.ToList().MoveAndUpdateList(12, opsMgmtModel.peopleHeadcountBreakdownForToday.Count() - 2);
            opsMgmtModel.peopleHeadcountBreakdownForToday = opsMgmtModel.peopleHeadcountBreakdownForToday.ToList().MoveAndUpdateList(21, opsMgmtModel.peopleHeadcountBreakdownForToday.Count() - 3);

            opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks = opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.ToList().MoveAndUpdateList(19, opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.Count() - 1);
            opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks = opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.ToList().MoveAndUpdateList(12, opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.Count() - 2);
            opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks = opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.ToList().MoveAndUpdateList(21, opsMgmtModel.peopleHeadcountBreakdownForTwoWeeks.Count() - 3);

            return opsMgmtModel;
        }
        public async Task<IActionResult> OpsMgmtDashboard(short employeeID)
        {
            
            var employee = await employeesService.IdentifyCurrentUserById(employeeID);
            var department = await employeesService.GetEmployeeDepartment(employeeID);

            if (employeesIDs.Contains(employeeID))
                department.Id = 9;

            var opsMgmtModel = new OpsDashboardViewModel()
            {
                departmentName = department.Name,
                groupBreakdowns = await operationsService.GetAllGroupsDataForOpsMgmt(department.Id),
                groupItemsBreakdowns = await operationsService.GetAllItemsByGroupsForOpsMgmt(department.Id),
                teamBreakdowns = await operationsService.GetAllTeamsDataForOpsMgmt(department.Id),
                teamItemsBreakdowns = await operationsService.GetAllItemsByTeamsForOpsMgmt(department.Id),
                invoicedRevenue = await operationsService.GetChartInvoicedRevenueForOpsMgmt(department.Id),
                peopleHeadcountBreakdownForToday = await operationsService.GetPeopleDataForTodayByDepartmentForOpsMgmt(department.Id),
                peopleHeadcountBreakdownForTwoWeeks = await operationsService.GetPeopleDataForTwoWeeksByDepartmentForOpsMgmt(department.Id),
                departmentId = department.Id,
                Employee = employee
            };

            if (department.Id == 9)
            {
                var tbaSystemEmp = await operationsService.GetTBAEmployeeJobsForOpsMgmt();
                if (tbaSystemEmp.Id != 0)
                {
                    var teamBreakdownList = opsMgmtModel.teamBreakdowns.ToList();
                    teamBreakdownList.Add(tbaSystemEmp);
                    opsMgmtModel.teamBreakdowns = teamBreakdownList;
                }

                var tbaItemsSystemEmp = await operationsService.GetTBAEmployeeItemsForOpsMgmt();
                if (tbaItemsSystemEmp.Id != 0)
                {
                    var teamBreakdownList = opsMgmtModel.teamItemsBreakdowns.ToList();
                    teamBreakdownList.Add(tbaItemsSystemEmp);
                    opsMgmtModel.teamItemsBreakdowns = teamBreakdownList;
                }

                opsMgmtModel = this.ReorderOpsMgmtTables(opsMgmtModel);
            }

            return View("Views/TMS/OpsMgmtHome.cshtml", opsMgmtModel);
        }
    }
}
