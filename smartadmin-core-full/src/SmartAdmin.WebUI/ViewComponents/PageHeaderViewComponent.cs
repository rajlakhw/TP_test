using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class PageHeaderViewComponent : ViewComponent
    {
        private readonly ITPEmployeesService employeesService;

        public PageHeaderViewComponent(ITPEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var employeeLoggedIn = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();

            return View(employeeLoggedIn);
        }
    }
}
