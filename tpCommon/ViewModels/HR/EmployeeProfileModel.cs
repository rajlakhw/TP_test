using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using ViewModels.EmployeeModels;

namespace ViewModels.HR
{
    public class EmployeeProfileModel
    {
        public Employee EmployeeToBeViewed { get; set; }
        public EmployeeTeam Team { get; set; }
        public EmployeeDepartment Department { get; set; }
        public EmployeeOffice Office { get; set; }
        public EmployeeViewModel loggedInEmployee { get; set; }
        public bool IsloggedInEmployeeAllowedToApproveHoliday { get; set; }
        public bool IsloggedInEmployeeAllowedToViewSickness { get; set; }
        public int holidayYearSelected { get; set; }
        public int SicknessYearSelected { get; set; }
        public int TrainingYearSelected { get; set; }
        public List<EmployeeHoliday> AllHolidayYearDetails { get; set; }

        public List<EmployeeHolidayRequest> AllHolidayRequests { get; set; }

        public EmployeeHoliday HolidayDetailsForThisYear { get; set; }

        public List<Data.Task> AllEmployeeTasks { get; set; }

        //public List<Data.EmployeeOwnershipRelationship> AllOwnerships { get; set; }

        public List<Data.Employee> AllTasksCreatedByEmployee { get; set; }

        public List<String> AllTasksActionString { get; set; }

        //public List<Org> AllOwnershipOrgs { get; set; }

        //public List<EmployeeOwnershipType> AllOwnershipTypes { get; set; }

        public bool AllowChangingOwnershipNotification { get => loggedInEmployee.AccessLevelControls.Any(x => x.ControlName == "ownership-notification-link"); }

        public List<int> AllSicknessYears { get; set; }

        public Decimal TotalSicknessDaysForYear { get; set; }

        public List<EmployeesSickness> AllEmployeeSickness { get; set; }

        public List<int> AllTrainingYears { get; set; }

        public List<TrainingSession> AllEmployeeTraining { get; set; }

        public List<String> AllTrainingCourseNames { get; set; }

        public List<String> AllTrainingTrainerNames { get; set; }

        public List<TrainingSessionAttendee> AllTrainingAttendence { get; set; }



    }
}
