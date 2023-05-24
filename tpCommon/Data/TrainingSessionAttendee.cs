using System;
using System.Collections.Generic;

#nullable disable

namespace Data
{
    public partial class TrainingSessionAttendee
    {
        public int Id { get; set; }
        public int TrainingSessionId { get; set; }
        public short EmployeeId { get; set; }
        public bool? AttendedTraining { get; set; }
        public byte InviteStatus { get; set; }
    }
}
