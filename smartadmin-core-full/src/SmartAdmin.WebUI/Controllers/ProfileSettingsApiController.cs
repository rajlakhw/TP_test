using Data;
using Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Services.Interfaces;
using ViewModels.EmployeeModels;

namespace SmartAdmin.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileSettingsApiController : ControllerBase
    {
        private readonly ITPEmployeesService employeesService;

        public ProfileSettingsApiController(ITPEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        public async Task<IActionResult> Get()
        {
            var employee = await GetEmployeeFromDb();
            if (employee == null) { return NotFound(); }

            var model = new ProfileSettingsViewModel()
            {
                showAnniversaries = employee.ShowAnniversariesOnHomePage.Value,
                showLikesAndComments = employee.ShowLikesOrCommentsOnHomePage.Value
            };
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProfileSettingsViewModel model)
        {
            var employee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            if (ModelState.IsValid)
            {
                await employeesService.UpdateProfileSettings(model, employee.Id);
            }

            return Ok();
        }

        private async Task<Employee> GetEmployeeFromDb()
        {
                var username = HttpContext.User.Identity.Name;
                //var username = Environment.UserName;
                username = GeneralUtils.GetUsernameFromNetwokUsername(username);
                var employeeLoggedIn = await employeesService.IdentifyCurrentUser<Employee>(username);

            return employeeLoggedIn;
        }
    }
}
