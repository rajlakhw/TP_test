using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class EmployeeDetailsViewComponent : ViewComponent
    {
        private readonly ITPEmployeesService employeesService;

        public EmployeeDetailsViewComponent(ITPEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }

        public async Task<IViewComponentResult> InvokeAsync(short? employeeId, bool displayJobTitle = false)
        {
            if (employeeId == 0 || employeeId == null)
                return Content("Not found");

            var employee = await employeesService.GetEmployeeDetailsForViewComponent(employeeId.Value);
            employee.DisplayJobTitle = displayJobTitle;

            return View(employee);
        }
    }
}
