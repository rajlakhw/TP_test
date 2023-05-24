using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class PlanningCalendarAppointment
    {
        public int Id { get; set; }
        public string ExtranetUserName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public short Category { get; set; }
        public short? WorkingTimeStartHours { get; set; }
        public short? WorkingTimeStartMinutes { get; set; }
        public short? WorkingTimeEndHours { get; set; }
        public short? WorkingTimeEndMinutes { get; set; }
        public string SubjectLine { get; set; }
        public string Notes { get; set; }

        public virtual PlanningCalendarCategory CategoryNavigation { get; set; }
    }
}
