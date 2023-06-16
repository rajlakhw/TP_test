using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using ViewModels.Common;
using ViewModels.EmployeeModels;
using ViewModels.EmployeeModels.EmployeeAccessMatrix;

namespace Services
{
    public class TPEmployeesService : ITPEmployeesService
    {
        private readonly IRepository<Employee> employeeRepository;
        //private readonly Data.QA.Repositories.IRepository<Data.QA.Employee> employeeQARepository;
        private readonly IRepository<EmployeeTeam> teamRepository;
        private readonly IRepository<EmployeeDepartment> deptRepository;
        private readonly Data.Repositories.IRepository<Data.JobOrder> jobRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IRepository<AccessControl> accessControlsRepository;
        private readonly IRepository<EmployeeOffice> officeRepository;

        public TPEmployeesService(IRepository<Employee> repository, IRepository<EmployeeTeam> repository1,
                                  IRepository<EmployeeDepartment> repository2,
                                  Data.Repositories.IRepository<Data.JobOrder> jobRepository, IRepository<EmployeeOffice> OfficeRepository, IRepository<AccessControl> accessControlsRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.employeeRepository = repository;
            this.teamRepository = repository1;
            this.deptRepository = repository2;
            //this.employeeQARepository = employeeQARepository;
            this.jobRepository = jobRepository;
            this.httpContextAccessor = httpContextAccessor;
            this.accessControlsRepository = accessControlsRepository;
            this.officeRepository = OfficeRepository;
        }
        public async Task<Data.Employee> IdentifyCurrentUser<Employee>(string LogonUserName)
        {
            var result = await employeeRepository.All().Where(o => o.NetworkUserName == LogonUserName && o.TerminateDate == null).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> IdentifyCurrentUserID<integer>(string LogonUserName)
        {
            var result = await employeeRepository.All().Where(o => o.NetworkUserName == LogonUserName).Where(o => o.TerminateDate == null).Select(o => o.Id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Data.EmployeeTeam> GetEmployeeTeam<EmployeeTeam>(int EmployeeID)
        {
            var result = await employeeRepository.All()
                               .Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                    .Where(o => o.emp.Id == EmployeeID).Select(tm => tm.team).SingleOrDefaultAsync();
            return result;
        }

        public async Task<Data.EmployeeDepartment> GetEmployeeDepartment(int EmployeeID)
        {
            var result = await employeeRepository.All()
                               .Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                    .Where(o => o.emp.Id == EmployeeID)
                                    .Select(tm => tm.team)
                                .Join(deptRepository.All(),
                                      t => t.DepartmentId,
                                      d => d.Id,
                                      (t, d) => new { dept = d }).Select(dpt => dpt.dept).SingleOrDefaultAsync();
            return result;
        }

        public async Task<Data.EmployeeOffice> GetEmployeeOffice(int EmployeeID)
        {
            var result = await employeeRepository.All()
                               .Join(officeRepository.All(),
                                    e => e.OfficeId,
                                    o => o.Id,
                                    (e, o) => new { emp = e, office = o })
                                    .Where(o => o.emp.Id == EmployeeID)
                                    .Select(of => of.office).SingleOrDefaultAsync();
            return result;
        }

        public async Task<List<Data.EmployeeDepartment>> GetAllDepartments<EmployeeDepartment>(bool showDeptWithNoEmployees = true)
        {
            if (showDeptWithNoEmployees == true)
            {
                var result = await deptRepository.All().ToListAsync();
                return result;
            }
            else
            {
                var result = await deptRepository.All().
                                    Join(teamRepository.All(),
                                    d => d.Id,
                                    t => t.DepartmentId,
                                    (d, t) => new { dept = d, team = t }).Select(d => d.dept).Distinct().ToListAsync();
                return result;
            }

        }

        public async Task<Employee> IdentifyCurrentUserById(int? EmployeeID)
        {
            var result = await employeeRepository.All().Where(x => x.Id == EmployeeID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<Employee> IdentifyCurrentUserByIdTerminate(int EmployeeID)
        {
            var result = await employeeRepository.All().Where(x => x.Id == EmployeeID).FirstOrDefaultAsync();
            return result;
        }
        public async Task<List<Data.Employee>> GetAllEmployees<Employee>(bool IncludeLeavers, bool IncludeNonHumnas)
        {
            if (IncludeLeavers == true)
            {
                if (IncludeNonHumnas == true)
                {
                    var result = await employeeRepository.All().OrderBy(x => x.FirstName).ThenBy(x => x.Surname).ToListAsync();
                    return result;
                }
                else
                {
                    var result = await employeeRepository.All().Where(x => x.Surname != "translate plus" && (x.EmployeeStatusType != 7 || (x.Surname == "Meeting Room" || x.Surname == "Booth" || x.Surname == "TBA" || x.FirstName == "TBA"))).OrderBy(x => x.FirstName).ThenBy(x => x.Surname).ToListAsync();
                    return result;
                }
            }
            else
            {
                if (IncludeNonHumnas == true)
                {
                    var result = await employeeRepository.All().Where(x => x.TerminateDate == null).OrderBy(x => x.FirstName).ThenBy(x => x.Surname).ToListAsync();
                    return result;
                }
                else
                {
                    var result = await employeeRepository.All().Where(x => x.Surname != "translate plus" && x.TerminateDate == null && (x.EmployeeStatusType != 7 || (x.Surname == "Meeting Room" || x.Surname == "Booth" || x.Surname == "TBA" || x.FirstName == "TBA"))).OrderBy(x => x.FirstName).ThenBy(x => x.Surname).ToListAsync();
                    return result;
                }
            }
        }


        public async Task<List<Data.Employee>> GetAllEmployeesInTeam<Employee>(int TeamID, bool IncludeTeamManager = true)
        {
            var result = await employeeRepository.All().
                Where(x => x.TeamId == TeamID && x.TerminateDate == null && (IncludeTeamManager == true || x.IsTeamManager == false)).
                ToListAsync();
            return result;
        }

        public async Task<List<Data.Employee>> GetAllEmployeesInTeamString(string TeamIDsString)
        {
            var result = await employeeRepository.All().
                         Where(x => x.TerminateDate == null && TeamIDsString.Contains(x.TeamId.ToString()))
                         .OrderBy(x => x.FirstName).ThenBy(x => x.Surname).ToListAsync();
            return result;
        }

        public async Task<List<Data.Employee>> GetAllEmployeesInDepartment<Employee>(int DepartmentID)
        {
            var result = await employeeRepository.All()
                               .Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                .Where(x => x.team.DepartmentId == DepartmentID && x.emp.TerminateDate == null)
                                .Select(e => e.emp)
                                .ToListAsync();
            return result;
        }

        public async Task<List<Data.EmployeeTeam>> GetAllTeams<EmployeeTeam>(int? DepartmentID = null, bool showTeamsWithNoEmployees = true)
        {
            if (showTeamsWithNoEmployees == true)
            {
                var result = await teamRepository.All()
                               .Where(x => (DepartmentID == null || x.DepartmentId == DepartmentID))
                               .OrderBy(o => o.Name)
                               .ToListAsync();
                return result;
            }
            else
            {
                var result = await teamRepository.All()
                                   .Join(employeeRepository.All(),
                                         t => t.Id,
                                         e => e.TeamId,
                                         (t, e) => new { team = t, emp = e })
                              .Where(x => (DepartmentID == null || x.team.DepartmentId == DepartmentID) && x.emp.TerminateDate == null)
                              .Select(t => t.team).Distinct()
                              .OrderBy(o => o.Name)
                              .ToListAsync();
                return result;
            }

        }

        public async Task<List<Data.EmployeeOffice>> GetAllOffices()
        {
            var result = await officeRepository.All()
                               .Join(employeeRepository.All(),
                                     o => o.Id,
                                     e => e.OfficeId,
                                     (o, e) => new { office = o, emp = e })
                          .Where(x => x.emp.TerminateDate == null)
                          .Select(t => t.office).Distinct()
                          .OrderBy(o => o.Name)
                          .ToListAsync();
            return result;


        }

        public async Task<Data.Employee> GetManagerOfEmployee(int EmployeeID)
        {
            int? managerID = employeeRepository.All().Where(x => x.Id == EmployeeID).Select(m => m.Manager).FirstOrDefault();

            if (managerID == null)
            {
                int employeeTeamId = employeeRepository.All().
                                 Where(x => x.Id == EmployeeID).Select(t => t.TeamId).FirstOrDefault();
                var result = employeeRepository.All().Where(t => t.TeamId == employeeTeamId && t.IsTeamManager == true && t.Id != EmployeeID && t.TerminateDate == null).FirstOrDefault();

                return result;
            }
            else
            {
                var result = await IdentifyCurrentUserById(managerID);
                return result;
            }

        }

        public async Task<List<Data.Employee>> GetAllEmployeeForThisManager<Employee>(int ManagerID)
        {
            Data.Employee manager = IdentifyCurrentUserById(ManagerID).Result;
            int? managersTeamId = employeeRepository.All().
                                 Where(x => x.Id == ManagerID && x.IsTeamManager == true).Select(t => t.TeamId).FirstOrDefault();

            int? managersDeptId = GetEmployeeDepartment(ManagerID).Result.Id;

            //Liina, Nikolay and Maria to view everyone's timesheet from GLS department
            if (ManagerID == 646 || ManagerID == 1298 || ManagerID == 12)
            {
                var result = await employeeRepository.All()
                                   .Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                    .Where(x => x.emp.TerminateDate == null && x.team.DepartmentId == 9 && x.emp.Id != ManagerID)
                                    .Select(e => e.emp)
                                   .OrderBy(o => o.FirstName).ThenBy(o => o.Surname).ToListAsync();
                return result;
            }
            //Jean and Isabel to view everyone's timesheet from T&P department
            if (ManagerID == 1305 || ManagerID == 20)
            {
                var result = await employeeRepository.All()
                                   .Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                    .Where(x => x.emp.TerminateDate == null && x.team.DepartmentId == 10 && x.emp.Id != ManagerID)
                                    .Select(e => e.emp)
                                   .OrderBy(o => o.FirstName).ThenBy(o => o.Surname).ToListAsync();
                return result;
            }
            else if (ManagerID == 41 || ManagerID == 475) //Ad and Umer to view all employees timesheet
            {
                var result = await employeeRepository.All().
                                  Where(x => x.TerminateDate == null && x.EmployeeStatusType != 5 && x.EmployeeStatusType != 7 &&
                                        x.EmployeeStatusType != 8 && x.Id != ManagerID)
                                  .OrderBy(o => o.FirstName).ThenBy(o => o.Surname).ToListAsync();
                return result;
            }
            //for T&P dept, all TMs need to be able to view all timesheets of all PMs in T&P dept, and not just for their team
            else if (managersDeptId == 10 && manager.IsTeamManager == true)
            {
                var result = await employeeRepository.All().
                                    Join(teamRepository.All(),
                                    e => e.TeamId,
                                    t => t.Id,
                                    (e, t) => new { emp = e, team = t })
                                   .Where(x => x.emp.TerminateDate == null &&
                                        ((x.team.DepartmentId == managersDeptId && x.emp.Id != ManagerID && x.emp.IsTeamManager == false) ||
                                         (x.emp.Manager == ManagerID)))
                                   .Select(e => e.emp)
                                   .OrderBy(o => o.FirstName).ThenBy(o => o.Surname).ToListAsync();
                return result;
            }
            else if (managersTeamId == 0)
            {
                var result = await employeeRepository.All().
                                   Where(x => x.Manager == ManagerID && x.TerminateDate == null).ToListAsync();
                return result;
            }
            else
            {
                var result = await employeeRepository.All().
                                   Where(x => x.TerminateDate == null &&
                                        ((x.TeamId == managersTeamId && x.Id != ManagerID) || (x.Manager == ManagerID)))
                                   .OrderBy(o => o.FirstName).ThenBy(o => o.Surname).ToListAsync();
                return result;
            }


        }

        //public async Task<Data.QA.Employee> GetQAEmployee(string LogonUserName)
        //{
        //    var res = await employeeQARepository.All().Where(o => o.NetworkUserName == LogonUserName && o.TerminateDate == null).FirstOrDefaultAsync();
        //    return res;
        //}

        public async Task<bool> UpdateProfileSettings(ProfileSettingsViewModel profileSettings, short empId)
        {
            var emp = await employeeRepository.All().Where(x => x.Id == empId).FirstOrDefaultAsync();

            emp.ShowAnniversariesOnHomePage = profileSettings.showAnniversaries;
            emp.ShowLikesOrCommentsOnHomePage = profileSettings.showLikesAndComments;

            employeeRepository.Update(emp);
            await employeeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<List<Data.EmployeeTeam>> GetAllTeamsForListOfDepartments<EmployeeTeam>(string AllDepartmentIDString)
        {
            var result = await teamRepository.All()
                               .Where(x => AllDepartmentIDString.Contains(x.DepartmentId.ToString()))
                               .OrderBy(x => x.Name)
                               .ToListAsync();
            return result;
        }

        public async Task<List<Data.Employee>> GetAllEmployeesForListOfDepartments<Employee>(string AllDepartmentIDString)
        {
            var result = await employeeRepository.All().Where(e => e.TerminateDate == null)
                               .Join(teamRepository.All(),
                                     e => e.TeamId,
                                     t => t.Id,
                                     (e, t) => new { emp = e, team = t })
                               .Where(x => AllDepartmentIDString.Contains(x.team.DepartmentId.ToString()))
                               .Select(e => e.emp)
                               .OrderBy(x => x.FirstName).ThenBy(x => x.Surname)
                               .ToListAsync();
            return result;
        }

        public async Task<EmployeeDetailsViewModel> GetEmployeeDetailsForViewComponent(short Id)
        {
            var res = await employeeRepository.All().Where(x => x.Id == (short)Id).Select(x => new EmployeeDetailsViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.Surname,
                ProfilePicture = x.ImageBase64,
                JobTitle = x.JobTitle,
            }).FirstOrDefaultAsync();
            return res;
        }

        /// <summary>
        /// Gets EmployeeViewModel from HTTP Context or Database async if the session is empty.
        /// </summary>
        /// <returns>EmployeeViewModel</returns>
        public async Task<EmployeeViewModel> GetLoggedInEmployeeFromSessionOrDatabase()
        {
            var employeeLoggedIn = httpContextAccessor.HttpContext.Session.Get<EmployeeViewModel>("EmployeeLoggedIn");

            if (employeeLoggedIn == null)
            {
                var username = httpContextAccessor.HttpContext.User.Identity.Name; // "evazrihe";
                username = GeneralUtils.GetUsernameFromNetwokUsername(username);
                employeeLoggedIn = await this.GetEmployeeByUsername(username);

                httpContextAccessor.HttpContext.Session.Set("EmployeeLoggedIn", employeeLoggedIn);
            }

            employeeLoggedIn.AccessLevelControls = await accessControlsRepository.All()
                .Where(x => x.EmployeeAccessLevelId == employeeLoggedIn.AccessLevel)
                .Include(x => x.EmployeeAccessLevel)
                .Include(x => x.AccessMatrixControl)
                .Select(x => new AccessLevelConrolsForEmployeeViewModel()
                {
                    AccessMatrixControlId = x.AccessMatrixControlId,
                    EmployeeAccessLevelId = x.EmployeeAccessLevelId,
                    ControlName = x.AccessMatrixControl.ControlName,
                    EmployeeAccessLevel = x.EmployeeAccessLevel.EmployeeAccessLevel,
                    Notes = x.EmployeeAccessLevel.Notes,
                    Page = x.AccessMatrixControl.Page,
                    RelationShipId = x.Id
                })
                .ToListAsync();

            return employeeLoggedIn;
        }

        public async Task<EmployeeViewModel> GetEmployeeByUsername(string username)
        {
            var employeeLoggedIn = await employeeRepository.All()
                .FirstOrDefaultAsync(x => x.NetworkUserName == username && x.TerminateDate == null);

            var employee = new EmployeeViewModel()
            {
                Id = employeeLoggedIn.Id,
                NetworkUserName = employeeLoggedIn.NetworkUserName,
                Surname = employeeLoggedIn.Surname,
                FirstName = employeeLoggedIn.FirstName,
                HireDate = employeeLoggedIn.HireDate,
                TerminateDate = employeeLoggedIn.TerminateDate,
                EmailAddress = employeeLoggedIn.EmailAddress,
                LandlineNumber = employeeLoggedIn.LandlineNumber,
                InternalExtension = employeeLoggedIn.InternalExtension,
                MobileNumber = employeeLoggedIn.MobileNumber,
                MobileNumberIsPersonalOnly = employeeLoggedIn.MobileNumberIsPersonalOnly,
                Notes = employeeLoggedIn.Notes,
                TeamId = employeeLoggedIn.TeamId,
                Manager = employeeLoggedIn.Manager,
                IsTeamManager = employeeLoggedIn.IsTeamManager,
                SalesNewBusinessCommissionPercentage = employeeLoggedIn.SalesNewBusinessCommissionPercentage,
                SalesAccountManagementCommissionPercentage = employeeLoggedIn.SalesAccountManagementCommissionPercentage,
                DateOfBirth = employeeLoggedIn.DateOfBirth,
                OfficeId = employeeLoggedIn.OfficeId,
                WorkingHoursNotes = employeeLoggedIn.WorkingHoursNotes,
                NextOfKinName = employeeLoggedIn.NextOfKinName,
                NextOfKinAddress = employeeLoggedIn.NextOfKinAddress,
                NextOfKinContactPhoneNumber = employeeLoggedIn.NextOfKinContactPhoneNumber,
                SkypeId = employeeLoggedIn.SkypeId,
                LinkedInUrl = employeeLoggedIn.LinkedInUrl,
                JobTitle = employeeLoggedIn.JobTitle,
                IncludeInPerformancePlusFigures = employeeLoggedIn.IncludeInPerformancePlusFigures,
                IsSalesAccountManager = employeeLoggedIn.IsSalesAccountManager,
                EmployeeStatusType = employeeLoggedIn.EmployeeStatusType,
                HighFiveCount = employeeLoggedIn.HighFiveCount,
                HrhighFiveNotes = employeeLoggedIn.HrhighFiveNotes,
                ProbationDuration = employeeLoggedIn.ProbationDuration,
                ResignationSubmitted = employeeLoggedIn.ResignationSubmitted,
                NoticePeriod = employeeLoggedIn.NoticePeriod,
                AdditionalLoyaltyPerks = employeeLoggedIn.AdditionalLoyaltyPerks,
                ImageBase64 = employeeLoggedIn.ImageBase64,
                AccessLevel = employeeLoggedIn.AccessLevel,
                ShowAnniversariesOnHomePage = employeeLoggedIn.ShowAnniversariesOnHomePage,
                ShowLikesOrCommentsOnHomePage = employeeLoggedIn.ShowLikesOrCommentsOnHomePage,
                Department = await this.GetEmployeeDepartment(employeeLoggedIn.Id)
            };

            return employee;
        }

        public async Task<Employee> GetCurrentManagerOfTeam(int TeamId)
        {
            var results = await employeeRepository.All().Where(e => e.TeamId == TeamId && e.IsTeamManager == true).FirstOrDefaultAsync();
            return results;
        }

        public async Task<int> GetCurrentLinguisticResourcesTeamManagerID()
        {
            var employeeId = await employeeRepository.All().Where(x => x.TeamId == 12 && x.TerminateDate == null && x.IsTeamManager == true).Select(x => x.Id).FirstOrDefaultAsync();

            if (employeeId == 0)
                return 12; // Liina

            return employeeId;
        }

        public async Task<Employee> CurrentSalesDepartmentManager()
        {
            var result = await GetCurrentManagerOfTeam(24);
            return result;
        }

        //public async Task<List<Data.Employee>> GetAllEmployeesInTeamForTimesheetApproval<Employee>(int TeamID)
        //{
        //    var result = await employeeRepository.All().Where(x => x.TeamId == TeamID && x.TerminateDate == null).ToListAsync();
        //    return result;
        //}

        public async Task<int> UpdateEmployeeImage(Employee model)
        {
            var thisEmployee = await IdentifyCurrentUserById(model.Id);

            if (thisEmployee != null)
            {
                thisEmployee.ImageBase64 = model.ImageBase64;
            }


            employeeRepository.Update(thisEmployee);
            await employeeRepository.SaveChangesAsync();

            return model.Id;
        }
        public async Task<Employee> GetEmployeeById(int empID)
        {
            var employee = await employeeRepository.All().AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == empID && x.TerminateDate == null);
            if (employee.Manager == null)
            {
                var teamManager = await employeeRepository.All().FirstOrDefaultAsync(x => x.TeamId == employee.TeamId && x.TerminateDate == null && x.IsTeamManager == true);
                if (teamManager != null && teamManager.Id != empID)
                {
                    employee.Manager = empID;
                }
            }

            return employee;
        }

        public async Task<bool> AttendsBoardMeetings(int empID)
        {
            var employee = await GetEmployeeById(empID);
            var DoesAttend = false;
            if (employee.EmailAddress == "Adrian.Metcalf@translateplus.com" ||
            employee.EmailAddress == "Liina.Puust@translateplus.com" ||
            employee.EmailAddress == "Svenja.Muller@translateplus.com" ||
            employee.EmailAddress == "Umer.Nizam@translateplus.com")
                DoesAttend = true;
            return DoesAttend;
        }
        public async Task<List<Employee>> GetAllManagersInDeparmenet(int DepartmentID, string AnyTeamIDsToExclude = "")
        {
            string[] allTeamIDs = AnyTeamIDsToExclude.Split(",");

            if (AnyTeamIDsToExclude != "") { }
            var result = await deptRepository.All().Where(d => d.Id == DepartmentID)
                               .Join(teamRepository.All().Where(t => (AnyTeamIDsToExclude == "" || ! allTeamIDs.Contains(t.Id.ToString()))),
                                     d => d.Id,
                                     t => t.DepartmentId,
                                     (d, t) => new { team = t }).Select(t => t.team)
                               .Join(employeeRepository.All().Where(e => e.TerminateDate == null && e.IsTeamManager == true),
                                     t => t.Id,
                                     e => e.TeamId,
                                     (t, e) => new { emp = e }).Select(e => e.emp).ToListAsync();
            return result;
        }
    }
}
