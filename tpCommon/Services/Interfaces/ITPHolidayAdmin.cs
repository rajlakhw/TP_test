using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.EmployeeModels;

namespace Services.Interfaces
{
    public interface ITPHolidayAdmin : IService
    {
        Task<List<EmployeeHolidayModel>> GetAllEmployeeHolidaysForOffice(byte officeId, int year);
        Task<short> UpdateEmployeeHoliday(short employeeId, int totalBaseAnnualHolidays, int? miscDays, int? previouslyWorkedDays, string holidayNotes, int year);
        Task<List<int>> GetAllEmployeeHolidayYears();
    }
}
