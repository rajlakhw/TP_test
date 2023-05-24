using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Timesheet
{
    public class TimesheetLogModel

    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int OrgID { get; set; }
        public DateTime TimeLogDate { get; set; }

    }
}
