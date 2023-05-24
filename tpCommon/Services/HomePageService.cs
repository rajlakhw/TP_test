using Data;
using Data.Repositories;
using ViewModels.HomePage;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class HomePageService : IHomePageService
    {
        private readonly IRepository<BirthdayWish> birthdayRepository;
        private readonly IRepository<WorkAnniversariesWish> annivarsaryRepository;
        private readonly IRepository<Employee> employeeRepository;
        private readonly IRepository<HighFife> highFiveRepository;

        private readonly short[] employeesIDsToEscape = new short[] { 5, 9, 10, 435, 244, 633, 1221 };
        private readonly byte[] employeeTypesToSelect = new byte[] { 1, 2, 3, 4, 6 };

        public HomePageService(IRepository<BirthdayWish> birthdayRepository,
            IRepository<WorkAnniversariesWish> annivarsaryRepository,
            IRepository<Employee> employeeRepository,
            IRepository<HighFife> highFiveRepository)
        {
            this.birthdayRepository = birthdayRepository;
            this.employeeRepository = employeeRepository;
            this.annivarsaryRepository = annivarsaryRepository;
            this.highFiveRepository = highFiveRepository;
        }

        // DONE: hardcode the IDs of the employees to escape (i plus, TP Admin, etc.)

        // DONE:  get all GetAllEmployeesBirthdays from the employees table
        // DONE: set deletedDateTime for the birthdays before today
        // DONE: check if there is new Birthdays that are not inserted
        // DONE: compare them with the records from the BirthdayWish table and insert the missing entities if any
        // DONE: select all birthdays from BirthdayWish (that have public privacy) with comments and likes(public as well) and return them


        private async Task<List<EmployeesAnnivarsaries>> GetAllEmployeesAnnivarsaries()
        {
            var (today, twoWeekDateFromToday) = GeneralUtils.GetTwoWeeksDatesFromToday();
            //var res = await employeeRepository.All().Where(e => (e.HireDate.Year < thisWeekStart.Year &&
            //                                                           (e.HireDate.Month == thisWeekStart.Month &&
            //                                                           e.HireDate.Day >= thisWeekStart.Day) &&
            //                                                           (e.HireDate.Month == thisWeekEnd.Month &&
            //                                                           e.HireDate.Day <= thisWeekEnd.Day)) &&
            //                                                           !employeesIDsToEscape.Contains(e.Id) &&
            //                                                           e.TerminateDate == null).OrderBy(e => e.HireDate.Day)
            //                                                           .Select(e => new EmployeesAnnivarsaries()
            //                                                           {
            //                                                               AnniversaryDate = e.HireDate,
            //                                                               FirstName = e.FirstName,
            //                                                               LastName = e.Surname,
            //                                                               EmployeeID = e.Id,
            //                                                               ImageBase64 = e.ImageBase64
            //                                                           })
            //                                                               .ToListAsync();

            var res = await employeeRepository.All().Where(e => 1 == (Math.Floor(EF.Functions.DateDiffDay(e.HireDate, twoWeekDateFromToday) / 365.25M) -
                Math.Floor(EF.Functions.DateDiffDay(e.HireDate, today) / 365.25M)) &&
                e.HireDate.Year != today.Year &&
                !employeesIDsToEscape.Contains(e.Id) &&
                employeeTypesToSelect.Contains(e.EmployeeStatusType.GetValueOrDefault()) &&
                e.TerminateDate == null)
                    .Select(e => new EmployeesAnnivarsaries()
                    {
                        AnniversaryDate = e.HireDate,
                        FirstName = e.FirstName,
                        LastName = e.Surname,
                        EmployeeID = e.Id,
                        ImageBase64 = e.ImageBase64
                    }).ToListAsync();

            return res;
        }

        private async Task<List<EmployeesBirthdays>> GetAllEmployeeBirthdays()
        {
            var (today, twoWeekDateFromToday) = GeneralUtils.GetTwoWeeksDatesFromToday();
            //var res = await employeeRepository.All().Where(e => ((e.DateOfBirth.Value.Month == thisWeekStart.Month &&
            //                                                           e.DateOfBirth.Value.Day >= thisWeekStart.Day) ||
            //                                                           (e.DateOfBirth.Value.Month == thisWeekEnd.Month &&
            //                                                           e.DateOfBirth.Value.Day <= thisWeekEnd.Day)) &&
            //                                                               !employeesIDsToEscape.Contains(e.Id) &&
            //                                                               e.TerminateDate == null).OrderBy(e => e.DateOfBirth.Value)
            //                                                               .Select(e => new EmployeesBirthdays()
            //                                                               {
            //                                                                   AnniversaryDate = e.DateOfBirth.Value,
            //                                                                   FirstName = e.FirstName,
            //                                                                   LastName = e.Surname,
            //                                                                   EmployeeID = e.Id,
            //                                                                   ImageBase64 = e.ImageBase64
            //                                                               })
            //                                                                   .ToListAsync();

            // doesnt get today birthdays but thats fine
            var res = await employeeRepository.All().Where(e => 1 == (Math.Floor(EF.Functions.DateDiffDay(e.DateOfBirth.Value, twoWeekDateFromToday) / 365.25M) -
                Math.Floor(EF.Functions.DateDiffDay(e.DateOfBirth.Value, today) / 365.25M)) &&
                !employeesIDsToEscape.Contains(e.Id) &&
                employeeTypesToSelect.Contains(e.EmployeeStatusType.GetValueOrDefault()) &&
                e.TerminateDate == null)
                    .Select(e => new EmployeesBirthdays()
                    {
                        AnniversaryDate = e.DateOfBirth.Value,
                        FirstName = e.FirstName,
                        LastName = e.Surname,
                        EmployeeID = e.Id,
                        ImageBase64 = e.ImageBase64
                    }).ToListAsync();

            return res;
        }

        public async Task<List<EmployeesBirthdays>> GetAllBirthdayWishesForCurrentWeek(short EmployeeLoggedInId)
        {
            var birthdays = await GetAllEmployeeBirthdays();
            var IsDeleted = await DeletePreviousWeekBirthdays();

            //MAYBE SELECT JUST A FEW COMMENTS TO SPEED UP THE LOAD TIME

            var res = await birthdayRepository.All().Where(x => x.DeletedDateTime == null).Select(x => new EmployeesBirthdays()
            {
                AnniversaryDate = x.Employee.DateOfBirth.Value,
                FirstName = x.Employee.FirstName,
                LastName = x.Employee.Surname,
                EmployeeID = x.EmployeeId,
                ImageBase64 = x.Employee.ImageBase64
            }).ToListAsync();

            var newBirthdays = birthdays.Except(res).ToList();

            if (newBirthdays.Count() > 0)
            {
                res = await InsertBirthdayEntities(newBirthdays);
            }
            else
            {
                res = await GetAllPublicBirthdays();
            }

            return res;
        }

        public async Task<List<EmployeesAnnivarsaries>> GetAllWorkAnniversariesWishesForCurrentWeek(short EmployeeLoggedInId)
        {
            var employeesAnnivarsaries = await GetAllEmployeesAnnivarsaries();
            var IsDeleted = await DeletePreviousWeekWorkAnniversaries();

            var (thisWeekStart, thisWeekEnd) = GeneralUtils.GetTwoWeeksDatesFromToday();
            //MAYBE SELECT JUST A FEW comments TO SPEED UP THE LOAD TIME

            var res = await annivarsaryRepository.All().Where(x => x.DeletedDateTime == null).Select(x => new EmployeesAnnivarsaries()
            {
                AnniversaryDate = x.Employee.HireDate,
                FirstName = x.Employee.FirstName,
                LastName = x.Employee.Surname,
                EmployeeID = x.EmployeeId,
                ImageBase64 = x.Employee.ImageBase64
            }).ToListAsync();

            var newAnnivarsaries = employeesAnnivarsaries.Except(res).ToList();

            if (newAnnivarsaries.Count() > 0)
            {
                res = await InsertWorkAnnivarsariesEntities(newAnnivarsaries);
            }
            else
            {
                res = await GetAllPublicWorkAnniversaries();
            }

            return res;
        }

        private async Task<List<EmployeesBirthdays>> InsertBirthdayEntities(List<EmployeesBirthdays> birthdaysList)
        {
            var listToInsert = birthdaysList.ConvertAll(x => new BirthdayWish() { EmployeeId = (short)x.EmployeeID, CreatedDateTime = GeneralUtils.GetCurrentUKTime() });

            listToInsert.ForEach(async b => await birthdayRepository.AddAsync(b));

            await birthdayRepository.SaveChangesAsync();

            return await GetAllPublicBirthdays();
        }

        private async Task<List<EmployeesAnnivarsaries>> InsertWorkAnnivarsariesEntities(List<EmployeesAnnivarsaries> annivarsariesList)
        {
            var listToInsert = annivarsariesList.ConvertAll(x => new WorkAnniversariesWish() { EmployeeId = (short)x.EmployeeID, CreatedDateTime = GeneralUtils.GetCurrentUKTime() });

            listToInsert.ForEach(async w=> await annivarsaryRepository.AddAsync(w));

            await annivarsaryRepository.SaveChangesAsync();

            return await GetAllPublicWorkAnniversaries();
        }

        private async Task<List<EmployeesBirthdays>> GetAllPublicBirthdays()
        {
            var res = await birthdayRepository.All().Where(x => x.DeletedDateTime == null &&
            x.Employee.ShowAnniversariesOnHomePage == true)
                .Include(e => e.BirthdayComments).ThenInclude(e => e.Employee)
                .Include(b => b.BirthdayLikes).ThenInclude(l => l.Employee)
                .Select(b => new EmployeesBirthdays()
                {
                    AnniversaryDate = b.Employee.DateOfBirth.Value,
                    FirstName = b.Employee.FirstName,
                    LastName = b.Employee.Surname,
                    EmployeeID = b.Employee.Id,
                    ImageBase64 = b.Employee.ImageBase64,
                    AnniversaryID = b.Id,
                    ShowLikesAndComments = b.Employee.ShowLikesOrCommentsOnHomePage.Value,
                    Comments = b.BirthdayComments.ToList(),
                    Likes = b.BirthdayLikes.ToList()
                })
                .ToListAsync();
            return res;
        }

        private async Task<List<EmployeesAnnivarsaries>> GetAllPublicWorkAnniversaries()
        {
            var res = await annivarsaryRepository.All().Where(x => x.DeletedDateTime == null &&
            x.Employee.ShowAnniversariesOnHomePage == true)
                .Include(e => e.WorkAnnivarsariesComments).ThenInclude(e => e.Employee)
                .Include(b => b.WorkAnniversariesLikes).ThenInclude(l => l.Employee)
                .Select(w => new EmployeesAnnivarsaries()
                {
                    AnniversaryDate = w.Employee.HireDate,
                    FirstName = w.Employee.FirstName,
                    LastName = w.Employee.Surname,
                    EmployeeID = w.Employee.Id,
                    ImageBase64 = w.Employee.ImageBase64,
                    AnniversaryID = w.Id,
                    ShowLikesAndComments = w.Employee.ShowLikesOrCommentsOnHomePage.Value,
                    Comments = w.WorkAnnivarsariesComments.ToList(),
                    Likes = w.WorkAnniversariesLikes.ToList()
                })
                .ToListAsync();
            return res;
        }

        private async Task<bool> DeletePreviousWeekBirthdays()
        {
            var (thisWeekStart, thisWeekEnd) = GeneralUtils.GetTwoWeeksDatesFromToday();

            var res = await birthdayRepository.All().Where(x => (x.Employee.DateOfBirth.Value.Month == thisWeekStart.Month &&
                                                                       x.Employee.DateOfBirth.Value.Day < thisWeekStart.Day) && x.DeletedDateTime == null).ToListAsync();
            if (res.Count() <= 0) { return false; }
            foreach (var birthday in res)
            {
                birthday.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
                birthdayRepository.Update(birthday);
            }
            await birthdayRepository.SaveChangesAsync();

            return true;
        }

        private async Task<bool> DeletePreviousWeekWorkAnniversaries()
        {
            var (thisWeekStart, thisWeekEnd) = GeneralUtils.GetTwoWeeksDatesFromToday();

            var res = await annivarsaryRepository.All().Where(x => (x.Employee.HireDate.Month == thisWeekStart.Month &&
                                                                       x.Employee.HireDate.Day < thisWeekStart.Day) && x.DeletedDateTime == null)
                                                                            .ToListAsync();
            if (res.Count() <= 0) { return false; }
            foreach (var workAnniversary in res)
            {
                workAnniversary.DeletedDateTime = GeneralUtils.GetCurrentUKTime();
                annivarsaryRepository.Update(workAnniversary);
            }
            await birthdayRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<EmployeesHighFives>> GetAllHighFivesForLastTwoWeeks()
        {
            var (thisWeekStart, thisWeekEnd) = GeneralUtils.GetPreviousTwoWeeksDatesFromToday();
            var res = await highFiveRepository.All().Where(e => e.CreatedDateTime >= thisWeekStart && e.CreatedDateTime <= thisWeekEnd &&
                                                                       e.DeletedDateTime == null).OrderByDescending(h => h.CreatedDateTime)
                                                                       .Join(employeeRepository.All(), h => h.EmployeeId, e => e.Id,
                                                                       (h, e) => new EmployeesHighFives()
                                                                       {
                                                                           EmployeeID = h.EmployeeId,
                                                                           Comment = h.HighFiveComment,
                                                                           CreatedDateTime = h.CreatedDateTime,
                                                                           FirstName = e.FirstName,
                                                                           LastName = e.Surname,
                                                                           ImageBase64 = e.ImageBase64
                                                                       }).ToListAsync();
            return res;
        }
    }
}
