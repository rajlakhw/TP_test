using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;

namespace Services.Interfaces
{
    public interface ITPTasksLogic : IService
    {
        Task<List<Data.Task>> GetAllTasksForDataObjectID(int dataObjectID, short dataObjectTypeId);
        Task<Data.Task> GetTaskDetails(int taskId);
        Task<int> UpdateEmployeeTask(int taskId, short employeeId, DateTime dueDate, String notes, bool isHotTask, bool isCompleted, short updatedByEmployeeId);

    }
}
