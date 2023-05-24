using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Data.Repositories;
using Data;
using Services.Interfaces;

namespace Services
{
    public class TPPublicHoliday : ITPPublicHoliday
    {
        private readonly IRepository<BankHoliday> bankHolidayRepo;

        public TPPublicHoliday(IRepository<BankHoliday> repository)
        {
            this.bankHolidayRepo = repository;
        }
        public async Task<List<Data.BankHoliday>> GetAllBankHolidaysForOffice<BankHoliday>(int officeID, int year)
        {
            var result = await bankHolidayRepo.All()
                                .Where(b => b.BankHolidayDate.Year == year &&
                                        b.DeletedDateTime == null &&
                                        ((officeID == 4 && b.IsAukbankHoliday == true) ||
                                         (officeID == 9 && b.IsJapanBankHoliday == true) ||
                                         (officeID == 11 && b.IsBulgarianBankHoliday == true) ||
                                         (officeID == 13 && b.IsRomanianBankHoliday == true) ||
                                         (officeID == 15 && b.IsCostaRicaBankHoliday == true)))
                                .OrderBy(o => o.BankHolidayDate)
                                .ToListAsync(); 

            return result;
        }

        public async Task<List<int>> GetAllBankHolidayYears()
        {
            var result = await bankHolidayRepo.All().Where(b => b.DeletedDateTime == null)
                                .Select(b => b.BankHolidayDate.Year).Distinct().OrderByDescending(o => o)
                                .ToListAsync();

            return result;
        }

        public async Task<int> AddPublicHoliday<BankHoliday>(string PublicHolidayName, DateTime PublicHolidayDate, Boolean IsAHalfDay, string AllOffices, short loggedInEmployeeId)
        {
            var allSelectedOffices = AllOffices.Split(",");
            Boolean UKBankHoliday = false;
            Boolean JapaneseBankHoliday = false;
            Boolean RomanianBankHoliday = false;
            Boolean BulgarianBankHoliday = false;
            Boolean CostaRicanBankHoliday = false;
            
            for (var i=0; i< allSelectedOffices.Length; i++)
            {
                if (allSelectedOffices.ElementAt(i) == "4")
                {
                    UKBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "9")
                {
                    JapaneseBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "11")
                {
                    BulgarianBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "13")
                {
                    RomanianBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "15")
                {
                    CostaRicanBankHoliday = true;
                }
            }
            var thisBankHoliday = new Data.BankHoliday()
            {
                BankHolidayDate = PublicHolidayDate,
                IsAukbankHoliday = UKBankHoliday,
                IsJapanBankHoliday = JapaneseBankHoliday,
                IsBulgarianBankHoliday = BulgarianBankHoliday,
                IsRomanianBankHoliday = RomanianBankHoliday,
                IsCostaRicaBankHoliday = CostaRicanBankHoliday,
                IsHalfDay = IsAHalfDay,
                Description = PublicHolidayName,
                CreatedDateTime = GeneralUtils.GetCurrentUKTime(),
                CreatedByEmployeeId = loggedInEmployeeId
            };

            await bankHolidayRepo.AddAsync(thisBankHoliday);
            await bankHolidayRepo.SaveChangesAsync();

            return thisBankHoliday.Id;
        }


        public async Task<int> UpdatePublicHoliday<BankHoliday>(int holidayId, string PublicHolidayName, DateTime PublicHolidayDate, Boolean IsAHalfDay, string AllOffices, short loggedInEmployeeId)
        {
            var thisPublicHoliday = await GetPublicHolidayById(holidayId);

            thisPublicHoliday.Description = PublicHolidayName;
            thisPublicHoliday.BankHolidayDate = PublicHolidayDate;
            thisPublicHoliday.IsHalfDay = IsAHalfDay;

            var allSelectedOffices = AllOffices.Split(",");
            Boolean UKBankHoliday = false;
            Boolean JapaneseBankHoliday = false;
            Boolean RomanianBankHoliday = false;
            Boolean BulgarianBankHoliday = false;
            for (var i = 0; i < allSelectedOffices.Length; i++)
            {
                if (allSelectedOffices.ElementAt(i) == "4")
                {
                    UKBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "9")
                {
                    JapaneseBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "11")
                {
                    BulgarianBankHoliday = true;
                }
                else if (allSelectedOffices.ElementAt(i) == "13")
                {
                    RomanianBankHoliday = true;
                }
            }
            thisPublicHoliday.IsAukbankHoliday = UKBankHoliday;
            thisPublicHoliday.IsJapanBankHoliday = JapaneseBankHoliday;
            thisPublicHoliday.IsBulgarianBankHoliday = BulgarianBankHoliday;
            thisPublicHoliday.IsRomanianBankHoliday = RomanianBankHoliday;
            thisPublicHoliday.LastModifiedDateTime = GeneralUtils.GetCurrentUKTime();
            thisPublicHoliday.LastModifiedByEmployeeId = loggedInEmployeeId;

            bankHolidayRepo.Update(thisPublicHoliday);
            await bankHolidayRepo.SaveChangesAsync();

            return holidayId;
        }

        public async Task<int> DeletePublicHoliday<BankHoliday>(int holidayId,  short loggedInEmployeeId)
        {
            var thisPublicHoliday = await GetPublicHolidayById(holidayId);

            thisPublicHoliday.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
            thisPublicHoliday.DeletedByEmployeeId = loggedInEmployeeId;

            bankHolidayRepo.Update(thisPublicHoliday);
            await bankHolidayRepo.SaveChangesAsync();

            return holidayId;
        }

        public async Task<BankHoliday> GetPublicHolidayById(int holidayId)
        {
            var result = await bankHolidayRepo.All().Where(tb => tb.Id == holidayId).FirstOrDefaultAsync();
            return result;
        }
    }
}
