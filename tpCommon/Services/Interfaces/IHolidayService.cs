using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;

namespace Services.Interfaces
{
    public interface IHolidayService : IService
    {
        DateTime GetNextWorkingDayForEmployee(int employeeId);
        Task<Boolean> IsFullDayBankHolidayForEmployee(int employeeId, DateTime DateToCheck);
        Task<Boolean> IsDateAlreadyBookedForHoliday(int employeeId, DateTime DateToCheck, short? AMPMOrFullDay = 0);
        Task<EmployeeHoliday> GetEmployeeHolidayDetailsForYear(int employeeId, int year);
        string GetNextWorkingDayAfterHolidayString(int employeeId, bool lastDayAmPmSelectorVisible, short StartDateAmorPmorFullDay,
                                                      short EndDateAmorPmorFullDay, DateTime StartDate, DateTime EndDate);
        Task<Boolean> IsHalfDayBankHolidayForEmployee(int employeeId, DateTime DateToCheck);

        double GetTotalNumberOfDaysForHolidayRequest(int employeeId, DateTime StartDate, DateTime EndDate, short StartDateAmorPmorFullDay,
                                                          short EndDateAmorPmorFullDay, bool lastDayAmPmSelectorVisible);
        Task<EmployeeHolidayRequest> AddHolidayRequest(short employeeId, DateTime holidayStartDateTime, byte startDateAMOrPMOrFullDay,
                                                                    DateTime holidayEndDateTime, byte endDateAMOrPMOrFullDay, decimal totalDays,
                                                                    short requestedByEmployeeId, double holidaysInStartDateYear, double holidaysInEndDateYear);

        Task<List<EmployeeHolidayRequest>> GetAllHolidayRequestsForEmployeeForYear(short employeeId, int year);
        Task<List<EmployeeHoliday>> GetAllHolidayDetailsForEmployee(short employeeId);

        Task<EmployeeHoliday> GetHolidayDetailsForYear(short employeeId, int year);

        Task<EmployeeHolidayRequest> GetHolidayRequestDetails(int holidayRequestID);

        Task<EmployeeHolidayRequest> DeleteHolidayRequest(int holidayRequestID, short deletedByEmployeeId);

        Task<List<EmployeeHolidayRequest>> GetAllTeamHolidaysInSameDateRange(int holidayRequestID);

        Task<EmployeeHolidayRequest> ApproveHolidayRequest(int holidayRequestID, short approvedByEmployeeId);

        Task<EmployeeHolidayRequest> DeclineHolidayRequest(int holidayRequestID, short declinedByEmployeeId);

        Task<bool> CheckIfEmployeeHasAccessToApproveHolidays(short employeeId, short employeeToBeApproved);

        Task<bool> CheckIfEmployeeHasAccessToAccessSickness(short employeeId, short employeeToBeApproved);
    }
}
