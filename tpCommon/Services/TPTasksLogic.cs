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
    public class TPTasksLogic : ITPTasksLogic
    {
        private readonly IRepository<Data.Task> TasksRepository;
        public TPTasksLogic(IRepository<Data.Task> repository)
        {
            this.TasksRepository = repository;
        }

        public async Task<List<Data.Task>> GetAllTasksForDataObjectID(int dataObjectID, short dataObjectTypeId)
        {
            var result = await TasksRepository.All()
                                .Where(t => t.DataObjectId == dataObjectID && t.DataObjectTypeId == dataObjectTypeId && t.DeletedDate == null)
                                .OrderByDescending(o => o.DueDateTime)
                                .ToListAsync();

            return result;
        }

        public async Task<Data.Task> GetTaskDetails(int taskId)
        {
            var result = await TasksRepository.All().Where(s => s.Id == taskId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> UpdateEmployeeTask(int taskId, short employeeId, DateTime dueDate, String notes, bool isHotTask, bool isCompleted, short updatedByEmployeeId)
        {
            var thisTask = await GetTaskDetails(taskId);

            thisTask.DataObjectId = employeeId;
            thisTask.DueDateTime = dueDate;
            thisTask.ProgressNotes = notes;
            thisTask.IsHot = isHotTask;

            if (isCompleted == true)
            {
                thisTask.CompletedDateTime = GeneralUtils.GetCurrentGMT();
                thisTask.CompletedByEmployeeId = updatedByEmployeeId;
            }
            else
            {
                thisTask.CompletedDateTime = null;
                thisTask.CompletedByEmployeeId = null;
            }

            thisTask.LastModifiedByEmployeeId = updatedByEmployeeId;
            thisTask.LastModifiedDate = GeneralUtils.GetCurrentGMT();


            TasksRepository.Update(thisTask);
            await TasksRepository.SaveChangesAsync();

            return taskId;
        }
    }
}
