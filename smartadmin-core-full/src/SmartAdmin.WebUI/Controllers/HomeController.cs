using Data;
using Services;
using ViewModels.HomePage;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Services.Interfaces;

namespace SmartAdmin.WebUI.Controllers
{
    public partial class HomeController : Controller
    {
        private readonly IHomePageService homePageService;
        private readonly ITPEmployeesService employeesService;

        public HomeController(IHomePageService homePageService, ITPEmployeesService employeesService)
        {
            this.homePageService = homePageService;
            this.employeesService = employeesService;
        }

        public async Task<IActionResult> Index()
        {
            var employee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase();
            if (employee == null) { return NotFound(); }


            var viewModel = new HomePageViewModel()
            {
                StaffAnniversaries = new StaffAnnivarsaries()
            };

            viewModel.LoggedInEmployeeId = employee.Id;

            viewModel.StaffAnniversaries.EmployeesBirthdays = await homePageService.GetAllBirthdayWishesForCurrentWeek(employee.Id);
            viewModel.StaffAnniversaries.EmployeesAnniversaries = await homePageService.GetAllWorkAnniversariesWishesForCurrentWeek(employee.Id);

            // sort the anniversaries by date
            var combinedList = viewModel.StaffAnniversaries.EmployeesAnniversaries
                .Cast<IAnniversary>()
                .Concat(viewModel.StaffAnniversaries.EmployeesBirthdays)
                .OrderBy(x=> x.DaysLeft)
                .ToList();
            viewModel.Anniversaries = combinedList;

            viewModel.EmployeesHighFives = await homePageService.GetAllHighFivesForLastTwoWeeks();

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return Redirect("Views/Page/Error.cshtml");
        }
    }
}
