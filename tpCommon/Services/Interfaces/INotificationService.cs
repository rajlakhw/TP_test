using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Common;
using Data;
//using Data.QA;

namespace Services.Interfaces
{
    public interface INotificationService : IService
    {
        Task<List<Employee>> GetAllNotificationRecipients(short employeeId, byte NotificationTypeId);
    }
}
