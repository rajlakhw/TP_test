using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPPublicHoliday : IService
    {
        Task<List<Data.BankHoliday>> GetAllBankHolidaysForOffice<BankHoliday>(int officeID, int year);

        Task<List<int>> GetAllBankHolidayYears();

        Task<int> AddPublicHoliday<BankHoliday>(string PublicHolidayName, DateTime PublicHolidayDate, bool IsAHalfDay, string AllOffices, short loggedInEmployeeId);

        Task<int> UpdatePublicHoliday<BankHoliday>(int holidayId, string PublicHolidayName, DateTime PublicHolidayDate, bool IsAHalfDay, string AllOffices, short loggedInEmployeeId);

        Task<BankHoliday> GetPublicHolidayById(int holidayId);

        Task<int> DeletePublicHoliday<BankHoliday>(int holidayId, short loggedInEmployeeId);


    }
}
