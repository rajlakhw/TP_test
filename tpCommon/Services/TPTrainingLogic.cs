using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data.Repositories;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class TPTrainingLogic : ITPTrainingLogic
    {
        private readonly IRepository<TrainingSession> trainingSessionRepo;
        private readonly IRepository<TrainingSessionAttendee> trainingSessionAttendeesRepo;
        private readonly IRepository<TrainingCourse> trainingCourseRepo;
        private readonly IRepository<Employee> employeeRepo;
        public TPTrainingLogic(IRepository<TrainingSession> repository,
                               IRepository<TrainingSessionAttendee> repository1,
                               IRepository<TrainingCourse> repository2,
                               IRepository<Employee> repository3)
        {
            this.trainingSessionRepo = repository;
            this.trainingSessionAttendeesRepo = repository1;
            this.trainingCourseRepo = repository2;
            this.employeeRepo = repository3;
        }

        public async Task<List<int>> GetAllYearsOfTrainingForEmployee(int employeeId)
        {
            var result = await trainingSessionRepo.All()
                         .Join(trainingSessionAttendeesRepo.All(),
                               s => s.Id,
                               a => a.TrainingSessionId,
                               (s, a) => new { TrainingSession = s, Attendees = a })
                         .Where(s => s.Attendees.EmployeeId == employeeId)

                         .Select(y => y.TrainingSession.TrainingSessionDate.Year)
                         .Distinct().OrderByDescending(o => o).ToListAsync();
            return result;
        }

        public async Task<List<TrainingSession>> GetAllTrainingSessionsForEmployeeForGivenYear(int employeeId, int year)
        {
            var result = await trainingSessionRepo.All()
                         .Join(trainingSessionAttendeesRepo.All(),
                               s => s.Id,
                               a => a.TrainingSessionId,
                               (s, a) => new { TrainingSession = s, Attendees = a })
                         .Where(t => t.Attendees.EmployeeId == employeeId &&
                                t.TrainingSession.TrainingSessionDate.Year == year && t.TrainingSession.DeletedDateTime == null)
                         .Select(t => t.TrainingSession)
                         .OrderByDescending(o => o.TrainingSessionDate)
                         .ToListAsync();
            return result;
        }

        public async Task<List<String>> GetAllTrainingCourseNamesForEmployeePage(int employeeId, int year)
        {
            var result = await GetAllTrainingSessionsForEmployeeForGivenYear(employeeId, year);

            List<String> AllCourseNames = new List<String>();
            for (var i = 0; i < result.Count; i++)
            {
                int trainingCourseId = result.ElementAt(i).TrainingCourseId;
                var courseName = await trainingCourseRepo.All().Where(c => c.Id == trainingCourseId).Select(t => t.TrainingName).FirstOrDefaultAsync();
                AllCourseNames.Add(courseName);
            }

            return AllCourseNames;
        }

        public async Task<List<String>> GetAllTrainersForEmployeePage (int employeeId, int year)
        {
            var result = await GetAllTrainingSessionsForEmployeeForGivenYear(employeeId, year);

            List<String> AllTrainerNames = new List<String>();
            for (var i = 0; i < result.Count; i++)
            {
                if (result.ElementAt(i).TrainerIsExternal == true)
                {
                    AllTrainerNames.Add(result.ElementAt(i).ExternalTrainer);
                }
                else
                {
                    if (result.ElementAt(i).TrainingOfficerEmployeeId != null)
                    {
                        var trainer = await employeeRepo.All().Where(e => e.Id == result.ElementAt(i).TrainingOfficerEmployeeId).FirstOrDefaultAsync();
                        var trainerName = trainer.FirstName + ' ' + trainer.Surname;
                        AllTrainerNames.Add(trainerName);
                    }
                }
            }

            return AllTrainerNames;
        }

        public async Task<List<TrainingSessionAttendee>> GetAllTrainingAttendenceForEmployeePage(int employeeId, int year)
        {
            var result = await GetAllTrainingSessionsForEmployeeForGivenYear(employeeId, year);

            List<TrainingSessionAttendee> AllAttendance = new List<TrainingSessionAttendee>();
            for (var i = 0; i < result.Count; i++)
            {
                var thisTrainingSession = trainingSessionAttendeesRepo.All().Where(a => a.EmployeeId == employeeId && result.ElementAt(i).Id == a.TrainingSessionId).FirstOrDefault();
                AllAttendance.Add(thisTrainingSession);
            }

            return AllAttendance;
        }
    }
}
