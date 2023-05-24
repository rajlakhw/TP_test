using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels.HR
{
    public class EmployeesOverviewModel
    {
        public List<Data.Employee> AllEmployees { get; set; }

        public List<string> AllEmployeeTeamNames { get; set; }

        public List<string> AllEmployeeDepartmentNames { get; set; }

        public List<string> AllEmployeeOfficesNames { get; set; }

        public List<Data.EmployeeDepartment> AllDepartments { get; set; }

        public List<Data.EmployeeTeam> AllTeams { get; set; }

        public List<Data.EmployeeOffice> AllOffices { get; set; }
    }
}
