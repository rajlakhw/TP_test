using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Reports
{
    public class ReportingToolModel
    {
        //public List<Data.Employee> AllEmployees { get; set; }
        //public List<Data.EmployeeDepartment> AllDepartments { get; set; }
        //public List<Data.EmployeeTeam> AllTeams { get; set; }
        public Dictionary<int, string> AllOrgGroupsIdAndName { get; set; }
        public Dictionary<int, string> AllOrgsIdAndName { get; set; }
        public List<Data.EndClient> AllEndClients { get; set; }
        public List<Data.EndClientData> AllBrands { get; set; }
        public List<Data.EndClientData> AllCategories { get; set; }
        public List<Data.EndClientData> AllCampaigns { get; set; }
    }
}
