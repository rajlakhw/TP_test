using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Timesheet
{
    public class TimesheetBreakdownModel
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public short TaskHours { get; set; }
        public short TaskMinutes { get; set; }

    }
}
