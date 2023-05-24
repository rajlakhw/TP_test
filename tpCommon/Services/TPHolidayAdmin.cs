using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.EmployeeModels;

namespace Services
{
    public class TPHolidayAdmin : ITPHolidayAdmin
    {
        private readonly IRepository<EmployeeHoliday> employeeHolidayRepo;
        private readonly IRepository<Employee> employeeRepo;
        public TPHolidayAdmin(IRepository<EmployeeHoliday> repository, IRepository<Employee> employees)
        {
            this.employeeHolidayRepo = repository;
            this.employeeRepo = employees;
        }

        public async Task<List<EmployeeHolidayModel>> GetAllEmployeeHolidaysForOffice(byte officeId, int year)
        {
            var employeeHolidayData = await employeeHolidayRepo.All().Join(employeeRepo.All(),
                empHolidays => empHolidays.EmployeeId,
                employee => employee.Id,
                (empHolidays, employee) => new EmployeeHolidayModel
                {
                    FirstName = employee.FirstName,
                    Surname = employee.Surname,
                    Id = empHolidays.Id,
                    EmployeeId = empHolidays.EmployeeId,
                    OfficeId = employee.OfficeId ?? 0,
                    Year = empHolidays.Year,
                    TeamId = employee.TeamId,
                    TerminateDate = employee.TerminateDate,
                    TotalBaseAnnualHolidays = (int)empHolidays.TotalBaseAnnualHolidays,
                    LoyaltyDays = empHolidays.LoyaltyDays ?? 0,
                    MiscDays = (int?)empHolidays.MiscDays ?? 0,
                    PreviouslyWorkedDays = empHolidays.PreviouslyWorkedDays ?? 0,
                    HolidaysRequested = empHolidays.HolidaysRequested ?? 0,
                    HolidaysRemaining = empHolidays.HolidaysRemaing ?? 0,
                    HolidayNotes = empHolidays.HolidayNotes
                }).Where(x => x.Year == year && x.OfficeId == officeId && (x.TeamId < 18 || x.TeamId > 18) && x.TerminateDate == null).OrderBy(x => x.FirstName).ToListAsync();
            
            return employeeHolidayData;

        }
        public async Task<short> UpdateEmployeeHoliday(short employeeId, int totalBaseAnnualHolidays, int? miscDays, int? previouslyWorkedDays, string holidayNotes, int year)
        {
            var employeeHoliday = await employeeHolidayRepo.All().Where(x => x.EmployeeId == employeeId && x.Year == year).FirstOrDefaultAsync();
            employeeHoliday.TotalBaseAnnualHolidays = totalBaseAnnualHolidays;
            employeeHoliday.MiscDays = miscDays;
            employeeHoliday.PreviouslyWorkedDays = previouslyWorkedDays;
            employeeHoliday.HolidayNotes = holidayNotes;

            var res = await employeeHolidayRepo.SaveChangesAsync();
            return employeeId;

        }

        public async Task<List<int>> GetAllEmployeeHolidayYears()
        {
            var result = await employeeHolidayRepo.All().Select(b => b.Year).Distinct().OrderByDescending(o => o)
                                .ToListAsync();

            return result;
        }
    }
}
