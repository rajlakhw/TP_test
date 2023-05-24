using Data;
using Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.EmployeeModels;

namespace Services.Interfaces
{
    public interface ITPEmployeesService : IService
    {
        Task<Data.Employee> IdentifyCurrentUser<Employee>(string LogonUserName);
        Task<int> IdentifyCurrentUserID<Einteger>(string LogonUserName);
        Task<Data.EmployeeDepartment> GetEmployeeDepartment(int EmployeeID);
        Task<Data.EmployeeTeam> GetEmployeeTeam<EmployeeTeam>(int EmployeeID);
        Task<Employee> IdentifyCurrentUserById(int? EmployeeID);
        Task<List<Data.Employee>> GetAllEmployeesInTeam<Employee>(int TeamID, bool IncludeTeamManager = true);
        Task<List<Data.Employee>> GetAllEmployeesInDepartment<Employee>(int DepartmentID);
        Task<List<Data.EmployeeTeam>> GetAllTeams<EmployeeTeam>(int? DepartmentID = null, bool showTeamsWithNoEmployees = true);
        Task<List<Data.EmployeeTeam>> GetAllTeamsForListOfDepartments<EmployeeTeam>(string AllDepartmentIDString);
        Task<List<Data.Employee>> GetAllEmployeesForListOfDepartments<Employee>(string AllDepartmentIDString);
        Task<List<Data.EmployeeDepartment>> GetAllDepartments<EmployeeDepartment>(bool showDeptWithNoEmployees = true);
        Task<List<Data.Employee>> GetAllEmployeeForThisManager<Employee>(int ManagerID);
        //Task<Data.QA.Employee> GetQAEmployee(string LogonUserName);
        Task<bool> UpdateProfileSettings(ProfileSettingsViewModel profileSettings, short empId);
        Task<List<Data.Employee>> GetAllEmployees<Employee>(bool IncludeLeavers, bool IncludeNonHumnas);
        Task<List<Data.Employee>> GetAllEmployeesInTeamString(string TeamIDsString);
        Task<EmployeeDetailsViewModel> GetEmployeeDetailsForViewComponent(short Id);
        Task<EmployeeViewModel> GetEmployeeByUsername(string username);
        Task<EmployeeViewModel> GetLoggedInEmployeeFromSessionOrDatabase();
        Task<Data.Employee> GetManagerOfEmployee(int EmployeeID);
        Task<Data.EmployeeOffice> GetEmployeeOffice(int EmployeeID);
        Task<List<Data.EmployeeOffice>> GetAllOffices();
        Task<Employee> GetCurrentManagerOfTeam(int TeamId);
        Task<Employee> IdentifyCurrentUserByIdTerminate(int EmployeeID);
        Task<int> GetCurrentLinguisticResourcesTeamManagerID();
        Task<Employee> CurrentSalesDepartmentManager();
        Task<int> UpdateEmployeeImage(Employee model);
        Task<Employee> GetEmployeeById(int empID);
        Task<bool> AttendsBoardMeetings(int empID);
        Task<List<Employee>> GetAllManagersInDeparmenet(int DepartmentID, string AnyTeamIDsToExclude = "");
    }
}
