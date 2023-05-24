using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using ViewModels.EmployeeModels.EmployeeAccessMatrix;

namespace SmartAdmin.WebUI.Controllers
{
    public class EmployeeAccessMatrixController : Controller
    {
        private readonly IEmployeeAccessMatrixService accessMatrixService;
        private readonly ITPEmployeesService employeesService;

        public EmployeeAccessMatrixController(IEmployeeAccessMatrixService accessMatrixService, ITPEmployeesService employeesService)
        {
            this.accessMatrixService = accessMatrixService;
            this.employeesService = employeesService;
        }
        public async Task<IActionResult> Index()
        {
            var loggedEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (loggedEmployee.TeamId != ((byte)Global_Settings.Enumerations.Teams.ITSupportAndAdministration) &&
                loggedEmployee.TeamId != ((byte)Global_Settings.Enumerations.Teams.ITSoftwareDevelopment) &&
                loggedEmployee.Department.Id != ((byte)Global_Settings.Enumerations.Departments.IT))
                return Redirect("/Page/Locked");

            var viewModel = new MatrixPageViewModel();
            viewModel.AccessMatrixViewModel = await accessMatrixService.GetAllEmployeeAccesses();
            viewModel.AccessMatrixControlViewModel = await accessMatrixService.GetAllEmployeeAccessControls();
            viewModel.AccessLevelControlRelationshipsViewModel = await accessMatrixService.GetAllEmployeeAccessControlsRelationships();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(short id, string notes)
        {
            var isSuccessful = await accessMatrixService.CreateAccessLevel(id, notes);
            return RedirectToAction(nameof(this.Index));
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<IActionResult> Update(short id, List<int> controlIds)
        {
            var isSuccessfull = await accessMatrixService.CreateAccessControlsPermissions(id, controlIds);

            return Ok(isSuccessfull);
        }
    }
}
