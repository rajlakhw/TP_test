using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;
using Data;
using Data.Repositories;
//using Data.QA;
//using Data.QA.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class NotificationService : INotificationService
    {
        private readonly IRepository<NotificationRecipients> notificationRecipientRepo;
        private readonly IRepository<Employee> employeeRepo;

        public NotificationService(IRepository<Employee> repository,
                              IRepository<NotificationRecipients> repository2)
        {
            this.employeeRepo = repository;
            this.notificationRecipientRepo = repository2;

        }
        public async Task<List<Employee>> GetAllNotificationRecipients(short employeeId, byte NotificationTypeId)
        {

            List<Employee> result = await notificationRecipientRepo.All().
                                    Where(n => n.DataObjectTypeId == 5 && n.DataObjectId == employeeId && n.NotificationTypeId == NotificationTypeId).
                                    Join(employeeRepo.All(), //second table to inner join with
                                    n => n.RecipientEmployeeId, //inner join column from table1
                                    e => e.Id, //inner join column from table2
                                    (n, e) => new { Recipients = n, emp = e }).Select(e => e.emp).ToListAsync();

            return result;
        }
    }
}
