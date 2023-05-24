using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartAdmin.WebUI.Models.Timesheet
{
    public class TimesheetModel
    {
        public int Id { get; set; }
        public int EmployeeID { get; set; }
        public int OrgID { get; set; }
        public DateTime TimeLogDate { get; set; }
        public Int16 ClientChargeInHours { get; set; }
        public Int16 ClientChargeInMinutes { get; set; }
        public Int16 NonChargeableTimeInHours { get; set; }
        public Int16 NonChargeableTimeInMinutes { get; set; }
        public Int16 NonClientChargeInHours { get; set; }
        public Int16 NonClientChargeInMinutes { get; set; }

    }
}
