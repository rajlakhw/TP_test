using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Interfaces;
using ViewModels.HR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace SmartAdmin.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayController: ControllerBase
    {
        private readonly ITPEmployeesService employeesService;
        private readonly ITPPublicHoliday publicHolidayService;
        private readonly IHolidayService holidayService;

        public HolidayController(ITPEmployeesService service, ITPPublicHoliday service1, IHolidayService service2)
        {
            employeesService = service;
            publicHolidayService = service1;
            holidayService = service2;
        }

        public async Task<IActionResult> Get()
        {
            Employee LoggedInEmployee = await employeesService.IdentifyCurrentUser<Employee>(HttpContext.User.Identity.Name.Remove(0, 3));
            DateTime NextWorkingDay = holidayService.GetNextWorkingDayForEmployee(LoggedInEmployee.Id);
                        
            Data.EmployeeHoliday thisHoliday = await holidayService.GetEmployeeHolidayDetailsForYear(LoggedInEmployee.Id, DateTime.Today.Year);

            bool isFirstDateMorningOff = false;
            short startDateFullOrAMOrPM = 0;
            short endDateFullOrAMOrPM = 0;
            DateTime startDate = NextWorkingDay;
            DateTime endDate = NextWorkingDay;

            HolidaysViewModel results = new HolidaysViewModel()
            {
                NextAvailableHolidayDate = NextWorkingDay.ToString("dd/MM/yyyy"),
                HolidaysRemaining = thisHoliday.HolidaysRemaing,
                TotalAnnualHolidays = thisHoliday.TotalAnnualHolidays,
                Year = thisHoliday.Year,
                TotalHolidaysForCurrentRequest = 1,
                NextWorkingDayAfterHolidayString = holidayService.GetNextWorkingDayAfterHolidayString(LoggedInEmployee.Id, isFirstDateMorningOff, startDateFullOrAMOrPM, endDateFullOrAMOrPM, startDate, endDate)
                //LoggedInEmployee = await employeesService.GetLoggedInEmployeeFromSessionOrDatabase()
            };
            return Ok(results);
        }
    }
}
