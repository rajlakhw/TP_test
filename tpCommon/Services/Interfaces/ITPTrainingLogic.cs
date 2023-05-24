using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;


namespace Services.Interfaces 
{
    public interface ITPTrainingLogic : IService
    {
        Task<List<int>> GetAllYearsOfTrainingForEmployee(int employeeId);
        Task<List<TrainingSession>> GetAllTrainingSessionsForEmployeeForGivenYear(int employeeId, int year);
        Task<List<String>> GetAllTrainingCourseNamesForEmployeePage(int employeeId, int year);

        Task<List<String>> GetAllTrainersForEmployeePage(int employeeId, int year);

        Task<List<TrainingSessionAttendee>> GetAllTrainingAttendenceForEmployeePage(int employeeId, int year);
    }
}
