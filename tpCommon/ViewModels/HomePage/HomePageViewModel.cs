using System.Collections.Generic;

namespace ViewModels.HomePage
{
    public class HomePageViewModel
    {
        public short LoggedInEmployeeId { get; set; }
        public StaffAnnivarsaries StaffAnniversaries { get; set; }
        public List<EmployeesHighFives> EmployeesHighFives { get; set; }
        public IEnumerable<IAnniversary> Anniversaries { get; set; }
    }
}
