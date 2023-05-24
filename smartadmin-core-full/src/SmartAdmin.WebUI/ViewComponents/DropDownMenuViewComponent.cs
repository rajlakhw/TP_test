using Data;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;

namespace SmartAdmin.WebUI.ViewComponents
{
    public class DropDownMenuViewComponent : ViewComponent
    {
        private readonly ITPEmployeesService employeesService;

        public DropDownMenuViewComponent(ITPEmployeesService employeesService)
        {
            this.employeesService = employeesService;
        }
        public IViewComponentResult Invoke()
        {
            var employeeLoggedIn = HttpContext.Session.Get<Employee>("EmployeeLoggedIn");

            if(employeeLoggedIn == null)
            {
                var username = HttpContext.User.Identity.Name;
                username = GeneralUtils.GetUsernameFromNetwokUsername(username);
                employeeLoggedIn = employeesService.IdentifyCurrentUser<Employee>(username).Result;
            }

            return View(employeeLoggedIn);
        }
    }
}
