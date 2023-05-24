using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeModels;

namespace ViewModels.HR
{
    public class HolidayAdminModel
    {
        public List<EmployeeHolidayModel> AllEmployeeHolidays { get; set; }
        public List<int> AllHolidayYears { get; set; }
        public List<Data.EmployeeOffice> AllOffices { get; set; }
        public EmployeeViewModel LoggedInEmployee { get; set; }
        public bool AllowAddingAndEditingOfEmployeeHoliday { get => LoggedInEmployee.AccessLevelControls.Any(x => x.ControlName == "bank-holiday-add-update-control"); }
    }
}
