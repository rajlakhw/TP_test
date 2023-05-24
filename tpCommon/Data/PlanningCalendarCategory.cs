using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class PlanningCalendarCategory
    {
        public PlanningCalendarCategory()
        {
            PlanningCalendarAppointments = new HashSet<PlanningCalendarAppointment>();
        }

        public short Id { get; set; }
        public string CategoryRgbcolor { get; set; }

        public virtual ICollection<PlanningCalendarAppointment> PlanningCalendarAppointments { get; set; }
    }
}
