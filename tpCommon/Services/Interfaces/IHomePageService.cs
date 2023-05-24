using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Common;
using ViewModels.HomePage;

namespace Services.Interfaces
{
    public interface IHomePageService : IService
    {
        Task<List<EmployeesBirthdays>> GetAllBirthdayWishesForCurrentWeek(short EmployeeLoggedInId);
        Task<List<EmployeesAnnivarsaries>> GetAllWorkAnniversariesWishesForCurrentWeek(short EmployeeLoggedInId);
        Task<List<EmployeesHighFives>> GetAllHighFivesForLastTwoWeeks();
    }
}
